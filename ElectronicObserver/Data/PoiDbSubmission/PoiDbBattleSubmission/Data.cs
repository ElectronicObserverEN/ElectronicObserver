using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class Data
{
	[JsonPropertyName("fleet")]
	public required Fleet Fleet { get; init; }

	[JsonPropertyName("fleetAfter")]
	public required FleetAfter FleetAfter { get; init; }

	/// <summary>
	/// [World ID, Map ID, Node ID]
	/// </summary>
	[JsonPropertyName("map")]
	public required List<int> Map { get; init; }

	/// <summary>
	/// Raw response data from battles.
	/// </summary>
	[JsonPropertyName("packet")]
	public required List<JsonNode> Packet { get; init; }

	/// <summary>
	/// "Normal" or "Boss"
	/// </summary>
	[JsonPropertyName("type")]
	public required string Type { get; init; }

	[JsonPropertyName("version")]
	public required string Version { get; init; }

	/// <summary>
	/// Unix microseconds at the time of submission.
	/// All battles in a sortie need to have the same group id so the analysis tools know which battles belong together.
	/// </summary>
	[JsonPropertyName("groupId")]
	public string GroupId { get; set; } = "0";

	/// <summary>
	/// Cell count of the current map.
	/// </summary>
	[JsonPropertyName("api_cell_data")]
	public required int ApiCellData { get; init; }

	[JsonPropertyName("mapLevel")]
	public required int MapLevel { get; init; }

	[JsonPropertyName("time")]
	public required long Time { get; init; }
}
