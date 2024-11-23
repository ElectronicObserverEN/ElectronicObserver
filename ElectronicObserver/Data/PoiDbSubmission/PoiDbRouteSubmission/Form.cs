using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class Form
{
	[JsonPropertyName("deck1")]
	public required List<JsonNode> Deck1 { get; init; }

	[JsonPropertyName("deck2")]
	public required List<JsonNode>? Deck2 { get; init; }

	[JsonPropertyName("slot1")]
	public required List<List<JsonNode>> Slot1 { get; init; }

	[JsonPropertyName("slot2")]
	public required List<List<JsonNode>>? Slot2 { get; init; }

	[JsonPropertyName("cell_ids")]
	public required List<int> CellIds { get; init; }

	/// <summary>
	/// key = world * 10 + map (591 for event 59 E1)
	/// value = selected difficulty
	/// </summary>
	[JsonPropertyName("mapLevels")]
	public required Dictionary<int, int> MapLevels { get; init; }

	[JsonPropertyName("nextInfo")]
	public required NextInfo NextInfo { get; init; }

	[JsonPropertyName("escapeList")]
	public required List<int> EscapeList { get; init; }

	[JsonPropertyName("teitokuLv")]
	public required int AdmiralLevel { get; init; }

	[JsonPropertyName("saku")]
	public required LosValues LosValues { get; init; }

	/// <summary>
	/// Cell count of the current map.
	/// </summary>
	[JsonPropertyName("api_cell_data")]
	public required int ApiCellData { get; init; }

	[JsonPropertyName("version")]
	public required string Version { get; init; }
}
