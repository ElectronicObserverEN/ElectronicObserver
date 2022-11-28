﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeDataModel
{
    [JsonPropertyName("eq_id")]
    public int EquipmentId { get; set; }

    /// <summary>
    /// Improvments possibles for this equipment
    /// </summary>
    [JsonPropertyName("improvement")]
    public List<EquipmentUpgradeImprovmentModel> Improvement { get; set; } = new List<EquipmentUpgradeImprovmentModel>();

    /// <summary>
    /// This equipment can be converted to those equipments
    /// </summary>
    [JsonPropertyName("convert_to")]
    public List<EquipmentUpgradeConversionModel> ConvertTo { get; set; } = new List<EquipmentUpgradeConversionModel>();

    /// <summary>
    /// This equipment is use in those equipments upgrades
    /// </summary>
    [JsonPropertyName("upgrade_for")]
    public List<int> UpgradeFor { get; set; } = new List<int>();
}
