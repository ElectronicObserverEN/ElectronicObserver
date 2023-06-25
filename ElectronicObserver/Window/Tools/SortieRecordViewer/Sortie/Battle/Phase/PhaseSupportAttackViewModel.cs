using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseSupportAttackViewModel : AttackViewModelBase
{
	public string Attacker => BattleRes.SupportFleet;
	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	private SupportType AttackType { get; }
	private List<SupportAttack> Attacks { get; }
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

		DamageDisplay =
			$"[{GetAttackKind(AttackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))} ";

		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage));

		if (Defender.HPCurrent > 0 && Defender.HPCurrent != hpAfterAttacks)
		{
			DamageDisplay += $"({Defender.HPCurrent} → {hpAfterAttacks})";
		}
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
