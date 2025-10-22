using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class NextInfo
{
	[JsonPropertyName("api_maparea_id")]
	public required string World { get; init; }

	[JsonPropertyName("api_mapinfo_no")]
	public required string Map { get; init; }

	[JsonPropertyName("api_itemget")]
	public required List<PoiApiItemget>? ItemGet { get; init; }

	[JsonPropertyName("api_happening")]
	public required PoiApiHappening? Happening { get; init; }

	[JsonPropertyName("api_required_defeat_count")]
	public required string? RequiredDefeatCount { get; init; }

	[JsonPropertyName("api_defeat_count")]
	public required string? DefeatCount { get; init; }

	[JsonPropertyName("api_max_maphp")]
	public required string? ApiMaxMaphp { get; init; }

	[JsonPropertyName("api_now_maphp")]
	public required string? ApiNowMaphp { get; init; }
}
