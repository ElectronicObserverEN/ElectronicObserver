using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;

public class EquipmentViewModel(IEquipmentData equipment)
{
	public IEquipmentData Equipment { get; } = equipment;
}
