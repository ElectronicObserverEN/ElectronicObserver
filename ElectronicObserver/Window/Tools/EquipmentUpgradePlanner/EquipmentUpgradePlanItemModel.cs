using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradePlanItemModel
{
	public int Id { get; set; }

	/// <summary>
	/// The id of the equipment owned by the player (drop id)
	/// </summary>
	public int? EquipmentMasterId { get; set; }

	/// <summary>
	/// The id of Master Data of the equipment
	/// </summary>
	public EquipmentId EquipmentId { get; set; }

	public UpgradeLevel DesiredUpgradeLevel { get; set; }
	public bool Finished { get; set; }

	public int Priority { get; set; }
}
