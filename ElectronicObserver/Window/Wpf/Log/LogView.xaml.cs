using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using ModernWpf.Controls;

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

	private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
	{
		if (e.OriginalSource is ScrollViewer scrollViewer &&
		Math.Abs(e.ExtentHeightChange) > 0.0)
		{
			scrollViewer.ScrollToBottom();
		}
	}
}
