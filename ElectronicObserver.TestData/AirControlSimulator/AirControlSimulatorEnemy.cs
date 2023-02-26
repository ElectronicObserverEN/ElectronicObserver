using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorEnemy
{
	[JsonPropertyName("id")]
	public ShipId Id { get; set; }

	[JsonPropertyName("type")]
	public int Type { get; set; }

	[JsonPropertyName("slot_count")]
	public int SlotCount { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("slots")]
	public List<int> Slots { get; set; } = new();

	[JsonPropertyName("items")]
	public List<int> Items { get; set; } = new();

	[JsonPropertyName("hp")]
	public int Hp { get; set; }

	[JsonPropertyName("aa")]
	public int Aa { get; set; }

	[JsonPropertyName("armor")]
	public int Armor { get; set; }

	[JsonPropertyName("speed")]
	public int Speed { get; set; }

	[JsonPropertyName("unknown")]
	public int Unknown { get; set; }
}
