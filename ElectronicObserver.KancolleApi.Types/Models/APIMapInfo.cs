namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMapInfo
{
	[JsonPropertyName("api_air_base_decks")]
	public int? ApiAirBaseDecks { get; set; } = default!;

	[JsonPropertyName("api_cleared")]
	public int ApiCleared { get; set; } = default!;

	[JsonPropertyName("api_defeat_count")]
	public int? ApiDefeatCount { get; set; } = default!;

	[JsonPropertyName("api_eventmap")]
	public ApiEventmap? ApiEventmap { get; set; } = default!;

	[JsonPropertyName("api_gauge_num")]
	public int? ApiGaugeNum { get; set; } = default!;

	[JsonPropertyName("api_gauge_type")]
	public int? ApiGaugeType { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_required_defeat_count")]
	public int? ApiRequiredDefeatCount { get; set; } = default!;

	[JsonPropertyName("api_sally_flag")]
	public List<int>? ApiSallyFlag { get; set; } = default!;
}
