using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Browser.WebView2Browser.AirControlSimulator;
using Browser.WebView2Browser.CompassPrediction;
using BrowserLibCore;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;
using Jot.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Browser;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public new static App Current => (App)Application.Current;

	public App()
	{
		this.InitializeComponent();
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		// Debugger.Launch();
		// string host = "test";
		// int port = 1;
		// string culture = "en-US";
		// FormBrowserHostから起動された時は引数に通信用URLが渡される

		if (e.Args.Length < 2)
		{
			MessageBox.Show("Please start the application using ElectronicObserver.exe",
				"Information", MessageBoxButton.OK, MessageBoxImage.Information);
			return;
		}

		/*
		System.Windows.Forms.Application.EnableVisualStyles();
		System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
		*/

		string host = e.Args[0];

		if (!int.TryParse(e.Args[1], out int port))
		{
			MessageBox.Show("Please start the application using ElectronicObserver.exe",
				"Information", MessageBoxButton.OK, MessageBoxImage.Information);
			return;
		}

		string culture = e.Args.Length switch
		{
			> 2 => e.Args[2],
			_ => CultureInfo.CurrentCulture.Name switch
			{
				"ja-JP" => "ja-JP",
				_ => "en-US",
			},
		};

		ServiceProvider services = new ServiceCollection()
			.AddSingleton<FormBrowserTranslationViewModel>()
			.AddSingleton<AirControlSimulatorTranslationViewModel>()
			.AddSingleton<CompassPredictionTranslationViewModel>()
			// external
			.AddSingleton(JotTracker())
			.BuildServiceProvider();

		Ioc.Default.ConfigureServices(services);

		// System.Windows.Forms.Application.Run(new FormBrowser(e.Args[0], port, culture));

		ToolTipService.ShowDurationProperty.OverrideMetadata(
			typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

		BrowserView browser = new(host, port, culture);

		MainWindow = browser;

		browser.ShowDialog();
	}

	private static Tracker JotTracker()
	{
		Tracker tracker = new(new JsonFileStore(@"Settings\WindowStates"));

		tracker
			.Configure<Window>()
			.Id(w => w.Name)
			.Property(w => w.Top)
			.Property(w => w.Left)
			.Property(w => w.Height)
			.Property(w => w.Width)
			.Property(w => w.WindowState)
			.PersistOn(nameof(Window.Closed))
			.StopTrackingOn(nameof(Window.Closed));

		tracker.Configure<CompassPredictionViewModel>()
			.Property(vm => vm.SynchronizeMap);

		return tracker;
	}
}
