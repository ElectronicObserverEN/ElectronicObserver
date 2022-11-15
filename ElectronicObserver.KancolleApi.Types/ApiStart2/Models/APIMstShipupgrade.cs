namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShipupgrade
{
	[System.Text.Json.Serialization.JsonPropertyName("api_aviation_mat_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiAviationMatCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_catapult_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCatapultCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_current_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCurrentShipId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_drawing_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDrawingCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_original_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiOriginalShipId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_report_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiReportCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sortno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSortno { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_upgrade_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUpgradeLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_upgrade_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUpgradeType { get; set; } = default!;
}
