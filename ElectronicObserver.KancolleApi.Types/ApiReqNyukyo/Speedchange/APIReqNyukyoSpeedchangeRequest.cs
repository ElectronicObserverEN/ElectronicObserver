namespace ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Speedchange;

public class ApiReqNyukyoSpeedchangeRequest
{
	[JsonPropertyName("api_ndock_id")]
	public string ApiNdockId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
