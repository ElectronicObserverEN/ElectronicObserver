using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorShip
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("album")]
	public int Album { get; set; }

	[JsonPropertyName("type")]
	public int Type { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("s_count")]
	public int SCount { get; set; }

	[JsonPropertyName("slots")]
	public List<int> Slots { get; set; } = new();

	[JsonPropertyName("final")]
	public int Final { get; set; }

	[JsonPropertyName("orig")]
	public int Orig { get; set; }

	[JsonPropertyName("ver")]
	public int Ver { get; set; }

	[JsonPropertyName("range")]
	public int Range { get; set; }

	[JsonPropertyName("type2")]
	public int Type2 { get; set; }

	[JsonPropertyName("hp")]
	public int Hp { get; set; }

	[JsonPropertyName("hp2")]
	public int Hp2 { get; set; }

	[JsonPropertyName("max_hp")]
	public int MaxHp { get; set; }

	[JsonPropertyName("fire")]
	public int Fire { get; set; }

	[JsonPropertyName("torpedo")]
	public int Torpedo { get; set; }

	[JsonPropertyName("anti_air")]
	public int AntiAir { get; set; }

	[JsonPropertyName("armor")]
	public int Armor { get; set; }

	[JsonPropertyName("luck")]
	public int Luck { get; set; }

	[JsonPropertyName("max_luck")]
	public int MaxLuck { get; set; }

	[JsonPropertyName("min_scout")]
	public int MinScout { get; set; }

	[JsonPropertyName("scout")]
	public int Scout { get; set; }

	[JsonPropertyName("min_asw")]
	public int MinAsw { get; set; }

	[JsonPropertyName("asw")]
	public int Asw { get; set; }

	[JsonPropertyName("min_avoid")]
	public int MinAvoid { get; set; }

	[JsonPropertyName("avoid")]
	public int Avoid { get; set; }

	[JsonPropertyName("speed")]
	public int Speed { get; set; }

	[JsonPropertyName("before")]
	public int Before { get; set; }

	[JsonPropertyName("next_lv")]
	public int NextLv { get; set; }

	[JsonPropertyName("sort")]
	public int Sort { get; set; }

	[JsonPropertyName("fuel")]
	public int Fuel { get; set; }

	[JsonPropertyName("ammo")]
	public int Ammo { get; set; }

	[JsonPropertyName("blueprints")]
	public int Blueprints { get; set; }

	[JsonPropertyName("reports")]
	public int Reports { get; set; }

	[JsonPropertyName("catapults")]
	public int Catapults { get; set; }
}
