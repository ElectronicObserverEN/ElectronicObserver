namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiCellDatum
{
	[System.Text.Json.Serialization.JsonPropertyName("api_color_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiColorNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_distance")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiDistance { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_passed")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPassed { get; set; } = default!;
}
