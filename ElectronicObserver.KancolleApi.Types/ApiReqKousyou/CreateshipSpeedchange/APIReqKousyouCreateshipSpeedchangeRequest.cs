namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.CreateshipSpeedchange;

public class ApiReqKousyouCreateshipSpeedchangeRequest
{
	[JsonPropertyName("api_highspeed")]
	public string ApiHighspeed { get; set; } = default!;

	[JsonPropertyName("api_kdock_id")]
	public string ApiKdockId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
