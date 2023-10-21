namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiMaphp
{
	[JsonPropertyName("api_gauge_num")]
	public int ApiGaugeNum { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_gauge_type")]
	public object? ApiGaugeType { get; set; }

	[JsonPropertyName("api_max_maphp")]
	public int ApiMaxMaphp { get; set; }

	[JsonPropertyName("api_now_maphp")]
	public int ApiNowMaphp { get; set; }
}
