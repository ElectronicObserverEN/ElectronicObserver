namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotsetEx;

public class ApiReqKaisouSlotsetExRequest
{
	[JsonPropertyName("api_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_item_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiItemId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
