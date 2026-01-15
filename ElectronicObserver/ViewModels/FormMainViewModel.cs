using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using AvalonDock.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Dialog.QuestTrackerManager;
using ElectronicObserver.Window.Dialog.ResourceChartWPF;
using ElectronicObserver.Window.Dialog.VersionInformation;
using ElectronicObserver.Window.Integrate;
using ElectronicObserver.Window.Settings;
using ElectronicObserver.Window.Tools.AirDefense;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.ConstructionRecordViewer;
using ElectronicObserver.Window.Tools.DatabaseExplorer;
using ElectronicObserver.Window.Tools.DevelopmentRecordViewer;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserver.Window.Tools.DropRecordViewer;
using ElectronicObserver.Window.Tools.EquipmentList;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserver.Window.Tools.ExpeditionRecordViewer;
using ElectronicObserver.Window.Tools.SenkaViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserver.Window.Wpf;
using ElectronicObserver.Window.Wpf.Arsenal;
using ElectronicObserver.Window.Wpf.BaseAirCorps;
using ElectronicObserver.Window.Wpf.Battle;
using ElectronicObserver.Window.Wpf.Compass;
using ElectronicObserver.Window.Wpf.Dock;
using ElectronicObserver.Window.Wpf.EquipmentUpgradePlanViewer;
using ElectronicObserver.Window.Wpf.ExpeditionCheck;
using ElectronicObserver.Window.Wpf.Fleet;
using ElectronicObserver.Window.Wpf.FleetOverview;
using ElectronicObserver.Window.Wpf.FleetPreset;
using ElectronicObserver.Window.Wpf.Headquarters;
using ElectronicObserver.Window.Wpf.InformationView;
using ElectronicObserver.Window.Wpf.Log;
using ElectronicObserver.Window.Wpf.Quest;
using ElectronicObserver.Window.Wpf.ShipGroupAvalonia;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using Jot;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using ModernWpf;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Windows.Forms.Timer;
using ElectronicObserver.Avalonia.ExpeditionCalculator;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;
using AvalonDock.Controls;
using ElectronicObserver.Core;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Dialog.UiBlocker;

#if DEBUG
using System.Text.Encodings.Web;
using ElectronicObserver.TestData.Models;
#endif

namespace ElectronicObserver.ViewModels;

public partial class FormMainViewModel : ObservableObject
{
	private FormMainWpf Window { get; }
	private DockingManager DockingManager { get; }
	private Configuration.ConfigurationData Config { get; }
	private UiBlockerManagerViewModel UiBlockerManager { get; }
	public FormMainTranslationViewModel FormMain { get; }
	private ToolService ToolService { get; }
	private FileService FileService { get; }
	private System.Windows.Forms.Timer UIUpdateTimer { get; }
	public bool Topmost { get; set; }
	public int GridSplitterSize { get; set; } = 1;
	public bool CanChangeGridSplitterSize { get; set; }
	public bool LockLayout { get; set; }
	public bool CanAutoHide => !LockLayout;

	private string LayoutFolder => @"Settings\Layout";
	private string DefaultLayoutPath => Path.Combine(LayoutFolder, "Default.xml");
	private string LayoutPath => Config.Life.LayoutFilePath;
	private string PositionPath => Path.ChangeExtension(LayoutPath, ".Position.json");
	private string IntegratePath => Path.ChangeExtension(LayoutPath, ".Integrate.json");

	private int VolumeUpdateState { get; set; }
	public bool NotificationsSilenced { get; set; }
	private DateTime PrevPlayTimeRecorded { get; set; } = DateTime.MinValue;
	public FontFamily Font { get; set; }
	public double FontSize { get; set; }
	public SolidColorBrush FontBrush { get; set; }
	public FontFamily SubFont { get; set; }
	public double SubFontSize { get; set; }
	public SolidColorBrush SubFontBrush { get; set; }

	public string MaintenanceText { get; set; } = "";
	public Visibility MaintenanceTextVisibility => string.IsNullOrEmpty(MaintenanceText) ? Visibility.Collapsed : Visibility.Visible;
	public bool UpdateAvailable { get; set; } = false;

	public string DownloadProgressString { get; private set; } = "";
	public Visibility DownloadProgressVisibility { get; private set; } = Visibility.Collapsed;

	public List<Theme> Themes { get; } = new()
	{
		new Vs2013LightTheme(),
		new Vs2013BlueTheme(),
		new Vs2013DarkTheme(),
	};
	public Theme CurrentTheme { get; set; }
	public Color BackgroundColor { get; set; }

	private WindowPosition Position { get; set; } = new();
	// single instance hack
	private EventLockPlannerWindow? EventLockPlannerWindow { get; set; }
	private AutoRefreshWindow? AutoRefreshWindow { get; set; }

	public ObservableCollection<AnchorableViewModel> Views { get; } = new();

	public List<FleetViewModel> Fleets { get; }
	public FleetOverviewViewModel FleetOverview { get; }
	// public ShipGroupWinformsViewModel FormShipGroup { get; }
	public ShipGroupAvaloniaViewModel ShipGroup { get; }
	public FleetPresetViewModel FleetPreset { get; }
	public ShipTrainingPlanViewerViewModel ShipTrainingPlanViewer { get; }
	public SenkaLeaderboardViewModel SenkaLeaderboardViewer { get; }

	public DockViewModel Dock { get; }
	public ArsenalViewModel Arsenal { get; }
	public EquipmentUpgradePlanViewerViewModel EquipmentUpgradePlanViewer { get; }
	public BaseAirCorpsViewModel BaseAirCorps { get; }

	public HeadquartersViewModel Headquarters { get; }

	public QuestViewModel Quest { get; }
	public ExpeditionCheckViewModel ExpeditionCheck { get; }
	public InformationViewModel FormInformation { get; }

	public CompassViewModel Compass { get; }
	public BattleViewModel Battle { get; }

	public FormBrowserHostViewModel FormBrowserHost { get; }
	public LogViewModel FormLog { get; }
	public FormJsonViewModel FormJson { get; }
	public FormWindowCaptureViewModel WindowCapture { get; }

	public StripStatusViewModel StripStatus { get; } = new();
	public int ClockFormat { get; set; }

	private bool DebugEnabled { get; set; }
	public Visibility DebugVisibility => DebugEnabled.ToVisibility();

	private string GeneratedDataFolder => "Generated";
	public bool GenerateMasterDataVisible =>
#if DEBUG
		true;
#else
		false;
#endif

	public bool DatabaseBrowserVisible =>
#if DEBUG
		true;
#else
		false;
#endif

