namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShipupgrade
{
	[JsonPropertyName("api_aviation_mat_count")]
	public int? ApiAviationMatCount { get; set; } = default!;

	[JsonPropertyName("api_catapult_count")]
	public int ApiCatapultCount { get; set; } = default!;

	[JsonPropertyName("api_current_ship_id")]
	public int ApiCurrentShipId { get; set; } = default!;

	[JsonPropertyName("api_drawing_count")]
	public int ApiDrawingCount { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_original_ship_id")]
	public int ApiOriginalShipId { get; set; } = default!;

	[JsonPropertyName("api_report_count")]
	public int ApiReportCount { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	public int ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_upgrade_level")]
	public int ApiUpgradeLevel { get; set; } = default!;

	[JsonPropertyName("api_upgrade_type")]
	public int ApiUpgradeType { get; set; } = default!;
}
