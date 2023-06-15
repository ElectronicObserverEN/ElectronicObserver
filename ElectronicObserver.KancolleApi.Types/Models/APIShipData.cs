namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiShipData
{
	[JsonPropertyName("api_set_ship")]
	[Required]
	public ApiSetShip ApiSetShip { get; set; } = new();

	[JsonPropertyName("api_unset_ship")]
	[Required]
	public ApiSetShip ApiUnsetShip { get; set; } = new();
}
