using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.DeckBuilder;

public class DeckBuilderData
{
	[JsonPropertyName("version")] public int Version { get; set; } = 4;
	[JsonPropertyName("hqlv")] public int HqLevel { get; set; }
	[JsonPropertyName("f1")] public DeckBuilderFleet? Fleet1 { get; set; }
	[JsonPropertyName("f2")] public DeckBuilderFleet? Fleet2 { get; set; }
	[JsonPropertyName("f3")] public DeckBuilderFleet? Fleet3 { get; set; }
	[JsonPropertyName("f4")] public DeckBuilderFleet? Fleet4 { get; set; }
	[JsonPropertyName("a1")] public DeckBuilderAirBase? AirBase1 { get; set; }
	[JsonPropertyName("a2")] public DeckBuilderAirBase? AirBase2 { get; set; }
	[JsonPropertyName("a3")] public DeckBuilderAirBase? AirBase3 { get; set; }
	[JsonPropertyName("s")] public DeckBuilderSortieData? Sortie { get; set; }
}