	public FormMainViewModel(DockingManager dockingManager, FormMainWpf window)
	{
		Window = window;
		DockingManager = dockingManager;

		Config = Configuration.Config;
		UiBlockerManager = new(new(DockingManager, Config), Config);
		FormMain = Ioc.Default.GetRequiredService<FormMainTranslationViewModel>();
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		FileService = Ioc.Default.GetRequiredService<FileService>();

		CultureInfo cultureInfo = new(Configuration.Config.UI.Culture);

		Thread.CurrentThread.CurrentCulture = cultureInfo;
		Thread.CurrentThread.CurrentUICulture = cultureInfo;

		Utility.Logger.Instance.LogAdded += data =>
		{
			if (Window.CheckAccess())
			{
				// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
				// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
				// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
				Window.Dispatcher.Invoke(new Utility.LogAddedEventHandler(Logger_LogAdded), data);
			}
			else
			{
				Logger_LogAdded(data);
			}
		};

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		string softwareName = CultureInfo.CurrentCulture.Name switch
		{
			"en-US" => SoftwareInformation.SoftwareNameEnglish,
			_ => SoftwareInformation.SoftwareNameJapanese
		};

		Utility.Logger.Add(2, softwareName + MainResources.Starting);

		ResourceManager.Instance.Load();
		RecordManager.Instance.Load();
		KCDatabase.Instance.Load();
		NotifierManager.Instance.Initialize(Window);
		SyncBGMPlayer.Instance.ConfigurationChanged();

		using ElectronicObserverContext db = new();
		db.Database.Migrate();

		UIUpdateTimer = new Timer() { Interval = 1000 };
		UIUpdateTimer.Tick += UIUpdateTimer_Tick;
		UIUpdateTimer.Start();

		APIObserver.Instance.Start(Configuration.Config.Connection.Port, Window);

		Fleets = new List<FleetViewModel>()
		{
			new(1),
			new(2),
			new(3),
			new(4),
		};
		foreach (FleetViewModel fleet in Fleets)
		{
			Views.Add(fleet);
		}
		Views.Add(FleetOverview = new FleetOverviewViewModel(Fleets));
		// Views.Add(FormShipGroup = new ShipGroupWinformsViewModel());
		Views.Add(ShipGroup = new ShipGroupAvaloniaViewModel());
		Views.Add(FleetPreset = new FleetPresetViewModel());
		ShipTrainingPlanViewer = Ioc.Default.GetRequiredService<ShipTrainingPlanViewerViewModel>();
		Views.Add(ShipTrainingPlanViewer);
		SenkaLeaderboardViewer = Ioc.Default.GetRequiredService<SenkaLeaderboardManager>().CurrentCutoffData;
		Views.Add(SenkaLeaderboardViewer);

		Views.Add(Dock = new DockViewModel());
		Views.Add(Arsenal = new ArsenalViewModel());
		Views.Add(EquipmentUpgradePlanViewer = new EquipmentUpgradePlanViewerViewModel());
		Views.Add(BaseAirCorps = new BaseAirCorpsViewModel());

		Views.Add(Headquarters = new HeadquartersViewModel());
		Views.Add(Quest = new QuestViewModel());
		Views.Add(ExpeditionCheck = new ExpeditionCheckViewModel());
		Views.Add(FormInformation = new InformationViewModel());

		Views.Add(Compass = new CompassViewModel());
		Views.Add(Battle = new BattleViewModel());

		Views.Add(FormBrowserHost = new FormBrowserHostViewModel() { Visibility = Visibility.Visible });
		Views.Add(FormLog = new LogViewModel());
		Views.Add(FormJson = new FormJsonViewModel());
		Views.Add(WindowCapture = new FormWindowCaptureViewModel(this));

		ConfigurationChanged(); //設定から初期化

		// LoadLayout();

		SoftwareInformation.CheckUpdate();
		Task.Run(async () => await SoftwareUpdater.CheckUpdateAsync());
		CancellationTokenSource cts = new();
		Task.Run(async () => await SoftwareUpdater.PeriodicUpdateCheckAsync(cts.Token));

#if DEBUG
		// Run only on debug for now (kinda worried it breaks stuff like equipment upgrade planner with the Initialize member that is only called once)
		Task.Run(LoadBaseAPI);
#endif

		APIObserver.Instance.ResponseReceived += (a, b) => UpdatePlayTime();


		// 🎃
		if (DateTime.Now.Month == 10 && DateTime.Now.Day == 31)
		{
			APIObserver.Instance.ApiPort_Port.ResponseReceived += CallPumpkinHead;
		}

		// 完了通知（ログインページを開く）
		// fBrowser.InitializeApiCompleted();
		if (FormBrowserHost.WinformsControl is FormBrowserHost host)
		{
			host.InitializeApiCompleted();
		}

		NotificationsSilenced = NotifierManager.Instance.GetNotifiers().All(n => n.IsSilenced);

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Topmost)) return;

			Configuration.Config.Life.TopMost = Topmost;
			ConfigurationChanged();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(GridSplitterSize)) return;

			SaveLayout(Window);
			LoadLayout(Window);
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(LockLayout)) return;

			Config.Life.LockLayout = LockLayout;
			SaveLayout(Window);
			ConfigurationChanged();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(DownloadProgressString)) return;

			DownloadProgressVisibility = string.IsNullOrEmpty(DownloadProgressString) ? Visibility.Collapsed : Visibility.Visible;
		};

		Logger.Add(3, Resources.StartupComplete);
	}

	// Toggle TopMost of Main Form back and forth to workaround a .Net Bug: KB2756203 (~win7) / KB2769674 (win8~)
	private void RefreshTopMost()
	{
		Topmost = !Topmost;
		Topmost = !Topmost;
	}

	[RelayCommand]
	private void AutoHide(LayoutAnchorable anchorable)
	{
		anchorable.ToggleAutoHide();
	}

	#region File

	[RelayCommand]
	private void SaveData()
	{
		RecordManager.Instance.SaveAll();
	}

	[RelayCommand]
	private void LoadData()
	{
		if (MessageBox.Show(Resources.AskLoad, MainResources.ConfirmatonCaption,
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
			== MessageBoxResult.Yes)
		{
			RecordManager.Instance.Load();
		}
	}

	[RelayCommand]
	public void SaveLayout(object? sender)
	{
		if (sender is not FormMainWpf window) return;

		foreach (FormIntegrateViewModel integrate in Views.OfType<FormIntegrateViewModel>())
		{
			integrate.RaiseContentIdChanged();
		}

		XmlLayoutSerializer serializer = new(DockingManager);
		serializer.Serialize(LayoutPath);

		Position.Top = window.Top;
		Position.Left = window.Left;
		Position.Height = window.Height;
		Position.Width = window.Width;
		Position.WindowState = window.WindowState;

		File.WriteAllText(PositionPath, JsonSerializer.Serialize(Position, new JsonSerializerOptions()
		{
			WriteIndented = true
		}));

		IEnumerable<FormIntegrate.WindowInfo> integrateWindows = Views
			.OfType<FormIntegrateViewModel>()
			.Select(i => (i.WinformsControl! as FormIntegrate)!.WindowData);

		byte[]? data = MessagePackSerializer.Serialize(integrateWindows);

		File.WriteAllText(IntegratePath, MessagePackSerializer.ConvertToJson(data));
	}

	[RelayCommand]
	public void LoadLayout(object? sender)
	{
		if (sender is not FormMainWpf window) return;
		if (Path.GetExtension(LayoutPath) is ".zip")
		{
			Config.Life.LayoutFilePath = DefaultLayoutPath;
		}
		if (!File.Exists(LayoutPath)) return;

		DockingManager.Layout = new LayoutRoot();

		if (File.Exists(IntegratePath) && WindowCapture.WinformsControl is FormWindowCapture capture)
		{
			try
			{
				capture.CloseAll();

				string integrateString = File.ReadAllText(IntegratePath);
				byte[]? data = MessagePackSerializer.ConvertFromJson(integrateString);

				IEnumerable<FormIntegrate.WindowInfo> integrateWindows = MessagePackSerializer
					.Deserialize<IEnumerable<FormIntegrate.WindowInfo>>(data);

				foreach (FormIntegrate.WindowInfo info in integrateWindows)
				{
					// the constructor captures it so no need to call AddCapturedWindow
					FormIntegrate integrate = new(this, info);
					// capture.AddCapturedWindow(integrate);
				}

				capture.AttachAll();
			}
			catch
			{
				Logger.Add(3, FormMain.WindowCaptureLoadFailed);
			}
		}

		try
		{
			XmlLayoutSerializer serializer = new(DockingManager);
			serializer.Deserialize(LayoutPath);
		}
		catch
		{
			if (MessageBox.Show(FormMain.LayoutLoadFailed, FormMain.LayoutLoadFailedTitle,
					MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes)
				== MessageBoxResult.Yes)
			{
				OpenLink(@"https://github.com/ElectronicObserverEN/ElectronicObserver/issues/71");
			}
		}

		if (File.Exists(PositionPath))
		{
			try
			{
				Position = JsonSerializer.Deserialize<WindowPosition>(File.ReadAllText(PositionPath)) ?? new WindowPosition();
			}
			catch
			{
				// couldn't get position, keep the default
			}
		}

		window.Top = Position.Top;
		window.Left = Position.Left;
		window.Width = Position.Width;
		window.Height = Position.Height;
		window.WindowState = Position.WindowState;

		SetAnchorableProperties();
	}

	[RelayCommand]
	private void OpenLayout()
	{
		string? newLayoutPath = FileService.OpenLayoutPath(Configuration.Config.Life.LayoutFilePath);

		if (newLayoutPath is null) return;

		string oldLayoutPath = Configuration.Config.Life.LayoutFilePath;
		Configuration.Config.Life.LayoutFilePath = newLayoutPath;

		try
		{
			LoadLayout(Window);
		}
		catch
		{
			try
			{
				Configuration.Config.Life.LayoutFilePath = oldLayoutPath;
				LoadLayout(Window);
			}
			catch
			{
				// can't really do anything if the old layout is broken for some reason
			}
		}
	}

	[RelayCommand]
	private void SaveLayoutAs()
	{
		string? newLayoutPath = FileService.SaveLayoutPath(Configuration.Config.Life.LayoutFilePath);

		if (newLayoutPath is null) return;

		Configuration.Config.Life.LayoutFilePath = newLayoutPath;
		SaveLayout(Window);
	}

	/// <summary>
	/// It seems like when you try to open a view and nothing happens,
	/// it's because the view is already open but located far off-screen? (left = 3000+)
	/// Seems like windows can get randomly moved all over the place after turning your monitor on.
	/// </summary>
	[RelayCommand]
	private void ResetFloatingWindows()
	{
		foreach (LayoutFloatingWindowControl window in DockingManager.FloatingWindows)
		{
			window.Left = 0;
			window.Top = 0;
		}
	}

	[RelayCommand]
	private void SilenceNotifications()
	{
		foreach (var n in NotifierManager.Instance.GetNotifiers())
			n.IsSilenced = NotificationsSilenced;
	}

	[RelayCommand]
	private void OpenConfiguration()
	{
		UpdatePlayTime();

		new ConfigurationWindow(new()).ShowDialog(Window);
	}

	#endregion

	#region View

	[RelayCommand]
	private void OpenView(AnchorableViewModel view)
	{
		view.Visibility = Visibility.Visible;
		view.IsSelected = true;
		view.IsActive = true;
	}

	[RelayCommand]
	private void OpenOldShipGroup()
	{
		// FormShipGroup.Visibility = Visibility.Visible;
		// FormShipGroup.IsSelected = true;
		// FormShipGroup.IsActive = true;
	}

	[RelayCommand]
	private void OpenNewShipGroup()
	{
		ShipGroup.Visibility = Visibility.Visible;
		ShipGroup.IsSelected = true;
		ShipGroup.IsActive = true;
	}

	[RelayCommand]
	public void CloseIntegrate(FormIntegrateViewModel integrate)
	{
		if (integrate.WinformsControl is FormIntegrate i)
		{
			i.Detach();
			if (WindowCapture.WinformsControl is FormWindowCapture capture)
			{
				capture.CapturedWindows.Remove(i);
			}
		}

		Views.Remove(integrate);

		// AvalonDock always hides anchorables, but integrate anchorables should always be closed
		List<LayoutAnchorable> integrateAnchorables = DockingManager.Layout.Hidden
			.Where(a => a.ContentId.StartsWith(FormIntegrate.Prefix))
			.ToList();

		foreach (LayoutAnchorable anchorable in integrateAnchorables)
		{
			DockingManager.Layout.Hidden.Remove(anchorable);
		}
	}

	[RelayCommand]
	private void StripMenu_WindowCapture_AttachAll_Click()
	{
		if (WindowCapture is not { WinformsControl: FormWindowCapture fwc }) return;

		fwc.AttachAll();
	}

	[RelayCommand]
	private void StripMenu_WindowCapture_DetachAll_Click()
	{
		if (WindowCapture is not { WinformsControl: FormWindowCapture fwc }) return;

		fwc.DetachAll();
	}

	#endregion

	#region Tools

	[RelayCommand]
	private void OpenEquipmentList()
	{
		new EquipmentListWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenSortieRecord()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		new SortieRecordViewerWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenExpeditionRecordViewer()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		new ExpeditionRecordViewerWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenDropRecord()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (RecordManager.Instance.ShipDrop.Record.Count == 0)
		{
			MessageBox.Show(GeneralRes.NoDevData, MainResources.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		new DropRecordViewerWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenDevelopmentRecord()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (RecordManager.Instance.Development.Record.Count == 0)
		{
			MessageBox.Show(GeneralRes.NoDevData, MainResources.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		new DevelopmentRecordViewerWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenConstructionRecord()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (RecordManager.Instance.Construction.Record.Count == 0)
		{
			MessageBox.Show(GeneralRes.NoBuildData, MainResources.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		new ConstructionRecordViewerWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenResourceChart()
	{
		new ResourceChartWPF().Show(Window);
	}

	[RelayCommand]
	private void OpenSenkaViewer()
	{
		new SenkaViewerWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenAlbumMasterShip()
	{

		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(MainResources.ShipDataNotLoaded, MainResources.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		DialogAlbumMasterShipWpf albumMasterShip = new();
		RefreshTopMost();
		albumMasterShip.Show(Window);
	}

	[RelayCommand]
	private void OpenAlbumMasterEquipment()
	{

		if (KCDatabase.Instance.MasterEquipments.Count == 0)
		{
			MessageBox.Show(MainResources.EquipmentDataNotLoaded, MainResources.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		DialogAlbumMasterEquipmentWpf dialogAlbumMasterEquipment = new();
		RefreshTopMost();
		dialogAlbumMasterEquipment.Show(Window);
	}

	[RelayCommand]
	private void OpenAntiAirDefense()
	{
		if (!KCDatabase.Instance.Fleet.IsAvailable)
		{
			MessageBox.Show
			(
				AntiAirDefenseResources.DataNotLoaded,
				AntiAirDefenseResources.Error,
				MessageBoxButton.OK, MessageBoxImage.Error
			);

			return;
		}

		new AirDefenseWindow(new()).Show(Window);
	}

	[RelayCommand]
	private void OpenFleetImageGenerator()
	{
		ToolService.FleetImageGenerator();
	}

	[RelayCommand]
	private void OpenAirControlSimulator()
	{
		ToolService.AirControlSimulator();
	}

	[RelayCommand]
	private void OpenOperationRoom()
	{
		ToolService.OperationRoom();
	}

	[RelayCommand]
	private void OpenCompassPrediction()
	{
		ToolService.CompassPrediction();
	}

	[RelayCommand]
	private void OpenExpChecker()
	{
		ToolService.ExpChecker();
	}

	[RelayCommand]
	private void OpenKancolleProgress()
	{
		new DialogKancolleProgressWpf().Show(Window);
	}

	[RelayCommand]
	private void OpenExtraBrowser()
	{
		ElectronicObserver.Window.FormBrowserHost.Instance.Browser.OpenExtraBrowser();
	}

	[RelayCommand]
	private void OpenQuestTrackerManager()
	{
		if (!KCDatabase.Instance.Quest.IsLoaded)
		{
			MessageBox.Show(MainResources.QuestDataNotLoaded, MainResources.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		new QuestTrackerManagerWindow().Show(Window);
	}

	[RelayCommand]
	private void OpenEquipmentUpgradePlanner()
	{
		ToolService.EquipmentUpgradePlanner();
	}

	[RelayCommand]
	private void OpenEventLockPlanner()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(MainResources.ShipDataNotLoaded, MainResources.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		if (EventLockPlannerWindow is not null) return;

		EventLockPlannerViewModel viewModel = new(KCDatabase.Instance.Ships.Values, KCDatabase.Instance.Translation.Lock);
		EventLockPlannerWindow = new(viewModel);

		EventLockPlannerWindow.Closed += (sender, args) =>
		{
			EventLockPlannerWindow = null;
		};

		EventLockPlannerWindow.Show(Window);
	}

	[RelayCommand]
	private void OpenAutoRefresh()
	{
		if (AutoRefreshWindow is not null) return;

		AutoRefreshViewModel viewModel = Ioc.Default.GetRequiredService<AutoRefreshViewModel>();
		viewModel.Areas = KCDatabase.Instance.MapArea.Values.ToList();
		viewModel.Infos = KCDatabase.Instance.MapInfo.Values.ToList();

		AutoRefreshWindow = new(viewModel);

		AutoRefreshWindow.Closed += (sender, args) =>
		{
			AutoRefreshWindow = null;
		};

		AutoRefreshWindow.Show(Window);
	}

	[RelayCommand]
	private void OpenExpeditionCalculator()
	{
		new ExpeditionCalculatorWindow(new()).Show();
	}

	[RelayCommand]
	private void OpenUiBlockerManager()
	{
		UiBlockerWindow blocker = new(UiBlockerManager);
		blocker.Show(Window);
	}

	[RelayCommand]
	private void OpenDatabaseExplorer()
	{
		new DatabaseExplorerWindow().Show(Window);
	}

	#endregion

	#region Debug

	[RelayCommand]
	private void LoadAPIFromFile()
	{

		/*/
		using ( var dialog = new DialogLocalAPILoader() ) {

			if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
				if ( APIObserver.Instance.APIList.ContainsKey( dialog.APIName ) ) {

					if ( dialog.IsResponse ) {
						APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
					}
					if ( dialog.IsRequest ) {
						APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
					}

				}
			}
		}
		/*/
		new DialogLocalAPILoader2().Show(Window);
		//*/
	}

	[RelayCommand]
	private async Task LoadInitialAPI()
	{
		using OpenFileDialog ofd = new();

		ofd.Title = "Load API List";
		ofd.Filter = "API List|*.txt|File|*";
		ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;
		if (!string.IsNullOrWhiteSpace(Utility.Configuration.Config.Debug.APIListPath))
			ofd.FileName = Utility.Configuration.Config.Debug.APIListPath;

		if (ofd.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			try
			{

				await Task.Factory.StartNew(() => LoadAPIList(ofd.FileName));

			}
			catch (Exception ex)
			{

				MessageBox.Show("Failed to load API List.\r\n" + ex.Message, MainResources.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);

			}

		}
	}

	[RelayCommand]
	private void LoadAPIList(string path)
	{

		string parent = Path.GetDirectoryName(path);

		using StreamReader sr = new(path);
		string line;
		while ((line = sr.ReadLine()) != null)
		{

			bool isRequest = false;
			{
				int slashindex = line.IndexOf('/');
				if (slashindex != -1)
				{

					switch (line.Substring(0, slashindex).ToLower())
					{
						case "q":
						case "request":
							isRequest = true;
							goto case "s";
						case "":
						case "s":
						case "response":
							line = line.Substring(Math.Min(slashindex + 1, line.Length));
							break;
					}

				}
			}

			if (APIObserver.Instance.APIList.ContainsKey(line))
			{
				APIBase api = APIObserver.Instance.APIList[line];

				if (isRequest ? api.IsRequestSupported : api.IsResponseSupported)
				{

					string[] files = Directory.GetFiles(parent,
						string.Format("*{0}@{1}.json", isRequest ? "Q" : "S", line.Replace('/', '@')),
						SearchOption.TopDirectoryOnly);

					if (files.Length == 0)
						continue;

					Array.Sort(files);

					using StreamReader sr2 = new(files[files.Length - 1]);
					if (isRequest)
					{
						Window.Dispatcher.Invoke((Action)(() =>
						{
							APIObserver.Instance.LoadRequest("/kcsapi/" + line, sr2.ReadToEnd());
						}));
					}
					else
					{
						Window.Dispatcher.Invoke((Action)(() =>
						{
							APIObserver.Instance.LoadResponse("/kcsapi/" + line, sr2.ReadToEnd());
						}));
					}

					//System.Diagnostics.Debug.WriteLine( "APIList Loader: API " + line + " File " + files[files.Length-1] + " Loaded." );
				}
			}
		}
	}

	/// <summary>
	/// Load some API files if they are saved
	/// </summary>
	[RelayCommand]
	private async Task LoadBaseAPI()
	{
		if (string.IsNullOrEmpty(Config.Connection.SaveDataPath)) return;
		if (!Directory.Exists(Config.Connection.SaveDataPath)) return;

		try
		{
			await LoadAPIResponse("api_start2/getData");
			await LoadAPIResponse("api_get_member/require_info");
			await LoadAPIResponse("api_port/port");
			await LoadAPIResponse("api_get_member/mapinfo");
			await LoadAPIResponse("api_get_member/questlist");
		}
		catch (Exception ex)
		{
			Logger.Add(3, LoggerRes.FailedLoadAPI + ex.Message);
		}
	}

	private async Task LoadAPIResponse(string apiName)
	{
		if (string.IsNullOrEmpty(Config.Connection.SaveDataPath)) return;
		if (!Directory.Exists(Config.Connection.SaveDataPath)) return;

		if (!APIObserver.Instance.APIList.ContainsKey(apiName)) return;

		APIBase api = APIObserver.Instance.APIList[apiName];

		if (!api.IsResponseSupported) return;

		string filePath = Path.Combine(Config.Connection.SaveDataPath, "kcsapi", apiName);

		if (!File.Exists(filePath)) return;

		string data = await File.ReadAllTextAsync(filePath);

		Window.Dispatcher.Invoke((() =>
		{
			APIObserver.Instance.LoadResponse($"/kcsapi/{apiName}", data);
		}));
	}

	[RelayCommand]
	private void LoadRecordFromOld()
	{

		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show("Please load normal api_start2 first.", MainResources.ErrorCaption,
				MessageBoxButton.OK,
				MessageBoxImage.Information);
			return;
		}


		using OpenFileDialog ofd = new();

		ofd.Title = "Build Record from Old api_start2";
		ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";

		if (ofd.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			try
			{
				using StreamReader sr = new(ofd.FileName);
				dynamic json = JsonObject.Parse(sr.ReadToEnd().Remove(0, 7));

				foreach (dynamic elem in json.api_data.api_mst_ship)
				{
					if (elem.api_name != "なし" && KCDatabase.Instance.MasterShips.ContainsKey((int)elem.api_id) &&
						KCDatabase.Instance.MasterShips[(int)elem.api_id].Name == elem.api_name)
					{
						RecordManager.Instance.ShipParameter.UpdateParameter((int)elem.api_id, 1,
							(int)elem.api_tais[0], (int)elem.api_tais[1], (int)elem.api_kaih[0],
							(int)elem.api_kaih[1], (int)elem.api_saku[0], (int)elem.api_saku[1]);

						int[] defaultslot = Enumerable.Repeat(-1, 5).ToArray();
						((int[])elem.api_defeq).CopyTo(defaultslot, 0);
						RecordManager.Instance.ShipParameter.UpdateDefaultSlot((int)elem.api_id, defaultslot);
					}
				}
			}
			catch (Exception ex)
			{

				MessageBox.Show("Failed to load API.\r\n" + ex.Message, MainResources.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}

	[RelayCommand]
	private void LoadDataFromOld()
	{

		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show("Please load normal api_start2 first.", MainResources.ErrorCaption,
				MessageBoxButton.OK,
				MessageBoxImage.Information);
			return;
		}


		using OpenFileDialog ofd = new();

		ofd.Title = "Restore Abyssal Data from Old api_start2";
		ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";
		ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

		if (ofd.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			try
			{

				using (StreamReader sr = new(ofd.FileName))
				{

					dynamic json = JsonObject.Parse(sr.ReadToEnd().Remove(0, 7));

					foreach (dynamic elem in json.api_data.api_mst_ship)
					{
						IShipDataMaster ship = KCDatabase.Instance.MasterShips[(int)elem.api_id];

						if (elem.api_name != "なし" && ship is { IsAbyssalShip: true })
						{

							KCDatabase.Instance.MasterShips[(int)elem.api_id].LoadFromResponse("api_start2", elem);
						}
					}
				}

				Utility.Logger.Add(1, "Restored data from old api_start2");

			}
			catch (Exception ex)
			{

				MessageBox.Show("Failed to load API.\r\n" + ex.Message, MainResources.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}

	[RelayCommand]
	private async Task DeleteOldAPI()
	{
		if (MessageBox.Show("This will delete old API data.\r\nAre you sure?", "Confirmation",
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
			== MessageBoxResult.Yes)
		{
			try
			{
				int count = await Task.Factory.StartNew(DeleteOldAPIInternal);

				MessageBox.Show("Delete successful.\r\n" + count + " files deleted.", "Delete Successful",
					MessageBoxButton.OK, MessageBoxImage.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to delete.\r\n" + ex.Message, MainResources.ErrorCaption,
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}
		}
	}

	private int DeleteOldAPIInternal()
	{
		//適当極まりない
		int count = 0;

		Dictionary<string, List<KeyValuePair<string, string>>> apilist = new();

		foreach (string s in Directory.EnumerateFiles(Utility.Configuration.Config.Connection.SaveDataPath,
			"*.json", SearchOption.TopDirectoryOnly))
		{
			int start = s.IndexOf('@');
			int end = s.LastIndexOf('.');

			start--;
			string key = s.Substring(start, end - start + 1);
			string date = s.Substring(0, start);

			if (!apilist.ContainsKey(key))
			{
				apilist.Add(key, new List<KeyValuePair<string, string>>());
			}

			apilist[key].Add(new KeyValuePair<string, string>(date, s));
		}

		foreach (var l in apilist.Values)
		{
			var l2 = l.OrderBy(el => el.Key).ToList();
			for (int i = 0; i < l2.Count - 1; i++)
			{
				File.Delete(l2[i].Value);
				count++;
			}
		}

		return count;
	}

	[RelayCommand]
	private async Task RenameShipResource()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show("Ship data is not loaded.", MainResources.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (MessageBox.Show("通信から保存した艦船リソース名を持つファイル及びフォルダを、艦船名に置換します。\r\n" +
							"対象は指定されたフォルダ以下のすべてのファイル及びフォルダです。\r\n" +
							"続行しますか？", "艦船リソースをリネーム", MessageBoxButton.YesNo, MessageBoxImage.Question,
				MessageBoxResult.Yes)
			== MessageBoxResult.Yes)
		{

			string path = null;

			using (FolderBrowserDialog dialog = new())
			{
				dialog.SelectedPath = Configuration.Config.Connection.SaveDataPath;
				if (dialog.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
				{
					path = dialog.SelectedPath;
				}
			}

			if (path == null) return;



			try
			{

				int count = await Task.Factory.StartNew(() => RenameShipResource(path));

				MessageBox.Show(string.Format("リネーム処理が完了しました。\r\n{0} 個のアイテムをリネームしました。", count), "処理完了",
					MessageBoxButton.OK, MessageBoxImage.Information);


			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, "艦船リソースのリネームに失敗しました。");
				MessageBox.Show("艦船リソースのリネームに失敗しました。\r\n" + ex.Message, MainResources.ErrorCaption,
					MessageBoxButton.OK,
					MessageBoxImage.Error);

			}



		}

	}

	private int RenameShipResource(string path)
	{

		int count = 0;

		foreach (var p in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
		{

			string name = Path.GetFileName(p);

			foreach (var ship in KCDatabase.Instance.MasterShips.Values)
			{

				if (name.Contains(ship.ResourceName))
				{

					name = name.Replace(ship.ResourceName,
						string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

					try
					{

						File.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
						count++;
						break;

					}
					catch (IOException)
					{
						//ファイルが既に存在する：＊にぎりつぶす＊
					}

				}

			}

		}

		foreach (var p in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
		{

			string name = Path.GetFileName(p); //GetDirectoryName だと親フォルダへのパスになってしまうため

			foreach (var ship in KCDatabase.Instance.MasterShips.Values)
			{

				if (name.Contains(ship.ResourceName))
				{

					name = name.Replace(ship.ResourceName,
						string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

					try
					{

						Directory.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
						count++;
						break;

					}
					catch (IOException)
					{
						//フォルダが既に存在する：＊にぎりつぶす＊
					}
				}

			}

		}


		return count;
	}

	[RelayCommand]
	private async Task GenerateMasterData()
	{
#if DEBUG
		void GetMissingDataFromWiki(IShipDataMaster ship, Dictionary<ShipId, IShipDataMaster> wikiShips)
		{
			if (!wikiShips.TryGetValue(ship.ShipId, out IShipDataMaster? wikiShip)) return;

			if (wikiShip.ASW.Minimum >= 0)
			{
				ship.ASW.MinimumEstMin = wikiShip.ASW.Minimum;
				ship.ASW.MinimumEstMax = wikiShip.ASW.Minimum;
			}

			if (wikiShip.ASW.Maximum >= 0)
			{
				ship.ASW.Maximum = wikiShip.ASW.Maximum;
			}

			if (wikiShip.LOS.Minimum >= 0)
			{
				ship.LOS.MinimumEstMin = wikiShip.LOS.Minimum;
				ship.LOS.MinimumEstMax = wikiShip.LOS.Minimum;
			}

			if (wikiShip.LOS.Maximum >= 0)
			{
				ship.LOS.Maximum = wikiShip.LOS.Maximum;
			}

			if (wikiShip.Evasion.Minimum >= 0)
			{
				ship.Evasion.MinimumEstMin = wikiShip.Evasion.Minimum;
				ship.Evasion.MinimumEstMax = wikiShip.Evasion.Minimum;
			}

			if (wikiShip.Evasion.Maximum >= 0)
			{
				ship.Evasion.Maximum = wikiShip.Evasion.Maximum;
			}

			if (wikiShip.DefaultSlot is not null)
			{
				RecordManager.Instance.ShipParameter.UpdateDefaultSlot(ship.ShipID, wikiShip.DefaultSlot.ToArray());
			}
		}

		void GetMissingAbyssalDataFromWiki(IShipDataMaster ship, Dictionary<ShipId, IShipDataMaster> wikiAbyssalShips, List<int> abyssalAicraft)
		{
			if (!wikiAbyssalShips.TryGetValue(ship.ShipId, out IShipDataMaster? wikiShip)) return;

			if (!ship.ASW.IsDetermined)
			{
				if (wikiShip.ASW.Minimum >= 0)
				{
					ship.ASW.MinimumEstMin = wikiShip.ASW.MinimumEstMin;
					ship.ASW.MinimumEstMax = wikiShip.ASW.MinimumEstMax;
				}

				if (wikiShip.ASW.Maximum >= 0)
				{
					ship.ASW.Maximum = wikiShip.ASW.Maximum;
				}
			}

			if (!ship.LOS.IsDetermined)
			{
				if (wikiShip.LOS.Minimum >= 0)
				{
					ship.LOS.MinimumEstMin = wikiShip.LOS.MinimumEstMin;
					ship.LOS.MinimumEstMax = wikiShip.LOS.MinimumEstMax;
				}

				if (wikiShip.LOS.Maximum >= 0)
				{
					ship.LOS.Maximum = wikiShip.LOS.Maximum;
				}
			}

			if (!ship.Evasion.IsDetermined)
			{
				if (wikiShip.Evasion.Minimum >= 0)
				{
					ship.Evasion.MinimumEstMin = wikiShip.Evasion.MinimumEstMin;
					ship.Evasion.MinimumEstMax = wikiShip.Evasion.MinimumEstMax;
				}

				if (wikiShip.Evasion.Maximum >= 0)
				{
					ship.Evasion.Maximum = wikiShip.Evasion.Maximum;
				}
			}

			if (wikiShip.DefaultSlot is not null)
			{
				RecordManager.Instance.ShipParameter.UpdateDefaultSlot(ship.ShipID, wikiShip.DefaultSlot.ToArray());
			}

			RecordManager.Instance.ShipParameter.UpdateAircraft(ship.ShipID, abyssalAicraft.ToArray());

			ShipParameterRecord.ShipParameterElement e = RecordManager.Instance.ShipParameter[ship.ShipID] ?? new();

			e.HPMin = wikiShip.HPMin;
			e.HPMax = wikiShip.HPMax;
			e.FirepowerMin = wikiShip.FirepowerMin;
			e.FirepowerMax = wikiShip.FirepowerMax;
			e.TorpedoMin = wikiShip.TorpedoMin;
			e.TorpedoMax = wikiShip.TorpedoMax;
			e.AAMin = wikiShip.AAMin;
			e.AAMax = wikiShip.AAMax;
			e.ArmorMin = wikiShip.ArmorMin;
			e.ArmorMax = wikiShip.ArmorMax;
			e.LuckMin = wikiShip.LuckMin;
			e.LuckMax = wikiShip.LuckMax;

			RecordManager.Instance.ShipParameter.Update(e);
		}

		Directory.CreateDirectory(GeneratedDataFolder);

		await using TestData.TestDataContext db = new();
		await db.Database.MigrateAsync();

		await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{nameof(TestData.TestDataContext.MasterShips)}]");
		await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{nameof(TestData.TestDataContext.MasterEquipment)}]");

		List<IEquipmentDataMaster> wikiEquipment = TestData.Wiki.WikiDataParser.Equipment();
		List<IEquipmentDataMaster> wikiAbyssalEquipment = TestData.Wiki.WikiDataParser.AbyssalEquipment();

		Dictionary<ShipId, IShipDataMaster> wikiShips = TestData.Wiki.WikiDataParser.Ships(wikiEquipment);
		Dictionary<ShipId, IShipDataMaster> wikiAbyssalShips = TestData.Wiki.WikiDataParser.AbyssalShips(wikiAbyssalEquipment);
		Dictionary<ShipId, List<int>> abyssalAircraft = await TestData.AirControlSimulator.AirControlSimulatorDataParser.AbyssalShipAircraft();

		foreach (IShipDataMaster ship in KCDatabase.Instance.MasterShips.Values)
		{
			if (ship.IsAbyssalShip)
			{
				GetMissingAbyssalDataFromWiki(ship, wikiAbyssalShips, abyssalAircraft[ship.ShipId]);
			}
			else
			{
				GetMissingDataFromWiki(ship, wikiShips);
			}
			await db.MasterShips.AddAsync(new(ship));
		}

		foreach (IEquipmentDataMaster equipment in KCDatabase.Instance.MasterEquipments.Values)
		{
			await db.MasterEquipment.AddAsync(EquipmentDataMasterRecord.FromMasterEquipment(equipment));
		}

		await db.SaveChangesAsync();

		List<ShipDataMasterRecord> masterShips = db.MasterShips.ToList();
		List<EquipmentDataMasterRecord> masterEquipment = db.MasterEquipment.ToList();

		JsonSerializerOptions options = new()
		{
			WriteIndented = true,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		};

		await File.WriteAllTextAsync(Path.Combine(GeneratedDataFolder, "ships.json"), JsonSerializer.Serialize(masterShips, options));
		await File.WriteAllTextAsync(Path.Combine(GeneratedDataFolder, "equipment.json"), JsonSerializer.Serialize(masterEquipment, options));
#endif
	}

	[RelayCommand]
	private void GenerateShipIdEnum()
	{
		static string CleanName(string name) => name
			.Replace(" ", "")
			.Replace(")", "")
			.Replace("-", "")
			.Replace("/", "")
			.Replace(".", "")
			.Replace("(", "")
			.Replace("+", "")
			.Replace("'", "");

		List<string> enumValues = KCDatabase.Instance.MasterShips.Values
			.Where(e => !e.IsAbyssalShip)
			.Select(s => (Name: CleanName(s.NameEN), Id: s.ShipID))
			.GroupBy(t => t.Name)
			.SelectMany(g => g.Count() switch
			{
				// if the name is the same, append the ID (Souya)
				> 1 => g.Select(t => (Name: $"{t.Name}{t.Id}", t.Id)).ToList(),
				1 => new List<(string Name, int Id)> { (g.First().Name, g.First().Id) }
			})
			.OrderBy(t => t.Id)
			.Select(t => $"{t.Name} = {t.Id}")
			.ToList();

		System.Windows.Clipboard.SetText(string.Join(",\n", enumValues));
	}

	[RelayCommand]
	private void GenerateEquipmentIdEnum()
	{
		static string CleanName(string name) => name
			.Replace(" ", "")
			.Replace(")", "")
			.Replace("-", "")
			.Replace("/", "")
			.Replace(".", "_")
			.Replace("(", "_")
			.Replace("&", "_")
			.Replace("+", "_");

		List<string> enumValues = KCDatabase.Instance.MasterEquipments.Values
			.Where(e => !e.IsAbyssalEquipment)
			.Select(eq => $"{eq.CategoryType}_{CleanName(eq.NameEN)} = {eq.ID}")
			.ToList();

		System.Windows.Clipboard.SetText(string.Join(",\n", enumValues));
	}

	#endregion

	#region Help

	[RelayCommand]
	private void ViewHelp()
	{

		if (MessageBox.Show(MainResources.OpenEOWiki, MainResources.HelpCaption,
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
			== MessageBoxResult.Yes)
		{
			OpenLink(MainResources.GitHubWikiLink);
		}

	}

	[RelayCommand]
	private void ReportIssue()
	{

		if (MessageBox.Show(MainResources.ReportIssue, MainResources.ReportIssueCaption,
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
			== MessageBoxResult.Yes)
		{
			OpenLink("https://github.com/ElectronicObserverEN/ElectronicObserver/issues");
		}

	}

	[RelayCommand]
	private void JoinDiscord() => OpenLink("https://discord.gg/6ZvX8DG");

	[RelayCommand]
	private async Task CheckForUpdate()
	{
		// translations and maintenance state
		await SoftwareUpdater.CheckUpdateAsync();
		// EO
		SoftwareInformation.CheckUpdate();
	}

	[RelayCommand]
	private void ViewVersion()
	{
		VersionInformationWindow? window = new VersionInformationWindow();
		window.ShowDialog(Window);
	}

	#endregion

	#region Updates
	[RelayCommand]
	private void StartSoftwareUpdate()
		=> Task.Run(SoftwareUpdater.UpdateSoftware);

	[RelayCommand]
	private void OpenReleaseNotes()
		=> OpenLink("https://github.com/ElectronicObserverEN/ElectronicObserver/releases/latest");
	#endregion

	#region Maintenance timer
	[RelayCommand]
	private void OpenMaintenanceInformationLink()
		=> OpenLink(SoftwareUpdater.LatestVersion.MaintenanceInformationLink);
	#endregion

	private void CallPumpkinHead(string apiname, dynamic data)
	{
		new DialogHalloween().Show(Window);
		APIObserver.Instance.ApiPort_Port.ResponseReceived -= CallPumpkinHead;
	}


	void Logger_LogAdded(Utility.Logger.LogData data)
	{
		// bottom bar
		StripStatus.Information = data.Message.Replace("\r", " ").Replace("\n", " ");
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData c = Configuration.Config;

		DebugEnabled = c.Debug.EnableDebugMenu;

		StripStatus.Visible = c.Life.ShowStatusBar;

		// Load で TopMost を変更するとバグるため(前述)
		if (UIUpdateTimer.Enabled)
		{
			Topmost = c.Life.TopMost;
		}

		ClockFormat = c.Life.ClockFormat;
		SetTheme();

		// FormShipGroup.ShipGroup.BackColor = System.Drawing.SystemColors.Control;
		// FormShipGroup.ShipGroup.ForeColor = System.Drawing.SystemColors.ControlText;

		LockLayout = c.Life.LockLayout;
		CanChangeGridSplitterSize = !LockLayout;
		GridSplitterSize = LockLayout switch
		{
			true => 0,
			_ => 1,
		};
		SetAnchorableProperties();
		Topmost = c.Life.TopMost;

		if (!c.Control.UseSystemVolume)
		{
			VolumeUpdateState = -1;
		}
	}

	private void SetAnchorableProperties()
	{
		foreach (AnchorableViewModel view in Views)
		{
			view.CanFloat = !LockLayout;
			view.CanClose = !LockLayout;
		}
	}

	private void SetFont()
	{
		Font = new FontFamily(Config.UI.MainFont.FontData.FontFamily.Name);
		FontSize = Config.UI.MainFont.FontData.ToSize();
		FontBrush = Config.UI.ForeColor.ToBrush();

		SubFont = new FontFamily(Config.UI.SubFont.FontData.FontFamily.Name);
		SubFontSize = Config.UI.SubFont.FontData.ToSize();
		SubFontBrush = Config.UI.SubForeColor.ToBrush();
	}

	private void SetTheme()
	{
		// todo switching themes doesn't update everything in runtime
		Utility.Configuration.Instance.ApplyTheme();

		BackgroundColor = Config.UI.BackColor.ToBrush().Color;

		CurrentTheme = Utility.Configuration.Config.UI.ThemeMode switch
		{
			0 => Themes[0], // light theme => light theme
			1 => Themes[2], // dark theme => dark theme
			_ => Themes[2], // custom theme => dark theme
		};

		ThemeManager.Current.ApplicationTheme = Utility.Configuration.Config.UI.ThemeMode switch
		{
			0 => ApplicationTheme.Light, // light theme => light theme
			1 => ApplicationTheme.Dark, // dark theme => dark theme
			_ => ApplicationTheme.Dark, // custom theme => dark theme
		};

		SetFont();
	}

	private void UIUpdateTimer_Tick(object sender, EventArgs e)
	{

		SystemEvents.OnUpdateTimerTick();

		// 東京標準時
		DateTime now = DateTimeHelper.GetJapanStandardTimeNow();

		MaintenanceText = GetMaintenanceText(FormMain, now);
		UpdateAvailable = SoftwareInformation.UpdateTime < SoftwareUpdater.LatestVersion.BuildDate;

		DownloadProgressString = SoftwareUpdater.DownloadProgressString;

		switch (ClockFormat)
		{
			case 0: //時計表示
				var pvpReset = now.Date.AddHours(3);
				while (pvpReset < now)
					pvpReset = pvpReset.AddHours(12);
				var pvpTimer = pvpReset - now;

				var questReset = now.Date.AddHours(5);
				if (questReset < now)
					questReset = questReset.AddHours(24);
				var questTimer = questReset - now;

				var resetMsg =
					$"{FormMain.NextExerciseReset} {pvpTimer:hh\\:mm\\:ss}\r\n" +
					$"{FormMain.NextQuestReset} {questTimer:hh\\:mm\\:ss}\r\n" +
					$"{MaintenanceText}";

				StripStatus.Clock = now.ToString("HH\\:mm\\:ss");
				StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd)\r\n") + resetMsg;

				break;

			case 1: //演習更新まで
			{
				var border = now.Date.AddHours(3);
				while (border < now)
					border = border.AddHours(12);

				var ts = border - now;
				StripStatus.Clock = ts.ToString("hh\\:mm\\:ss");
				StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

			}
			break;

			case 2: //任務更新まで
			{
				var border = now.Date.AddHours(5);
				if (border < now)
					border = border.AddHours(24);

				var ts = border - now;
				StripStatus.Clock = ts.ToString("hh\\:mm\\:ss");
				StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

			}
			break;
		}

		// todo: I'm not sure if that's still an issue, but I don't want to figure it out right now
		// WMP コントロールによって音量が勝手に変えられてしまうため、前回終了時の音量の再設定を試みる。
		// 10回試行してダメなら諦める(例外によるラグを防ぐため)
		// 起動直後にやらないのはちょっと待たないと音量設定が有効にならないから
		if (VolumeUpdateState != -1 && VolumeUpdateState < 10 && Configuration.Config.Control.UseSystemVolume)
		{
			try
			{
				uint id = (uint)Environment.ProcessId;
				float volume = Configuration.Config.Control.LastVolume;
				bool mute = Configuration.Config.Control.LastIsMute;

				BrowserLibCore.VolumeManager.SetApplicationVolume(id, volume);
				BrowserLibCore.VolumeManager.SetApplicationMute(id, mute);

				SyncBGMPlayer.Instance.SetInitialVolume((int)(volume * 100));
				foreach (NotifierBase not in NotifierManager.Instance.GetNotifiers())
				{
					not.Sound.Volume = ((int)(volume * 100));
				}

				VolumeUpdateState = -1;

			}
			catch (Exception)
			{
				VolumeUpdateState++;
			}
		}
	}

	private static string GetMaintenanceText(FormMainTranslationViewModel formMain, DateTime now)
	{
		TimeSpan maintTimer = new(0);
		MaintenanceState eventState = SoftwareUpdater.LatestVersion.EventState;

		DateTime maintStartDate = SoftwareUpdater.LatestVersion.MaintenanceStart;
		DateTime? maintEndDate = SoftwareUpdater.LatestVersion.MaintenanceEnd;

		if (eventState != MaintenanceState.None)
		{
			if (maintStartDate > now)
			{
				maintTimer = maintStartDate - now;
			}
			else if (maintEndDate > now && maintEndDate is { } end)
			{
				maintTimer = end - now;
			}
		}

		string message = (eventState, maintStartDate > now, maintEndDate > now, maintEndDate) switch
		{
			(not MaintenanceState.None, false, _, null) => formMain.MaintenanceHasStarted,

			(MaintenanceState.EventStart, true, true, _) => formMain.MaintenanceStartsIn,
			(MaintenanceState.EventStart, false, true, _) => formMain.EventStartsIn,
			(MaintenanceState.EventStart, false, false, _) => formMain.EventHasStarted,

			(MaintenanceState.EventEnd, true, true, _) => formMain.EventEndsIn,
			(MaintenanceState.EventEnd, false, _, _) => formMain.EventHasEnded,

			(MaintenanceState.Regular, true, true, _) => formMain.MaintenanceStartsIn,
			(MaintenanceState.Regular, false, true, _) => formMain.MaintenanceEndsIn,
			(MaintenanceState.Regular, false, false, _) => formMain.MaintenanceHasEnded,

			_ => string.Empty,
		};

		return string.Format(message, $"{maintTimer:d\\.hh\\:mm\\:ss}");
	}

	private void UpdatePlayTime()
	{
		var c = Configuration.Config.Log;
		DateTime now = DateTime.Now;

		double span = (now - PrevPlayTimeRecorded).TotalSeconds;
		if (span < c.PlayTimeIgnoreInterval)
		{
			c.PlayTime += span;
		}

		PrevPlayTimeRecorded = now;
	}

	private void OpenLink(string link)
	{
		try
		{
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = link,
				UseShellExecute = true
			};
			Process.Start(psi);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, MainResources.FailedToOpenBrowser);
		}
	}

	[RelayCommand]
	private void Closing(CancelEventArgs e)
	{
		string name = CultureInfo.CurrentCulture.Name switch
		{
			"en-US" => SoftwareInformation.SoftwareNameEnglish,
			_ => SoftwareInformation.SoftwareNameJapanese
		};

		if (Configuration.Config.Life.ConfirmOnClosing)
		{
			if (MessageBox.Show(
					string.Format(MainResources.ExitConfirmation, name),
					MainResources.ConfirmatonCaption,
					MessageBoxButton.YesNo,
					MessageBoxImage.Question,
					MessageBoxResult.No)
				== MessageBoxResult.No)
			{
				e.Cancel = true;
				return;
			}
		}

		Logger.Add(2, name + Resources.IsClosing);

		UIUpdateTimer.Stop();

		if (FormBrowserHost.WinformsControl is FormBrowserHost host)
		{
			host.CloseBrowser();
		}

		UpdatePlayTime();
		SystemEvents.OnSystemShuttingDown();

		// SaveLayout(Configuration.Config.Life.LayoutFilePath);

		// 音量の保存
		try
		{
			uint id = (uint)Environment.ProcessId;
			Configuration.Config.Control.LastVolume = BrowserLibCore.VolumeManager.GetApplicationVolume(id);
			Configuration.Config.Control.LastIsMute = BrowserLibCore.VolumeManager.GetApplicationMute(id);

		}
		catch (Exception)
		{
			/* ぷちっ */
		}
	}

	[RelayCommand]
	private void Closed()
	{
		NotifierManager.Instance.ApplyToConfiguration();
		Configuration.Instance.Save();
		RecordManager.Instance.SavePartial();
		KCDatabase.Instance.Save();
		APIObserver.Instance.Stop();
		Ioc.Default.GetService<Tracker>().PersistAll();

		Logger.Add(2, Resources.ClosingComplete);

		if (Configuration.Config.Log.SaveLogFlag)
			Logger.Save();
	}
}
