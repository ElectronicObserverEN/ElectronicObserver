namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Slotset;

public class ApiReqKaisouSlotsetResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
