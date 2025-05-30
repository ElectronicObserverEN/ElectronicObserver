﻿using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseSupportAttackViewModel : AttackViewModelBase
{
	public string Attacker => BattleRes.SupportFleet;

	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	public List<int> DefenderHpBeforeAttacks { get; } = new();

	private SupportType AttackType { get; }
	private List<SupportAttack> Attacks { get; }
	private IEquipmentData? UsedDamecon { get; }
	public string DamageDisplay { get; }

	public PhaseSupportAttackViewModel(BattleFleets fleets, PhaseSupportAttack attack)
	{
		DefenderIndex = attack.Defenders.First().Defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		AttackType = attack.AttackType;
		Attacks = attack.Defenders
			.Select(d => new SupportAttack
			{
				Defender = fleets.GetShip(d.Defender)!,
				AttackKind = AttackType,
				Damage = d.Damage,
				GuardsFlagship = d.GuardsFlagship,
				CriticalFlag = d.CriticalFlag,
			})
			.ToList();

		DefenderHpBeforeAttacks.Add(Defender.HPCurrent);

		foreach (SupportAttack supportAttack in Attacks)
		{
			DefenderHpBeforeAttacks.Add(Math.Max(0, DefenderHpBeforeAttacks[^1] - supportAttack.Damage));
		}

		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage));

		if (hpAfterAttacks <= 0 && GetDamecon(Defender) is { } damecon)
		{
			UsedDamecon = damecon;
		}

		DamageDisplay =
			$"[{GetAttackKind(AttackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))} ";

		if (Defender.HPCurrent > 0 && Defender.HPCurrent != hpAfterAttacks)
		{
			DamageDisplay += $"({Defender.HPCurrent} → {hpAfterAttacks})";
		}

		DamageDisplay += UsedDamecon switch
		{
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairGoddess } => $"　{BattleRes.GoddessActivated} HP{Defender.HPMax}",
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairPersonnel } => $"　{BattleRes.DameconActivated} HP{(int)(Defender.HPMax * 0.2)}",
			_ => null,
		};
	}

	private static string AttackDisplay(SupportAttack dayAttack)
		=> AttackDisplay(dayAttack.GuardsFlagship, dayAttack.Damage, dayAttack.CriticalFlag);

	private static string GetAttackKind(SupportType supportType) => supportType switch
	{
		SupportType.Aerial => ConstantsRes.AirAttack,
		SupportType.Shelling => ConstantsRes.Shelling,
		SupportType.Torpedo => ConstantsRes.TorpedoAttack,
		SupportType.AntiSubmarine => ConstantsRes.BombingAttack,
		_ => ConstantsRes.Unknown,
	};
}
