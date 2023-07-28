using System.Threading.Tasks;
using BrowserLibCore;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace Browser.WebView2Browser.AirControlSimulator;

/// <summary>
/// Interaction logic for AirControlSimulatorWindow.xaml
/// </summary>
public partial class AirControlSimulatorWindow
{
	private Tracker Tracker { get; }
	private AirControlSimulatorViewModel ViewModel { get; }

	public AirControlSimulatorWindow(string url, IBrowserHost browserHost)
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

	private async Task InitializeAsync()
	{
		await Browser.EnsureCoreWebView2Async(WebView2ViewModel.Environment);

		Browser.CoreWebView2.Navigate(ViewModel.Uri);
	}
}
