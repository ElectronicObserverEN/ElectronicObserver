using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Control.Paging;

/// <summary>
/// Interaction logic for PagingControlView.xaml
/// </summary>
public partial class PagingControlView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(PagingControlViewModel), typeof(PagingControlView), new PropertyMetadata(default(PagingControlViewModel)));

	public PagingControlViewModel ViewModel
	{
		get => (PagingControlViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public PagingControlView()
	{
		InitializeComponent();
	}
}
