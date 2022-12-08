namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiShipData
{
	[JsonPropertyName("api_set_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiSetShip ApiSetShip { get; set; } = new();

	[JsonPropertyName("api_unset_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiSetShip ApiUnsetShip { get; set; } = new();
}
