namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Stop;

public class ApiReqQuestStopResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
