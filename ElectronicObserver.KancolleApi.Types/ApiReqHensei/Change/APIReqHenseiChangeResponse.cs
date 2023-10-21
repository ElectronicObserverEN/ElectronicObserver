namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Change;

public class ApiReqHenseiChangeResponse
{
	[JsonPropertyName("api_change_count")]
	public int? ApiChangeCount { get; set; }

	[JsonPropertyName("api_result")]
	public int? ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string? ApiResultMsg { get; set; }
}
