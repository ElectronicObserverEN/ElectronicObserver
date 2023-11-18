namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;

public class EquipmentAssignmentItemModel
{
	public int Id { get; set; }

	public EquipmentUpgradePlanItemModel Plan { get; set; } = null!;

	public int EquipmentId { get; set; }
}
