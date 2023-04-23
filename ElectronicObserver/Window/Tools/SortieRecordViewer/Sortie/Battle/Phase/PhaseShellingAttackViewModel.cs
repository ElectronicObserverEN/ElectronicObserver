using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseShellingAttackViewModel
{
	public BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	private DayAttackKind AttackType { get; }
	private List<DayAttack> Attacks { get; }
	public string DamageDisplay { get; }

	public PhaseShellingAttackViewModel(BattleFleets fleets, PhaseShellingAttack attack)
	{
		AttackerIndex = attack.Attacker;
		Attacker = fleets.GetShip(AttackerIndex)!;
		DefenderIndex = attack.Defenders.First().Defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		AttackType = attack.AttackType;
		Attacks = attack.Defenders
			.Select(d => new DayAttack
			{
				Attacker = Attacker,
				Defender = fleets.GetShip(d.Defender)!,
				AttackKind = AttackType,
				Damage = d.Damage,
				GuardsFlagship = d.GuardsFlagship,
				CriticalFlag = d.CriticalFlag,
			})
			.ToList();

		DamageDisplay =
			$"[{ElectronicObserverTypes.Attacks.DayAttack.AttackDisplay(AttackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))} ";

		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage));

		if (Defender.HPCurrent != hpAfterAttacks)
		{
			DamageDisplay += $"({Defender.HPCurrent} → {hpAfterAttacks})";
		}
	}

	private static string AttackDisplay(DayAttack dayAttack) => dayAttack.CriticalFlag switch
	{
		HitType.Hit => $"{HitDisplay(dayAttack)} Dmg",
		HitType.Critical => $"{HitDisplay(dayAttack)} Critical!",
		HitType.Miss => "Miss",
		_ => "",
	};

	private static string HitDisplay(DayAttack dayAttack) => $"{ProtectedDisplay(dayAttack)}{dayAttack.Damage}";

	private static string ProtectedDisplay(DayAttack dayAttack) => dayAttack.GuardsFlagship switch
	{
		true => "protected ",
		_ => "",
	};
}
