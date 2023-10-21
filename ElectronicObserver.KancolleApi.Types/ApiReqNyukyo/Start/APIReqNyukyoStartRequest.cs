namespace ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Start;

public class ApiReqNyukyoStartRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_highspeed")]
	public string ApiHighspeed { get; set; } = "";

	[JsonPropertyName("api_ndock_id")]
	public string ApiNdockId { get; set; } = "";

	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
