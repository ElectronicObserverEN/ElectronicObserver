using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Browser.WebView2Browser.AirControlSimulator;
using Browser.WebView2Browser.CompassPrediction;
using Browser.WebView2Browser.ExtraBrowser;
using BrowserLibCore;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using Microsoft.Web.WebView2.Wpf;

namespace Browser.WebView2Browser;

public class WebView2ViewModel : BrowserViewModel
{
	public static CoreWebView2Environment? Environment { get; private set; }
	private DevToolsProtocolHelper? DevToolsHelper { get; set; }

	public override object? Browser => WebView2;
	public WebView2? WebView2 { get; set; }
	private static string BrowserCachePath => BrowserConstants.WebView2CachePath;
	private bool IsRefreshing { get; set; }
	private bool IsNavigating { get; set; }

	private CoreWebView2Frame? gameframe { get; set; }
	private CoreWebView2Frame? kancolleframe { get; set; }

	public WebView2ViewModel(string host, int port, string culture) : base(host, port, culture)
	{
	}

	public override async void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (sender is not Window window) return;

		var handle = new WindowInteropHelper(window).Handle;
		SetWindowLong(handle, GWL_STYLE, WS_CHILD);

		await Task.Run(ConfigurationChanged);

		// ウィンドウの親子設定＆ホストプロセスから接続してもらう
		await BrowserHost.ConnectToBrowser((long)handle);

		// 親ウィンドウが生きているか確認
		HeartbeatTimer.Elapsed += async (sender2, e2) =>
		{
			try
			{
				var alive = await BrowserHost.IsServerAlive();
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

		await WebView2Check();

		WebView2 = new WebView2();
		InitializeAsync();
	}

	private async Task WebView2Check()
	{
		string? version = null;

		try
		{
			version = CoreWebView2Environment.GetAvailableBrowserVersionString();
		}
		catch
		{
			// no webview2 version available
		}

		if (version is not null)
		{
			if (CoreWebView2Environment.CompareBrowserVersions(version, "107.0.1418.22") < 0)
			{
				AddLog(2, FormBrowser.OlderBrowserVersionDetected);

				await InstallWebView2();
			}

			if (Configuration.UseVulkanWorkaround)
			{
				await RenameVulkanFiles(version);
			}

			return;
		}

		AddLog(2, FormBrowser.WebView2NotFound);

		await InstallWebView2();
	}

	private async Task InstallWebView2()
	{
		try
		{
			const string webView2InstallerName = "MicrosoftEdgeWebView2Setup.exe";
			// this is scoped so the installer file closes before we attempt to run it 
			{
				File.Delete(webView2InstallerName);

				HttpClient client = new();

				await using var stream = await client.GetStreamAsync("https://go.microsoft.com/fwlink/p/?LinkId=2124703");
				await using FileStream file = new(webView2InstallerName, FileMode.CreateNew);

				await stream.CopyToAsync(file);
			}

			AddLog(2, FormBrowser.WebView2DownloadComplete);

			ProcessStartInfo psi = new(webView2InstallerName, "/install");
			using var process = Process.Start(psi);
			await process.WaitForExitAsync();

			File.Delete(webView2InstallerName);

			AddLog(2, FormBrowser.WebView2InstallationComplete);
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.Message, FormBrowser.InstallationFailed);
		}
	}

