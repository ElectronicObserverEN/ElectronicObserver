namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotExchangeIndex;

public class ApiReqKaisouSlotExchangeIndexRequest
{
	[JsonPropertyName("api_dst_idx")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDstIdx { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_src_idx")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSrcIdx { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
