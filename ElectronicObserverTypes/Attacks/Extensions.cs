using System.Collections.Generic;

namespace ElectronicObserverTypes.Attacks;

public static class Extensions
{
	public static bool IsSpecialAttack(this DayAttackKind dayAttack) => dayAttack is
		DayAttackKind.SpecialNelson or
		DayAttackKind.SpecialNagato or
		DayAttackKind.SpecialMutsu or
		DayAttackKind.SpecialColorado or
		DayAttackKind.SpecialKongo or
		DayAttackKind.SpecialSubmarineTender23 or
		DayAttackKind.SpecialSubmarineTender34 or
		DayAttackKind.SpecialSubmarineTender24 or
		DayAttackKind.SpecialYamato2Ships or
		DayAttackKind.SpecialYamato3Ships;

	public static bool IsSpecialAttack(this NightAttackKind nightAttack) => nightAttack is
		NightAttackKind.CutinZuiun or
		NightAttackKind.SpecialNelson or
		NightAttackKind.SpecialNagato or
		NightAttackKind.SpecialMutsu or
		NightAttackKind.SpecialColorado or
		NightAttackKind.SpecialKongou or
		NightAttackKind.SpecialSubmarineTender23 or
		NightAttackKind.SpecialSubmarineTender34 or
		NightAttackKind.SpecialSubmarineTender24 or
		NightAttackKind.SpecialYamato2Ships or
		NightAttackKind.SpecialYamato3Ships;

	public static List<int> SpecialAttackParticipationIndexes(this DayAttackKind? dayAttack)
		=> dayAttack switch
		{
			DayAttackKind.SpecialNelson => [0, 2, 4],

			DayAttackKind.SpecialNagato or
			DayAttackKind.SpecialMutsu or
			DayAttackKind.SpecialYamato2Ships => [0, 1],

			DayAttackKind.SpecialColorado or
			DayAttackKind.SpecialYamato3Ships or
			DayAttackKind.SpecialSubmarineTender23 => [0, 1, 2],

			DayAttackKind.SpecialSubmarineTender34 => [0, 2, 3],
			DayAttackKind.SpecialSubmarineTender24 => [0, 1, 3],

			_ => [],
		};
}
