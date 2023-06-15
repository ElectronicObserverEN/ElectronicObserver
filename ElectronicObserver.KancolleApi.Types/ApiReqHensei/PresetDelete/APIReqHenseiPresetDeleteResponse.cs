namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetDelete;

public class ApiReqHenseiPresetDeleteResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
