using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorMap
{
	[JsonPropertyName("area")]
	public int Area { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("boss")]
	public List<string> Boss { get; set; } = new();

	[JsonPropertyName("has_detail")]
	public int HasDetail { get; set; }

	[JsonPropertyName("has_air_raid")]
	public int HasAirRaid { get; set; }
}
