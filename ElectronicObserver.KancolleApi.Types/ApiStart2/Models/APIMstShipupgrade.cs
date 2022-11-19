namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShipupgrade
{
	[JsonPropertyName("api_aviation_mat_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiAviationMatCount { get; set; } = default!;

	[JsonPropertyName("api_catapult_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCatapultCount { get; set; } = default!;

	[JsonPropertyName("api_current_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCurrentShipId { get; set; } = default!;

	[JsonPropertyName("api_drawing_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDrawingCount { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_original_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOriginalShipId { get; set; } = default!;

	[JsonPropertyName("api_report_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiReportCount { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_upgrade_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUpgradeLevel { get; set; } = default!;

	[JsonPropertyName("api_upgrade_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUpgradeType { get; set; } = default!;
}
