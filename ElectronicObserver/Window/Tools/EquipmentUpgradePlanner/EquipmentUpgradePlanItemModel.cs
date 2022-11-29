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

	/// <summary>
	/// Level at which the user is gonna start using the slider for improvments
	/// Used for cost calculation
	/// </summary>
	public UpgradeLevel SliderLevel { get; set; }
}
