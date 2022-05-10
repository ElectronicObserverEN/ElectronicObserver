using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserver.Window.Tools.EventLockPlanner;
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
	public new static App Current => (App)Application.Current;

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

			if (true)
			{
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
			}
			else
			{
				try
				{
					// todo why does this exception happen?
					// observed first after I added the wpf version of KC progress
					System.Windows.Forms.Application.Run(new FormMain());
				}
				catch (System.Runtime.InteropServices.SEHException ex)
				{

				}
			}

			// Shutdown();
		}
	}

	private void ConfigureServices()
	{
		ServiceProvider services = new ServiceCollection()
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
			.AddSingleton<DialogEquipmentListTranslationViewModel>()
			.AddSingleton<QuestTrackerManagerTranslationViewModel>()
			.AddSingleton<EventLockPlannerTranslationViewModel>()
			.AddSingleton<ShipFilterTranslationViewModel>()
			// tools
			.AddSingleton<ShipPickerViewModel>()
		
			.BuildServiceProvider();

		Ioc.Default.ConfigureServices(services);
	}
}
