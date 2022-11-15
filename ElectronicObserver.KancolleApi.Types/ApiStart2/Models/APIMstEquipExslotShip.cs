namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipExslotShip
{
	[System.Text.Json.Serialization.JsonPropertyName("api_ship_ids")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipIds { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_slotitem_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSlotitemId { get; set; } = default!;
}
