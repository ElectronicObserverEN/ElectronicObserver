using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ElectronicObserver.Core.Types.Serialization.FitBonus;

public class FitBonusData
{
	[JsonPropertyName("shipClass")] public List<ShipClass>? ShipClasses { get; set; }

	/// <summary>
	/// Master id = exact id of the ship
	/// </summary>
	[JsonPropertyName("shipX")] public List<ShipId>? ShipMasterIds { get; set; }

	/// <summary>
	/// Base id of the ship (minimum remodel), bonus applies to all of the ship forms
	/// </summary>
	[JsonPropertyName("shipS")] public List<ShipId>? ShipIds { get; set; }

	[JsonPropertyName("shipType")] public List<ShipTypes>? ShipTypes { get; set; }

	[JsonPropertyName("shipNationality")] public List<ShipNationality>? ShipNationalities { get; set; }

	[JsonPropertyName("requires")] public List<EquipmentId>? EquipmentRequired { get; set; }

	/// <summary>
	/// If <see cref="EquipmentRequired"/> is set, equipments requires to be at least this level
	/// </summary>
	[JsonPropertyName("requiresLevel")] public UpgradeLevel? EquipmentRequiresLevel { get; set; } = null;

	/// <summary>
	/// If <see cref="EquipmentRequired"/> is set, you need this number of equipments
	/// </summary>
	[JsonPropertyName("requiresNum")] public int? NumberOfEquipmentsRequired { get; set; }

	[JsonPropertyName("requiresType")] public List<EquipmentTypes>? EquipmentTypesRequired { get; set; }
	[JsonPropertyName("requiresNumType")] public int? NumberOfEquipmentTypesRequired { get; set; }

	/// <summary>
	/// Improvement level of the equipment required
	/// </summary>
	[JsonPropertyName("level")] public int? EquipmentLevel { get; set; }

	/// <summary>
	/// Number Of Equipments Required after applying the improvement filter
	/// </summary>
	[JsonPropertyName("num")] public int? NumberOfEquipmentsRequiredAfterOtherFilters { get; set; }

	/// <summary>
	/// Bonuses to apply
	/// Applied x times, x being the number of equipment matching the conditions of the bonus fit 
	/// If NumberOfEquipmentsRequiredAfterOtherFilters or EquipmentRequired or EquipmentTypesRequired, bonus is applied only once
	/// </summary>
	[JsonPropertyName("bonus")] public FitBonusValue? Bonuses { get; set; }

	/// <summary>
	/// Bonuses to apply if ship has a radar with LOS >= 5
	/// </summary>
	[JsonPropertyName("bonusSR")] public FitBonusValue? BonusesIfSurfaceRadar { get; set; }

	/// <summary>
	/// Bonuses to apply if ship has a radar with AA >= 2
	/// </summary>
	[JsonPropertyName("bonusAR")] public FitBonusValue? BonusesIfAirRadar { get; set; }

	/// <summary>
	/// Bonuses to apply if ship has a radar with ACC >= 8
	/// </summary>
	[JsonPropertyName("bonusAccR")] public FitBonusValue? BonusesIfAccuracyRadar { get; set; }

}
