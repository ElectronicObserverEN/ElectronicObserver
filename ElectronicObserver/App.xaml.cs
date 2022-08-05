using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using ElectronicObserver.Window.Tools.AirDefense;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserver.Window.Tools.EquipmentList;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserver.Window.Wpf.ExpeditionCheck;
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

			Logger.Add(3, ElectronicObserver.Properties.Window.FormMain.CopyingToClipboardFailed);
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
					ElectronicObserver.Properties.Resources.MultiInstanceNotification,
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

			try
			{
				Directory.CreateDirectory(@"Settings\Layout");
			}
			catch (UnauthorizedAccessException)
			{
				MessageBox.Show(ElectronicObserver.Properties.Window.FormMain.MissingPermissions,
					ElectronicObserver.Properties.Window.FormMain.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);
				throw;
			}

			Configuration.Instance.Load();

			ConfigureServices();

			ToolTipService.ShowDurationProperty.OverrideMetadata(
				typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));
			ToolTipService.InitialShowDelayProperty.OverrideMetadata(
				typeof(DependencyObject), new FrameworkPropertyMetadata(0));

			FormMainWpf mainWindow = new();

			MainWindow = mainWindow;
			ShutdownMode = ShutdownMode.OnMainWindowClose;

			mainWindow.ShowDialog();

			// Shutdown();
		}
	}

	private void ConfigureServices()
	{
		ServiceProvider services = new ServiceCollection()
			.AddSingleton<IKCDatabase>(KCDatabase.Instance)
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
			// tool translations
			.AddSingleton<DialogAlbumMasterShipTranslationViewModel>()
			.AddSingleton<DialogAlbumMasterEquipmentTranslationViewModel>()
			.AddSingleton<DialogDevelopmentRecordViewerTranslationViewModel>()
			.AddSingleton<DialogDropRecordViewerTranslationViewModel>()
			.AddSingleton<DialogConstructionRecordViewerTranslationViewModel>()
			.AddSingleton<DialogResourceChartTranslationViewModel>()
			.AddSingleton<DialogEquipmentListTranslationViewModel>()
			.AddSingleton<AirDefenseTranslationViewModel>()
			.AddSingleton<QuestTrackerManagerTranslationViewModel>()
			.AddSingleton<EventLockPlannerTranslationViewModel>()
			.AddSingleton<ShipFilterTranslationViewModel>()
			.AddSingleton<AirControlSimulatorTranslationViewModel>()
			.AddSingleton<AutoRefreshTranslationViewModel>()
			.AddSingleton<ExpeditionCheckTranslationViewModel>()
			// tools
			.AddSingleton<ShipPickerViewModel>()
			.AddSingleton<AutoRefreshViewModel>()
			// services
			.AddSingleton<DataSerializationService>()
			.AddSingleton<ToolService>()
			.AddSingleton<TransliterationService>()
			// external
			.AddSingleton(JotTracker())

			.BuildServiceProvider();

		Ioc.Default.ConfigureServices(services);
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
			.Property(w => w.ViewModel.ColumnProperties)
			.Property(w => w.ViewModel.SortDescriptions);

		tracker
			.Configure<DialogAlbumMasterEquipmentWpf>()
			.Property(w => w.ViewModel.ColumnProperties)
			.Property(w => w.ViewModel.SortDescriptions);

		tracker
			.Configure<EquipmentListWindow>()
			.Property(w => w.ViewModel.EquipmentGridColumnProperties)
			.Property(w => w.ViewModel.EquipmentGridSortDescriptions)
			.Property(w => w.ViewModel.EquipmentGridWidth)
			.Property(w => w.ViewModel.DetailGridColumnProperties)
			.Property(w => w.ViewModel.DetailGridSortDescriptions);

		tracker
			.Configure<ExpeditionCheckView>()
			.Property(w => w.ViewModel.ColumnProperties)
			.Property(w => w.ViewModel.SortDescriptions);

		return tracker;
	}
}
