namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Start;

public class ApiReqQuestStartRequest
{
	[JsonPropertyName("api_quest_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiQuestId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
