using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.AnchorageRepair;

public class ApiReqMapAnchorageRepairResponse
{
	[JsonPropertyName("api_repair_ships")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiRepairShips { get; set; } = new();

	[JsonPropertyName("api_ship_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiShipDatum> ApiShipData { get; set; } = new();

	[JsonPropertyName("api_used_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUsedShip { get; set; } = default!;
}
