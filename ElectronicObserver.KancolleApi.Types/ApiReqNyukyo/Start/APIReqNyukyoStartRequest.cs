namespace ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Start;

public class ApiReqNyukyoStartRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_highspeed")]
	[Required(AllowEmptyStrings = true)]
	public string ApiHighspeed { get; set; } = default!;

	[JsonPropertyName("api_ndock_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNdockId { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
