using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;

public class ApiReqKaisouPowerupResponse
{
	[JsonPropertyName("api_deck")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiDeck> ApiDeck { get; set; } = new();

	[JsonPropertyName("api_powerup_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPowerupFlag { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiShip ApiShip { get; set; } = new();
}
