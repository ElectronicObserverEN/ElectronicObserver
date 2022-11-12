namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

public class ApiReqQuestClearitemgetRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_quest_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiQuestId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_select_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiSelectNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_select_no2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiSelectNo2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
