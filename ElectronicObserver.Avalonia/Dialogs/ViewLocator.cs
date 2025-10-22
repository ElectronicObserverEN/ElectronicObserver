using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using HanumanInstitute.MvvmDialogs.Avalonia;

namespace ElectronicObserver.Avalonia.Dialogs;

public class ViewLocator : StrongViewLocator
{
	public ViewLocator()
	{
		Register<ShipSelectorViewModel, ShipSelectorView, ShipSelectorWindow>();
		Register<EquipmentSelectorViewModel, EquipmentSelectorView, EquipmentSelectorWindow>();
	}
}
