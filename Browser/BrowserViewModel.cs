using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Browser.AirControlSimulator;
using Browser.ExtraBrowser;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Grpc.Core;
using MagicOnion.Client;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using Microsoft.Web.WebView2.Wpf;
using ModernWpf;

namespace Browser;

public class ImageProvider
{
	public ImageSource? Screenshot { get; }
	public ImageSource? Zoom { get; }
	public ImageSource? ZoomIn { get; }
	public ImageSource? ZoomOut { get; }
	public ImageSource? Unmute { get; }
	public ImageSource? Mute { get; }
	public ImageSource? Refresh { get; }
	public ImageSource? Navigate { get; }
	public ImageSource? Other { get; }

	public ImageProvider(byte[][] images)
	{
		Screenshot = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[0]);
		Zoom = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[1]);
		ZoomIn = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[2]);
		ZoomOut = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[3]);
		Unmute = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[4]);
		Mute = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[5]);
		Refresh = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[6]);
		Navigate = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[7]);
		Other = (ImageSource?)new ImageSourceConverter().ConvertFrom(images[8]);
	}
}

public partial class BrowserViewModel : ObservableObject, BrowserLibCore.IBrowser
{
	public FormBrowserTranslationViewModel FormBrowser { get; }
	private BrowserConfiguration Configuration { get; set; }
	public static CoreWebView2Environment? Environment { get; private set; }
	public DevToolsProtocolHelper DevToolsHelper { get; set; }
	private string Host { get; }
	private int Port { get; }
	private string Culture { get; }
	public static BrowserLibCore.IBrowserHost BrowserHost { get; private set; }
	public string? ProxySettings { get; set; }

	public ImageProvider? Icons { get; set; }
	public WebView2? Browser { get; set; }
	public bool IsRefreshing { get; set; }
	public bool IsNavigating { get; set; }
	private System.Drawing.Size KanColleSize { get; } = new(1200, 720);
	private string KanColleUrl => "http://www.dmm.com/netgame/social/-/gadgets/=/app_id=854854/";
	private string BrowserCachePath => BrowserConstants.CachePath;

	private string StyleClassID { get; } = Guid.NewGuid().ToString().Substring(0, 8);

	// user setting for stylesheet
	public bool StyleSheetEnabled { get; set; }
	// todo: flag to temporarily disable the stylesheet
	// seems to be used for when you navigate to something other than kancolle
	public bool ShouldStyleSheetApply { get; set; } = true;
	public bool StyleSheetApplied => StyleSheetEnabled && ShouldStyleSheetApply;

	public Dock ToolMenuDock { get; set; } = Dock.Top;
	public Orientation ToolMenuOrientation => ToolMenuDock switch
	{
		Dock.Left or Dock.Right => Orientation.Vertical,
		_ => Orientation.Horizontal
	};
	public Visibility ToolMenuVisibility { get; set; } = Visibility.Visible;

	public WindowsFormsHost BrowserWrapper { get; } = new();

	public double ActualWidth { get; set; }
	public double ActualHeight { get; set; }

	private System.Timers.Timer HeartbeatTimer { get; } = new();

	private bool IsKanColleLoaded { get; set; }

	public string? LastScreenShotPath { get; set; }
	public BitmapSource? LastScreenshot { get; set; }

	private VolumeManager? VolumeManager { get; set; }
	public int RealVolume { get; set; }
	public bool IsMuted { get; set; }

	public CoreWebView2Frame? gameframe { get; private set; }
	public CoreWebView2Frame? kancolleframe { get; private set; }

	public bool ZoomFit { get; set; }
	public string CurrentZoom { get; set; } = "";

	/// <summary>
	/// </summary>
	/// <param name="serverUri">ホストプロセスとの通信用URL</param>
	public BrowserViewModel(string host, int port, string culture)
	{
		// System.Diagnostics.Debugger.Launch();

		FormBrowser = Ioc.Default.GetService<FormBrowserTranslationViewModel>()!;

		Host = host;
		Port = port;
		Culture = culture;
		CultureInfo c = new(culture);

		Thread.CurrentThread.CurrentCulture = c;
		Thread.CurrentThread.CurrentUICulture = c;

		PropertyChanged += (sender, args) =>
		{
			if (Configuration is null) return;
			if (args.PropertyName is not nameof(ZoomFit)) return;

			Configuration.ZoomFit = ZoomFit;
			ApplyZoom();
			ConfigurationUpdated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (Configuration is null) return;
			if (args.PropertyName is not nameof(StyleSheetEnabled)) return;

			Configuration.AppliesStyleSheet = StyleSheetEnabled;

			ApplyStyleSheet();
			ApplyZoom();
			ConfigurationUpdated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(ActualWidth) or nameof(ActualHeight))) return;

