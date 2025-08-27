using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class AswAttackAccuracy
{
	public static double GetAswAttackAccuracy(this IShipData ship, IFleetData fleet)
	{
		int baseAccuracy = fleet.BaseAccuracy();
		double shipAccuracy = ship.Accuracy();
		double equipAccuracy = ship.AllSlotInstance
			.Where(e => e != null)
			.Sum(e => e!.AswAccuracyBonus() + e!.AswAccuracy());

		return (baseAccuracy + shipAccuracy + equipAccuracy)
			   * ship.ConditionMod();
	}

	private static int BaseAccuracy(this IFleetData fleet) => 80;

	private static double AswAccuracyBonus(this IEquipmentData equip) => equip switch
	{
		{ MasterEquipment.IsSonar: true } => 1.3 * Math.Sqrt(equip.Level),

		_ => 0,
	};

	private static double AswAccuracy(this IEquipmentData equip) => equip switch
	{
		{ MasterEquipment.IsSonar: true } => equip.MasterEquipment.ASW * 2,

		_ => 0,
	};

	private static double ConditionMod(this IShipData ship) => ship.Condition switch
	{
		> 52 => 1.2,
		> 32 => 1,
		> 22 => 0.8,
		_ => 0.5,
	};
}
