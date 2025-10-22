using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.DeckBuilder;

public class DeckBuilderEnemyEquipment
{
	[JsonPropertyName("id")] public EquipmentId Id { get; set; }
}
