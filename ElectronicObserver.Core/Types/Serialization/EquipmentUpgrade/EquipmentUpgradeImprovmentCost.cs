using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeImprovementCost
{
	[JsonPropertyName("fuel")]
	public int Fuel { get; set; }

	[JsonPropertyName("ammo")]
	public int Ammo { get; set; }

	[JsonPropertyName("steel")]
	public int Steel { get; set; }

	[JsonPropertyName("baux")]
	public int Bauxite { get; set; }

	/// <summary>
	/// Costs for level 0 -> 6
	/// </summary>
	[JsonPropertyName("p1")]
	public EquipmentUpgradeImprovementCostDetail Cost0To5 { get; set; } = new();

	/// <summary>
	/// Costs for level 7 -> 10
	/// </summary>
	[JsonPropertyName("p2")]
	public EquipmentUpgradeImprovementCostDetail Cost6To9 { get; set; } = new();

	/// <summary>
	/// Costs for conversion
	/// </summary>
	[JsonPropertyName("conv")]
	public EquipmentUpgradeImprovementCostDetail? CostMax { get; set; } = null;
}
