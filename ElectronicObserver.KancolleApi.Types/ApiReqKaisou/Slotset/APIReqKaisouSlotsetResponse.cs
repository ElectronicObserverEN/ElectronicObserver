namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Slotset;

public class ApiReqKaisouSlotsetResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