	private async Task RenameVulkanFiles(string version)
	{
		List<string> basePaths = new()
		{
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles),
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86),
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
		};

		List<string> vulkanFiles = new()
		{
			"vk_swiftshader.dll",
			"vk_swiftshader_icd.json",
			"vulkan-1.dll",
		};

		try
		{
			foreach (var basePath in basePaths)
			{
				if (string.IsNullOrEmpty(basePath)) continue;

				try
				{
					foreach (var vulkanFile in vulkanFiles)
					{
						var path = Path.Combine(basePath, @"Microsoft\EdgeWebView\Application", version, vulkanFile);

						if (!File.Exists(path)) continue;

						File.Move(path, Path.ChangeExtension(path, "eoworkaround"));
					}
				}
				catch (UnauthorizedAccessException)
				{
					AddLog(2, Resources.MissingPermissionsToRenameVulkanFiles);
				}
			}
		}
		catch
		{
			// it's probably safe to just ignore all other exceptions
		}
	}

	/// <summary>
	/// ブラウザを初期化します。
	/// 最初の呼び出しのみ有効です。二回目以降は何もしません。
	/// </summary>
	protected override async void InitializeAsync()
	{
		if (WebView2 is null) return;
		if (WebView2.CoreWebView2 != null) return;
		if (ProxySettings == null) return;

		List<string> browserArgs = new()
		{
			$"--proxy-server=\"{ProxySettings}\"",
			"--disable-features=\"HardwareMediaKeyHandling\"",
			"--lang=\"ja\"",
			"--log-file=\"BrowserLog.log\" ",
		};

		if (Configuration.ForceColorProfile)
		{
			browserArgs.Add("--force-color-profile=\"sRGB\"");
		}

		if (!Configuration.HardwareAccelerationEnabled)
		{
			browserArgs.Add("--disable-gpu");
		}

		if (Configuration.SavesBrowserLog)
		{
			browserArgs.Add("--log-level=2");
		}

		var corewebviewoptions = new CoreWebView2EnvironmentOptions
		{
			AdditionalBrowserArguments = string.Join(" ", browserArgs)
		};
		var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder: BrowserCachePath, options: corewebviewoptions);

		Environment = env;

		await WebView2.EnsureCoreWebView2Async(env);
		DevToolsHelper = WebView2.CoreWebView2.GetDevToolsProtocolHelper();
		WebView2.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
		WebView2.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
		WebView2.CoreWebView2.Settings.IsStatusBarEnabled = false;
		WebView2.Source = new Uri("about:blank");
		WebView2.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
		WebView2.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.Script);
		WebView2.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.Media);
		WebView2.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
		WebView2.CoreWebView2.FrameCreated += CoreWebView2_FrameCreated;
		WebView2.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarted;
		WebView2.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;
		WebView2.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
		WebView2.CoreWebView2.IsMuted = Configuration.IsMute;
		WebView2.CoreWebView2.IsDocumentPlayingAudioChanged += OnDocumentPlayingAudioChanged;
		WebView2.PreviewKeyDown += Browser_PreviewKeyDown;
		SetCookie();
		WebView2.CoreWebView2.Navigate(KanColleUrl);
	}

	private void OnDocumentPlayingAudioChanged(object? sender, object o)
	{
		/*

		This fires every time any audio starts playing in WebView2
		It fires even if WebView2 is muted - the sound process won't exist in that case
		VolumeProcessInitialized gets used to only run this logic the first time

		SetVolumeState() - reads volume manager state and saves it
		to BrowserViewModel and config
		
		Mute() - toggles volume manager mute and calls SetVolumeState()

		VolumeChanged() - adjusts volume, removes mute and saves
		it to config

		todo: describe how exactly the whole process flows

		*/
		if (VolumeProcessInitialized) return;

		VolumeProcessInitialized = true;

		TryGetVolumeManager();

		// if the browser is muted, it shouldn't spawn a sound process
		if (VolumeManager is null) return;

		RealVolume = (int)(VolumeManager.Volume * 100);
		// for some reason WebView2 doesn't remember the old mute state
		// so we set the manager mute state based on the config
		VolumeManager.IsMute = Configuration.IsMute;
		WebView2!.CoreWebView2.IsMuted = false;
	}

	private void CoreWebView2_ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs e)
	{
		if (WebView2.CoreWebView2 == null) return;
		if (gameframe != null)
		{
			e.Handled = !Configuration.IsBrowserContextMenuEnabled;
		}
	}

	private void Browser_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (WebView2.CoreWebView2 == null) return;
		switch (e.Key)
		{
			case Key.F5:
				RefreshCommand.Execute(null);
				break;
			case Key.F12:
				OpenDevtoolsCommand.Execute(null);
				break;
			case Key.F2:
				ScreenshotCommand.Execute(null);
				break;
			case Key.F7:
				MuteCommand.Execute(null);
				break;
		}
		if (e.Key == Key.F5 && (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl))
		{
			HardRefreshCommand.Execute(null);
		}
	}

	private void CoreWebView2_ProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
	{
		switch (e.ProcessFailedKind)
		{
			case CoreWebView2ProcessFailedKind.BrowserProcessExited:
				AddLog(2, "Browser Process Exited");
				break;
			case CoreWebView2ProcessFailedKind.GpuProcessExited:
				AddLog(2, "GPU Process Exited");
				break;
			case CoreWebView2ProcessFailedKind.RenderProcessUnresponsive:
				AddLog(2, "Render Process Unresponsive");
				break;
			case CoreWebView2ProcessFailedKind.UnknownProcessExited:
				AddLog(2, "Unknown Process Exited");
				break;
			default:
				AddLog(2, "Proccess Failed");
				break;
		}
	}


	private void CoreWebView2_NavigationStarted(object? sender, CoreWebView2NavigationStartingEventArgs e)
	{
		if (WebView2.CoreWebView2 == null) return;
		if (IsNavigating) return;
		if (e.Uri.Contains(@"/rt.gsspat.jp/"))
		{
			e.Cancel = true;
		}
		if (new Uri(e.Uri).Host.Contains("accounts.google.com"))
		{
			var settings = WebView2.CoreWebView2.Settings;
			settings.UserAgent = "Chrome";
		}
		if (gameframe != null && !IsRefreshing)
		{
			e.Cancel = true;
		}
	}

	private void CoreWebView2_FrameCreated(object? sender, CoreWebView2FrameCreatedEventArgs e)
	{
		if (e.Frame.Name.Contains(@"game_frame"))
		{
			gameframe = e.Frame;
			IsRefreshing = false;
			IsNavigating = false;
		}
		if (e.Frame.Name.Contains(@"htmlWrap"))
		{
			kancolleframe = e.Frame;
		}
	}

	private void CoreWebView2_WebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
	{
		if (e.Request.Uri.Contains(@"gadget_html5") && Configuration?.UseGadgetRedirect is true)
		{
			e.Request.Uri = e.Request.Uri.Replace("http://203.104.209.7/gadget_html5/", Configuration.GadgetBypassServer.GetReplaceUrl(Configuration.GadgetBypassServerCustom));
		}

		if (e.Request.Uri.Contains("/kcs2/resources/bgm/"))
		{
			//not working in webview2
			//e.Request.Headers.RemoveHeader("Range");
		}
	}

	private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		if (e.IsSuccess)
		{
			ApplyStyleSheet();
			ApplyZoom();
			SetCookie();
			DestroyDMMreloadDialog();
		}
	}

	protected override void Exit()
	{
		// if (!BrowserHost.Closed)
		{
			// BrowserHost.Close();
			App.Current.Dispatcher.Invoke(() =>
			{
				HeartbeatTimer.Stop();
				Task.Run(async () => await BrowserHost.DisposeAsync()).Wait();
				WebView2?.Dispose();
				App.Current.Shutdown();
			});
		}
	}

	protected override void ApplyStyleSheet()
	{
		if (WebView2 is not { IsInitialized: true }) return;

		try
		{
			if (gameframe is null) return;

			if (!StyleSheetApplied)
			{
				WebView2.ExecuteScriptAsync(string.Format(Resources.RestoreScript, StyleClassId));
				gameframe.ExecuteScriptAsync(string.Format(Resources.RestoreScript, StyleClassId));
			}
			else
			{
				WebView2.ExecuteScriptAsync(string.Format(Resources.PageScript, StyleClassId));
				gameframe.ExecuteScriptAsync(string.Format(Resources.FrameScript, StyleClassId));
			}
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToApplyStylesheet);
		}
	}

	/// <summary>
	/// DMMによるページ更新ダイアログを非表示にします。
	/// </summary>
	protected override void DestroyDMMreloadDialog()
	{
		if (WebView2 is not { IsInitialized: true }) return;
		if (!Configuration.IsDMMreloadDialogDestroyable) return;

		try
		{
			WebView2?.CoreWebView2.ExecuteScriptAsync(Resources.DMMScript);
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToHideDmmRefreshDialog);
		}
	}

	/// <summary>
	/// 指定した URL のページを開きます。
	/// </summary>
	public override void Navigate(string url)
	{
		if (WebView2 is null) return;

		WebView2.CoreWebView2.Navigate(url);
		IsNavigating = true;
	}

	/// <summary>
	/// ブラウザを再読み込みします。
	/// </summary>
	protected override void RefreshBrowser() => RefreshBrowser(false);

	/// <summary>
	/// ブラウザを再読み込みします。
	/// </summary>
	/// <param name="ignoreCache">キャッシュを無視するか。</param>
	protected override void RefreshBrowser(bool ignoreCache)
	{
		if (WebView2 is null) return;

		if (ignoreCache)
		{
			DevToolsHelper.Page.ReloadAsync(true);
		}
		else
		{
			WebView2.CoreWebView2.Reload();
		}

		IsRefreshing = true;
	}

	/// <summary>
	/// ズームを適用します。
	/// </summary>
	protected override void ApplyZoom()
	{
		if (WebView2 is not { IsInitialized: true }) return;
		var zoomRate = Configuration.ZoomRate;
		var fit = Configuration.ZoomFit && StyleSheetApplied;

		double zoomFactor;

		if (fit)
		{
			var rateX = ActualWidth / KanColleSize.Width;
			var rateY = ActualHeight / KanColleSize.Height;

			zoomFactor = Math.Min(rateX, rateY);
		}
		else
		{
			zoomFactor = Math.Clamp(zoomRate, 0.1, 10);
		}

		// DpiScaleX and DpiScaleY should always be the same so it doesn't matter which one you use
		WebView2.ZoomFactor = zoomFactor;

		if (StyleSheetApplied && gameframe != null)
		{
			var newWidth = (int)(KanColleSize.Width * zoomFactor);
			var newHeight = (int)(KanColleSize.Height * zoomFactor);

			WebView2.Width = newWidth;
			WebView2.Height = newHeight;
		}
		else
		{
			WebView2.Width = double.NaN;
			WebView2.Height = double.NaN;
		}

		CurrentZoom = fit switch
		{
			true => FormBrowser.Other_Zoom_Current_Fit,
			_ => FormBrowser.Other_Zoom_Current + $" {zoomRate:p1}",
		};
	}

	private void SetCookie()
	{
		var gamesCookies = WebView2.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "games.dmm.com", "/");
		gamesCookies.Expires = DateTime.Now.AddYears(6);
		gamesCookies.IsSecure = true;
		WebView2.CoreWebView2.CookieManager.AddOrUpdateCookie(gamesCookies);
		var dmmCookie = WebView2.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", ".dmm.com", "/");
		dmmCookie.Expires = DateTime.Now.AddYears(6);
		dmmCookie.IsSecure = true;
		WebView2.CoreWebView2.CookieManager.AddOrUpdateCookie(dmmCookie);
		var acccountsCookie = WebView2.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "accounts.dmm.com", "/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		acccountsCookie.IsSecure = true;
		WebView2.CoreWebView2.CookieManager.AddOrUpdateCookie(acccountsCookie);
		var osapiCookie = WebView2.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "osapi.dmm.com", "/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		acccountsCookie.IsSecure = true;
		WebView2.CoreWebView2.CookieManager.AddOrUpdateCookie(osapiCookie);
		var gameserverCookie = WebView2.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "203.104.209.7", "/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		WebView2.CoreWebView2.CookieManager.AddOrUpdateCookie(gameserverCookie);
		var gamepathCookie = WebView2.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "www.dmm.com", "/netgame/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		WebView2.CoreWebView2.CookieManager.AddOrUpdateCookie(gamepathCookie);
	}

	protected override void TryGetVolumeManager()
	{
		VolumeManager = VolumeManager.CreateInstanceByProcessName("msedgewebview2", ProxySettings);

		if (VolumeManager is not null && WebView2 is not null)
		{
			// we're calling this from an async task so the dispatcher is needed
			App.Current.Dispatcher.Invoke(() =>
			{
				VolumeManager.IsMute = WebView2.CoreWebView2.IsMuted;
				RealVolume = (int)(VolumeManager.Volume * 100);
			});
		}
	}

	protected override void SetVolumeState()
	{
		if (!VolumeProcessInitialized) return;
		if (WebView2 is null) return;

		try
		{
			if (VolumeManager is null)
			{
				TryGetVolumeManager();
			}

			if (VolumeManager is not null)
			{
				IsMuted = VolumeManager.IsMute;
				RealVolume = (int)(VolumeManager.Volume * 100);
				WebView2.CoreWebView2.IsMuted = VolumeManager.IsMute;
			}
			else
			{
				IsMuted = WebView2.CoreWebView2.IsMuted;
			}
		}
		catch (Exception)
		{
			// 音量データ取得不能時
			VolumeManager = null;
			IsMuted = false;
			RealVolume = 100;
		}

		Configuration.IsMute = IsMuted;
		ConfigurationUpdated();
	}

	/// <inheritdoc />
	protected override async void Screenshot()
	{
		var savemode = Configuration.ScreenShotSaveMode;
		var format = Configuration.ScreenShotFormat;
		var folderPath = Configuration.ScreenShotPath;
		var is32bpp = format != 1 && Configuration.AvoidTwitterDeterioration;

		// to file
		try
		{
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			ImageFormat? imageFormat;
			CoreWebView2CapturePreviewImageFormat browserImageFormat;
			string ext;
			switch (format)
			{
				case 1:
					imageFormat = ImageFormat.Jpeg;
					browserImageFormat = CoreWebView2CapturePreviewImageFormat.Jpeg;
					ext = "jpg";
					break;
				case 2:
				default:
					imageFormat = ImageFormat.Png;
					browserImageFormat = CoreWebView2CapturePreviewImageFormat.Png;
					ext = "png";
					break;
			}

			var path = $"{folderPath}\\{DateTime.Now:yyyyMMdd_HHmmssff}.{ext}";

			await using MemoryStream memoryStream = new();
			await WebView2.CoreWebView2.CapturePreviewAsync(browserImageFormat, memoryStream).ConfigureAwait(false);

			var image = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(memoryStream, true);

			await App.Current.Dispatcher.BeginInvoke(() => LastScreenshot = image.ToBitmapSource());

			if (savemode is 1 or 3)
			{
				image.Save(path, imageFormat);
				AddLog(2, string.Format(FormBrowser.ScreenshotSavedTo, path));
				LastScreenShotPath = Path.GetFullPath(path);
			}

			if ((savemode & 2) != 0)
			{
				App.Current.Dispatcher.Invoke(() => Clipboard.SetImage(image.ToBitmapSource()));
				AddLog(2, string.Format(FormBrowser.ScreenshotCopiedToClipboard));
			}
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToSaveScreenshot);
		}
	}

	protected override void Mute()
	{
		if (WebView2 is null) return;

		if (VolumeManager is null)
		{
			// VolumeManager being null can mean the browser is muted
			// so the sound process doesn't get created
			// make sure the browser isn't muted
			if (WebView2.CoreWebView2.IsMuted)
			{
				WebView2.CoreWebView2.IsMuted = false;
				SetVolumeState();

				// because the volume manager gets created some time after you un-mute the browser
				// and the browser doesn't have an event indicating when that will happen
				// just try to grab the volume manager after un-muting, with a bit of delay
				Task.Run(async () =>
				{
					var maxRetryCount = 100;
					var retryCount = 0;

					while (VolumeManager is null && retryCount < maxRetryCount)
					{
						await Task.Delay(100);
						TryGetVolumeManager();
						retryCount++;
					}
				});
				return;
			}

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
			System.Media.SystemSounds.Beep.Play();
		}

		SetVolumeState();
	}

	protected override void GoTo()
	{
		if (WebView2 is null) return;

		BrowserHost.RequestNavigation(WebView2.CoreWebView2?.Source ?? "");
	}

	protected override void OpenDevtools()
	{
		if (WebView2 is not { IsInitialized: true }) return;

		WebView2.CoreWebView2?.OpenDevToolsWindow();
	}

	protected override async void ClearCache()
	{
		if (MessageBox.Show(FormBrowser.ClearCacheMessage, FormBrowser.ClearCacheTitle,
			MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			await DevToolsHelper.Network.ClearBrowserCacheAsync();
			AddLog(2, FormBrowser.CacheCleared);
		}
	}

	public override void OpenExtraBrowser()
	{
		new ExtraBrowserWindow().Show();
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
		new CompassPredictionView(BrowserHost)
		{
			Owner = App.Current.MainWindow,
		}.Show();
	}
}
