using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderEnemyShip
{
	[JsonPropertyName("id")] public ShipId Id { get; set; }
	[JsonPropertyName("items")] public List<DeckBuilderEnemyEquipment> Equipment { get; set; } = new();
}
