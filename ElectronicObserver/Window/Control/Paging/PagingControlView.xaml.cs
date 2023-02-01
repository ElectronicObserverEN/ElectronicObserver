using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Control.Paging;

/// <summary>
/// Interaction logic for PagingControlView.xaml
/// </summary>
public partial class PagingControlView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(object), typeof(PagingControlView), new PropertyMetadata(default(object)));

	public object ViewModel
	{
		get => GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public PagingControlView()
	{
		InitializeComponent();
	}
}
