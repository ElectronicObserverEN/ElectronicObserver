using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks.Specials;

namespace ElectronicObserver.Utility.Data;

public static class FleetSpecialAttacks
{
	public static List<SpecialAttack> GetSpecialAttacks(this IFleetData fleet)
	{
		List<SpecialAttack> attacks =
		[
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

		return attacks
			.Where(attack => attack.CanTrigger())
			.ToList();
	}
}
