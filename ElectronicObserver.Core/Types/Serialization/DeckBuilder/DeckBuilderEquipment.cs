using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.DeckBuilder;

public class DeckBuilderEquipment
{
	[JsonPropertyName("id")] public EquipmentId Id { get; set; }
	[JsonPropertyName("rf")] public int Level { get; set; }
	[JsonPropertyName("mas")] public int? AircraftLevel { get; set; }
}