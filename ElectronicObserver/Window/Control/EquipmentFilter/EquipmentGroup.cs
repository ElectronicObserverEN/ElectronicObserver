using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Control.EquipmentFilter;

public class EquipmentGroup
{
	public EquipmentTypes Id { get; set; }
	public string Name { get; set; } = "";
	public List<EquipmentData> Equipments { get; set; } = new();
}
