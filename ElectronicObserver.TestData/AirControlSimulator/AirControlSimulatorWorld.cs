using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorWorld
{
	[JsonPropertyName("world")]
	public int World { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
}
