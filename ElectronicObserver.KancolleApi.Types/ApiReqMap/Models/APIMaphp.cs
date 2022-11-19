namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiMaphp
{
	[JsonPropertyName("api_gauge_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGaugeNum { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_gauge_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object? ApiGaugeType { get; set; } = default!;

	[JsonPropertyName("api_max_maphp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMaxMaphp { get; set; } = default!;

	[JsonPropertyName("api_now_maphp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNowMaphp { get; set; } = default!;
}
