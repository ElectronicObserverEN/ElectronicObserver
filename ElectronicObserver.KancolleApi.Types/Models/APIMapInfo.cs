namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMapInfo
{
	[JsonPropertyName("api_air_base_decks")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiAirBaseDecks { get; set; } = default!;

	[JsonPropertyName("api_cleared")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCleared { get; set; } = default!;

	[JsonPropertyName("api_defeat_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiDefeatCount { get; set; } = default!;

	[JsonPropertyName("api_eventmap")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiEventmap? ApiEventmap { get; set; } = default!;

	[JsonPropertyName("api_gauge_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiGaugeNum { get; set; } = default!;

	[JsonPropertyName("api_gauge_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiGaugeType { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_required_defeat_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiRequiredDefeatCount { get; set; } = default!;

	[JsonPropertyName("api_sally_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiSallyFlag { get; set; } = default!;
}
