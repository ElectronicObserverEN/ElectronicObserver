using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Browser.WebView2Browser.CompassPrediction;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Grpc.Core;
using Jot;
using MagicOnion.Client;
using ModernWpf;

namespace Browser;

public abstract partial class BrowserViewModel : ObservableObject, IBrowser
{
	public FormBrowserTranslationViewModel FormBrowser { get; }
	protected BrowserConfiguration? Configuration { get; private set; }
	public ImageProvider? Icons { get; private set; }

	public abstract object? Browser { get; }

	protected System.Timers.Timer HeartbeatTimer { get; } = new();

	private string Host { get; }
	private int Port { get; }
	public string Culture { get; protected set; }
	protected IBrowserHost BrowserHost { get; }
	protected string? ProxySettings { get; private set; }

	protected Size KanColleSize { get; } = new(1200, 720);
	protected string KanColleUrl => "https://play.games.dmm.com/game/kancolle/";

	public bool ZoomFit { get; set; }
	public string CurrentZoom { get; set; } = "";

	// user setting for stylesheet
	public bool StyleSheetEnabled { get; set; }

	// todo: flag to temporarily disable the stylesheet
	// seems to be used for when you navigate to something other than kancolle
	private bool ShouldStyleSheetApply { get; set; } = true;
	protected bool StyleSheetApplied => StyleSheetEnabled && ShouldStyleSheetApply;

	/// <summary>
	/// WebView2 doesn't need this.
	/// </summary>
	public DpiScale DpiScale { get; set; }

	public double TextScaleFactor { get; set; }
	public double ActualWidth { get; set; }
	public double ActualHeight { get; set; }

	public Dock ToolMenuDock { get; set; } = Dock.Top;

	public Orientation ToolMenuOrientation => ToolMenuDock switch
	{
		Dock.Left or Dock.Right => Orientation.Vertical,
		_ => Orientation.Horizontal,
	};

	public Visibility ToolMenuVisibility { get; set; } = Visibility.Visible;

	protected bool VolumeProcessInitialized { get; set; }
	protected VolumeManager? VolumeManager { get; set; }
	public int RealVolume { get; set; }
	public bool IsMuted { get; set; }

	protected string? LastScreenShotPath { get; set; }
	public BitmapSource? LastScreenshot { get; set; }

	protected string BrowserFontStyleId { get; } = Guid.NewGuid().ToString()[..8];

	protected CompassPredictionViewModel CompassPredictionViewModel { get; set; }

	private string StyleClassId { get; } = Guid.NewGuid().ToString()[..8];

	protected string PageScript =>
		$$"""
			try
			{
				var node = document.getElementById('{{StyleClassId}}');
				if (node)
				{
					document.head.removeChild(node);
				}

				var style = document.createElement('style');
				style.id = '{{StyleClassId}}';
				style.textContent = `
					body
					{
						margin: 0;
						padding: 0;
						min-width: 0;
						min-height: 0;
						overflow: hidden;
						background-color: black;
					}

					#main-ntg
					{
						position: static;
					}

					#area-game
					{
						margin-left: 0;
						margin-right: 0;
						padding: 0;
						width: 1200,
						height: 720,
						position: relative
					}

					.dmm-ntgnavi
					{
						display: none;
					}

					.area-naviapp
					{
						display: none;
					}

					#ntg-recommend
					{
						display: none;
					}

					#foot, #foot+img
					{
						display: none;
					}

					#w, #main-ntg, #page
					{
						margin: 0,
						padding: 0,
						width: 100%,
						height: 0
						background: none!important;
					}

					#main-ntg
					{
						margin: 0!important;
					}

					.gamesResetStyle, gamesResetStyle *
					{
						background: none !important;
					}

					#game_frame
					{
						--game-frame-width: 1200px;
						--game-frame-height: 720px;
						position: absolute;
						top: 0;
						left: 0;
					}
				`;

				document.head.appendChild(style);
			}
			catch (e)
			{
				alert("ページCSS適用に失敗しました: " + e);
			}
		""";

