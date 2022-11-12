namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipShip
{
	[System.Text.Json.Serialization.JsonPropertyName("api_equip_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEquipType { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiShipId { get; set; } = default!;
}
