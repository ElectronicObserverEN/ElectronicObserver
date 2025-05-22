using System.Collections.Generic;

namespace ElectronicObserver.Core.Types.Mocks;

public class BaseAirCorpsDataMock : IBaseAirCorpsData
{
	public int MapAreaID { get; set; }
	public int AirCorpsID { get; set; }
	public string Name { get; set; }
	public int Distance { get; set; }
	public int BonusDistance { get; set; }
	public int BaseDistance { get; set; }
	public AirBaseActionKind ActionKind { get; set; }
	public List<int> StrikePoints { get; set; }
	public IDictionary<int, IBaseAirCorpsSquadron> Squadrons { get; set; }
	public int ID { get; set; }
	public bool IsAvailable { get; set; }
	public int HPCurrent { get; set; }
	public int HPMax { get; set; }
}
