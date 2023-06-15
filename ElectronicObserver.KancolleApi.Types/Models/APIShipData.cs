namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiShipData
{
	[JsonPropertyName("api_set_ship")]
	public ApiShip ApiSetShip { get; set; } = new();

	[JsonPropertyName("api_unset_ship")]
	public ApiShip ApiUnsetShip { get; set; } = new();
}
