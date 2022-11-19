using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotExchangeIndex;

public class ApiReqKaisouSlotExchangeIndexResponse
{
	[JsonPropertyName("api_ship_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiShipData? ApiShipData { get; set; } = default!;

	[JsonPropertyName("api_slot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiSlot { get; set; } = default!;
}
