namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Combined;

public class ApiReqHenseiCombinedRequest
{
	[JsonPropertyName("api_combined_type")]
	public string ApiCombinedType { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
