using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderAirBase
{
	[JsonPropertyName("name")] public string? Name { get; set; }
	[JsonPropertyName("items")] public DeckBuilderAirBaseEquipmentList Equipment { get; set; } = new();
	[JsonPropertyName("mode")] public AirBaseActionKind Mode { get; set; }
	[JsonPropertyName("distance")] public int Distance { get; set; }
	[JsonPropertyName("sp")] public List<int>? StrikePoints { get; set; }
}
