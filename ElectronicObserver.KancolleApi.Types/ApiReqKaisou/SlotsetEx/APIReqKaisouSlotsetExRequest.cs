namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotsetEx;

public class ApiReqKaisouSlotsetExRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = "";

	[JsonPropertyName("api_item_id")]
	public string ApiItemId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
