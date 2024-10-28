using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbAirDefenseSubmission;

public class PoiDbAirDefenseSubmission
{
	[JsonPropertyName("maparea_id")]
	public required int World { get; init; }

	[JsonPropertyName("mapinfo_no")]
	public required int Map { get; init; }

	[JsonPropertyName("curCellId")]
	public required int Cell { get; init; }

	[JsonPropertyName("mapLevel")]
	public required int MapLevel { get; init; }

	[JsonPropertyName("version")]
	public required string Version { get; init; }
}
