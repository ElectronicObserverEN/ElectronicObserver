namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Slotset;

public class ApiReqKaisouSlotsetRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = "";

	[JsonPropertyName("api_item_id")]
	public string ApiItemId { get; set; } = "";

	[JsonPropertyName("api_slot_idx")]
	public string ApiSlotIdx { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
