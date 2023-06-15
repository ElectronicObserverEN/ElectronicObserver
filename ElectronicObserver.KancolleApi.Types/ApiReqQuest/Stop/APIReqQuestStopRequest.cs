namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Stop;

public class ApiReqQuestStopRequest
{
	[JsonPropertyName("api_quest_id")]
	public string ApiQuestId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
