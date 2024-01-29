using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Extensions;
public static class FleetDataExtensions
{
	public static int NumberOfSurfaceShipNotRetreatedNotSunk(this IFleetData fleet) =>
		fleet.MembersWithoutEscaped?.Count(ship => ship is not null && ship.HPCurrent > 0 && !ship.MasterShip.IsSubmarine) ?? 0;

	public static SupportType GetSupportType(this IFleetData fleet)
	{
		int destroyers = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Destroyer);

		if (destroyers < 2) return SupportType.None;

		if (IsAirSupport(fleet))
		{
			if (IsAntiSubmarineSupport(fleet))
			{
				return SupportType.AntiSubmarine;
			}

			return SupportType.Aerial;
		}

		if (IsShellingSupport(fleet))
		{
			return SupportType.Shelling;
		}

		return SupportType.Torpedo;
	}

	private static bool IsAirSupport(IFleetData fleet)
	{
		int carriers = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.AircraftCarrier or
			ShipTypes.ArmoredAircraftCarrier or
			ShipTypes.LightAircraftCarrier);

		int carrierSupportA = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.SeaplaneTender or
			ShipTypes.AmphibiousAssaultShip);

		int carrierSupportB = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.AviationBattleship or
			ShipTypes.AviationCruiser or
			ShipTypes.FleetOiler);

		int gunboats = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Battleship or
			ShipTypes.Battlecruiser or
			ShipTypes.HeavyCruiser);

		if (gunboats is 0)
		{
			if (carriers >= 1) return true;
			if (carrierSupportA >= 2) return true;
			if (carrierSupportB >= 2) return true;
		}

		if (gunboats is 1)
		{
			return carriers + carrierSupportA >= 2;
		}

		return false;
	}

	/// <summary>
	/// This function doesn't check if it's a valid air support,
	/// so that check must be performed before calling this one.
	/// </summary>
	private static bool IsAntiSubmarineSupport(IFleetData fleet)
	{
		List<IShipData> antiSubmarineAircraftCarriers = fleet.MembersInstance
			.Where(s => s is not null)
			.Cast<IShipData>()
			.Where(s => s.HasAntiSubmarineAircraft())
			.ToList();

		int lightCarriers = antiSubmarineAircraftCarriers
			.Count(s => s.MasterShip.ShipType is ShipTypes.LightAircraftCarrier);

		int escorts = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Escort);

		return lightCarriers > 0 && (antiSubmarineAircraftCarriers.Count > 1 || escorts > 1);
	}

	private static bool IsShellingSupport(IFleetData fleet)
	{
		int battleships = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Battleship or
			ShipTypes.Battlecruiser or
			ShipTypes.AviationBattleship);

		if (battleships >= 2) return true;

		int heavyCruisers = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.HeavyCruiser);

		int aviationCruisers = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.AviationCruiser);

		if (battleships > 0 && heavyCruisers + aviationCruisers > 0) return true;

		return heavyCruisers > 3;
	}
}
