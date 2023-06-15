namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.StartAirBase;

public class ApiReqMapStartAirBaseResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
