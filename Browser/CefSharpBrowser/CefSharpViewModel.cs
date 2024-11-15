using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Browser.CefSharpBrowser.AirControlSimulator;
using Browser.CefSharpBrowser.CefOp;
using Browser.CefSharpBrowser.CompassPrediction;
using Browser.CefSharpBrowser.ExtraBrowser;
using BrowserLibCore;
using CefSharp;
using CefSharp.Handler;
using CefSharp.WinForms;
using Cef = CefSharp.Cef;
using DownloadHandler = CefSharp.Fluent.DownloadHandler;
using IBrowser = CefSharp.IBrowser;

namespace Browser.CefSharpBrowser;

public class CefSharpViewModel : BrowserViewModel
{
	public override object? Browser => Host;
	private WindowsFormsHost Host { get; } = new();
	private ChromiumWebBrowser? CefSharp { get; set; }
	private static string BrowserCachePath => BrowserConstants.CefSharpCachePath;

	public CefSharpViewModel(string host, int port, string culture) : base(host, port, culture)
	{
		// Debugger.Launch();
	}

	public override async void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (sender is not Window window) return;

		IntPtr handle = new WindowInteropHelper(window).Handle;
		SetWindowLong(handle, GWL_STYLE, WS_CHILD);

		ConfigurationChanged();

		// ウィンドウの親子設定＆ホストプロセスから接続してもらう
		await BrowserHost.ConnectToBrowser((long)handle);

		// 親ウィンドウが生きているか確認
		HeartbeatTimer.Elapsed += async (sender2, e2) =>
		{
			try
			{
				bool alive = await BrowserHost.IsServerAlive();
			}
			catch (Exception)
			{
				Debug.WriteLine("host died");
				Exit();
			}
		};
		HeartbeatTimer.Interval = 2000; // 2秒ごと　
		HeartbeatTimer.Start();

		SetIconResource();

