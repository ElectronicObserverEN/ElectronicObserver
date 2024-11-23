using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbFriendFleetSubmission;

public class Body
{
	[JsonPropertyName("maparea_id")]
	public required int World { get; init; }

	[JsonPropertyName("mapinfo_no")]
	public required int Map { get; init; }

	[JsonPropertyName("curCellId")]
	public required int Cell { get; init; }

	[JsonPropertyName("mapLevel")]
	public required int MapLevel { get; init; }

	[JsonPropertyName("friendly_status")]
	public required FriendlyStatus FriendlyStatus { get; init; }

	[JsonPropertyName("api_friendly_battle")]
	public required JsonNode ApiFriendlyBattle { get; init; }

	[JsonPropertyName("escapeList")]
	public required List<int> EscapeList { get; init; }

	[JsonPropertyName("formation")]
	public required int Formation { get; init; }

	[JsonPropertyName("enemy")]
	public required Dictionary<string, JsonNode?> Enemy { get; init; }

	[JsonPropertyName("deck1")]
	public required List<JsonNode> Deck1 { get; init; }

	[JsonPropertyName("deck2")]
	public required List<JsonNode>? Deck2 { get; init; }

	[JsonPropertyName("version")]
	public required string Version { get; init; }
}
