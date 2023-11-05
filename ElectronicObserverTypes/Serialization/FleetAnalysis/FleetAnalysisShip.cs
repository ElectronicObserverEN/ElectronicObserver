using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.FleetAnalysis;

public class FleetAnalysisShip
{
	[JsonPropertyName("api_id")] public int DropId { get; set; }
	[JsonPropertyName("api_ship_id")] public ShipId ShipId { get; set; }
	[JsonPropertyName("api_lv")] public int Level { get; set; }
	[JsonPropertyName("api_kyouka")] public IEnumerable<int>? Modernization { get; set; }
	[JsonPropertyName("api_exp")] public IEnumerable<double>? Experience { get; set; }
	[JsonPropertyName("api_slot_ex")] public int ExpansionSlot { get; set; }
	[JsonPropertyName("api_sally_area")] public int SallyArea { get; set; }
}
