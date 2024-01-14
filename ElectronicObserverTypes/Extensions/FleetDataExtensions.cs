using System.Linq;

namespace ElectronicObserverTypes.Extensions;
public static class FleetDataExtensions
{
	public static int NumberOfSurfaceShipNotRetreatedNotSunk(this IFleetData fleet) =>
		fleet.MembersWithoutEscaped?.Count(ship => ship is not null && ship.HPCurrent > 0 && !ship.MasterShip.IsSubmarine) ?? 0;

	public static SupportType GetSupportType(this IFleetData fleet)
	{
		int destroyerCount = 0;
		int aircraftCarrierCount = 0;
		int aircraftAuxiliaryCount = 0;
		int aircraftShellingCount = 0;
		int shellingCount = 0;
		int battleshipCount = 0;
		int heavyCruiserCount = 0;

		foreach (IShipData? ship in fleet.MembersInstance)
		{
			if (ship is null) continue;

			switch (ship.MasterShip.ShipType)
			{
				case ShipTypes.Destroyer:
					destroyerCount++;
					break;

				case ShipTypes.AircraftCarrier:
				case ShipTypes.LightAircraftCarrier:
				case ShipTypes.ArmoredAircraftCarrier:
					aircraftCarrierCount++;
					break;

				case ShipTypes.SeaplaneTender:
				case ShipTypes.AmphibiousAssaultShip:
					aircraftAuxiliaryCount++;
					break;

				case ShipTypes.AviationBattleship:
					aircraftShellingCount++;
					battleshipCount++;
					break;

				case ShipTypes.AviationCruiser:
					aircraftShellingCount++;
					heavyCruiserCount++;
					break;

				case ShipTypes.Transport:
					aircraftShellingCount++;
					break;

				case ShipTypes.Battleship:
				case ShipTypes.Battlecruiser:
					shellingCount++;
					battleshipCount++;
					break;

				case ShipTypes.HeavyCruiser:
					shellingCount++;
					heavyCruiserCount++;
					break;
			}

		}

		// 発生しない
		if (destroyerCount < 2) return SupportType.None;

		if (shellingCount == 0)
		{
			if (aircraftCarrierCount >= 1 ||
				aircraftAuxiliaryCount >= 2 ||
				aircraftShellingCount >= 2)
				return SupportType.Aerial;   // 空撃
		}
		if (shellingCount == 1)
		{
			if (aircraftCarrierCount + aircraftAuxiliaryCount >= 2)
				return SupportType.Aerial;   // 空撃
		}

		if (battleshipCount >= 2 ||
			(battleshipCount == 1 && heavyCruiserCount >= 3) ||
			heavyCruiserCount >= 4)
			return SupportType.Shelling;       // 砲撃

		return SupportType.Torpedo;           // 雷撃
	}
}
