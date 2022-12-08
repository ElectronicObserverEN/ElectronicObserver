namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

public class ApiReqQuestClearitemgetRequest
{
	[JsonPropertyName("api_quest_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiQuestId { get; set; } = default!;

	[JsonPropertyName("api_select_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiSelectNo { get; set; } = default!;

	[JsonPropertyName("api_select_no2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiSelectNo2 { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
