using System.Linq;

namespace ElectronicObserverTypes.Extensions;
public static class FleetDataExtensions
{
	public static int NumberOfSurfaceShipNotRetreatedNotSunk(this IFleetData fleet) =>
		fleet.MembersInstance.Count(ship => ship is not null && ship.HPCurrent > 0 && !ship.MasterShip.IsSubmarine && !fleet.EscapedShipList.Contains(ship.MasterID));
}
