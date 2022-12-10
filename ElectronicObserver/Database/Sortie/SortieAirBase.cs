using System.Collections.Generic;
using ElectronicObserverTypes;

namespace ElectronicObserver.Database.Sortie;

public class SortieAirBase
{
	public string Name { get; set; } = "";
	public int MapAreaId { get; set; }
	public int AirCorpsId { get; set; }
	public AirBaseActionKind ActionKind { get; set; }
	public int BaseDistance { get; set; }
	public int BonusDistance { get; set; }
	public List<SortieAirBaseSquadron> Squadrons { get; set; } = new();
}
