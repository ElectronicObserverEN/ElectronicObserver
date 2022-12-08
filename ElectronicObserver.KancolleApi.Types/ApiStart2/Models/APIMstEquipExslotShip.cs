namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipExslotShip
{
	[JsonPropertyName("api_ship_ids")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipIds { get; set; } = new();

	[JsonPropertyName("api_slotitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSlotitemId { get; set; } = default!;
}
