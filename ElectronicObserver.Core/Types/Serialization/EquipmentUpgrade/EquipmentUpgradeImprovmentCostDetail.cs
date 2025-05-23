using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeImprovementCostDetail
{
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
	public List<EquipmentUpgradeImprovementCostItemDetail> EquipmentDetail { get; set; } = new List<EquipmentUpgradeImprovementCostItemDetail>();

	[JsonPropertyName("consumable")]
	public List<EquipmentUpgradeImprovementCostItemDetail> ConsumableDetail { get; set; } = new List<EquipmentUpgradeImprovementCostItemDetail>();

}
