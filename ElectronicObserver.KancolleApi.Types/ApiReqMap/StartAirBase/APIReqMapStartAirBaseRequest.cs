namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.StartAirBase;

public class ApiReqMapStartAirBaseRequest
{
	[JsonPropertyName("api_strike_point_1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiStrikePoint1 { get; set; } = default!;

	[JsonPropertyName("api_strike_point_2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiStrikePoint2 { get; set; } = default!;

	[JsonPropertyName("api_strike_point_3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiStrikePoint3 { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
