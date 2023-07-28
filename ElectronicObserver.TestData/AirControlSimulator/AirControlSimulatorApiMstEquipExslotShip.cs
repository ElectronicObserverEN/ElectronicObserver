using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorApiMstEquipExslotShip
{
	[JsonPropertyName("api_slotitem_id")]
	public int ApiSlotitemId { get; set; }

	[JsonPropertyName("api_ship_ids")]
	public List<int> ApiShipIds { get; set; } = new();
}