	protected string FrameScript =>
		$$"""
			try
			{
				var node = document.getElementById('{{StyleClassId}}');
				if (node)
				{
					document.head.removeChild(node);
				}

				var style = document.createElement('style');
				style.id = '{{StyleClassId}}';
				style.textContent = `
					body
					{
						visibility: hidden;
					}

					#flashWrap
					{
						position: fixed;
						left: 0;
						top: 0;
						width: 100% !important;
						height: 100% !important;
					}

					#htmlWrap
					{
						visibility: visible;
						width: 100% !important;
						height: 100% !important;
					}
				`;

				document.head.appendChild(style);
			}
			catch (e)
			{
				alert("フレームCSS適用に失敗しました: " + e);
			}
		""";

	protected string RestoreScript =>
		$$"""
			var node = document.getElementById('{{StyleClassId}}');
			if (node)
			{
				document.head.removeChild(node);
			}
		""";

	protected BrowserViewModel(string host, int port, string culture)
	{
		// Debugger.Launch();

		FormBrowser = Ioc.Default.GetService<FormBrowserTranslationViewModel>()!;
		CompassPredictionTranslationViewModel translation = Ioc.Default.GetRequiredService<CompassPredictionTranslationViewModel>();
		Tracker tracker = Ioc.Default.GetRequiredService<Tracker>();

		Host = host;
		Port = port;
		Culture = culture;
		CultureInfo c = new(culture);

		Thread.CurrentThread.CurrentCulture = c;
		Thread.CurrentThread.CurrentUICulture = c;

		// ホストプロセスに接続
		Channel grpChannel = new(Host, Port, ChannelCredentials.Insecure);
		BrowserHost = StreamingHubClient.Connect<IBrowserHost, IBrowser>(grpChannel, this);

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

			VolumeChanged();
		};

