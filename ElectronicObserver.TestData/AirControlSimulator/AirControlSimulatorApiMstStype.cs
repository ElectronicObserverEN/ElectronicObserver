using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorApiMstStype
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; }

	[JsonPropertyName("api_equip_type")]
	public List<int> ApiEquipType { get; set; }
}