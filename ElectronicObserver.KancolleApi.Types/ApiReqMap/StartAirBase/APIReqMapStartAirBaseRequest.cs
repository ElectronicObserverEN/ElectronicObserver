namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.StartAirBase;

public class ApiReqMapStartAirBaseRequest
{
	[JsonPropertyName("api_strike_point_1")]
	public string? ApiStrikePoint1 { get; set; } = default!;

	[JsonPropertyName("api_strike_point_2")]
	public string? ApiStrikePoint2 { get; set; } = default!;

	[JsonPropertyName("api_strike_point_3")]
	public string? ApiStrikePoint3 { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
