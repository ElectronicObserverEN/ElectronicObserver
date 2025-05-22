using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.FleetAnalysis;

public class FleetAnalysisShip
{
	[JsonPropertyName("api_id")] public required int DropId { get; set; }
	[JsonPropertyName("api_ship_id")] public required ShipId ShipId { get; set; }
	[JsonPropertyName("api_lv")] public required int Level { get; set; }
	[JsonPropertyName("api_kyouka")] public required IEnumerable<int> Modernization { get; set; }
	[JsonPropertyName("api_exp")] public required IEnumerable<double> Experience { get; set; }
	[JsonPropertyName("api_slot_ex")] public required int ExpansionSlot { get; set; }
	[JsonPropertyName("api_sally_area")] public required int SallyArea { get; set; }
	[JsonPropertyName("api_sp_effect_items")] public required List<FleetAnalysisSpecialEffectItem> SpecialEffectItems { get; set; }
}
