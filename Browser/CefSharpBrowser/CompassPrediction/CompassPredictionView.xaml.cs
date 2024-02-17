using System.Threading.Tasks;
using Browser.WebView2Browser.CompassPrediction;
using CefSharp;
using CefSharp.Fluent;
using CefSharp.WinForms;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace Browser.CefSharpBrowser.CompassPrediction;

/// <summary>
/// Interaction logic for CompassPredictionView.xaml
/// </summary>
public partial class CompassPredictionView
{
	private Tracker Tracker { get; }
	private CompassPredictionViewModel ViewModel { get; }
	private ChromiumWebBrowser Browser { get; }

	public CompassPredictionView(CompassPredictionViewModel viewModel)
	{
		Tracker = Ioc.Default.GetRequiredService<Tracker>();

		ViewModel = viewModel;

		DataContext = ViewModel;

		InitializeComponent();

		Browser = new ChromiumWebBrowser
		{
			DownloadHandler = DownloadHandler
				.AskUser((chromiumBrowser, browser, downloadItem, callback) =>
				{
					// don't need any extra code here
				}),
		};

		BrowserHost.Child = Browser;

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
		await Browser.LoadUrlAsync(ViewModel.Uri);
	}
}
