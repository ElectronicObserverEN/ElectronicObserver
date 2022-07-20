using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ElectronicObserverTypes.Serialization.FitBonus
{
    public class FitBonusData
    {
        [JsonPropertyName("shipClass")] public List<int>? ShipClasses { get; set; }

        /// <summary>
        /// Master id = exact id of the ship
        /// </summary>
        [JsonPropertyName("shipX")] public List<int>? ShipMasterIds { get; set; }

        /// <summary>
        /// Base id of the ship (minimum remodel), bonus applies to all of the ship forms
        /// </summary>
        [JsonPropertyName("shipS")] public List<int>? ShipIds { get; set; }

        [JsonPropertyName("shipType")] public List<int>? ShipTypes { get; set; }


        [JsonPropertyName("requires")] public List<int>? EquipmentRequired { get; set; }

        [JsonPropertyName("requiresNum")] public int? NumberOfEquipmentsRequired { get; set; }


        [JsonPropertyName("requiresType")] public List<int>? EquipmentTypesRequired { get; set; }

        [JsonPropertyName("requiresNumType")] public int? NumberOfEquipmentTypesRequired { get; set; }

        /// <summary>
        /// Improvment level of the equipment required
        /// </summary>
        [JsonPropertyName("level")] public int? EquipmentLevel { get; set; }

        /// <summary>
        /// Number Of Equipments Required after applying the improvment filter
        /// </summary>
        [JsonPropertyName("num")] public int? NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }

        /// <summary>
        /// Bonuses to apply
        /// Applied x times, x being the number of equipment matching the conditions of the bonus fit 
        /// If NumberOfEquipmentsRequiredAfterOtherFilters or EquipmentRequired or EquipmentTypesRequired, bonus is applied only once
        /// </summary>
        [JsonPropertyName("bonus")] public FitBonusValue Bonuses { get; set; } = null;

        /// <summary>
        /// Bonuses to apply if ship had a radar with LOS >= 5
        /// </summary>
        [JsonPropertyName("bonusSR")] public FitBonusValue BonusesIfLOSRadar { get; set; } = null;

        /// <summary>
        /// Bonuses to apply if ship had a radar with AA >= 2
        /// </summary>
        [JsonPropertyName("bonusAR")] public FitBonusValue BonusesIfAirRadar { get; set; } = null;

    }
}
