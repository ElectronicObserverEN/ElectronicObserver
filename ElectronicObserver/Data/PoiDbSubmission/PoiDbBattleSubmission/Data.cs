using System.Collections.Generic;
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
	/// Raw response strings from battles.
	/// </summary>
	[JsonPropertyName("packet")]
	public required List<string> Packet { get; init; }

	/// <summary>
	/// "Normal" or "Boss"
	/// </summary>
	[JsonPropertyName("type")]
	public required string Type { get; init; }

	[JsonPropertyName("version")]
	public required string Version { get; init; }

	/// <summary>
	/// Not used.
	/// </summary>
	[JsonPropertyName("groupId")]
	public string GroupId { get; init; } = "0";

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
