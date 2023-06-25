using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseNightBattleAttackViewModel : AttackViewModelBase
{
	public BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	private List<NightAttack> Attacks { get; }
	public string DamageDisplay { get; }

	public PhaseNightBattleAttackViewModel(BattleFleets fleets, BattleIndex attacker,
		BattleIndex defender, NightAttackKind attackType, List<PhaseNightBattleDefender> defenders)
	{
		AttackerIndex = attacker;
		Attacker = fleets.GetShip(AttackerIndex)!;
		DefenderIndex = defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		Attacks = defenders
			.Select(d => new NightAttack
			{
				Attacker = Attacker,
				Defender = fleets.GetShip(d.Defender)!,
				AttackKind = attackType,
				Damage = d.Damage,
				GuardsFlagship = d.GuardsFlagship,
				CriticalFlag = d.CriticalFlag,
			})
			.ToList();

		DamageDisplay =
			$"[{ElectronicObserverTypes.Attacks.NightAttack.AttackDisplay(attackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))}";

		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage));

		if (Defender.HPCurrent > 0 && Defender.HPCurrent != hpAfterAttacks)
		{
			DamageDisplay += $" ({Defender.HPCurrent} → {hpAfterAttacks})";
		}
	}

	private static string AttackDisplay(NightAttack nightAttack)
		=> AttackDisplay(nightAttack.GuardsFlagship, nightAttack.Damage, nightAttack.CriticalFlag);
}
