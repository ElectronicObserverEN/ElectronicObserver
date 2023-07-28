using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.AnchorageRepair;

public class ApiReqMapAnchorageRepairResponse
{
	[JsonPropertyName("api_repair_ships")]
	public List<int> ApiRepairShips { get; set; } = new();

	[JsonPropertyName("api_ship_data")]
	public List<ApiShip> ApiShipData { get; set; } = new();

	[JsonPropertyName("api_used_ship")]
	public int ApiUsedShip { get; set; }
}
