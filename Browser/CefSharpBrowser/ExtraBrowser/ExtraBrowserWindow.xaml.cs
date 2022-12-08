using System.Windows;
using CefSharp;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Browser.CefSharpBrowser.ExtraBrowser;

public partial class ExtraBrowserWindow : Window
{
	public FormBrowserTranslationViewModel FormBrowser { get; }

	public ExtraBrowserWindow()
	{
		FormBrowser = Ioc.Default.GetService<FormBrowserTranslationViewModel>()!;

		InitializeComponent();
	}

	private void DmmPointsButtonClick(object sender, RoutedEventArgs e)
	{
		Browser.Address = "https://point.dmm.com/choice/pay";
	}

	private void AkashiListButtonClick(object sender, RoutedEventArgs e)
	{
		Browser.Address = "https://akashi-list.me/";
	}

	private void ShowDevToolsMenuItemClick(object sender, RoutedEventArgs e)
	{
		Browser.ShowDevTools();
	}
}
