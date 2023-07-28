using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Log;

/// <summary>
/// Interaction logic for LogView.xaml
/// </summary>
public partial class LogView
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(LogViewModel), typeof(LogView), new PropertyMetadata(default(LogViewModel)));

	public LogViewModel ViewModel
	{
		get => (LogViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public LogView()
	{
		InitializeComponent();
	}

	private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		if (e.OriginalSource is ScrollViewer scrollViewer &&
		Math.Abs(e.ExtentHeightChange) > 0.0)
		{
			scrollViewer.ScrollToBottom();
		}
	}
}
