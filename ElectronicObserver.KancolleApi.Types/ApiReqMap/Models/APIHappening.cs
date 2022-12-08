namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiHappening
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_dentan")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDentan { get; set; } = default!;

	[JsonPropertyName("api_icon_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiIconId { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_usemst")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUsemst { get; set; } = default!;
}
