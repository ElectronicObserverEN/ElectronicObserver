using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotExchangeIndex;

public class ApiReqKaisouSlotExchangeIndexResponse
{
	[JsonPropertyName("api_ship_data")]
	public ApiShipData? ApiShipData { get; set; }

	[JsonPropertyName("api_slot")]
	public List<int>? ApiSlot { get; set; }
}
