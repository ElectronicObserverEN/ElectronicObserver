namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.CreateshipSpeedchange;

public class ApiReqKousyouCreateshipSpeedchangeRequest
{
	[JsonPropertyName("api_highspeed")]
	[Required(AllowEmptyStrings = true)]
	public string ApiHighspeed { get; set; } = default!;

	[JsonPropertyName("api_kdock_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiKdockId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