		CompassPredictionViewModel = new(BrowserHost, translation, tracker);
	}

	public abstract void OnLoaded(object sender, RoutedEventArgs e);

	protected async Task SetIconResource()
	{
		byte[][] canvas = await BrowserHost.GetIconResource();

		Icons = new(canvas);
	}

	protected abstract void ApplyZoom();

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
			_ => Dock.Top,
		};

		ToolMenuVisibility = Configuration.IsToolMenuVisible switch
		{
			true => Visibility.Visible,
			_ => Visibility.Collapsed,
		};

		await App.Current.Dispatcher.BeginInvoke(async () =>
		{
			ThemeManager.Current.ApplicationTheme = (ApplicationTheme)await BrowserHost.GetTheme();
		});

		IsMuted = Configuration.IsMute;

		await ApplyCustomBrowserFont(Configuration);
	}

	protected virtual Task ApplyCustomBrowserFont(BrowserConfiguration configuration)
	{
		return Task.CompletedTask;
	}

	protected virtual Task RemoveCustomBrowserFont()
	{
		return Task.CompletedTask;
	}

	protected virtual Task AddCustomBrowserFont(BrowserConfiguration configuration)
	{
		return Task.CompletedTask;
	}

	protected void ConfigurationUpdated()
	{
		if (Configuration is null) return;

		BrowserHost.ConfigurationUpdated(Configuration);
	}

	protected void AddLog(int priority, string message)
	{
		BrowserHost.AddLog(priority, message);
	}

	protected void SendErrorReport(string exceptionName, string message)
	{
		BrowserHost.SendErrorReport(exceptionName, message);
	}

	protected abstract void ApplyStyleSheet();

	protected abstract void DestroyDMMreloadDialog();

	protected abstract void TryGetVolumeManager();

	private void VolumeChanged()
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

	protected abstract void SetVolumeState();

	public void InitialAPIReceived()
	{
		//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
		ApplyStyleSheet();
		ApplyZoom();
		DestroyDMMreloadDialog();

		//起動直後はまだ音声が鳴っていないのでミュートできないため、この時点で有効化
		SetVolumeState();

		if (Configuration is null) return;

		ApplyCustomBrowserFont(Configuration);
	}

	public abstract void Navigate(string url);

	protected abstract void InitializeAsync();

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

	protected abstract void RefreshBrowser();

	protected abstract void RefreshBrowser(bool ignoreCache);

	public void RequestAutoRefresh()
	{
		MessageBoxResult messageBoxResult = MessageBox.Show
		(
			Resources.AutoRefreshNotification,
			FormBrowser.Confirmation,
			MessageBoxButton.OKCancel,
			MessageBoxImage.Exclamation,
			MessageBoxResult.Cancel
		);

		if (messageBoxResult is not MessageBoxResult.OK) return;

		RefreshBrowser();
	}

	public void RequestCompassPredictionFleetUpdate()
	{
		CompassPredictionViewModel.UpdateFleet();
	}

	public void RequestCompassPredictionMapUpdate(int area, int map)
	{
		CompassPredictionViewModel.UpdateDisplayedMap(area, map);
	}

	public void CloseBrowser()
	{
		HeartbeatTimer.Stop();
		// リモートコールでClose()呼ぶのばヤバそうなので非同期にしておく
		App.Current.Dispatcher.BeginInvoke(Exit);
	}

	protected abstract void Exit();

	/// <summary>
	/// スクリーンショットを撮影し、設定で指定された保存先に保存します。
	/// </summary>
	[RelayCommand]
	protected abstract void Screenshot();

	[RelayCommand]
	private void SetZoom(string? zoomParameter)
	{
		if (Configuration is null) return;
		if (!double.TryParse(zoomParameter, out double zoom)) return;

		Configuration.ZoomRate = zoom;
		Configuration.ZoomFit = ZoomFit = false;
		ApplyZoom();
		ConfigurationUpdated();
	}

	[RelayCommand]
	private void ModifyZoom(string? zoomParameter)
	{
		if (Configuration is null) return;
		if (!double.TryParse(zoomParameter, out double zoom)) return;

		Configuration.ZoomRate = Math.Clamp(Configuration.ZoomRate + zoom, 0.1, 10);
		Configuration.ZoomFit = ZoomFit = false;
		ApplyZoom();
		ConfigurationUpdated();
	}

	[RelayCommand]
	private void SetToolMenuAlignment(Dock dock)
	{
		if (Configuration is null) return;

		ToolMenuDock = dock;

		Configuration.ToolMenuDockStyle = dock switch
		{
			Dock.Top => 1,
			Dock.Bottom => 2,
			Dock.Left => 3,
			Dock.Right => 4,
			_ => 1,
		};

		ConfigurationUpdated();
	}

	[RelayCommand]
	private void SetToolMenuVisibility(Visibility visibility)
	{
		if (Configuration is null) return;

		ToolMenuVisibility = visibility;

		Configuration.IsToolMenuVisible = visibility == Visibility.Visible;
		ConfigurationUpdated();
	}

	[RelayCommand]
	protected abstract void Mute();

	[RelayCommand]
	private void Refresh()
	{
		if (Configuration is null) return;

		if (!Configuration.ConfirmAtRefresh ||
			MessageBox.Show(FormBrowser.ReloadDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			RefreshBrowser();
		}
	}

	[RelayCommand]
	private void HardRefresh()
	{
		if (Configuration is null) return;

		if (!Configuration.ConfirmAtRefresh ||
			MessageBox.Show(FormBrowser.ReloadHardDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			RefreshBrowser(true);
		}
	}

	[RelayCommand]
	private void GoToLoginPage()
	{
		if (Configuration is null) return;

		if (MessageBox.Show(FormBrowser.LoginDialog, FormBrowser.Confirmation,
				MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel)
			== MessageBoxResult.OK)
		{
			Navigate(Configuration.LogInPageURL);
		}
	}

	[RelayCommand]
	protected abstract void GoTo();

	[RelayCommand]
	private void OpenLastScreenshot()
	{
		if (LastScreenShotPath is null || !File.Exists(LastScreenShotPath)) return;

		ProcessStartInfo psi = new()
		{
			FileName = LastScreenShotPath,
			UseShellExecute = true,
		};

		Process.Start(psi);
	}

	[RelayCommand]
	private void OpenScreenshotFolder()
	{
		if (Configuration is null) return;
		if (!Directory.Exists(Configuration.ScreenShotPath)) return;

		ProcessStartInfo psi = new()
		{
			FileName = Configuration.ScreenShotPath,
			UseShellExecute = true,
		};

		Process.Start(psi);
	}

	[RelayCommand]
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

	[RelayCommand]
	protected abstract void OpenDevtools();

	[RelayCommand]
	protected abstract void ClearCache();

	[RelayCommand]
	public abstract void OpenExtraBrowser();

	[RelayCommand]
	public abstract void OpenAirControlSimulator(string url);

	[RelayCommand]
	public abstract void OpenCompassPrediction();

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
