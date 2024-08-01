﻿using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseFriendNightBattleAttackViewModel : AttackViewModelBase
{
	public override BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public int AttackerHpBeforeAttack { get; }
	public override string AttackerDisplay { get; }

	public override BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	private List<int> DefenderHpBeforeAttacks { get; } = [];
	public override string DefenderDisplay { get; }

	public override double Damage { get; }
	private List<NightAttack> Attacks { get; }
	private IEquipmentData? UsedDamecon { get; }
	public override string DamageDisplay { get; }

	public PhaseFriendNightBattleAttackViewModel(BattleFleets fleets, BattleIndex attacker,
		BattleIndex defender, NightAttackKind attackType, List<PhaseNightBattleDefender> defenders)
	{
		AttackerIndex = attacker;
		Attacker = fleets.GetFriendShip(AttackerIndex)!;
		AttackerDisplay = $"{Attacker.Name} {AttackerIndex.Display}";

		DefenderIndex = defender;
		Defender = fleets.GetFriendShip(DefenderIndex)!;
		DefenderDisplay = $"{Defender.Name} {DefenderIndex.Display}";

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
		Damage = Attacks.Sum(a => a.Damage);

		AttackerHpBeforeAttack = Attacker.HPCurrent;
		DefenderHpBeforeAttacks.Add(Defender.HPCurrent);

		foreach (NightAttack nightAttack in Attacks)
		{
			DefenderHpBeforeAttacks.Add(Math.Max(0, DefenderHpBeforeAttacks[^1] - nightAttack.Damage));
		}


		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - (int)Damage);

		if (hpAfterAttacks <= 0 && GetDamecon(Defender) is { } damecon)
		{
			UsedDamecon = damecon;
		}

		DamageDisplay =
			$"[{ElectronicObserverTypes.Attacks.NightAttack.AttackDisplay(attackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))}";

		if (Defender.HPCurrent > 0 && Defender.HPCurrent != hpAfterAttacks)
		{
			DamageDisplay += $" ({Defender.HPCurrent} → {hpAfterAttacks})";
		}

		DamageDisplay += UsedDamecon switch
		{
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairGoddess } => $"　{BattleRes.GoddessActivated} HP{Defender.HPMax}",
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairPersonnel } => $"　{BattleRes.DameconActivated} HP{(int)(Defender.HPMax * 0.2)}",
			_ => null,
		};
	}

	private static string AttackDisplay(NightAttack nightAttack)
		=> AttackDisplay(nightAttack.GuardsFlagship, nightAttack.Damage, nightAttack.CriticalFlag);
}
