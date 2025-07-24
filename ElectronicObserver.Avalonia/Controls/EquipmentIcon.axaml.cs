using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.Controls;

public partial class EquipmentIcon : UserControl
{
	public static readonly StyledProperty<EquipmentIconType> IconTypeProperty =
		AvaloniaProperty.Register<EquipmentIcon, EquipmentIconType>(nameof(IconType), defaultBindingMode: BindingMode.OneWay);

	public EquipmentIconType IconType
	{
		get => GetValue(IconTypeProperty);
		set => SetValue(IconTypeProperty, value);
	}
	
	public EquipmentIcon()
	{
		InitializeComponent();
	}
}

