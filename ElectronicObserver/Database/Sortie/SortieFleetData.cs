using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Database.Sortie;

public class SortieFleetData
{
	[JsonPropertyName("FleetId")]
	public int FleetId { get; set; }

	/// <summary>
	/// 0 = none, 1~4 = fleets
	/// </summary>
	[JsonPropertyName("NodeSupportFleetId")]
	public int NodeSupportFleetId { get; set; }

	[JsonPropertyName("BossSupportFleetId")]
	public int BossSupportFleetId { get; set; }

	[JsonPropertyName("CombinedFlag")]
	public FleetType CombinedFlag { get; set; }

	[JsonPropertyName("Fleets")]
	public List<SortieFleet?> Fleets { get; set; } = new();

	[JsonPropertyName("AirBases")]
	public List<SortieAirBase> AirBases { get; set; } = new();
}
