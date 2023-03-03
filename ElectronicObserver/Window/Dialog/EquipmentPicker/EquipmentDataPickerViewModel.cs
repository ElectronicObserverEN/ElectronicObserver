using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public class EquipmentDataPickerViewModel : EquipmentPickerViewModel
{
	protected override List<IEquipmentData> AllEquipments =>
		KCDatabase.Instance.Equipments
		.Values
		.Cast<IEquipmentData>()
		.OrderBy(s => s.MasterEquipment.CategoryType)
		.ThenBy(s => s.MasterID)
		.ToList();
}
