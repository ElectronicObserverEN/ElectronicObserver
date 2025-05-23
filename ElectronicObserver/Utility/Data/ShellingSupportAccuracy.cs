using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class ShellingSupportAccuracy
{
	public static double GetShellingSupportAccuracy(this IShipData ship)
	{
		int baseAccuracy = 64;
		double shipAccuracy = ship.Accuracy();
		double equipAccuracy = ship.AllSlotInstance
			.Where(e => e != null)
			.Sum(e => e.MasterEquipment.Accuracy);
		
		return Math.Floor(baseAccuracy + shipAccuracy + equipAccuracy) * ship.ConditionMod();
	}

	private static double ConditionMod(this IShipData ship) => ship.Condition switch
	{
		> 52 => 1.2,
		> 32 => 1,
		> 22 => 0.8,
		_ => 0.5
	};
}
