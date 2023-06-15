namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotExchangeIndex;

public class ApiReqKaisouSlotExchangeIndexRequest
{
	[JsonPropertyName("api_dst_idx")]
	public string ApiDstIdx { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_src_idx")]
	public string ApiSrcIdx { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
