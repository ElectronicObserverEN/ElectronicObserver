using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ElectronicObserver.Window.Wpf.Log;
/// <summary>
/// Interaction logic for LogView.xaml
/// </summary>
public partial class LogView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
"ViewModel", typeof(LogViewViewModel), typeof(LogView), new PropertyMetadata(default(LogViewViewModel)));

	public LogViewViewModel ViewModel
	{
		get => (LogViewViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public LogView()
	{
		InitializeComponent();
	}

	private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Selector selector = sender as Selector;
		if (selector is ListBox)
		{
			(selector as ListBox).ScrollIntoView(selector.SelectedItem);
		}
	}


	//	private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
	//	{
	//		if (e.ExtentHeightChange != 0)
	//		{
	//			//ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ExtentHeight);
	//		}
	//	}
}
