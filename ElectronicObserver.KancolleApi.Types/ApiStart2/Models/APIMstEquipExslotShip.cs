namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipExslotShip
{
	[JsonPropertyName("api_ship_ids")]
	public List<int> ApiShipIds { get; set; } = new();

	[JsonPropertyName("api_slotitem_id")]
	public int ApiSlotitemId { get; set; }
}
