using CommunityToolkit.Mvvm.DependencyInjection;
using CefSharp;
using CefSharp.WinForms;
using Jot;

namespace Browser.CefSharpBrowser.AirControlSimulator;

/// <summary>
/// Interaction logic for AirControlSimulatorWindow.xaml
/// </summary>
public partial class AirControlSimulatorWindow
{
	private Tracker Tracker { get; }
	private AirControlSimulatorViewModel ViewModel { get; }

	private ChromiumWebBrowser Browser { get; }

	public AirControlSimulatorWindow(string url, BrowserLibCore.IBrowserHost browserHost)
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;

		ViewModel = new(browserHost)
		{
			Uri = url,
		};

		DataContext = ViewModel;

		InitializeComponent();

		Browser = new();
		BrowserHost.Child = Browser;

		ViewModel.ExecuteScriptAsync = Browser.ExecuteScriptAsync;

		InitializeAsync();

		Loaded += (_, _) =>
		{
			StartJotTracking();
		};
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}

	private async void InitializeAsync()
	{
		Browser.Load(ViewModel.Uri);
	}
}
