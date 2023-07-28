namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.UnsetslotAll;

public class ApiReqKaisouUnsetslotAllResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
