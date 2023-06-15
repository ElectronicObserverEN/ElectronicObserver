namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMapInfo
{
	[JsonPropertyName("api_air_base_decks")]
	public int? ApiAirBaseDecks { get; set; }

	[JsonPropertyName("api_cleared")]
	public int ApiCleared { get; set; }

	[JsonPropertyName("api_defeat_count")]
	public int? ApiDefeatCount { get; set; }

	[JsonPropertyName("api_eventmap")]
	public ApiEventmap? ApiEventmap { get; set; }

	[JsonPropertyName("api_gauge_num")]
	public int? ApiGaugeNum { get; set; }

	[JsonPropertyName("api_gauge_type")]
	public int? ApiGaugeType { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_required_defeat_count")]
	public int? ApiRequiredDefeatCount { get; set; }

	[JsonPropertyName("api_sally_flag")]
	public List<int>? ApiSallyFlag { get; set; }
}
