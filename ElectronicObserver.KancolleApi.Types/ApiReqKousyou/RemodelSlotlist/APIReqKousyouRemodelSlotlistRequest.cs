namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;

public class ApiReqKousyouRemodelSlotlistRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
