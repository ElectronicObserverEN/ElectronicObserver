namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;

public class ApiReqKaisouSlotDepriveRequest
{
	[JsonPropertyName("api_set_idx")]
	public string ApiSetIdx { get; set; } = "";

	[JsonPropertyName("api_set_ship")]
	public string ApiSetShip { get; set; } = "";

	[JsonPropertyName("api_set_slot_kind")]
	public string ApiSetSlotKind { get; set; } = "";

	[JsonPropertyName("api_unset_idx")]
	public string ApiUnsetIdx { get; set; } = "";

	[JsonPropertyName("api_unset_ship")]
	public string ApiUnsetShip { get; set; } = "";

	[JsonPropertyName("api_unset_slot_kind")]
	public string ApiUnsetSlotKind { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
