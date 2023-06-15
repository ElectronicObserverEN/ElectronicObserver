namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

public class ApiReqQuestClearitemgetRequest
{
	[JsonPropertyName("api_quest_id")]
	public string ApiQuestId { get; set; } = "";

	[JsonPropertyName("api_select_no")]
	public string? ApiSelectNo { get; set; }

	[JsonPropertyName("api_select_no2")]
	public string? ApiSelectNo2 { get; set; }

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
