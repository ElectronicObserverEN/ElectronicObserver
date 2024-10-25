using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class FleetAfter
{
	[JsonPropertyName("main")]
	public required List<JsonNode> Main { get; init; }

	[JsonPropertyName("escort")]
	public required List<JsonNode>? Escort { get; init; }

	[JsonPropertyName("LBAC")]
	public required List<ApiAirBase> Lbac { get; init; }
}
