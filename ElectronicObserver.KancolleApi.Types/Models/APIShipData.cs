namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiShipData
{
	[System.Text.Json.Serialization.JsonPropertyName("api_set_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiSetShip ApiSetShip { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiSetShip ApiUnsetShip { get; set; } = new();
}
