using System;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class DayAttackAccuracy
{
	public static double GetDayAttackAccuracy(this IShipData ship, Enum attack, IFleetData fleet)
	{
		int baseAccuracy = fleet.BaseAccuracy(ship) - fleet.PositionPenalty(ship);
		double shipAccuracy = ship.Accuracy();
		double equipAccuracy = ship.AllSlotInstance
			.Where(e => e != null)
			.Sum(e => e.MasterEquipment.Accuracy + e.DayAccuracyBonus());

		double f1 = Math.Floor(baseAccuracy + shipAccuracy + equipAccuracy);
		double f2 = Math.Floor(f1 * ship.ConditionMod() /* + gun fit */);

		// todo: we currently assume the AP shell bonus always applies
		// this is done for simplicity and consistency with damage calculation
		return f2 * AttackKindMod(attack) * ship.ApShellMod();
	}

	public static double GetDayAttackAccuracy(this IShipData ship, Attack hit, IFleetData fleet) 
		=> GetDayAttackAccuracy(ship, DayAttackKind.NormalAttack, fleet) * hit.AccuracyModifier;

	private static int BaseAccuracy(this IFleetData fleet, IShipData ship) => (fleet.FleetType, ship.Fleet) switch
	{
		(FleetType.Single, _) => 90,

		(FleetType.Carrier, 1) => 78,
		(FleetType.Carrier, 2) => 43,

		(FleetType.Surface, 1) => 46,
		(FleetType.Surface, 2) => 70,

		(FleetType.Transport, 1) => 51,
		(FleetType.Transport, 2) => 45,

		_ => 0,
	};

	private static int PositionPenalty(this IFleetData fleet, IShipData ship) =>
		(fleet.FleetType, ship.Fleet, fleet.MembersInstance.FirstOrDefault()?.ShipID == ship.ShipID) switch
		{
			(FleetType.Single, _, _) => 0,

			(FleetType.Carrier, 2, true) => 10,
			(FleetType.Surface, 2, true) => 10,
			(FleetType.Transport, 2, true) => 10,

			_ => 0,
		};

	private static double DayAccuracyBonus(this IEquipmentData? equip) => equip?.MasterEquipment.CategoryType switch
	{
		EquipmentTypes.RadarSmall or
		EquipmentTypes.RadarLarge or
		EquipmentTypes.RadarLarge2 when equip.MasterEquipment.IsSurfaceRadar => 1.7 * Math.Sqrt(equip.Level),

		EquipmentTypes.RadarSmall or
		EquipmentTypes.RadarLarge or
		EquipmentTypes.RadarLarge2 => Math.Sqrt(equip.Level),

		EquipmentTypes.MainGunSmall or
		EquipmentTypes.MainGunMedium or
		EquipmentTypes.MainGunLarge or
		EquipmentTypes.MainGunLarge2 or
		EquipmentTypes.SecondaryGun or
		EquipmentTypes.AAShell or
		EquipmentTypes.APShell or
		EquipmentTypes.AADirector or
		EquipmentTypes.Searchlight or
		EquipmentTypes.SearchlightLarge or
		EquipmentTypes.SurfaceShipPersonnel => Math.Sqrt(equip.Level),

		_ => 0,
	};

	private static double ConditionMod(this IShipData ship) => ship.Condition switch
	{
		> 52 => 1.2,
		> 32 => 1,
		> 22 => 0.8,
		_ => 0.5,
	};

	private static double AttackKindMod(Enum attack) => attack switch
	{
		DayAttackKind.CutinMainRadar => 1.5,

		DayAttackKind.CutinMainSub => 1.3,
		DayAttackKind.CutinMainAP => 1.3,

		DayAttackKind.CutinMainMain => 1.2,

		DayAttackKind.DoubleShelling => 1.1,

		_ => 1,
	};

	private static double ApShellMod(this IShipData ship)
	{
		bool ap = ship.AllSlotInstance.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.APShell);
		bool main = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.IsMainGun ?? false);
		bool sub = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);
		bool radar = ship.AllSlotInstance.Any(eq => eq?.MasterEquipment.IsRadar ?? false);

		return (ap, main, sub, radar) switch
		{
			(true, true, false, false) => 1.1,
			(true, true, false, true) => 1.25,
			(true, true, true, false) => 1.2,
			(true, true, true, true) => 1.3,

			_ => 1,
		};
	}
}
