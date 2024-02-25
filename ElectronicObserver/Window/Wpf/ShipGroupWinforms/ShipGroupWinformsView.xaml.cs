using System.Windows;

namespace ElectronicObserver.Window.Wpf.ShipGroupWinforms;

/// <summary>
/// Interaction logic for ShipGroupWinformsView.xaml
/// </summary>
public partial class ShipGroupWinformsView
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(ShipGroupWinformsViewModel), typeof(ShipGroupWinformsView), new PropertyMetadata(default(ShipGroupWinformsViewModel)));

	public ShipGroupWinformsViewModel ViewModel
	{
		get => (ShipGroupWinformsViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public ShipGroupWinformsView()
	{
		InitializeComponent();
	}
}
