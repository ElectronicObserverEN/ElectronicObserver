using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Attacks;

public static class Extensions
{
	public static bool IsSpecialAttack(this DayAttackKind dayAttack) => dayAttack is
		DayAttackKind.SpecialNelson or
		DayAttackKind.SpecialNagato or
		DayAttackKind.SpecialMutsu or
		DayAttackKind.SpecialColorado or
		DayAttackKind.SpecialKongo or
		DayAttackKind.SpecialRichelieu or
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
		NightAttackKind.SpecialRichelieu or
		NightAttackKind.SpecialSubmarineTender23 or
		NightAttackKind.SpecialSubmarineTender34 or
		NightAttackKind.SpecialSubmarineTender24 or
		NightAttackKind.SpecialYamato2Ships or
		NightAttackKind.SpecialYamato3Ships;

	public static List<int> SpecialAttackParticipationIndexes(this DayAttackKind? dayAttack)
		=> dayAttack switch
		{
			{ } atk => atk switch
			{
				DayAttackKind.SpecialSubmarineTender23 or
				DayAttackKind.SpecialSubmarineTender34 or
				DayAttackKind.SpecialSubmarineTender24 => SpecialAttackIndexes(atk)
					.Append(0)
					.Distinct()
					.ToList(),

				_ => SpecialAttackIndexes(atk)
					.Distinct()
					.ToList()
			},

			_ => [],
		};

	public static List<int> SpecialAttackParticipationIndexes(this NightAttackKind? nightAttack)
		=> nightAttack switch
		{
			{ } atk => atk switch
			{
				NightAttackKind.SpecialSubmarineTender23 or
				NightAttackKind.SpecialSubmarineTender34 or
				NightAttackKind.SpecialSubmarineTender24 => SpecialAttackIndexes(atk)
					.Append(0)
					.Distinct()
					.ToList(),

				_ => SpecialAttackIndexes(atk)
					.Distinct()
					.ToList()
			},

			_ => [],
		};

	public static List<int> SpecialAttackIndexes(this DayAttackKind dayAttack)
		=> dayAttack switch
		{
			DayAttackKind.SpecialNelson => [0, 2, 4],

			DayAttackKind.SpecialNagato or
			DayAttackKind.SpecialMutsu or
			DayAttackKind.SpecialYamato2Ships or
			DayAttackKind.SpecialRichelieu => [0, 0, 1],

			DayAttackKind.SpecialColorado or
			DayAttackKind.SpecialYamato3Ships => [0, 1, 2],

			DayAttackKind.SpecialSubmarineTender23 => [1, 2, 1, 2],
			DayAttackKind.SpecialSubmarineTender34 => [2, 3, 2, 3],
			DayAttackKind.SpecialSubmarineTender24 => [1, 3, 1, 3],

			_ => [],
		};
	
	public static List<int> SpecialAttackIndexes(this NightAttackKind nightAttack)
		=> nightAttack switch
		{
			NightAttackKind.SpecialNelson => [0, 2, 4],

			NightAttackKind.SpecialNagato or
			NightAttackKind.SpecialMutsu or
			NightAttackKind.SpecialYamato2Ships or
			NightAttackKind.SpecialRichelieu => [0, 0, 1],

			NightAttackKind.SpecialKongou => [0, 1],

			NightAttackKind.SpecialColorado or
			NightAttackKind.SpecialYamato3Ships => [0, 1, 2],

			NightAttackKind.SpecialSubmarineTender23 => [1, 2, 1, 2],
			NightAttackKind.SpecialSubmarineTender34 => [2, 3, 2, 3],
			NightAttackKind.SpecialSubmarineTender24 => [1, 3, 1, 3],

			_ => [],
		};
}
