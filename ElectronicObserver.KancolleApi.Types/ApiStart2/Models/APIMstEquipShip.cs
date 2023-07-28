namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipShip
{
	[JsonPropertyName("api_equip_type")]
	public List<int> ApiEquipType { get; set; } = new();

	[JsonPropertyName("api_ship_id")]
	public int ApiShipId { get; set; }
}
