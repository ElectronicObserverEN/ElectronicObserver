using System.Collections.Generic;

namespace ElectronicObserver.Database.Sortie;

public class SortieFleetData
{
	public int FleetId { get; set; }

	/// <summary>
	/// 0 = none, 1~4 = fleets
	/// </summary>
	public int NodeSupportFleetId { get; set; }
	public int BossSupportFleetId { get; set; }
	public int CombinedFlag { get; set; }
	public List<SortieFleet?> Fleets { get; set; } = new();
	public List<SortieAirBase> AirBases { get; set; } = new();
}