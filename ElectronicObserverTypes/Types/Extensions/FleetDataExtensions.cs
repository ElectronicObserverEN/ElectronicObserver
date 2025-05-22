using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Core.Types.Extensions;
public static class FleetDataExtensions
{
	public static int NumberOfSurfaceShipNotRetreatedNotSunk(this IFleetData fleet) =>
		fleet.MembersWithoutEscaped?.Count(ship => ship is { HPCurrent: > 0, MasterShip.IsSubmarine: false }) ?? 0;

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
			ShipTypes.HeavyCruiser or
			ShipTypes.AviationCruiser);

		if (battleships > 0 && heavyCruisers > 2) return true;

		return heavyCruisers > 3;
	}

	/// <summary>
	/// https://x.com/yukicacoon/status/1739480992090632669
	/// </summary>
	/// <param name="fleet"></param>
	/// <returns></returns>
	public static List<SmokeGeneratorTriggerRate> GetSmokeTriggerRates(this IFleetData fleet) => GetSmokeTriggerRates([fleet]);

	/// <summary>
	/// https://x.com/Xe_UCH/status/1767407602554855730
	/// </summary>
	/// <param name="fleets"></param>
	/// <returns></returns>
	public static List<SmokeGeneratorTriggerRate> GetSmokeTriggerRates(this List<IFleetData> fleets)
	{
		if (fleets.Count is 0) return [];

		IShipData? flagship = fleets[0].MembersWithoutEscaped?.First();

		if (flagship is null) return [];

		List<IShipData> ships = fleets
			.SelectMany(fleet => fleet.MembersWithoutEscaped ?? new([]))
			.OfType<IShipData>()
			.ToList();

		List<IEquipmentData> smokeGenerators = ships
			.SelectMany(ship => ship.AllSlotInstance)
			.Where(e => e?.MasterEquipment.EquipmentId is EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen)
			.Cast<IEquipmentData>()
			.ToList();

		List<IEquipmentData> smokeGeneratorsKai = ships
			.SelectMany(ship => ship.AllSlotInstance)
			.Where(e => e?.MasterEquipment.EquipmentId is EquipmentId.SurfaceShipEquipment_SmokeGeneratorKai_SmokeScreen)
			.Cast<IEquipmentData>()
			.ToList();

		// https://twitter.com/yukicacoon/status/1739480992090632669
		int smokeGeneratorCount = smokeGenerators.Count + smokeGeneratorsKai.Count * 2;
		double upgradeModifier = 0.3 * smokeGenerators.Sum(eq => eq.Level) + 0.5 * smokeGeneratorsKai.Sum(eq => eq.Level);
		double modifier = Math.Ceiling(Math.Sqrt(flagship.LuckTotal) + upgradeModifier);
		double triggerRate = 1 - Math.Max(3.2 - 0.2 * modifier - smokeGeneratorCount, 0);

		if (smokeGeneratorCount >= 3)
		{
			double tripleTrigger = Math.Min(3 * Math.Ceiling(5 * smokeGeneratorCount + 1.5 * Math.Sqrt(flagship.LuckTotal) + upgradeModifier - 15) + 1, 100);
			double doubleTrigger = 30 - (tripleTrigger > 70 ? tripleTrigger - 70 : 0);
			double singleTrigger = Math.Max(100 - tripleTrigger - doubleTrigger, 0);

			return
			[
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 3,
					ActivationRatePercentage = tripleTrigger * triggerRate,
				},
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 2,
					ActivationRatePercentage = doubleTrigger * triggerRate,
				},
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 1,
					ActivationRatePercentage = singleTrigger * triggerRate,
				},
			];
		}

		if (smokeGeneratorCount == 2)
		{
			double doubleTrigger = Math.Min(3 * Math.Ceiling(5 * smokeGeneratorCount + 1.5 * Math.Sqrt(flagship.LuckTotal) + upgradeModifier - 5) + 1, 100);
			double singleTrigger = Math.Max(100 - doubleTrigger, 0);

			return
			[
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 2,
					ActivationRatePercentage = doubleTrigger * triggerRate,
				},
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 1,
					ActivationRatePercentage = singleTrigger * triggerRate,
				},
			];
		}

		return triggerRate switch
		{
			> 0 =>
			[
				new()
				{
					SmokeGeneratorCount = 1,
					ActivationRatePercentage = triggerRate * 100,
				},
			],
			_ => [],
		};
	}
}
