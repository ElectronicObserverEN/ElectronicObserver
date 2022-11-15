namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Change;

public class ApiReqHenseiChangeResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_change_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiChangeCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_result")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiResult { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_result_msg")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiResultMsg { get; set; } = default!;
}
