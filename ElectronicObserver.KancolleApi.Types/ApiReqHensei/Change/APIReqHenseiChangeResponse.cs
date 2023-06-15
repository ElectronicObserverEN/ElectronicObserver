namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Change;

public class ApiReqHenseiChangeResponse
{
	[JsonPropertyName("api_change_count")]
	public int? ApiChangeCount { get; set; } = default!;

	[JsonPropertyName("api_result")]
	public int? ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string? ApiResultMsg { get; set; } = default!;
}
