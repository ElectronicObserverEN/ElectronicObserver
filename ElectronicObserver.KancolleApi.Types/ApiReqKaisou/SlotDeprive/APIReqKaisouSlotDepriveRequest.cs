namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;

public class ApiReqKaisouSlotDepriveRequest
{
	[JsonPropertyName("api_set_idx")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSetIdx { get; set; } = default!;

	[JsonPropertyName("api_set_ship")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSetShip { get; set; } = default!;

	[JsonPropertyName("api_set_slot_kind")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSetSlotKind { get; set; } = default!;

	[JsonPropertyName("api_unset_idx")]
	[Required(AllowEmptyStrings = true)]
	public string ApiUnsetIdx { get; set; } = default!;

	[JsonPropertyName("api_unset_ship")]
	[Required(AllowEmptyStrings = true)]
	public string ApiUnsetShip { get; set; } = default!;

	[JsonPropertyName("api_unset_slot_kind")]
	[Required(AllowEmptyStrings = true)]
	public string ApiUnsetSlotKind { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
