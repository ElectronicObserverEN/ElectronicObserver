using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeImprovementModel
{
	[JsonPropertyName("convert")]
	public EquipmentUpgradeConversionModel? ConversionData { get; set; }

	[JsonPropertyName("helpers")]
	public List<EquipmentUpgradeHelpersModel> Helpers { get; set; } = new List<EquipmentUpgradeHelpersModel>();

	[JsonPropertyName("costs")]
	public EquipmentUpgradeImprovementCost Costs { get; set; } = new EquipmentUpgradeImprovementCost();
}
