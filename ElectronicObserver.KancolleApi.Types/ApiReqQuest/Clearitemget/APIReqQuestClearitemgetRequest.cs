namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

public class ApiReqQuestClearitemgetRequest
{
	[JsonPropertyName("api_quest_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiQuestId { get; set; } = default!;

	[JsonPropertyName("api_select_no")]
	public string? ApiSelectNo { get; set; } = default!;

	[JsonPropertyName("api_select_no2")]
	public string? ApiSelectNo2 { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
