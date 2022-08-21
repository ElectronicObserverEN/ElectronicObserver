using System.Collections.Generic;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public class EquipmentGroup
{
	public EquipmentTypes Id { get; set; }
	public string Name { get; set; }
	public List<IEquipmentDataMaster> Equipments { get; set; } = new();
}