			ApplyZoom();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(RealVolume)) return;

			ToolMenu_Other_Volume_ValueChanged();
		};
	}

	public async void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (sender is not System.Windows.Window window) return;

		IntPtr handle = new WindowInteropHelper(window).Handle;
		SetWindowLong(handle, GWL_STYLE, WS_CHILD);

		// ホストプロセスに接続
		Channel grpChannel = new(Host, Port, ChannelCredentials.Insecure);
		BrowserHost = StreamingHubClient.Connect<BrowserLibCore.IBrowserHost, BrowserLibCore.IBrowser>(grpChannel, this);

		await Task.Run(ConfigurationChanged);

		// ウィンドウの親子設定＆ホストプロセスから接続してもらう
		Task.Run(async () => await BrowserHost.ConnectToBrowser((long)handle)).Wait();

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

		await WebView2Check();

		Browser = new WebView2();
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
			if (CoreWebView2Environment.CompareBrowserVersions(version, "97.0.1072.76") < 0)
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

				await using Stream stream = await client.GetStreamAsync("https://go.microsoft.com/fwlink/p/?LinkId=2124703");
				await using FileStream file = new(webView2InstallerName, FileMode.CreateNew);

				await stream.CopyToAsync(file);
			}

			AddLog(2, FormBrowser.WebView2DownloadComplete);

			ProcessStartInfo psi = new(webView2InstallerName, "/install");
			using Process? process = Process.Start(psi);
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
			foreach (string basePath in basePaths)
			{
				if (string.IsNullOrEmpty(basePath)) continue;

				try
				{
					foreach (string vulkanFile in vulkanFiles)
					{
						string path = Path.Combine(basePath, @"Microsoft\EdgeWebView\Application", version, vulkanFile);

						if (!File.Exists(path)) continue;

						File.Move(path, Path.ChangeExtension(path, "eoworkaround"));
					}
				}
				catch (UnauthorizedAccessException)
				{
					AddLog(2, Properties.Resources.MissingPermissionsToRenameVulkanFiles);
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
	public async void InitializeAsync()
	{
		if (Browser is null) return;
		if (Browser.CoreWebView2 != null) return;
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

		await Browser.EnsureCoreWebView2Async(env);
		DevToolsHelper = Browser.CoreWebView2.GetDevToolsProtocolHelper();
		Browser.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
		Browser.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
		Browser.CoreWebView2.Settings.IsStatusBarEnabled = false;
		Browser.Source = new Uri("about:blank");
		Browser.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
		Browser.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.Script);
		Browser.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.Media);
		Browser.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
		Browser.CoreWebView2.FrameCreated += CoreWebView2_FrameCreated;
		Browser.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarted;
		Browser.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;
		Browser.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
		Browser.CoreWebView2.IsMuted = Configuration.IsMute;
		Browser.CoreWebView2.IsDocumentPlayingAudioChanged += OnDocumentPlayingAudioChanged;
		Browser.PreviewKeyDown += Browser_PreviewKeyDown;
		SetCookie();
		Browser.CoreWebView2.Navigate(KanColleUrl);
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

		ToolMenu_Other_Volume_ValueChanged() - adjusts volume, removes mute and saves
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
		Browser!.CoreWebView2.IsMuted = false;
	}

	private bool VolumeProcessInitialized { get; set; }

	private void CoreWebView2_ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs e)
	{
		if (Browser.CoreWebView2 == null) return;
		if (gameframe != null)
		{
			e.Handled = !Configuration.IsBrowserContextMenuEnabled;
		}
	}

	private void Browser_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (Browser.CoreWebView2 == null) return;
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
		if (Browser.CoreWebView2 == null) return;
		if (IsNavigating) return;
		if (e.Uri.Contains(@"/rt.gsspat.jp/"))
		{
			e.Cancel = true;
		}
		if (new Uri(e.Uri).Host.Contains("accounts.google.com"))
		{
			var settings = Browser.CoreWebView2.Settings;
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
		if (e.Request.Uri.Contains(@"gadget_html5") && Configuration.UseGadgetRedirect)
		{
			e.Request.Uri = e.Request.Uri.Replace("http://203.104.209.7/gadget_html5/", "https://kcwiki.github.io/cache/gadget_html5/");
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


	private void Exit()
	{
		// if (!BrowserHost.Closed)
		{
			// BrowserHost.Close();
			App.Current.Dispatcher.Invoke(() =>
			{
				HeartbeatTimer.Stop();
				Task.Run(async () => await BrowserHost.DisposeAsync()).Wait();
				Browser.Dispose();
				App.Current.Shutdown();
			});
		}
	}

	public void CloseBrowser()
	{
		HeartbeatTimer.Stop();
		// リモートコールでClose()呼ぶのばヤバそうなので非同期にしておく
		App.Current.Dispatcher.BeginInvoke((Action)(() => Exit()));
	}

	public async void ConfigurationChanged()
	{
		Configuration = await BrowserHost.Configuration();

		ZoomFit = Configuration.ZoomFit;
		ApplyZoom();
		StyleSheetEnabled = Configuration.AppliesStyleSheet;

		ToolMenuDock = Configuration.ToolMenuDockStyle switch
		{
			1 => Dock.Top,
			2 => Dock.Bottom,
			3 => Dock.Left,
			4 => Dock.Right,
			_ => Dock.Top
		};

		ToolMenuVisibility = Configuration.IsToolMenuVisible switch
		{
			true => Visibility.Visible,
			_ => Visibility.Collapsed
		};

		await App.Current.Dispatcher.BeginInvoke(async () =>
		{
			ThemeManager.Current.ApplicationTheme = (ApplicationTheme)await BrowserHost.GetTheme();
		});

		IsMuted = Configuration.IsMute;

		// SizeAdjuster.BackColor = System.Drawing.Color.FromArgb(unchecked((int)Configuration.BackColor));
		// ToolMenu.BackColor = System.Drawing.Color.FromArgb(unchecked((int)Configuration.BackColor));
		// ToolMenu_Other_ClearCache.Visible = conf.EnableDebugMenu;
	}

	private void ConfigurationUpdated()
	{
		BrowserHost.ConfigurationUpdated(Configuration);
	}

	private void AddLog(int priority, string message)
	{
		BrowserHost.AddLog(priority, message);
	}

	private void SendErrorReport(string exceptionName, string message)
	{
		BrowserHost.SendErrorReport(exceptionName, message);
	}

	public void InitialAPIReceived()
	{
		IsKanColleLoaded = true;

		//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
		ApplyStyleSheet();
		ApplyZoom();
		DestroyDMMreloadDialog();

		//起動直後はまだ音声が鳴っていないのでミュートできないため、この時点で有効化
		SetVolumeState();
	}

	// hack: it makes an infinite loop in the wpf version for some reason
	private int Counter { get; set; }


	private void ApplyStyleSheet()
	{
		if (Browser is not { IsInitialized: true }) return;
		try
		{

			if (gameframe == null)
				return;

			if (!StyleSheetApplied)
			{
				Browser.ExecuteScriptAsync(string.Format(Properties.Resources.RestoreScript, StyleClassID));
				gameframe.ExecuteScriptAsync(string.Format(Properties.Resources.RestoreScript, StyleClassID));
				gameframe.ExecuteScriptAsync("document.body.style.backgroundColor = \"#000000\";");
			}
			else
			{
				Browser.ExecuteScriptAsync(String.Format(Properties.Resources.PageScript, StyleClassID));
				gameframe.ExecuteScriptAsync(String.Format(Properties.Resources.FrameScript, StyleClassID));
				gameframe.ExecuteScriptAsync("document.body.style.backgroundColor = \"#000000\";");
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
	private void DestroyDMMreloadDialog()
	{
		if (Browser is not { IsInitialized: true }) return;
		if (!Configuration.IsDMMreloadDialogDestroyable) return;

		try
		{
			Browser?.CoreWebView2.ExecuteScriptAsync(Properties.Resources.DMMScript);
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.ToString(), FormBrowser.FailedToHideDmmRefreshDialog);
		}
	}


	// タイミングによっては(特に起動時)、ブラウザの初期化が完了する前に Navigate() が呼ばれることがある
	// その場合ロードに失敗してブラウザが白画面でスタートしてしまう（手動でログインページを開けば続行は可能だが）
	// 応急処置として失敗したとき後で再試行するようにしてみる
	private string? NavigateCache { get; set; }



	/// <summary>
	/// 指定した URL のページを開きます。
	/// </summary>
	public void Navigate(string url)
	{
		Browser.CoreWebView2?.Navigate(url);
		IsNavigating = true;
	}
	/// <summary>
	/// ブラウザを再読み込みします。
	/// </summary>
	private void RefreshBrowser() => RefreshBrowser(false);

	/// <summary>
	/// ブラウザを再読み込みします。
	/// </summary>
	/// <param name="ignoreCache">キャッシュを無視するか。</param>
	private void RefreshBrowser(bool ignoreCache)
	{
		//Browser.Reload(ignoreCache);
		if (ignoreCache)
		{
			DevToolsHelper.Page.ReloadAsync(true);
		}
		else
		{
			Browser.CoreWebView2.Reload();
		}
		IsRefreshing = true;
	}

	/// <summary>
	/// ズームを適用します。
	/// </summary>
	private void ApplyZoom()
	{
		if (Browser is not { IsInitialized: true }) return;
		double zoomRate = Configuration.ZoomRate;
		bool fit = Configuration.ZoomFit && StyleSheetApplied;

		double zoomFactor;

		if (fit)
		{
			double rateX = ActualWidth / KanColleSize.Width;
			double rateY = ActualHeight / KanColleSize.Height;

			zoomFactor = Math.Min(rateX, rateY);
		}
		else
		{
			zoomFactor = Math.Clamp(zoomRate, 0.1, 10);
		}

		// DpiScaleX and DpiScaleY should always be the same so it doesn't matter which one you use
		Browser.ZoomFactor = zoomFactor;

		if (StyleSheetApplied && gameframe != null)
		{
			int newWidth = (int)(KanColleSize.Width * zoomFactor);
			int newHeight = (int)(KanColleSize.Height * zoomFactor);

			Browser.Width = newWidth;
			Browser.Height = newHeight;

			// Browser.MinWidth = newWidth;
			// Browser.MinHeight = newHeight;
		}
		else
		{
			Browser.Width = double.NaN;
			Browser.Height = double.NaN;
		}

		CurrentZoom = fit switch
		{
			true => FormBrowser.Other_Zoom_Current_Fit,
			_ => FormBrowser.Other_Zoom_Current + $" {zoomRate:p1}"
		};
	}
	/// <summary>
	/// スクリーンショットを撮影し、設定で指定された保存先に保存します。
	/// </summary>
	private async Task SaveScreenShot()
	{
		int savemode = Configuration.ScreenShotSaveMode;
		int format = Configuration.ScreenShotFormat;
		string folderPath = Configuration.ScreenShotPath;
		bool is32bpp = format != 1 && Configuration.AvoidTwitterDeterioration;

		// to file
		try
		{
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			ImageFormat imageFormat = null;
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

			string path = $"{folderPath}\\{DateTime.Now:yyyyMMdd_HHmmssff}.{ext}";

			await using MemoryStream memoryStream = new();
			await Browser.CoreWebView2.CapturePreviewAsync(browserImageFormat, memoryStream).ConfigureAwait(false);

			Bitmap image = (Bitmap)Bitmap.FromStream(memoryStream, true);

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

	public void SetProxy(string proxy)
	{
		if (ushort.TryParse(proxy, out ushort port))
		{
			// WinInetUtil.SetProxyInProcessForNekoxy(port);
			ProxySettings = "http=127.0.0.1:" + port; // todo: 動くには動くが正しいかわからない
		}
		else
		{
			// WinInetUtil.SetProxyInProcess(proxy, "local");
			ProxySettings = proxy;
		}
		InitializeAsync();
		BrowserHost.SetProxyCompleted();
	}

	private void SetCookie()
	{
		var gamesCookies = Browser.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "games.dmm.com", "/");
		gamesCookies.Expires = DateTime.Now.AddYears(6);
		gamesCookies.IsSecure = true;
		Browser.CoreWebView2.CookieManager.AddOrUpdateCookie(gamesCookies);
		var dmmCookie = Browser.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", ".dmm.com", "/");
		dmmCookie.Expires = DateTime.Now.AddYears(6);
		dmmCookie.IsSecure = true;
		Browser.CoreWebView2.CookieManager.AddOrUpdateCookie(dmmCookie);
		var acccountsCookie = Browser.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "accounts.dmm.com", "/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		acccountsCookie.IsSecure = true;
		Browser.CoreWebView2.CookieManager.AddOrUpdateCookie(acccountsCookie);
		var osapiCookie = Browser.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "osapi.dmm.com", "/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		acccountsCookie.IsSecure = true;
		Browser.CoreWebView2.CookieManager.AddOrUpdateCookie(osapiCookie);
		var gameserverCookie = Browser.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "203.104.209.7", "/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		Browser.CoreWebView2.CookieManager.AddOrUpdateCookie(gameserverCookie);
		var gamepathCookie = Browser.CoreWebView2.CookieManager.CreateCookie("ckcy", "1", "www.dmm.com", "/netgame/");
		acccountsCookie.Expires = DateTime.Now.AddYears(6);
		Browser.CoreWebView2.CookieManager.AddOrUpdateCookie(gamepathCookie);
	}

	private async void SetIconResource()
	{
		byte[][] canvas = await BrowserHost.GetIconResource();

		Icons = new(canvas);
	}

	private void TryGetVolumeManager()
	{
		VolumeManager = VolumeManager.CreateInstanceByProcessName("msedgewebview2", ProxySettings);

		if (VolumeManager is not null && Browser is not null)
		{
			// we're calling this from an async task so the dispatcher is needed
			App.Current.Dispatcher.Invoke(() =>
			{
				VolumeManager.IsMute = Browser.CoreWebView2.IsMuted;
				RealVolume = (int)(VolumeManager.Volume * 100);
			});
		}
	}

	private void SetVolumeState()
	{
		if (!VolumeProcessInitialized) return;
		if (Browser is null) return;

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
				Browser.CoreWebView2.IsMuted = VolumeManager.IsMute;
			}
			else
			{
				IsMuted = Browser.CoreWebView2.IsMuted;
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

	[ICommand]
	private async void Screenshot()
	{
		await SaveScreenShot();
	}

	[ICommand]
	private void SetZoom(string? zoomParameter)
	{
		if (!double.TryParse(zoomParameter, out double zoom)) return;

		Configuration.ZoomRate = zoom;
		Configuration.ZoomFit = ZoomFit = false;
		ApplyZoom();
		ConfigurationUpdated();
	}

	[ICommand]
	private void ModifyZoom(string? zoomParameter)
	{
		if (!double.TryParse(zoomParameter, out double zoom)) return;

		Configuration.ZoomRate = Math.Clamp(Configuration.ZoomRate + zoom, 0.1, 10);
		Configuration.ZoomFit = ZoomFit = false;
		ApplyZoom();
		ConfigurationUpdated();
	}

	[ICommand]
	private void SetToolMenuAlignment(Dock dock)
	{
		ToolMenuDock = dock;

		Configuration.ToolMenuDockStyle = dock switch
		{
			Dock.Top => 1,
			Dock.Bottom => 2,
			Dock.Left => 3,
			Dock.Right => 4,
			_ => 1
		};

		ConfigurationUpdated();
	}

	[ICommand]
	private void SetToolMenuVisibility(Visibility visibility)
	{
		ToolMenuVisibility = visibility;

		Configuration.IsToolMenuVisible = visibility == Visibility.Visible;
		ConfigurationUpdated();
	}

	[ICommand]
	private void Mute()
	{
		if (Browser is null) return;

		if (VolumeManager == null)
		{
			// VolumeManager being null can mean the browser is muted
			// so the sound process doesn't get created
			// make sure the browser isn't muted
			if (Browser.CoreWebView2.IsMuted)
			{
				Browser.CoreWebView2.IsMuted = false;
				SetVolumeState();

				// because the volume manager gets created some time after you un-mute the browser
				// and the browser doesn't have an event indicating when that will happen
				// just try to grab the volume manager after un-muting, with a bit of delay
				Task.Run(async () =>
				{
					int maxRetryCount = 100;
					int retryCount = 0;

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

	private void ToolMenu_Other_Volume_ValueChanged()
	{
		if (!VolumeProcessInitialized) return;

		if (VolumeManager is null)
		{
			TryGetVolumeManager();
		}

		try
		{
			if (VolumeManager is not null)
			{
				IsMuted = false;
				VolumeManager.IsMute = false;
				VolumeManager.Volume = RealVolume / 100f;
				// control.BackColor = System.Drawing.SystemColors.Window;
			}
			else
			{
				// todo: add red color to indicate volume manager isn't active?
				// control.BackColor = System.Drawing.Color.MistyRose;
			}

		}
		catch (Exception)
		{
		}

		Configuration.IsMute = IsMuted;
		ConfigurationUpdated();
	}

	[ICommand]
	private void Refresh()
	{
		if (!Configuration.ConfirmAtRefresh ||
			MessageBox.Show(FormBrowser.ReloadDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			RefreshBrowser();
		}
	}

	[ICommand]
	private void HardRefresh()
	{
		if (!Configuration.ConfirmAtRefresh ||
			MessageBox.Show(FormBrowser.ReloadHardDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			RefreshBrowser(true);
		}
	}

	[ICommand]
	private void GoToLoginPage()
	{
		if (MessageBox.Show(FormBrowser.LoginDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			Navigate(Configuration.LogInPageURL);
		}
	}

	[ICommand]
	private void GoTo()
	{
		BrowserHost.RequestNavigation(Browser.CoreWebView2?.Source ?? "");
	}

	private void ToolMenu_Other_AppliesStyleSheet_Click()
	{
		/*
		Configuration.AppliesStyleSheet = ToolMenu.Other_AppliesStyleSheet.Checked;
		if (!Configuration.AppliesStyleSheet)
			RestoreStyleSheet = true;
		ApplyStyleSheet();
		ApplyZoom();
		ConfigurationUpdated();
		*/
	}



	private void ContextMenuTool_ShowToolMenu_Click(object sender, EventArgs e)
	{
		/*
		ToolMenu.Visible = Configuration.IsToolMenuVisible = true;
		ConfigurationUpdated();
		*/
	}

	private void FormBrowser_Activated(object sender, EventArgs e)
	{
		//Browser?.Focus();
	}

	[ICommand]
	void OpenLastScreenshot()
	{
		if (LastScreenShotPath is null || !File.Exists(LastScreenShotPath)) return;

		ProcessStartInfo psi = new()
		{
			FileName = LastScreenShotPath,
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	[ICommand]
	private void OpenScreenshotFolder()
	{
		if (!Directory.Exists(Configuration.ScreenShotPath)) return;

		ProcessStartInfo psi = new()
		{
			FileName = Configuration.ScreenShotPath,
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	[ICommand]
	private void CopyLastScreenshot()
	{
		if (LastScreenshot is null) return;

		try
		{
			Clipboard.SetImage(LastScreenshot);
			AddLog(2, string.Format(FormBrowser.LastScreenshotCopiedToClipboard, LastScreenShotPath));
		}
		catch (Exception ex)
		{
			SendErrorReport(ex.Message, FormBrowser.FailedToCopyScreenshotToClipboard);
		}
	}

	[ICommand]
	private void OpenDevtools()
	{
		if (Browser is not { IsInitialized: true }) return;
		if (Browser.CoreWebView2 is null) return;
		Browser.CoreWebView2.OpenDevToolsWindow();
	}

	[ICommand]
	private async void ClearCache()
	{
		if (MessageBox.Show(FormBrowser.ClearCacheMessage, FormBrowser.ClearCacheTitle,
			MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			await DevToolsHelper.Network.ClearBrowserCacheAsync();
			AddLog(2, FormBrowser.CacheCleared);
		}
	}

	[ICommand]
	public void OpenExtraBrowser()
	{
		new ExtraBrowserWindow().Show();
	}

	[ICommand]
	public void OpenAirControlSimulator(string url)
	{
		new AirControlSimulatorWindow(url)
		{
			Owner = App.Current.MainWindow,
		}.Show();
	}

	public void RequestAutoRefresh()
	{
		MessageBoxResult messageBoxResult = MessageBox.Show
		(
			Properties.Resources.AutoRefreshNotification,
			FormBrowser.Confirmation,
			MessageBoxButton.OKCancel,
			MessageBoxImage.Exclamation,
			MessageBoxResult.Cancel
		);

		if (messageBoxResult is not MessageBoxResult.OK) return;

		RefreshBrowser();
	}

	/*
	protected override void WndProc(ref Message m)
	{
		if (m.Msg == WM_ERASEBKGND)
			// ignore this message
			return;

		base.WndProc(ref m);
	}

	*/

	#region 呪文

	[DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
	public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

	[DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
	public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

	public const int GWL_STYLE = (-16);
	public const uint WS_CHILD = 0x40000000;
	public const uint WS_VISIBLE = 0x10000000;
	public const int WM_ERASEBKGND = 0x14;

	#endregion
}
