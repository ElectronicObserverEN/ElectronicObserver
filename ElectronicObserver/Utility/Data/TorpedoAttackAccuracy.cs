using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class TorpedoAttackAccuracy
{
	public static double GetTorpedoAttackAccuracy(this IShipData ship, IFleetData fleet)
	{
		int baseAccuracy = fleet.BaseAccuracy();
		double shipAccuracy = ship.Accuracy();
		double equipAccuracy = ship.AllSlotInstance
			.Where(e => e != null)
			.Sum(e => e.MasterEquipment.Accuracy + e.TorpedoAccuracyBonus());

		// ReSharper disable once PossibleLossOfFraction
		// it's floored in the formula so the loss is intended
		return (baseAccuracy + shipAccuracy + equipAccuracy + ship.TorpedoPower / 5)
			   * ship.ConditionMod();
	}

	private static int BaseAccuracy(this IFleetData fleet) => 85;

	private static double TorpedoAccuracyBonus(this IEquipmentData equip) => equip switch
	{
		{ } when equip.MasterEquipment.IsTorpedo => 2 * Math.Sqrt(equip.Level),

		_ => 0
	};

	private static double ConditionMod(this IShipData ship) => ship.Condition switch
	{
		int condition when condition > 52 => 1.3,
		int condition when condition > 32 => 1,
		int condition when condition > 22 => 0.7,
		_ => 0.35
	};
}
