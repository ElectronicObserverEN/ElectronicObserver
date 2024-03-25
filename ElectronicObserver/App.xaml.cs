using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.ElectronicObserverApi;
using ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control.ShipFilter;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserver.Window.Settings;
using ElectronicObserver.Window.Settings.Behavior;
using ElectronicObserver.Window.Settings.BGM;
using ElectronicObserver.Window.Settings.Connection;
using ElectronicObserver.Window.Settings.Debugging;
using ElectronicObserver.Window.Settings.Log;
using ElectronicObserver.Window.Settings.Notification;
using ElectronicObserver.Window.Settings.Notification.Base;
using ElectronicObserver.Window.Settings.SubWindow;
using ElectronicObserver.Window.Settings.SubWindow.AirBase;
using ElectronicObserver.Window.Settings.SubWindow.Arsenal;
using ElectronicObserver.Window.Settings.SubWindow.Browser;
using ElectronicObserver.Window.Settings.SubWindow.Combat;
using ElectronicObserver.Window.Settings.SubWindow.Compass;
using ElectronicObserver.Window.Settings.SubWindow.Dock;
using ElectronicObserver.Window.Settings.SubWindow.Fleet;
using ElectronicObserver.Window.Settings.SubWindow.Group;
using ElectronicObserver.Window.Settings.SubWindow.Headquarters;
using ElectronicObserver.Window.Settings.SubWindow.Json;
using ElectronicObserver.Window.Settings.SubWindow.Quest;
using ElectronicObserver.Window.Settings.SubWindow.ShipTraining;
using ElectronicObserver.Window.Settings.UI;
using ElectronicObserver.Window.Settings.Window;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using ElectronicObserver.Window.Tools.AirDefense;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.ConstructionRecordViewer;
using ElectronicObserver.Window.Tools.DevelopmentRecordViewer;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserver.Window.Tools.DropRecordViewer;
using ElectronicObserver.Window.Tools.EquipmentList;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserver.Window.Tools.ExpChecker;
using ElectronicObserver.Window.Tools.ExpeditionRecordViewer;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserver.Window.Tools.SenkaViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserver.Window.Tools.Telegram;
using ElectronicObserver.Window.Wpf;
using ElectronicObserver.Window.Wpf.EquipmentUpgradePlanViewer;
using ElectronicObserver.Window.Wpf.ExpeditionCheck;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
using ElectronicObserverTypes.Data;
using Jot;
using Jot.Storage;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public new static App? Current => (App)Application.Current;

	public App()
	{
		this.InitializeComponent();

		DispatcherUnhandledException += (sender, args) =>
		{
			if (args.Exception.StackTrace?.Contains("AvalonDock.Controls.TransformExtensions.TransformActualSizeToAncestor") ?? false)
			{
				// hack: ignore the exception when trying to resize the autohide area
				args.Handled = true;
				return;
			}

			// https://stackoverflow.com/questions/12769264/openclipboard-failed-when-copy-pasting-data-from-wpf-datagrid
			const int CLIPBRD_E_CANT_OPEN = unchecked((int)0x800401D0);

			if (args.Exception is not COMException { ErrorCode: CLIPBRD_E_CANT_OPEN }) return;

			Logger.Add(3, MainResources.CopyingToClipboardFailed);
			args.Handled = true;
		};
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		bool allowMultiInstance = e.Args.Contains("-m") || e.Args.Contains("--multi-instance");


		using (var mutex = new Mutex(false, Application.ResourceAssembly.Location.Replace('\\', '/'),
			out var created))
		{

			/*
			bool hasHandle = false;

			try
			{
				hasHandle = mutex.WaitOne(0, false);
			}
			catch (AbandonedMutexException)
			{
				hasHandle = true;
			}
			*/

			if (!created && !allowMultiInstance)
			{
				System.Windows.Window temp = new() { Visibility = Visibility.Hidden };
				temp.Show();

				string caption = CultureInfo.CurrentCulture.Name switch
				{
					"ja-JP" => SoftwareInformation.SoftwareNameJapanese,
					_ => SoftwareInformation.SoftwareNameEnglish
				};

				// 多重起動禁止
				MessageBox.Show
				(
					ElectronicObserver.Translations.Resources.MultiInstanceNotification,
					caption,
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation
				);

				Shutdown();
				return;
			}

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			// hack: needed for running the winforms version
			// remove this and the Shutdown call when moving to wpf only
			// ShutdownMode = ShutdownMode.OnExplicitShutdown;

#if !DEBUG
			AppCenter.Start("7fdbafa0-058a-4691-b317-a700be513b95", typeof(Analytics), typeof(Crashes));
#endif

			Task.Run(() =>
			{
				// pre-load ef model to avoid performance hits later
				using ElectronicObserverContext db = new();
				_ = db.Model;
			});

			try
			{
				Directory.CreateDirectory(@"Settings\Layout");
			}
			catch (UnauthorizedAccessException)
			{
				MessageBox.Show(MainResources.MissingPermissions,
					MainResources.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);
				throw;
			}

			Configuration.Instance.Load();

			ConfigureServices();

			ToolTipService.ShowDurationProperty.OverrideMetadata(
				typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));
			ToolTipService.InitialShowDelayProperty.OverrideMetadata(
				typeof(DependencyObject), new FrameworkPropertyMetadata(0));

			UpdateFont();
			Configuration.Instance.ConfigurationChanged += UpdateFont;

			FormMainWpf mainWindow = new();

			MainWindow = mainWindow;
			ShutdownMode = ShutdownMode.OnMainWindowClose;

			mainWindow.ShowDialog();

			// Shutdown();
		}
	}

	private void UpdateFont()
	{
		FontOverrides? overrides = Resources.MergedDictionaries
			.OfType<FontOverrides>()
			.FirstOrDefault();

		if (overrides is null) return;

		overrides.FontFamily = new FontFamily(Configuration.Config.UI.MainFont.FontData.Name);
		overrides.FontSize = Configuration.Config.UI.MainFont.FontData.ToSize();
	}

	private void ConfigureServices()
	{
		ServiceProvider services = new ServiceCollection()
			.AddSingleton<IKCDatabase>(KCDatabase.Instance)
			// config translations
			.AddSingleton<ConfigurationTranslationViewModel>()
			.AddSingleton<ConfigurationConnectionTranslationViewModel>()
			.AddSingleton<ConfigurationUITranslationViewModel>()
			.AddSingleton<ConfigurationLogTranslationViewModel>()
			.AddSingleton<ConfigurationBehaviorTranslationViewModel>()
			.AddSingleton<ConfigurationDebugTranslationViewModel>()
			.AddSingleton<ConfigurationWindowTranslationViewModel>()
			.AddSingleton<ConfigurationSubWindowTranslationViewModel>()
			.AddSingleton<ConfigurationNotificationTranslationViewModel>()
			.AddSingleton<ConfigurationNotificationBaseTranslationViewModel>()
			.AddSingleton<ConfigurationBGMTranslationViewModel>()
			.AddSingleton<SoundHandleEditTranslationViewModel>()
			.AddSingleton<ConfigurationFleetTranslationViewModel>()
			.AddSingleton<ConfigurationArsenalTranslationViewModel>()
			.AddSingleton<ConfigurationDockTranslationViewModel>()
			.AddSingleton<ConfigurationHeadquartersTranslationViewModel>()
			.AddSingleton<ConfigurationCompassTranslationViewModel>()
			.AddSingleton<ConfigurationQuestTranslationViewModel>()
			.AddSingleton<ConfigurationGroupTranslationViewModel>()
			.AddSingleton<ConfigurationCombatTranslationViewModel>()
			.AddSingleton<ConfigurationBrowserTranslationViewModel>()
			.AddSingleton<ConfigurationAirBaseTranslationViewModel>()
			.AddSingleton<ConfigurationJsonTranslationViewModel>()
			.AddSingleton<ConfigurationShipTrainingTranslationViewModel>()
			// view translations
			.AddSingleton<FormArsenalTranslationViewModel>()
			.AddSingleton<FormBaseAirCorpsTranslationViewModel>()
			.AddSingleton<FormBattleTranslationViewModel>()
			.AddSingleton<FormBrowserHostTranslationViewModel>()
			.AddSingleton<FormCompassTranslationViewModel>()
			.AddSingleton<FormDockTranslationViewModel>()
			.AddSingleton<FormFleetTranslationViewModel>()
			.AddSingleton<FormFleetOverviewTranslationViewModel>()
			.AddSingleton<FormFleetPresetTranslationViewModel>()
			.AddSingleton<FormHeadquartersTranslationViewModel>()
			.AddSingleton<FormInformationTranslationViewModel>()
			.AddSingleton<FormJsonTranslationViewModel>()
			.AddSingleton<FormLogTranslationViewModel>()
			.AddSingleton<FormMainTranslationViewModel>()
			.AddSingleton<FormQuestTranslationViewModel>()
			.AddSingleton<FormShipGroupTranslationViewModel>()
			.AddSingleton<FormWindowCaptureTranslationViewModel>()
			.AddSingleton<EquipmentUpgradePlanViewerTranslationViewModel>()
			// tool translations
			.AddSingleton<DialogAlbumMasterShipTranslationViewModel>()
			.AddSingleton<DialogAlbumMasterEquipmentTranslationViewModel>()
			.AddSingleton<DialogDevelopmentRecordViewerTranslationViewModel>()
			.AddSingleton<SortieRecordViewerTranslationViewModel>()
			.AddSingleton<SortieCostViewerTranslationViewModel>()
			.AddSingleton<ExpeditionRecordViewerTranslationViewModel>()
			.AddSingleton<DialogDropRecordViewerTranslationViewModel>()
			.AddSingleton<DialogConstructionRecordViewerTranslationViewModel>()
			.AddSingleton<DialogResourceChartTranslationViewModel>()
			.AddSingleton<SenkaViewerTranslationViewModel>()
			.AddSingleton<DialogEquipmentListTranslationViewModel>()
			.AddSingleton<AirDefenseTranslationViewModel>()
			.AddSingleton<QuestTrackerManagerTranslationViewModel>()
			.AddSingleton<EventLockPlannerTranslationViewModel>()
			.AddSingleton<ShipFilterTranslationViewModel>()
			.AddSingleton<AirControlSimulatorTranslationViewModel>()
			.AddSingleton<AutoRefreshTranslationViewModel>()
			.AddSingleton<ExpeditionCheckTranslationViewModel>()
			.AddSingleton<FleetImageGeneratorTranslationViewModel>()
			.AddSingleton<ExpCheckerTranslationViewModel>()
			.AddSingleton<ShipTrainingPlannerTranslationViewModel>()
			.AddSingleton<EquipmentUpgradePlannerTranslationViewModel>()
			.AddSingleton<AlbumMasterEquipmentUpgradeTranslationViewModel>()
			.AddSingleton<SortieDetailTranslationViewModel>()
			.AddSingleton<TelegramTranslationViewModel>()
			.AddSingleton<ElectronicObserverApiTranslationViewModel>()
			// tools
			.AddSingleton<ShipPickerViewModel>()
			.AddSingleton<AutoRefreshViewModel>()
			.AddSingleton<ShipTrainingPlanViewerViewModel>()
			.AddSingleton<PhaseFactory>()
			.AddSingleton<BattleFactory>()
			// services
			.AddSingleton<DataSerializationService>()
			.AddSingleton<ToolService>()
			.AddSingleton<TransliterationService>()
			.AddSingleton<GameAssetDownloaderService>()
			.AddSingleton<FileService>()
			.AddSingleton<EquipmentPickerService>()
			.AddSingleton<EquipmentUpgradePlanManager>()
			.AddSingleton<TimeChangeService>()
			.AddSingleton<ColorService>()
			.AddSingleton<ElectronicObserverApiService>()
			.AddSingleton<DataAndTranslationIssueReporter>()
			.AddSingleton<SortieRecordMigrationService>()
			//.ActivateSingleton<DataAndTranslationIssueReporter>() todo uncomment and test with .net 8 
			// external
			.AddSingleton(JotTracker())

			.BuildServiceProvider();

		Ioc.Default.ConfigureServices(services);

		Ioc.Default.GetRequiredService<DataAndTranslationIssueReporter>(); // todo remove with .net 8 if the above test is successful
	}

	private static Tracker JotTracker()
	{
		Tracker tracker = new(new JsonFileStore(@"Settings\WindowStates"));

		tracker
			.Configure<System.Windows.Window>()
			.Id(w => w.Name)
			.Property(w => w.Top)
			.Property(w => w.Left)
			.Property(w => w.Height)
			.Property(w => w.Width)
			.Property(w => w.WindowState)
			.PersistOn(nameof(System.Windows.Window.Closed))
			.StopTrackingOn(nameof(System.Windows.Window.Closed));

		// EventLockPlannerWindow extends System.Windows.Window so the config above applies to it
		tracker
			.Configure<EventLockPlannerWindow>()
			.Property(w => w.ViewModel.ShowFinishedPhases);

		tracker
			.Configure<DialogAlbumMasterShipWpf>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<DialogAlbumMasterEquipmentWpf>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<EquipmentListWindow>()
			.Property(w => w.ViewModel.EquipmentGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.EquipmentGridViewModel.SortDescriptions)
			.Property(w => w.ViewModel.EquipmentGridWidth)
			.Property(w => w.ViewModel.EquipmentDetailGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.EquipmentDetailGridViewModel.SortDescriptions);

		tracker
			.Configure<ExpeditionCheckView>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<EquipmentUpgradePlanViewerViewModel>()
			.Property(w => w.Filters.DisplayFinished)
			.Property(w => w.Filters.SelectAllDay)
			.Property(w => w.Filters.SelectToday)
			.Property(w => w.DataGridViewModel.ColumnProperties)
			.Property(w => w.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<ShipTrainingPlanViewerViewModel>()
			.Property(w => w.DataGridViewModel.ColumnProperties)
			.Property(w => w.DataGridViewModel.SortDescriptions)
			.Property(w => w.DisplayFinished);

		tracker
			.Configure<EquipmentUpgradePlannerWindow>()
			.Property(w => w.ViewModel.Filters.DisplayFinished)
			.Property(w => w.ViewModel.Filters.SelectAllDay)
			.Property(w => w.ViewModel.Filters.SelectToday)
			.Property(w => w.ViewModel.EquipmentUpgradePlanManager.CompactMode)
			.Property(w => w.ViewModel.PlanListWidth)
			.Property(w => w.ViewModel.PlannedUpgradesPager.ItemsPerPage);
		
		tracker
			.Configure<ExpCheckerWindow>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<BaseAirCorpsSimulationContentDialog>()
			.Property(w => w.ViewModel.MaxAircraftLevelFleet)
			.Property(w => w.ViewModel.MaxAircraftLevelAirBase);

		tracker
			.Configure<AirDefenseWindow>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<ConfigurationWindow>()
			.Property(w => w.ViewModel.BGM.ColumnProperties)
			.Property(w => w.ViewModel.BGM.SortDescriptions);

		tracker
			.Configure<ConstructionRecordViewerWindow>()
			.Property(w => w.ViewModel.DataGridRawRowsViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridRawRowsViewModel.SortDescriptions)
			.Property(w => w.ViewModel.DataGridMergedRowsAllViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridMergedRowsAllViewModel.SortDescriptions)
			.Property(w => w.ViewModel.DataGridMergedRowsFilteredByShipViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridMergedRowsFilteredByShipViewModel.SortDescriptions);

		tracker
			.Configure<DevelopmentRecordViewerWindow>()
			.Property(w => w.ViewModel.DataGridRawRowsViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridRawRowsViewModel.SortDescriptions)
			.Property(w => w.ViewModel.DataGridMergedRowsViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridMergedRowsViewModel.SortDescriptions);

		tracker
			.Configure<DropRecordViewerWindow>()
			.Property(w => w.ViewModel.DataGridRawRowsViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridRawRowsViewModel.SortDescriptions)
			.Property(w => w.ViewModel.DataGridMergedRowsViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridMergedRowsViewModel.SortDescriptions);

		tracker
			.Configure<EventLockPlannerWindow>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<SenkaViewerWindow>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<SortieRecordViewerWindow>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		tracker
			.Configure<ExpeditionRecordViewerWindow>()
			.Property(w => w.ViewModel.DataGridViewModel.ColumnProperties)
			.Property(w => w.ViewModel.DataGridViewModel.SortDescriptions);

		return tracker;
	}
}
