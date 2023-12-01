using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderEnemyEquipment
{
	[JsonPropertyName("id")] public EquipmentId Id { get; set; }
}
