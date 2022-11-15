namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiGetShip
{
	[System.Text.Json.Serialization.JsonPropertyName("api_ship_getmes")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiShipGetmes { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiShipId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiShipName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiShipType { get; set; } = default!;
}
