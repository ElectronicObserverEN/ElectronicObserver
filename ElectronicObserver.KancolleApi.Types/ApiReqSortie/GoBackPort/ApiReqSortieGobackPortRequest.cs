namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.GoBackPort;

public class ApiReqSortieGobackPortRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