		InitializeAsync();
	}

	/// <summary>
	/// ブラウザを初期化します。
	/// 最初の呼び出しのみ有効です。二回目以降は何もしません。
	/// </summary>
	protected override void InitializeAsync()
	{
		if (CefSharp is not null) return;
		if (ProxySettings is null) return;
		if (Configuration is null) return;

		CefSettings? settings;

		try
		{
			settings = new CefSettings
			{
				CachePath = Path.GetFullPath(BrowserCachePath),
				Locale = "ja",
				AcceptLanguageList = "ja,en-US,en", // todo: いる？
				LogSeverity = Configuration.SavesBrowserLog ? LogSeverity.Error : LogSeverity.Disable,
				LogFile = "BrowserLog.log",
			};
		}
		catch (BadImageFormatException)
		{
			if (MessageBox.Show(FormBrowser.InstallVisualCpp, FormBrowser.CefSharpLoadErrorTitle,
					MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes)
				== MessageBoxResult.Yes)
			{
				ProcessStartInfo psi = new()
				{
					FileName = @"https://support.microsoft.com/en-us/topic/the-latest-supported-visual-c-downloads-2647da03-1eea-4433-9aff-95f26a218cc0",
					UseShellExecute = true
				};
				Process.Start(psi);
			}
			throw;
		}

		if (!Configuration.HardwareAccelerationEnabled)
			settings.DisableGpuAcceleration();

		settings.CefCommandLineArgs.Add("proxy-server", ProxySettings);
		settings.CefCommandLineArgs.Add("enable-features", "EnableDrDc");
		// prevent CEF from taking over media keys
		if (settings.CefCommandLineArgs.ContainsKey("disable-features"))
		{
			List<string> disabledFeatures = settings.CefCommandLineArgs["disable-features"]
				.Split(",")
				.ToList();

			disabledFeatures.Add("HardwareMediaKeyHandling");

			settings.CefCommandLineArgs["disable-features"] = string.Join(",", disabledFeatures);
		}
		else
		{
			settings.CefCommandLineArgs.Add("disable-features", "HardwareMediaKeyHandling");
		}


		if (Configuration.ForceColorProfile)
		{
			settings.CefCommandLineArgs.Add("force-color-profile", "srgb");
		}

		CefSharpSettings.SubprocessExitIfParentProcessClosed = true;
		CefSharpSettings.RuntimeStyle = CefRuntimeStyle.Chrome;
		Cef.Initialize(settings, false, (IBrowserProcessHandler?)null);

		CustomRequestHandler requestHandler = new(Configuration);

		requestHandler.RenderProcessTerminated += (mes) => AddLog(3, mes);

		CefSharp = new ChromiumWebBrowser(KanColleUrl)
		{
			RequestHandler = requestHandler,
			KeyboardHandler = new CefKeyboardHandler(this),
			MenuHandler = new MenuHandler(),
			DragHandler = new DragHandler(),
			DownloadHandler = DownloadHandler
				.AskUser((chromiumBrowser, browser, downloadItem, callback) =>
				{
					// don't need any extra code here
				}),
		};

		CefSharp.BrowserSettings.StandardFontFamily = "Microsoft YaHei"; // Fixes text rendering position too high
		CefSharp.LoadingStateChanged += Browser_LoadingStateChanged;

		Host.Child = CefSharp;
	}

	private void Browser_LoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
	{
		SetCookie();

		// DocumentCompleted に相当?
		// note: 非 UI thread からコールされるので、何かしら UI に触る場合は適切な処置が必要

		if (e.IsLoading) return;

		App.Current.Dispatcher.BeginInvoke(() =>
		{
			ApplyStyleSheet();
			ApplyZoom();
			DestroyDMMreloadDialog();
		});
	}

	// タイミングによっては(特に起動時)、ブラウザの初期化が完了する前に Navigate() が呼ばれることがある
	// その場合ロードに失敗してブラウザが白画面でスタートしてしまう（手動でログインページを開けば続行は可能だが）
	// 応急処置として失敗したとき後で再試行するようにしてみる
	private string? NavigateCache { get; set; }

	private void SetCookie()
	{
		string cookieScript = $$"""
			try
			{
				document.cookie="ckcy=1;expires={{DateTime.Now.AddYears(6):ddd, dd MMM yyyy HH:mm:ss 'GMT'}};path=/netgame;domain=.dmm.com";
			}
			catch
			{
				console.log("Setting the cookie failed.");
			}
			""";

		CefSharp.ExecuteScriptAsync(cookieScript);
	}

	protected override void ApplyZoom()
	{
		if (Configuration is null) return;
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		double zoomRate = Configuration.ZoomRate;
		bool fit = Configuration.ZoomFit && StyleSheetApplied;

		double zoomFactor;

		if (fit)
		{
			double rateX = ActualWidth * DpiScale.DpiScaleX / (KanColleSize.Width * TextScaleFactor);
			double rateY = ActualHeight * DpiScale.DpiScaleY / (KanColleSize.Height * TextScaleFactor);

			zoomFactor = Math.Min(rateX, rateY);
		}
		else
		{
			zoomFactor = Math.Clamp(zoomRate, 0.1, 10);
		}

		// DpiScaleX and DpiScaleY should always be the same, so it doesn't matter which one you use
		CefSharp.SetZoomLevel(Math.Log(zoomFactor / DpiScale.DpiScaleX, 1.2));

		if (StyleSheetApplied)
		{
			int newWidth = (int)(KanColleSize.Width * TextScaleFactor * zoomFactor);
			int newHeight = (int)(KanColleSize.Height * TextScaleFactor * zoomFactor);

			CefSharp.Width = newWidth;
			CefSharp.Height = newHeight;
		}

		CurrentZoom = fit switch
		{
			true => FormBrowser.Other_Zoom_Current_Fit,
			_ => FormBrowser.Other_Zoom_Current + $" {zoomRate:p1}",
		};
	}

	protected override void ApplyStyleSheet()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		try
		{
			IFrame? mainframe = GetMainFrame();
			IFrame? gameframe = GetGameFrame();

			if (mainframe == null || gameframe == null) return;

			if (!StyleSheetApplied)
			{
				mainframe.EvaluateScriptAsync(string.Format(Resources.RestoreScript, StyleClassId));
				gameframe.EvaluateScriptAsync(string.Format(Resources.RestoreScript, StyleClassId));
			}
			else
			{
				mainframe.EvaluateScriptAsync(string.Format(Resources.PageScript, StyleClassId));
				gameframe.EvaluateScriptAsync(string.Format(Resources.FrameScript, StyleClassId));
			}
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToApplyStylesheet);
		}
	}

	private IFrame? GetMainFrame()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return null;

		IBrowser browser = CefSharp.GetBrowser();
		IFrame frame = browser.MainFrame;

		return (frame.Url.Contains(@"http://www.dmm.com/netgame/social/")) switch
		{
			true => frame,
			_ => null,
		};
	}

	private IFrame? GetGameFrame()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return null;

		IBrowser browser = CefSharp.GetBrowser();
		IEnumerable<IFrame> frames = browser
			.GetFrameIdentifiers()
			.Select(id => browser.GetFrameByIdentifier(id));

		return frames.FirstOrDefault(f => f.Url.Contains(@"http://osapi.dmm.com/gadgets/"));
	}

	private IFrame? GetKanColleFrame()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return null;

		IBrowser browser = CefSharp.GetBrowser();
		IEnumerable<IFrame> frames = browser
			.GetFrameIdentifiers()
			.Select(id => browser.GetFrameByIdentifier(id));

		return frames.FirstOrDefault(f => f.Url.Contains(@"/kcs2/index.php"));
	}

	protected override void DestroyDMMreloadDialog()
	{
		if (Configuration is null) return;
		if (!Configuration.IsDMMreloadDialogDestroyable) return;
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		try
		{
			GetMainFrame()?.EvaluateScriptAsync(Resources.DMMScript);
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToHideDmmRefreshDialog);
		}
	}

	protected override void TryGetVolumeManager()
	{
		VolumeManager = VolumeManager.CreateInstanceByProcessName("CefSharp.BrowserSubprocess");

		if (VolumeManager is not null)
		{
			RealVolume = (int)(VolumeManager.Volume * 100);
			VolumeProcessInitialized = true;
		}
	}

	protected override void SetVolumeState()
	{
		if (Configuration is null) return;

		bool mute;
		float volume;

		try
		{
			if (VolumeManager == null)
			{
				TryGetVolumeManager();
			}

			mute = VolumeManager?.IsMute ?? false;
			volume = (VolumeManager?.Volume ?? 1) * 100;
		}
		catch (Exception)
		{
			// 音量データ取得不能時
			VolumeManager = null;
			mute = false;
			volume = 100;
		}

		RealVolume = (int)volume;
		Configuration.Volume = volume;
		Configuration.IsMute = mute;
		IsMuted = mute;
		ConfigurationUpdated();
	}

	public override void Navigate(string url)
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		CefSharp.LoadUrl(url);
		// 大方ロードできないのであとで再試行する
		NavigateCache = url;
	}

	protected override void RefreshBrowser() => RefreshBrowser(false);

	protected override void RefreshBrowser(bool ignoreCache)
	{
		CefSharp?.Reload(ignoreCache);
	}

	protected override void Exit()
	{
		App.Current.Dispatcher.Invoke(() =>
		{
			HeartbeatTimer.Stop();
			Task.Run(async () => await BrowserHost.DisposeAsync()).Wait();
			Cef.Shutdown();
			App.Current.Shutdown();
		});
	}

	protected override async void Screenshot()
	{
		int savemode = Configuration.ScreenShotSaveMode;
		int format = Configuration.ScreenShotFormat;
		string folderPath = Configuration.ScreenShotPath;
		bool is32bpp = format != 1 && Configuration.AvoidTwitterDeterioration;

		System.Drawing.Bitmap? image = null;
		try
		{
			image = await TakeScreenShot();


			if (image == null) return;

			if (is32bpp)
			{
				if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
				{
					var imgalt = new System.Drawing.Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					using (var g = System.Drawing.Graphics.FromImage(imgalt))
					{
						g.DrawImage(image, new System.Drawing.Rectangle(0, 0, imgalt.Width, imgalt.Height));
					}

					image.Dispose();
					image = imgalt;
				}

				// 不透明ピクセルのみだと jpeg 化されてしまうため、1px だけわずかに透明にする
				System.Drawing.Color temp = image.GetPixel(image.Width - 1, image.Height - 1);
				image.SetPixel(image.Width - 1, image.Height - 1, System.Drawing.Color.FromArgb(252, temp.R, temp.G, temp.B));
			}
			else
			{
				if (image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb)
				{
					var imgalt = new System.Drawing.Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
					using (var g = System.Drawing.Graphics.FromImage(imgalt))
					{
						g.DrawImage(image, new System.Drawing.Rectangle(0, 0, imgalt.Width, imgalt.Height));
					}

					image.Dispose();
					image = imgalt;
				}
			}

			App.Current.Dispatcher.Invoke(() => LastScreenshot = image.ToBitmapSource());

			// to file
			if ((savemode & 1) != 0)
			{
				try
				{
					Directory.CreateDirectory(folderPath);

					string ext;
					System.Drawing.Imaging.ImageFormat imgFormat;

					switch (format)
					{
						case 1:
							ext = "jpg";
							imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
							break;
						case 2:
						default:
							ext = "png";
							imgFormat = System.Drawing.Imaging.ImageFormat.Png;
							break;
					}

					string path = $"{folderPath}\\{DateTime.Now:yyyyMMdd_HHmmssff}.{ext}";
					image.Save(path, imgFormat);
					LastScreenShotPath = Path.GetFullPath(path);

					AddLog(2, string.Format(FormBrowser.ScreenshotSavedTo, path));
				}
				catch (Exception ex)
				{
					SendErrorReport(ex.ToString(), FormBrowser.FailedToSaveScreenshot);
				}
			}


			// to clipboard
			if ((savemode & 2) != 0)
			{
				try
				{
					App.Current.Dispatcher.Invoke(() =>
					{
						Clipboard.SetImage(image.ToBitmapSource());

						if ((savemode & 3) != 3)
							AddLog(2, FormBrowser.ScreenshotCopiedToClipboard);
					});
				}
				catch (Exception ex)
				{
					SendErrorReport(ex.ToString(), FormBrowser.FailedToCopyScreenshotToClipboard);
				}
			}
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.ScreenshotError);
		}
		finally
		{
			image?.Dispose();
		}
	}

	/// <summary>
	/// スクリーンショットを撮影します。
	/// </summary>
	private async Task<System.Drawing.Bitmap?> TakeScreenShot()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return null;

		IFrame? kancolleFrame = GetKanColleFrame();

		if (kancolleFrame is null)
		{
			AddLog(3, FormBrowser.KancolleNotLoadedCannotTakeScreenshot);
			System.Media.SystemSounds.Beep.Play();
			return null;
		}


		Task<ScreenShotPacket> InternalTakeScreenShot()
		{
			ScreenShotPacket request = new();

			string script = $@"
(async function()
{{
	await CefSharp.BindObjectAsync('{request.ID}');
	let canvas = document.querySelector('canvas');
	requestAnimationFrame(() =>
	{{
		let dataurl = canvas.toDataURL('image/png');
		{request.ID}.complete(dataurl);
	}});
}})();
";

			CefSharp.JavascriptObjectRepository.Register(request.ID, request);
			kancolleFrame.ExecuteJavaScriptAsync(script);

			return request.TaskSource.Task;
		}

		ScreenShotPacket result = await InternalTakeScreenShot();

		// ごみ掃除
		CefSharp.JavascriptObjectRepository.UnRegister(result.ID);
		kancolleFrame.ExecuteJavaScriptAsync($@"delete {result.ID}");

		return result.GetImage();
	}

	protected override void Mute()
	{
		if (VolumeManager == null)
		{
			TryGetVolumeManager();
		}

		try
		{
			if (VolumeManager is null)
			{
				System.Media.SystemSounds.Beep.Play();
			}
			else
			{
				VolumeManager.ToggleMute();
			}
		}
		catch (Exception)
		{

		}

		SetVolumeState();
	}

	protected override void GoTo()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		BrowserHost.RequestNavigation(CefSharp.GetMainFrame()?.Url ?? "");
	}

	protected override void OpenDevtools()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		CefSharp.GetBrowser().ShowDevTools();
	}

	protected override async void ClearCache()
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		if (MessageBox.Show(FormBrowser.ClearCacheMessage, FormBrowser.ClearCacheTitle,
				MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			await CefSharp.ClearCache();
			AddLog(2, FormBrowser.CacheCleared);
		}
	}

	public override void OpenExtraBrowser()
	{
		new ExtraBrowserWindow
		{
			Owner = App.Current.MainWindow,
		}.Show();
	}

	public override void OpenAirControlSimulator(string url)
	{
		new AirControlSimulatorWindow(url, BrowserHost)
		{
			Owner = App.Current.MainWindow,
		}.Show();
	}

	public override void OpenCompassPrediction()
	{
		new CompassPredictionView(CompassPredictionViewModel)
		{
			Owner = App.Current.MainWindow,
		}.Show();
	}

	protected override async Task ApplyCustomBrowserFont(BrowserConfiguration configuration)
	{
		if (CefSharp is not { IsBrowserInitialized: true }) return;

		try
		{
			await RemoveCustomBrowserFont();

			if (!configuration.UseCustomBrowserFont) return;

			await AddCustomBrowserFont(configuration);
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToApplyBrowserFont);
		}
	}

	protected override async Task RemoveCustomBrowserFont()
	{
		if (GetKanColleFrame() is not IFrame kancolleFrame) return;

		string removeStyleScript = $$"""
			try
			{
				const style = document.getElementById("{{BrowserFontStyleId}}");

				if (style != null)
				{
					style.parentElement.removeChild(style);
				}
			}
			catch
			{
			}
			""";

		_ = await kancolleFrame.EvaluateScriptAsync(removeStyleScript);
	}

	protected override async Task AddCustomBrowserFont(BrowserConfiguration configuration)
	{
		if (GetKanColleFrame() is not IFrame kancolleFrame) return;

		string font = configuration.MatchMainFont switch
		{
			true => configuration.MainFont,
			_ => configuration.BrowserFont ?? configuration.MainFont,
		};

		string fontData =
			$$"""@font-face { font-family: "font_j"; src: local("{{font}}"); font-weight: normal; }""" +
			"""\n""" +
			$$"""@font-face { font-family: "font_j"; src: local("{{font}}"); font-weight: bold; }""";

		string addStyleScript = $$"""
			try
			{
				const style = document.createElement("style");
				style.type = "text/css";
				style.id = "{{BrowserFontStyleId}}";
				style.innerHTML = '{{fontData}}';

				document.getElementsByTagName("head")[0].appendChild(style);
			}
			catch
			{
			}
			""";

		JavascriptResponse? response = await kancolleFrame.EvaluateScriptAsync(addStyleScript);

		if (!response.Success)
		{
			AddLog(2, FormBrowser.FailedToApplyBrowserFont);
		}
	}
}
