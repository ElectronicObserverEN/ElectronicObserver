using System.Windows;
using System.Windows.Media;
using Windows.UI.ViewManagement;
using Browser.WebView2Browser;
using BrowserLibCore;

namespace Browser;

public partial class BrowserView
{
	public BrowserViewModel ViewModel { get; }
	
	public BrowserView(string host, int port, string culture)
	{
		InitializeComponent();

		ViewModel =  new WebView2ViewModel(host, port, culture);

		Loaded += ViewModel.OnLoaded;
		DataContext = ViewModel;
	}

	private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (sender is not FrameworkElement control) return;

		ViewModel.DpiScale = VisualTreeHelper.GetDpi(this);
		ViewModel.TextScaleFactor = new UISettings().TextScaleFactor;

		ViewModel.ActualHeight = control.ActualHeight;
		ViewModel.ActualWidth = control.ActualWidth;
	}
}
