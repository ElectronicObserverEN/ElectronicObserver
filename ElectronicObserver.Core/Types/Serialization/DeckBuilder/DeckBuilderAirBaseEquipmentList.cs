using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.DeckBuilder;

public class DeckBuilderAirBaseEquipmentList
{
	[JsonPropertyName("i1")] public DeckBuilderEquipment? Equipment1 { get; set; }
	[JsonPropertyName("i2")] public DeckBuilderEquipment? Equipment2 { get; set; }
	[JsonPropertyName("i3")] public DeckBuilderEquipment? Equipment3 { get; set; }
	[JsonPropertyName("i4")] public DeckBuilderEquipment? Equipment4 { get; set; }
}