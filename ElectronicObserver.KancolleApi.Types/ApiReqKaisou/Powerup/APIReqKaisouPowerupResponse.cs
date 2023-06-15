using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;

public class ApiReqKaisouPowerupResponse
{
	[JsonPropertyName("api_deck")]
	[Required]
	public List<ApiDeck> ApiDeck { get; set; } = new();

	[JsonPropertyName("api_powerup_flag")]
	public int ApiPowerupFlag { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[Required]
	public ApiShip ApiShip { get; set; } = new();
}
