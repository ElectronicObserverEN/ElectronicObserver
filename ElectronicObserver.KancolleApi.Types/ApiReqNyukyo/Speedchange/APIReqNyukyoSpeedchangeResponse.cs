namespace ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Speedchange;

public class ApiReqNyukyoSpeedchangeResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
