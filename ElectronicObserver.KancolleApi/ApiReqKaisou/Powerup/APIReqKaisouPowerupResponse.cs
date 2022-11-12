using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;

public class ApiReqKaisouPowerupResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_deck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiDeck> ApiDeck { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_powerup_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPowerupFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiShip ApiShip { get; set; } = new();
}
