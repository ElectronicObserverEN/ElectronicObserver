using System.Threading.Tasks;
using BrowserLibCore;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace Browser.WebView2Browser.CompassPrediction;

/// <summary>
/// Interaction logic for CompassPredictionView.xaml
/// </summary>
public partial class CompassPredictionView
{
	private Tracker Tracker { get; }
	private CompassPredictionViewModel ViewModel { get; }

	public CompassPredictionView(IBrowserHost browserHost)
	{
		Tracker = Ioc.Default.GetRequiredService<Tracker>();
		CompassPredictionTranslationViewModel translation = Ioc.Default.GetRequiredService<CompassPredictionTranslationViewModel>();

		ViewModel = new(browserHost, translation);

		DataContext = ViewModel;

		InitializeComponent();

		ViewModel.ExecuteScriptAsync = s => Browser.ExecuteScriptAsync(s);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		InitializeAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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
