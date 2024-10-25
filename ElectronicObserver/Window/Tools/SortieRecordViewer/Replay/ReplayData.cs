using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;

public class ReplayData
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("now_maphp")]
	public int NowMaphp { get; set; }

	[JsonPropertyName("max_maphp")]
	public int MaxMaphp { get; set; }

	[JsonPropertyName("defeat_count")]
	public int DefeatCount { get; set; }

	[JsonPropertyName("world")]
	public int World { get; set; }
	
	[JsonPropertyName("mapnum")]
	public int Mapnum { get; set; }

	[JsonPropertyName("fleetnum")]
	public int Fleetnum { get; set; }

	[JsonPropertyName("combined")]
	public FleetType Combined { get; set; }
	
	[JsonPropertyName("fleet1")]
	public List<ReplayShip>? Fleet1 { get; set; }
	
	[JsonPropertyName("fleet2")]
	public List<ReplayShip>? Fleet2 { get; set; }

	[JsonPropertyName("fleet3")]
	public List<ReplayShip>? Fleet3 { get; set; }
	
	[JsonPropertyName("fleet4")]
	public List<ReplayShip>? Fleet4 { get; set; }
	
	[JsonPropertyName("support1")]
	public int Support1 { get; set; }
	
	[JsonPropertyName("support2")]
	public int Support2 { get; set; }
	
	[JsonPropertyName("lbas")]
	public List<ReplayAirBase>? AirBases { get; set; }
	
	[JsonPropertyName("time")]
	public long Time { get; set; }

	[JsonPropertyName("battles")]
	public List<ReplayBattle> Battles { get; set; } = new();
}
