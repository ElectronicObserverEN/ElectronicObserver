using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeImprovmentModel
{
	[JsonPropertyName("convert")]
	public EquipmentUpgradeConversionModel? ConversionData { get; set; }

	[JsonPropertyName("helpers")]
	public List<EquipmentUpgradeHelpersModel> Helpers { get; set; } = new List<EquipmentUpgradeHelpersModel>();

	[JsonPropertyName("costs")]
	public EquipmentUpgradeImprovmentCost Costs { get; set; } = new EquipmentUpgradeImprovmentCost();
}
