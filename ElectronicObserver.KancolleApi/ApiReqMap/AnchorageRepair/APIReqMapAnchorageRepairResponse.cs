using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.AnchorageRepair;

public class ApiReqMapAnchorageRepairResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_repair_ships")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiRepairShips { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiShipDatum> ApiShipData { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_used_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUsedShip { get; set; } = default!;
}
