using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeHelpersModel
{
	/// <summary>
	/// Ids of the helpers
	/// </summary>
	[JsonPropertyName("ship_ids")]
	public List<int> ShipIds { get; set; } = new List<int>();

	/// <summary>
	/// Days those helpers can help
	/// </summary>
	[JsonPropertyName("days")]
	public List<DayOfWeek> CanHelpOnDays { get; set; } = new List<DayOfWeek>();
}
