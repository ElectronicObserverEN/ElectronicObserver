using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;

public class ApiReqKaisouPowerupResponse
{
	[JsonPropertyName("api_deck")]
	public List<ApiDeck> ApiDeck { get; set; } = new();

	[JsonPropertyName("api_powerup_flag")]
	public int ApiPowerupFlag { get; set; }

	[JsonPropertyName("api_ship")]
	public ApiShip ApiShip { get; set; } = new();
}
