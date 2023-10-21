using System.Windows;
using CefSharp;
using CefSharp.Fluent;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Browser.CefSharpBrowser.ExtraBrowser;

public partial class ExtraBrowserWindow
{
	public FormBrowserTranslationViewModel FormBrowser { get; }

	public ExtraBrowserWindow()
	{
		FormBrowser = Ioc.Default.GetService<FormBrowserTranslationViewModel>()!;

		InitializeComponent();

		Address.Text = "www.duckduckgo.com";
		Browser.Load(Address.Text);
		Address.PreviewKeyDown += Address_PreviewKeyDown;
		Browser.DownloadHandler = DownloadHandler
			.AskUser((chromiumBrowser, browser, downloadItem, callback) =>
			{
				// don't need any extra code here
			});
	}

	private void Address_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
	{
		if (e.Key == System.Windows.Input.Key.Enter)
		{
			Browser.Load(Address.Text);
		}
	}

	private void DmmPointsButtonClick(object sender, RoutedEventArgs e)
	{
		Address.Text = "https://point.dmm.com/choice/pay/";
		Browser.Load(Address.Text);
	}

	private void AkashiListButtonClick(object sender, RoutedEventArgs e)
	{
		Address.Text = "https://akashi-list.me/";
		Browser.Load(Address.Text);
	}

	private void ShowDevToolsMenuItemClick(object sender, RoutedEventArgs e)
	{
		Browser.ShowDevTools();
	}

	private void Back(object sender, RoutedEventArgs e)
	{
		Browser.Back();
	}

	private void Forward(object sender, RoutedEventArgs e)
	{
		Browser.Forward();
	}
}
