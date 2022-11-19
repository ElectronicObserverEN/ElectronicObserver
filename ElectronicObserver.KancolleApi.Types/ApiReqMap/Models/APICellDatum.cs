namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiCellDatum
{
	[JsonPropertyName("api_color_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiColorNo { get; set; } = default!;

	[JsonPropertyName("api_distance")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiDistance { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_passed")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPassed { get; set; } = default!;
}
