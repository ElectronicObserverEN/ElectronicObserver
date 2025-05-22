using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.FitBonus;

public class FitBonusPerEquipment
{
	[JsonPropertyName("types")] public List<EquipmentTypes>? EquipmentTypes { get; set; }

	[JsonPropertyName("ids")] public List<EquipmentId>? EquipmentIds { get; set; }

	[JsonPropertyName("bonuses")] public List<FitBonusData> Bonuses { get; set; } = new();
}
