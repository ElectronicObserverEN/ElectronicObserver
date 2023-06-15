namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;

public class ApiReqKaisouSlotDepriveRequest
{
	[JsonPropertyName("api_set_idx")]
	public string ApiSetIdx { get; set; } = default!;

	[JsonPropertyName("api_set_ship")]
	public string ApiSetShip { get; set; } = default!;

	[JsonPropertyName("api_set_slot_kind")]
	public string ApiSetSlotKind { get; set; } = default!;

	[JsonPropertyName("api_unset_idx")]
	public string ApiUnsetIdx { get; set; } = default!;

	[JsonPropertyName("api_unset_ship")]
	public string ApiUnsetShip { get; set; } = default!;

	[JsonPropertyName("api_unset_slot_kind")]
	public string ApiUnsetSlotKind { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
