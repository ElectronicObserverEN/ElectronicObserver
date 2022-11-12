using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;

public class ApiReqKaisouSlotDepriveResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_ship_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiShipData ApiShipData { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiUnsetList? ApiUnsetList { get; set; } = default!;
}
