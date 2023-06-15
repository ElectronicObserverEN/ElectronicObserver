namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiShipData
{
	[JsonPropertyName("api_set_ship")]
	public ApiSetShip ApiSetShip { get; set; } = new();

	[JsonPropertyName("api_unset_ship")]
	public ApiSetShip ApiUnsetShip { get; set; } = new();
}
