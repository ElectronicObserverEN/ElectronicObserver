namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Change;

public class ApiReqHenseiChangeResponse
{
	[JsonPropertyName("api_change_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiChangeCount { get; set; } = default!;

	[JsonPropertyName("api_result")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiResultMsg { get; set; } = default!;
}
