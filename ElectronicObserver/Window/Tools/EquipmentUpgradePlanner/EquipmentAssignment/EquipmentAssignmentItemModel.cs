using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;

public class EquipmentAssignmentItemModel
{
	public int Id { get; set; }

	public EquipmentUpgradePlanItemModel Plan { get; set; } = null!;

	public EquipmentId EquipmentMasterDataId { get; set; }

	public int EquipmentId { get; set; }
}
