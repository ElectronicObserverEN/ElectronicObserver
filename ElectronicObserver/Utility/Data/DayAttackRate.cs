using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Utility.Data;

public static class DayAttackRate
{
	/// <summary>
	/// Takes a list of day attack rates, assumes the last attack is a normal attack
	/// </summary>
	[Pure]
	public static List<double> TotalRates(this List<double> rates)
	{
		if (rates.Count == 0) return rates;

		List<double> totalRates = rates
			.Select(r => Math.Min(r, 1))
			.ToList();

		totalRates[^1] = 1;

		for (int i = 1; i < totalRates.Count - 1; i++)
		{
			totalRates[i] = (1 - totalRates.GetRange(0, i).Sum()) * totalRates[i];
		}

		totalRates = totalRates.Select(d => Math.Round(d * 1000) / 1000).ToList();

		totalRates[^1] = 1 - totalRates.ToArray()[..^1].Sum();

		return totalRates;
	}

	public static double GetDayAttackRate(this IShipData ship, Enum attack, IFleetData fleet,
		AirState state = AirState.Supremacy)
	{
		int attackMod = AttackMod(attack);
		if (attackMod == 0) return 0;

		double luckPart = Math.Floor(Math.Sqrt(ship.LuckTotal));
		double shipLos = ship.AllSlotInstance.Sum(e => e?.MasterEquipment.LOS ?? 0);

		double baseRate = 10 + luckPart + AirStateBonus(state) + FlagshipBonus(fleet, ship) +
						  Math.Floor(fleet.SpottingLoS() + AirStateShipModifier(state) * shipLos) *
						  AirStateFleetModifier(state);

		return Math.Floor(baseRate) / attackMod;
	}

	private static double AirStateBonus(AirState state) => state switch
	{
		AirState.Supremacy => 10,
		_ => 0
	};

	private static double AirStateShipModifier(AirState state) => state switch
	{
		AirState.Supremacy => 1.6,
		_ => 1.2
	};

	private static double AirStateFleetModifier(AirState state) => state switch
	{
		AirState.Supremacy => 0.7,
		_ => 0.6
	};

	private static double FlagshipBonus(IFleetData fleet, IShipData ship) => fleet.MembersWithoutEscaped
			.FirstOrDefault()?.MasterShip.ShipId switch
	{
		{ } id when id == ship.MasterShip.ShipId => 15,
		_ => 0
	};

	private static double SpottingLoS(this IFleetData fleet)
	{
		double rawLos = fleet.MembersWithoutEscaped
			.Where(s => s != null)
			.Sum(s => s.LOSBase + s.AllSlotInstance
				.Zip(s.Aircraft, (e, size) => (e, size))
				.Where(slot => slot.e?.MasterEquipment.CategoryType switch
				{
					EquipmentTypes.SeaplaneRecon => true,
					EquipmentTypes.SeaplaneBomber => true,
					_ => false
				})
				.Sum(slot => slot.e!.MasterEquipment.LOS * Math.Floor(Math.Sqrt(slot.size))));

		return Math.Floor(Math.Sqrt(rawLos) + rawLos / 10.0);
	}

	private static int AttackMod(Enum attack) => attack switch
	{
		DayAttackKind.SeaAirMultiAngle => 130,
		DayAttackKind.ZuiunMultiAngle => 120,
		DayAttackKind.CutinMainMain => 150,
		DayAttackKind.CutinMainAP => 130,
		DayAttackKind.CutinMainRadar => 130,
		DayAttackKind.CutinMainSub => 120,
		DayAttackKind.DoubleShelling => 130,

		DayAirAttackCutinKind.FighterBomberAttacker => 125,
		DayAirAttackCutinKind.BomberBomberAttacker => 135,
		DayAirAttackCutinKind.BomberAttacker => 150,

		_ => 0
	};
}
