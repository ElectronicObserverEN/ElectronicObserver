using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CefSharp;
using CefSharp.Fluent;
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

		Browser = new ChromiumWebBrowser
		{
			DownloadHandler = DownloadHandler
				.AskUser((chromiumBrowser, browser, downloadItem, callback) =>
				{
					// don't need any extra code here
				}),
		};

		BrowserHost.Child = Browser;

		ViewModel.ExecuteScriptAsync = Browser.ExecuteScriptAsync;

#pragma warning disable CS4014
		InitializeAsync();
#pragma warning restore CS4014

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

	/// <inheritdoc />
	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);

		Browser.Dispose();
	}
}
