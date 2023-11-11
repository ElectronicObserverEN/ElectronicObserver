using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradePlanItemModel
{
	public int Id { get; set; }

	public EquipmentUpgradePlanItemModel? Parent { get; set; }

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
	/// Level at which the user is gonna start using the slider for improvements
	/// Used for cost calculation
	/// </summary>
	public SliderUpgradeLevel SliderLevel { get; set; } = SliderUpgradeLevel.Never;

	/// <summary>
	/// Helper for upgrades
	/// Used for cost calculation
	/// </summary>
	public ShipId SelectedHelper { get; set; }
}
