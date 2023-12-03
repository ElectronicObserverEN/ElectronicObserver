using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;

public class EquipmentAssignmentItemModel
{
	public int Id { get; set; }

	public required EquipmentUpgradePlanItemModel Plan { get; set; }

	public EquipmentId EquipmentMasterDataId { get; set; }

	public int EquipmentId { get; set; }

	public bool WillBeUsedForConversion { get; set; }
}
