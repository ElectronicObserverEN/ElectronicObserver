using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks.Specials;

namespace ElectronicObserver.Utility.Data;

public static class FleetSpecialAttacks
{
	public static List<SpecialAttack> GetSpecialAttacks(this IFleetData fleet)
	{
		List<SpecialAttack> attacks = [];

		if (fleet.FleetType is FleetType.Single || fleet.FleetID == 1)
		{
			attacks = [
				new NelsonSpecialAttack(fleet),
				new NagatoSpecialAttack(fleet),
				new ColoradoSpecialAttack(fleet),
				new Yamato123SpecialAttack(fleet),
				new Yamato12SpecialAttack(fleet),
				new KongouSpecialAttack(fleet),
				new SubmarineSpecialAttack(fleet),
				new RichelieuSpecialAttack(fleet),
				new QueenElizabethSpecialAttack(fleet),
			];
		}
		else
		{
			attacks = [
				new KongouSpecialAttack(fleet),
				new SubmarineSpecialAttack(fleet),
			];
		}
		
		return attacks
			.Where(attack => attack.CanTrigger())
			.ToList();
	}
}
