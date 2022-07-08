using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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

	private void ListBox_OnLoaded(object sender, RoutedEventArgs e)
	{
		var listBox = (ListBox)sender;

		var scrollViewer = FindScrollViewer(listBox);

		if (scrollViewer != null)
		{
			scrollViewer.ScrollChanged += (o, args) =>
			{
				if (args.ExtentHeightChange > 0)
					scrollViewer.ScrollToBottom();
			};
		}
	}
	// Search for ScrollViewer, breadth-first
	private static ScrollViewer FindScrollViewer(DependencyObject root)
	{
		var queue = new Queue<DependencyObject>(new[] { root });

		do
		{
			var item = queue.Dequeue();

			if (item is ScrollViewer)
				return (ScrollViewer)item;

			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
				queue.Enqueue(VisualTreeHelper.GetChild(item, i));
		} while (queue.Count > 0);

		return null;
	}
}
