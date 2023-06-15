namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Start;

public class ApiReqQuestStartResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
