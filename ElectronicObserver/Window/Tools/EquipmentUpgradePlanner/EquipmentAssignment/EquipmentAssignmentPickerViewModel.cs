using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Control.EquipmentFilter;
using ElectronicObserver.Window.Dialog.EquipmentPicker;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;

public class EquipmentAssignmentPickerViewModel : EquipmentPickerViewModel
{
	protected override List<IEquipmentData> AllEquipments { get; }

	public EquipmentAssignmentPickerViewModel(EquipmentUpgradePlanManager planManager, EquipmentId equipmentId, bool excludeUpgradedEquipments)
	{
		AllEquipments =
			KCDatabase.Instance.Equipments
				.Values
				.Cast<IEquipmentData>()
				.Where(eq => eq.EquipmentId == equipmentId)
				.Where(eq => !excludeUpgradedEquipments || eq.UpgradeLevel is UpgradeLevel.Zero)
				.Where(eq => !planManager.GetAssignments(equipmentId).Select(assignment => assignment.EquipmentId).Contains(eq.ID))
				.OrderBy(s => s.MasterEquipment.CategoryType)
				.ThenBy(s => s.MasterID)
				.ToList();

		Filters = new EquipmentFilterViewModel(true);

		Initialize();
	}
}
