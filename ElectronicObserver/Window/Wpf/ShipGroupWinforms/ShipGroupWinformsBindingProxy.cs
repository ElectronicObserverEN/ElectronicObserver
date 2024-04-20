using System.Windows;

namespace ElectronicObserver.Window.Wpf.ShipGroupWinforms;

public class ShipGroupWinformsBindingProxy : Freezable
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		nameof(ViewModel), typeof(ShipGroupWinformsViewModel), typeof(ShipGroupWinformsBindingProxy), new PropertyMetadata(default(ShipGroupWinformsViewModel)));

	public ShipGroupWinformsViewModel ViewModel
	{
		get => (ShipGroupWinformsViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	protected override Freezable CreateInstanceCore()
	{
		return new ShipGroupWinformsBindingProxy();
	}
}
