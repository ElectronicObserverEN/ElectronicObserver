namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Start;

public class ApiReqQuestStartRequest
{
	[JsonPropertyName("api_quest_id")]
	public string ApiQuestId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
