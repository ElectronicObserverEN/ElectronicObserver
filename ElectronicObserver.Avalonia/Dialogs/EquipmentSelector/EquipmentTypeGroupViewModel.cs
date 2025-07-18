using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;

public class EquipmentTypeGroupViewModel : ObservableObject
{
	public required EquipmentTypeGroup EquipmentTypeGroup { get; init; }
	public required List<IEquipmentData> Equipment { get; init; }
	public string Display => EquipmentTypeGroup.Display();
}
