using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;

public class ApiReqKaisouSlotDepriveResponse
{
	[JsonPropertyName("api_ship_data")]
	public ApiShipData ApiShipData { get; set; } = new();

	[JsonPropertyName("api_unset_list")]
	public ApiUnsetList? ApiUnsetList { get; set; }
}
