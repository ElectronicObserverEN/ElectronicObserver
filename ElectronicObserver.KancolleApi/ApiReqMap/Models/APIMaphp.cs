namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiMaphp
{
	[System.Text.Json.Serialization.JsonPropertyName("api_gauge_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGaugeNum { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[System.Text.Json.Serialization.JsonPropertyName("api_gauge_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public object? ApiGaugeType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_maphp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMaxMaphp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_now_maphp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNowMaphp { get; set; } = default!;
}
