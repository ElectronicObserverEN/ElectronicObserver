using System.Windows;

namespace ElectronicObserver.Window.Wpf.ShipGroupAvalonia;

public partial class ShipGroupAvaloniaView
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(ShipGroupAvaloniaViewModel), typeof(ShipGroupAvaloniaView), new PropertyMetadata(default(ShipGroupAvaloniaViewModel)));

	public ShipGroupAvaloniaViewModel ViewModel
	{
		get => (ShipGroupAvaloniaViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public ShipGroupAvaloniaView()
	{
		InitializeComponent();
	}
}
