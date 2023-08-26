using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrowserHost;
using BrowserLibCore;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Tools.AutoRefresh;
using Grpc.Core;
using MagicOnion.Hosting;
using Microsoft.Extensions.Hosting;
using ModernWpf;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window;

/// <summary>
/// ブラウザのホスト側フォーム
/// </summary>
// [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public partial class FormBrowserHost : Form
{
	private static FormBrowserHost _instance;
	public static FormBrowserHost Instance => _instance;

	private AutoRefreshViewModel AutoRefresh { get; }

	public static string BrowserExeName => "EOBrowser.exe";

	private string Host { get; }
	private int Port { get; }

	/// <summary>
	/// FormBrowserHostの通信サーバ
	/// </summary>
	private string ServerUri => "net.pipe://localhost/" + Process.GetCurrentProcess().Id + "/ElectronicObserver";

	/// <summary>
	/// FormBrowserとの通信インターフェース
	/// </summary>
	// private PipeCommunicator<IBrowser> Browser { get; set; }

	public List<BrowserHostHub> Hubs { get; } = new List<BrowserHostHub>();

	public IBrowser Browser => Hubs.FirstOrDefault()?.Browser ?? throw new Exception();

	private Process BrowserProcess { get; set; }

	private IntPtr BrowserWnd { get; set; } = IntPtr.Zero;



	[Flags]
	private enum InitializationStageFlag
	{
		InitialAPILoaded = 1,
		BrowserConnected = 2,
		SetProxyCompleted = 4,
		Completed = 7,
	}

	/// <summary>
	/// 初期化ステージカウント
	/// 完了したらログインページを開く (各処理が終わらないと正常にロードできないため)
	/// </summary>
	private InitializationStageFlag _initializationStage = 0;
	private InitializationStageFlag InitializationStage
	{
		get => _initializationStage;
		set
		{
			//AddLog( 1, _initializationStage + " -> " + value );
			if (_initializationStage != InitializationStageFlag.Completed && value == InitializationStageFlag.Completed)
			{
				if (Utility.Configuration.Config.FormBrowser.IsEnabled)
				{
					// browser handles the navigation
					// NavigateToLogInPage();
				}
			}

			_initializationStage = value;
		}
	}

	public FormBrowserHost(object parent)
	{
		AutoRefresh = Ioc.Default.GetRequiredService<AutoRefreshViewModel>();
		AutoRefresh.Loaded();

		InitializeComponent();

		_instance = this;

		Host = "localhost";
		Port = GetPort();

		Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormBrowser]);

		Translate();
		SubscribeToApis();
	}

	private static int GetPort()
	{
		TcpListener server = new(IPAddress.Loopback, 0);
		server.Start();
		
		int port = ((IPEndPoint)server.LocalEndpoint).Port;
		
		server.Stop();
		
		return port;
	}

	public void Translate()
	{
		Text = BrowserHostResources.Title;
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiReqMap_Start.ResponseReceived += RefreshIfAdvanceIsNotAllowed;
		APIObserver.Instance.ApiReqMap_Next.ResponseReceived += RefreshIfAdvanceIsNotAllowed;
	}

	private void RefreshIfAdvanceIsNotAllowed(string apiname, object data)
	{
		CompassData compass = KCDatabase.Instance.Battle.Compass;

		if (ShouldRefresh(compass.MapAreaID, compass.MapInfoID, compass.Destination, AutoRefresh))
		{
			Browser.RequestAutoRefresh();
		}
	}

	public static bool ShouldRefresh(int mapAreaId, int mapInfoId, int destination, AutoRefreshViewModel autoRefresh)
	{
		if (autoRefresh.IsSingleMapMode && autoRefresh.SelectedSingleMapModeMap is not null)
		{
			if (mapAreaId != autoRefresh.SelectedSingleMapModeMap.AreaId) return true;
			if (mapInfoId != autoRefresh.SelectedSingleMapModeMap.InfoId) return true;
		}

		List<AutoRefreshRuleViewModel> enabledRules = autoRefresh.Rules
			.Where(r => r.IsEnabled)
			.ToList();

		if (enabledRules.Count is 0) return false;

		return !enabledRules.All(IsAllowed);

		bool IsAllowed(AutoRefreshRuleViewModel rule)
		{
			if (mapAreaId != rule.Map.AreaId) return true;
			if (mapInfoId != rule.Map.InfoId) return true;

			return rule.AllowedCells.Select(c => c.Id).Contains(destination);
		}
	}

	public void InitializeApiCompleted()
	{
		InitializationStage |= InitializationStageFlag.InitialAPILoaded;
	}

	private void FormBrowser_Load(object sender, EventArgs e)
	{
		// without wait you get:
		// Invoke or BeginInvoke cannot be called on a control until the window handle has been created
		Task.Run(MakeHost).Wait();
		LaunchBrowserProcess();
	}

	public void Connect(BrowserHostHub hub)
	{
		Hubs.Clear();
		Hubs.Add(hub);
	}

	private async void MakeHost()
	{
		await MagicOnionHost.CreateDefaultBuilder()
			.UseMagicOnion(new ServerPort(Host, Port, ServerCredentials.Insecure))
			.RunConsoleAsync();
	}

	private void LaunchBrowserProcess()
	{
		try
		{
			// プロセス起動
			string arguments = $"{Host} {Port} {Utility.Configuration.Config.UI.Culture} {Utility.Configuration.Config.FormBrowser.Browser}";

			if (File.Exists(BrowserExeName))
			{
				BrowserProcess = Process.Start(BrowserExeName, arguments);
			}
			else //デバッグ環境用 作業フォルダにかかわらず自分と同じフォルダのを参照する
			{
				string fileName =
					Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" +
					BrowserExeName;

				BrowserProcess = Process.Start(fileName, arguments);
			}

			// 残りはサーバに接続してきたブラウザプロセスがドライブする

		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, Resources.FailedBrowserStart);
			MessageBox.Show(Resources.FailedBrowserStart + ex.Message,
				BrowserHostResources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	internal void ConfigurationChanged()
	{
		Font = Utility.Configuration.Config.UI.MainFont;
		Browser.ConfigurationChanged();
	}

	//ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
	void InitialAPIReceived(string apiname, dynamic data)
	{
		if (InitializationStage != InitializationStageFlag.Completed)       // 初期化が終わってから
			return;

		Browser.InitialAPIReceived();

		// Browser.AsyncRemoteRun(() => Browser.Proxy.InitialAPIReceived());
	}


	/// <summary>
	/// 指定した URL のページを開きます。
	/// </summary>
	private void Navigate(string url)
	{
		Browser.Navigate(url);
		// Browser.AsyncRemoteRun(() => Browser.Proxy.Navigate(url));
	}

	/// <summary>
	/// 艦これのログインページを開きます。
	/// </summary>
	private void NavigateToLogInPage()
	{
		Navigate(Utility.Configuration.Config.FormBrowser.LogInPageURL);
	}

	public void SendErrorReport(string exceptionName, string message)
	{
		Utility.ErrorReporter.SendErrorReport(new Exception(exceptionName), message);
	}

	public void AddLog(int priority, string message)
	{
		Utility.Logger.Add(priority, message);
	}

	public BrowserConfiguration ConfigurationCore
	{
		get
		{
			Configuration.ConfigurationData.ConfigFormBrowser c = Configuration.Config.FormBrowser;

			return new BrowserConfiguration
			{
				ZoomRate = c.ZoomRate,
				ZoomFit = c.ZoomFit,
				LogInPageURL = c.LogInPageURL,
				IsEnabled = c.IsEnabled,
				ScreenShotPath = c.ScreenShotPath,
				ScreenShotFormat = c.ScreenShotFormat,
				ScreenShotSaveMode = c.ScreenShotSaveMode,
				StyleSheet = c.StyleSheet,
				IsScrollable = c.IsScrollable,
				AppliesStyleSheet = c.AppliesStyleSheet,
				IsDMMreloadDialogDestroyable = c.IsDMMreloadDialogDestroyable,
				AvoidTwitterDeterioration = c.AvoidTwitterDeterioration,
				ToolMenuDockStyle = (int)c.ToolMenuDockStyle,
				IsToolMenuVisible = c.IsToolMenuVisible,
				ConfirmAtRefresh = c.ConfirmAtRefresh,
				HardwareAccelerationEnabled = c.HardwareAccelerationEnabled,
				PreserveDrawingBuffer = c.PreserveDrawingBuffer,
				BackColor = BackColor.ToArgb(),
				ForceColorProfile = c.ForceColorProfile,
				SavesBrowserLog = c.SavesBrowserLog,
				EnableDebugMenu = Utility.Configuration.Config.Debug.EnableDebugMenu,
				UseGadgetRedirect = c.UseGadgetRedirect,
				GadgetBypassServer = c.GadgetBypassServer,
				GadgetBypassServerCustom = c.GadgetBypassServerCustom,
				UseVulkanWorkaround = c.UseVulkanWorkaround,
				Volume = c.Volume,
				IsMute = c.IsMute,
				IsBrowserContextMenuEnabled = c.IsBrowserContextMenuEnabled, 
				MainFont = Utility.Configuration.Config.UI.MainFont.FontData!.Name,
				UseCustomBrowserFont = Utility.Configuration.Config.UI.UseCustomBrowserFont,
				BrowserFont = Utility.Configuration.Config.UI.BrowserFontName,
				MatchMainFont = Utility.Configuration.Config.UI.MatchMainFont,
			};
		}
	}

	internal void ConfigurationUpdated(BrowserConfiguration config)
	{
		var c = Utility.Configuration.Config.FormBrowser;

		c.ZoomRate = config.ZoomRate;
		c.ZoomFit = config.ZoomFit;
		c.LogInPageURL = config.LogInPageURL;
		c.IsEnabled = config.IsEnabled;
		c.ScreenShotPath = config.ScreenShotPath;
		c.ScreenShotFormat = config.ScreenShotFormat;
		c.ScreenShotSaveMode = config.ScreenShotSaveMode;
		c.StyleSheet = config.StyleSheet;
		c.IsScrollable = config.IsScrollable;
		c.AppliesStyleSheet = config.AppliesStyleSheet;
		c.IsDMMreloadDialogDestroyable = config.IsDMMreloadDialogDestroyable;
		c.AvoidTwitterDeterioration = config.AvoidTwitterDeterioration;
		c.ToolMenuDockStyle = (DockStyle)config.ToolMenuDockStyle;
		c.IsToolMenuVisible = config.IsToolMenuVisible;
		c.ConfirmAtRefresh = config.ConfirmAtRefresh;
		c.HardwareAccelerationEnabled = config.HardwareAccelerationEnabled;
		c.PreserveDrawingBuffer = config.PreserveDrawingBuffer;
		c.ForceColorProfile = config.ForceColorProfile;
		c.SavesBrowserLog = config.SavesBrowserLog;
		c.Volume = config.Volume;
		c.IsMute = config.IsMute;
		c.IsBrowserContextMenuEnabled = config.IsBrowserContextMenuEnabled;
		//Utility.Configuration.Config.Debug.EnableDebugMenu = config.EnableDebugMenu;

		// volume
		if (Utility.Configuration.Config.BGMPlayer.SyncBrowserMute)
		{
			Utility.SyncBGMPlayer.Instance.IsMute = config.IsMute;
		}
	}

	public byte[][] GetIconResource()
	{

		string[] keys =
		{
			"Browser_ScreenShot",
			"Browser_Zoom",
			"Browser_ZoomIn",
			"Browser_ZoomOut",
			"Browser_Unmute",
			"Browser_Mute",
			"Browser_Refresh",
			"Browser_Navigate",
			"Browser_Other",
		};
		int unitsize = 16 * 16 * 4;
		//  
		//  unitsize
		byte[][] canvas = new byte[keys.Length][];

		for (int i = 0; i < keys.Length; i++)
		{
			Image img = ResourceManager.Instance.Icons.Images[keys[i]];
			if (img == null) continue;

			canvas[i] = (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
			/*
			using (Bitmap bmp = new Bitmap(img))
			{

				BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				Marshal.Copy(bmpdata.Scan0, canvas, unitsize * i, unitsize);
				bmp.UnlockBits(bmpdata);

			}
			*/
		}

		return canvas;
	}

	public int GetTheme()
	{
		return App.Current.Dispatcher.Invoke(() => (int)ThemeManager.Current.ApplicationTheme);
	}

	public void RequestNavigation(string baseurl)
	{
		using (var dialog = new Dialog.DialogTextInput(Resources.AskNavTitle, Resources.AskNavText))
		{
			dialog.InputtedText = baseurl;

			App.Current.Dispatcher.Invoke(() =>
			{
				if (dialog.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
				{
					Navigate(dialog.InputtedText);
				}
			});
		}
	}
	public void ConnectToBrowser(IntPtr hwnd)
	{
		Invoke((Action)(() => ConnectToBrowserInternal(hwnd)));
	}

	private void ConnectToBrowserInternal(IntPtr hwnd)
	{
		BrowserWnd = hwnd;

		// 子ウィンドウに設定
		SetParent(BrowserWnd, Handle);
		MoveWindow(BrowserWnd, 0, 0, this.Width, this.Height, true);

		//キー入力をブラウザに投げる
		Application.AddMessageFilter(new KeyMessageGrabber(BrowserWnd));

		// デッドロックするので非同期で処理
		BeginInvoke((Action)(() =>
		{
			// ブラウザプロセスに接続
			// todo: need browser connect?
			// Browser.Connect(ServerUri + "Browser/Browser");
			// Browser.Faulted += Browser_Faulted;

			ConfigurationChanged();

			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			APIObserver.Instance.ApiStart2_GetData.ResponseReceived +=
				(string apiname, dynamic data) => InitialAPIReceived(apiname, data);

			// プロキシをセット
			Browser.SetProxy(BuildDownstreamProxy());
			// Browser.AsyncRemoteRun(() => Browser.Proxy.SetProxy(BuildDownstreamProxy()));
			APIObserver.Instance.ProxyStarted += () =>
			{
				Browser.SetProxy(BuildDownstreamProxy());
				// Browser.AsyncRemoteRun(() => Browser.Proxy.SetProxy(BuildDownstreamProxy()));
			};

			InitializationStage |= InitializationStageFlag.BrowserConnected;

		}));
	}

	private string BuildDownstreamProxy()
	{
		var config = Utility.Configuration.Config.Connection;

		if (!string.IsNullOrEmpty(config.DownstreamProxy))
		{
			return config.DownstreamProxy;

		}
		else if (config.UseSystemProxy)
		{
			return APIObserver.Instance.ProxyPort.ToString();
		}
		else if (config.UseUpstreamProxy)
		{
			return string.Format(
				"http=127.0.0.1:{0};https={1}:{2}",
				APIObserver.Instance.ProxyPort,
				config.UpstreamProxyAddress,
				config.UpstreamProxyPort);
		}
		else
		{
			return string.Format("http=127.0.0.1:{0}", APIObserver.Instance.ProxyPort);
		}
	}


	public void SetProxyCompleted()
	{
		InitializationStage |= InitializationStageFlag.SetProxyCompleted;
	}


	void Browser_Faulted(Exception e)
	{
		/*if ( Browser.Proxy == null ) 
		{
			Utility.Logger.Add( 3, Resources.BrowserClosedWithoutWarning );
		} 
		else
		{
			Utility.ErrorReporter.SendErrorReport( e, Resources.BrowserThrewError );
		}*/
	}


	private void TerminateBrowserProcess()
	{
		// have to unset parent because the browsers Application.Exit call propagates up to EO otherwise
		SetParent(BrowserWnd, IntPtr.Zero);
		if (!BrowserProcess.WaitForExit(2000))
		{
			try
			{
				// 2秒待って終了しなかったらKill
				BrowserProcess.Kill();
			}
			catch (Exception)
			{
				// プロセスが既に終了してた場合などに例外が出る
			}
		}
		BrowserWnd = IntPtr.Zero;
	}

	public void CloseBrowser()
	{
		try
		{
			if (Browser == null)
			{
				// ブラウザを開いていない場合はnullなので
				return;
			}

			Browser.CloseBrowser();
			TerminateBrowserProcess();
		}
		catch (Exception ex)
		{
			//ブラウザプロセスが既に終了していた場合など
			Utility.ErrorReporter.SendErrorReport(ex, Resources.BrowserCloseError);
		}

	}

	private void FormBrowserHost_Resize(object sender, EventArgs e)
	{
		if (BrowserWnd != IntPtr.Zero)
		{
			MoveWindow(BrowserWnd, 0, 0, this.Width, this.Height, true);
		}
	}

	private void FormBrowserHost_Paint(object sender, PaintEventArgs e)
	{
		if (BrowserProcess?.HasExited ?? false)
		{
			var image = ResourceManager.Instance.Icons.Images[(int)IconContent.ConditionVeryTired];
			e.Graphics.DrawImage(image, new Rectangle(16, 16, 16, 16));

			e.Graphics.DrawString(GeneralRes.BrowserProcessClickStart, Utility.Configuration.Config.UI.MainFont, new SolidBrush(Utility.Configuration.Config.UI.ForeColor), new PointF(48, 16));
		}
	}


	private void FormBrowserHost_Click(object sender, EventArgs e)
	{
		Refresh();

		if (InitializationStage == InitializationStageFlag.Completed && (BrowserProcess?.HasExited ?? false))
		{
			InitializationStage = InitializationStageFlag.InitialAPILoaded;
			LaunchBrowserProcess();
		}
	}


	protected string GetPersistString()
	{
		return "Browser";
	}


	#region 呪文

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

	#endregion

}


/// <summary>
/// 別プロセスのウィンドウにフォーカスがあるとキーボードショートカットが効かなくなるため、
/// キー関連のメッセージのコピーを別のウィンドウに送る
/// </summary>
internal class KeyMessageGrabber : IMessageFilter
{
	private IntPtr TargetWnd;

	[DllImport("user32.dll")]
	private static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

	private const int WM_KEYDOWN = 0x100;
	private const int WM_KEYUP = 0x101;
	private const int WM_SYSKEYDOWN = 0x0104;
	private const int WM_SYSKEYUP = 0x0105;

	public KeyMessageGrabber(IntPtr targetWnd)
	{
		TargetWnd = targetWnd;
	}

	public bool PreFilterMessage(ref Message m)
	{
		switch (m.Msg)
		{
			case WM_KEYDOWN:
			case WM_KEYUP:
			case WM_SYSKEYDOWN:
			case WM_SYSKEYUP:
				PostMessage(TargetWnd, m.Msg, m.WParam, m.LParam);
				break;
		}
		return false;
	}
}
