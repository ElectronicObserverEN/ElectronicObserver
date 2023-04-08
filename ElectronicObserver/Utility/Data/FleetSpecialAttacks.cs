using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks.Specials;

namespace ElectronicObserver.Utility.Data;

public static class FleetSpecialAttacks
{
	public static IEnumerable<SpecialAttack> GetSpecialAttacks(this IFleetData fleet)
	{
		IEnumerable<SpecialAttack> attacks = new List<SpecialAttack>()
		{
			new NelsonSpecialAttack(fleet),
			new NagatoSpecialAttack(fleet),
		};

		return attacks.Where(attack => attack.TriggerRate != -1);
	}
}
