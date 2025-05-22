using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Core.Types.Mocks;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public class MasterEquipmentPickerViewModel : EquipmentPickerViewModel
{
	protected override List<IEquipmentData> AllEquipments { get; }

	public MasterEquipmentPickerViewModel()
	{
		AllEquipments =
		KCDatabase.Instance.MasterEquipments.Values
			.Where(s => !s.IsAbyssalEquipment)
			.Select(s => new EquipmentDataMock(s))
			.Cast<IEquipmentData>()
			.OrderBy(s => s.MasterEquipment.CategoryType)
			.ThenBy(s => s.MasterID)
			.ToList();

		Initialize();
	}
}
