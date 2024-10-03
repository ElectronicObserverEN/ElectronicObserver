using System;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class NightAttackAccuracy
{
	public static double GetNightAttackAccuracy(this IShipData ship, Attack attack, IFleetData fleet)
	{
		int baseAccuracy = fleet.BaseAccuracy();
		double shipAccuracy = ship.Accuracy();
		double equipAccuracy = ship.AllSlotInstance
			.Where(e => e != null)
			.Sum(e => e!.MasterEquipment.Accuracy + e.NightAccuracyBonus());

		// if night equip is present assume it activates
		return Math.Floor(fleet.NightScoutMod()
						  * (baseAccuracy + fleet.StarShellBonus())
						  + shipAccuracy
						  + equipAccuracy)
				* ship.ConditionMod()
				* attack.AccuracyModifier
				+ fleet.SearchlightBonus()
				+ ship.HeavyCruiserBonus();
	}

	private static int BaseAccuracy(this IFleetData fleet) => 69;

	private static double NightAccuracyBonus(this IEquipmentData? equip) => equip?.MasterEquipment.CategoryType switch
	{
		EquipmentTypes.RadarSmall or
		EquipmentTypes.RadarLarge or
		EquipmentTypes.RadarLarge2 when equip.MasterEquipment.Accuracy >= 3
			=> 1.6 * Math.Sqrt(equip.Level),

		EquipmentTypes.MainGunSmall or
		EquipmentTypes.MainGunMedium or
		EquipmentTypes.MainGunLarge or
		EquipmentTypes.MainGunLarge2 or
		EquipmentTypes.SecondaryGun or
		EquipmentTypes.Torpedo or
		EquipmentTypes.MidgetSubmarine or
		EquipmentTypes.RadarSmall or
		EquipmentTypes.RadarLarge or
		EquipmentTypes.RadarLarge2 or
		EquipmentTypes.AAShell or
		EquipmentTypes.APShell or
		EquipmentTypes.LandingCraft or
		EquipmentTypes.SpecialAmphibiousTank or
		EquipmentTypes.ArmyInfantry or
		EquipmentTypes.CommandFacility or
		EquipmentTypes.AADirector or
		EquipmentTypes.Searchlight or
		EquipmentTypes.SearchlightLarge or
		EquipmentTypes.SurfaceShipPersonnel or
		EquipmentTypes.SurfaceShipEquipment or
		EquipmentTypes.ASPatrol or
		EquipmentTypes.Autogyro or
		EquipmentTypes.AviationPersonnel or
		EquipmentTypes.Rocket
			=> 1.3 * Math.Sqrt(equip.Level),

		_ => 0,
	};

	/// <summary>
	/// <see href="https://twitter.com/yukicacoon/status/1542443860109819904"/>
	/// </summary>
	private static double NightScoutMod(this IFleetData fleet) => fleet.MembersWithoutEscaped!
		.Where(s => s is not null)
		.SelectMany(s => s!.AllSlotInstance)
		.Where(e => e is not null)
		.Where(e => e!.MasterEquipment.IsNightSeaplane())
		.DefaultIfEmpty()
		.MaxBy(e => e?.MasterEquipment.Accuracy)
		?.MasterEquipment.Accuracy switch
		{
			null => 1,
			< 2 => 1.1,
			2 => 1.15,
			> 2 => 1.2,
		};

	private static int StarShellBonus(this IFleetData fleet) => fleet switch
	{
		_ when fleet.HasStarShell() => 5,
		_ => 0,
	};

	private static int SearchlightBonus(this IFleetData fleet) => fleet switch
	{
		_ when fleet.HasSearchlight() => 7,
		_ => 0,
	};

	private static double ConditionMod(this IShipData ship) => ship.Condition switch
	{
		> 52 => 1.2,
		> 32 => 1,
		> 22 => 0.8,
		_ => 0.5,
	};

	private static int HeavyCruiserBonus(this IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.HeavyCruiser => ship.Night20CmBonus(),
		ShipTypes.AviationCruiser => ship.Night20CmBonus(),
		_ => 0,
	};

	private static int Night20CmBonus(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.EquipmentId is
			EquipmentId.MainGunMedium_20_3cmTwinGun or
			EquipmentId.MainGunMedium_20_3cm_No_2TwinGun or
			EquipmentId.MainGunMedium_20_3cm_No_3TwinGun
		) switch
		{
			0 => 0,
			1 => 10,
			_ => 15,
		};
}
