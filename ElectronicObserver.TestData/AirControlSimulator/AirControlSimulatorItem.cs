using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorItem
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("type")]
	public int Type { get; set; }

	[JsonPropertyName("itype")]
	public int Itype { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("abbr")]
	public string Abbr { get; set; } = string.Empty;

	[JsonPropertyName("fire")]
	public int Fire { get; set; }

	[JsonPropertyName("antiAir")]
	public int AntiAir { get; set; }

	[JsonPropertyName("torpedo")]
	public int Torpedo { get; set; }

	[JsonPropertyName("bomber")]
	public int Bomber { get; set; }

	[JsonPropertyName("armor")]
	public int Armor { get; set; }

	[JsonPropertyName("asw")]
	public int Asw { get; set; }

	[JsonPropertyName("antiBomber")]
	public int AntiBomber { get; set; }

	[JsonPropertyName("interception")]
	public int Interception { get; set; }

	[JsonPropertyName("scout")]
	public int Scout { get; set; }

	[JsonPropertyName("canRemodel")]
	public int CanRemodel { get; set; }

	[JsonPropertyName("accuracy")]
	public int Accuracy { get; set; }

	[JsonPropertyName("avoid2")]
	public int Avoid2 { get; set; }

	[JsonPropertyName("radius")]
	public int Radius { get; set; }

	[JsonPropertyName("cost")]
	public int Cost { get; set; }

	[JsonPropertyName("avoid")]
	public int Avoid { get; set; }

	[JsonPropertyName("range")]
	public int Range { get; set; }

	[JsonPropertyName("grow")]
	public int Grow { get; set; }
}
