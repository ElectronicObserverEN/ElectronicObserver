namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipShip
{
	[JsonPropertyName("api_equip_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEquipType { get; set; } = new();

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiShipId { get; set; } = default!;
}
