using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorApiMstEquipShip
{
	[JsonPropertyName("api_ship_id")]
	public int ApiShipId { get; set; }

	[JsonPropertyName("api_equip_type")]
	public List<int> ApiEquipType { get; set; } = new();
}
