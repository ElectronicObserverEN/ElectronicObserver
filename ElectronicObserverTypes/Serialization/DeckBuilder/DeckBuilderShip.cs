using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderShip
{
	[JsonPropertyName("id")] public ShipId Id { get; set; }
	[JsonPropertyName("lv")] public int Level { get; set; }
	[JsonPropertyName("exa")] public bool IsExpansionSlotAvailable { get; set; }
	[JsonPropertyName("items")] public DeckBuilderEquipmentList Equipment { get; set; } = new();
	[JsonPropertyName("hp")] public int Hp { get; set; }
	[JsonPropertyName("fp")] public int Firepower { get; set; }
	[JsonPropertyName("tp")] public int Torpedo { get; set; }
	[JsonPropertyName("aa")] public int AntiAir { get; set; }
	[JsonPropertyName("ar")] public int Armor { get; set; }
	[JsonPropertyName("asw")] public int AntiSubmarine { get; set; }
	[JsonPropertyName("ev")] public int Evasion { get; set; }
	[JsonPropertyName("los")] public int Los { get; set; }
	[JsonPropertyName("luck")] public int Luck { get; set; }
	[JsonPropertyName("sp")] public int Speed { get; set; }
	[JsonPropertyName("ra")] public int Range { get; set; }
	[JsonPropertyName("sp")] public required List<DeckBuilderSpecialEffectItem> SpecialEffectItems { get; set; }
}
