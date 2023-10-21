namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Remodeling;

public class ApiReqKaisouRemodelingResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
