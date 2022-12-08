using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;

public class ApiReqKaisouSlotDepriveResponse
{
	[JsonPropertyName("api_ship_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiShipData ApiShipData { get; set; } = new();

	[JsonPropertyName("api_unset_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiUnsetList? ApiUnsetList { get; set; } = default!;
}
