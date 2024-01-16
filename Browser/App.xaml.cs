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
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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

#if !DEBUG
		AppCenter.Start("63423a72-dfe4-45f0-abbd-3d0b67c02f0c",typeof(Analytics), typeof(Crashes));
#endif

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
		System.AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

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

		if (!Enum.TryParse(e.Args.Skip(3).FirstOrDefault(), out BrowserOption browserOption))
		{
			browserOption = BrowserOption.CefSharp;
		}

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

		BrowserView browser = new(host, port, culture, browserOption);

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

		return tracker;
	}

	private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		if (args.Name.StartsWith("CefSharp"))
		{
			string asmname = args.Name.Split(",".ToCharArray(), 2)[0] + ".dll";
			string arch = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, Environment.Is64BitProcess ? "x64" : "x86", asmname);

			if (!System.IO.File.Exists(arch))
				return null;

			try
			{
				return System.Reflection.Assembly.LoadFile(arch);
			}
			catch (IOException ex) when (ex is FileNotFoundException || ex is FileLoadException)
			{
				if (MessageBox.Show(
						$@"The browser component could not be loaded.
Microsoft Visual C++ 2015 Redistributable is required.
Open the download page?
(Please install vc_redist.{(Environment.Is64BitProcess ? "x64" : "x86")}.exe)",
						"CefSharp Load Error", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes)
					== MessageBoxResult.Yes)
				{
					ProcessStartInfo psi = new ProcessStartInfo
					{
						FileName = @"https://www.microsoft.com/en-US/download/details.aspx?id=53587",
						UseShellExecute = true
					};
					Process.Start(psi);
				}

				// なんにせよ今回は起動できないのであきらめる
				throw;
			}
			catch (NotSupportedException)
			{
				// 概ね ZoneID を外し忘れているのが原因

				if (MessageBox.Show(
						@"Browser startup failed.
This may be caused by the fact that the operation required for installation has not been performed.
Do you want to open the installation guide?",
						"Browser Load Failed", MessageBoxButton.YesNo, MessageBoxImage.Error)
					== MessageBoxResult.Yes)
				{
					ProcessStartInfo psi = new ProcessStartInfo
					{
						FileName = @"https://github.com/andanteyk/ElectronicObserver/wiki/Install",
						UseShellExecute = true
					};
					Process.Start(psi);
				}

				// なんにせよ今回は起動できないのであきらめる
				throw;
			}
		}
		return null;
	}

}
