using System.Windows;
using Browser.WebView2Browser;

namespace Browser;

/// <summary>
/// Interaction logic for BrowserView.xaml
/// </summary>
public partial class BrowserView
{
	public BrowserViewModel ViewModel { get; }
	
	public BrowserView(string host, int port, string culture)
	{
		InitializeComponent();

		ViewModel = new WebView2ViewModel(host, port, culture);

		Loaded += ViewModel.OnLoaded;
		DataContext = ViewModel;
	}

	private void FrameworkElement_OnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (sender is not FrameworkElement control) return;

		ViewModel.ActualHeight = control.ActualHeight;
		ViewModel.ActualWidth = control.ActualWidth;
	}
}
