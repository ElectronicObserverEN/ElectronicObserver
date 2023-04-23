using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseNightBattleAttackViewModel
{
	public BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	private NightAttackKind AttackType { get; }
	private List<NightAttack> Attacks { get; }
	public string DamageDisplay { get; }

	public PhaseNightBattleAttackViewModel(BattleFleets fleets, BattleIndex attacker,
		BattleIndex defender, NightAttackKind attackType, List<PhaseNightBattleDefender> defenders)
	{
		AttackerIndex = attacker;
		Attacker = fleets.GetShip(AttackerIndex)!;
		DefenderIndex = defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		AttackType = attackType;
		Attacks = defenders
			.Select(d => new NightAttack
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
			$"[{ElectronicObserverTypes.Attacks.NightAttack.AttackDisplay(AttackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))} " +
			$"({Defender.HPCurrent} → {Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage))})";

	}

	private static string AttackDisplay(NightAttack nightAttack) => nightAttack.CriticalFlag switch
	{
		HitType.Hit => $"{HitDisplay(nightAttack)} Dmg",
		HitType.Critical => $"{HitDisplay(nightAttack)} Critical!",
		HitType.Miss => "Miss",
		_ => "",
	};

	private static string HitDisplay(NightAttack nightAttack) => $"{ProtectedDisplay(nightAttack)}{nightAttack.Damage}";

	private static string ProtectedDisplay(NightAttack nightAttack) => nightAttack.GuardsFlagship switch
	{
		true => $"<{BattleRes.Protected}> ",
		_ => "",
	};
}
