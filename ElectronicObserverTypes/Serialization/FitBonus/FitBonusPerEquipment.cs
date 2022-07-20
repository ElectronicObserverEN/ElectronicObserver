using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.FitBonus
{
    public class FitBonusPerEquipment
    {
        [JsonPropertyName("types")] public List<int>? EquipmentTypes { get; set; }

        [JsonPropertyName("ids")] public List<int>? EquipmentIds { get; set; }

        [JsonPropertyName("bonuses")] public List<FitBonusData>? Bonuses { get; set; }
    }
}
