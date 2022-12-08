namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

public record EquipmentUpgradePlanCostItemModel
{
	/// <summary>
	/// Id of the item
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Number of this equipment required
	/// </summary>
	public int Required { get; set; }
}
