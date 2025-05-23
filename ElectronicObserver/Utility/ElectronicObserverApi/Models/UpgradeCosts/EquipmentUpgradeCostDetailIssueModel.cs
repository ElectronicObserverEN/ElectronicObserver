using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models.UpgradeCosts;

public class EquipmentUpgradeCostDetailIssueModel
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
	/// Devmat cost
	/// </summary>
	[JsonPropertyName("devmats")]
	public int DevmatCost { get; set; }

	/// <summary>
	/// Devmat cost if slider is used
	/// </summary>
	[JsonPropertyName("devmats_sli")]
	public int SliderDevmatCost { get; set; }

	/// <summary>
	/// Screw cost
	/// </summary>
	[JsonPropertyName("screws")]
	public int ImproveMatCost { get; set; }

	/// <summary>
	/// Screw cost if slider is used
	/// </summary>
	[JsonPropertyName("screws_sli")]
	public int SliderImproveMatCost { get; set; }

	[JsonPropertyName("equips")]
	public List<EquipmentUpgradeImprovementCostItemDetail> EquipmentDetail { get; set; } = [];

	[JsonPropertyName("consumable")]
	public List<EquipmentUpgradeImprovementCostItemDetail> ConsumableDetail { get; set; } = [];
}
