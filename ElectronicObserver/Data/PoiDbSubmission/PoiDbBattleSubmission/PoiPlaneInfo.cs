using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class PoiPlaneInfo
{
	[JsonPropertyName("poi_slot")]
	public required PoiPlaneInfoSlot? PoiSlot { get; init; }

	[JsonPropertyName("api_cond")]
	public required AirBaseCondition? ApiCond { get; init; }

	[JsonPropertyName("api_count")]
	public required int? ApiCount { get; init; }

	[JsonPropertyName("api_max_count")]
	public required int? ApiMaxCount { get; init; }

	[JsonPropertyName("api_squadron_id")]
	public required int ApiSquadronId { get; init; }

	[JsonPropertyName("api_state")]
	public required int ApiState { get; init; }
}
