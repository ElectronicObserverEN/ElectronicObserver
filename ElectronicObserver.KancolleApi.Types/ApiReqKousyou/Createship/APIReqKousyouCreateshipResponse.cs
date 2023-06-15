namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createship;

public class ApiReqKousyouCreateshipResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
