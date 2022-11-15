namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMapInfo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_air_base_decks")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiAirBaseDecks { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_cleared")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCleared { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_defeat_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiDefeatCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_eventmap")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiEventmap? ApiEventmap { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_gauge_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiGaugeNum { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_gauge_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiGaugeType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_required_defeat_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiRequiredDefeatCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sally_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiSallyFlag { get; set; } = default!;
}
