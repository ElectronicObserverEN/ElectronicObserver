using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteSubmissionData
{
	[JsonPropertyName("combined_type")]
	public required FleetType FleetType { get; init; }

	[JsonPropertyName("deck1")]
	public required List<PoiDbRouteShip> Deck1 { get; init; }

	[JsonPropertyName("deck2")]
	public required List<PoiDbRouteShip>? Deck2 { get; init; }

	/// <summary>
	/// The type here is either <see cref="PoiDbRouteEquipment" /> or -1 />
	/// </summary>
	[JsonPropertyName("slot1")]
	public required List<List<object>> Slot1 { get; init; }

	/// <summary>
	/// The type here is either <see cref="PoiDbRouteEquipment" /> or -1 />
	/// </summary>
	[JsonPropertyName("slot2")]
	public required List<List<object>>? Slot2 { get; init; }

	[JsonPropertyName("cell_ids")]
	public required List<int> CellIds { get; init; }

	/// <summary>
	/// list with a single element of all difficulties
	/// key = world * 10 + map (591 for event 59 E1)
	/// value = selected difficulty
	/// </summary>
	[JsonPropertyName("mapLevels")]
	public required List<Dictionary<int, string>> MapLevels { get; init; }

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
