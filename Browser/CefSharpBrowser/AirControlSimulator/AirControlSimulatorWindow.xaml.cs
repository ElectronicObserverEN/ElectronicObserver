using CommunityToolkit.Mvvm.DependencyInjection;
using CefSharp;
using Jot;

namespace Browser.CefSharpBrowser.AirControlSimulator;

/// <summary>
/// Interaction logic for AirControlSimulatorWindow.xaml
/// </summary>
public partial class AirControlSimulatorWindow
{
	private Tracker Tracker { get; }
	private AirControlSimulatorViewModel ViewModel { get; }

	public AirControlSimulatorWindow(string url, BrowserLibCore.IBrowserHost browserHost)
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;

		ViewModel = new(browserHost)
		{
			Uri = url,
		};

		DataContext = ViewModel;

		InitializeComponent();

		ViewModel.ExecuteScriptAsync = s => Browser.ExecuteScriptAsync(s);

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
