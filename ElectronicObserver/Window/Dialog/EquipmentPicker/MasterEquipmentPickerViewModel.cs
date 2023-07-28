using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public class MasterEquipmentPickerViewModel : EquipmentPickerViewModel
{
	protected override List<IEquipmentData> AllEquipments =>
		KCDatabase.Instance.MasterEquipments.Values
					.Where(s => !s.IsAbyssalEquipment)
					.Select(s => new EquipmentDataMock(s))
					.Cast<IEquipmentData>()
					.OrderBy(s => s.MasterEquipment.CategoryType)
					.ThenBy(s => s.MasterID)
					.ToList();
}
