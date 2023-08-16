using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseTorpedoAttackViewModel : AttackViewModelBase
{
	public BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public int AttackerHpBeforeAttack { get; }

	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	public List<int> DefenderHpBeforeAttacks { get; } = new();

	private DayAttackKind AttackType { get; }
	public List<DayAttack> Attacks { get; }
	private IEquipmentData? UsedDamecon { get; }
	public string DamageDisplay { get; }

	public PhaseTorpedoAttackViewModel(BattleFleets fleets, PhaseTorpedoAttack attack)
	{
		AttackerIndex = attack.Attacker;
		Attacker = fleets.GetShip(AttackerIndex)!;
		DefenderIndex = attack.Defenders.First().Defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		AttackType = ProcessAttack(Attacker, Defender, attack.AttackType);
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

		AttackerHpBeforeAttack = Attacker.HPCurrent;
		DefenderHpBeforeAttacks.Add(Defender.HPCurrent);

		foreach (DayAttack dayAttack in Attacks)
		{
			DefenderHpBeforeAttacks.Add(DefenderHpBeforeAttacks[^1] - dayAttack.Damage);
		}

		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage));

		if (hpAfterAttacks <= 0 && GetDamecon(Defender) is { } damecon)
		{
			UsedDamecon = damecon;
		}

		DamageDisplay =
			$"[{ElectronicObserverTypes.Attacks.DayAttack.AttackDisplay(AttackType)}] " +
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

	private static string AttackDisplay(DayAttack dayAttack)
		=> AttackDisplay(dayAttack.GuardsFlagship, dayAttack.Damage, dayAttack.CriticalFlag);

	private static DayAttackKind ProcessAttack(IShipData attacker, IShipData defender, DayAttackKind attack)
	{
		if (attack is not DayAttackKind.NormalAttack) return attack;

		int[] slots = attacker.AllSlotInstance
			.Where(e => e is not null)
			.Select(e => e!.EquipmentID)
			.ToArray();

		return Calculator.GetDayAttackKind(slots, attacker.ShipID, defender.ShipID, false);
	}
}
