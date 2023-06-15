namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Slotset;

public class ApiReqKaisouSlotsetRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_item_id")]
	public string ApiItemId { get; set; } = default!;

	[JsonPropertyName("api_slot_idx")]
	public string ApiSlotIdx { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
