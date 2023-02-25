using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorMap
{
	[JsonPropertyName("area")]
	public int Area { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("boss")]
	public List<string> Boss { get; set; }

	[JsonPropertyName("has_detail")]
	public int HasDetail { get; set; }

	[JsonPropertyName("has_air_raid")]
	public int HasAirRaid { get; set; }
}