using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Properties.Data;
using ElectronicObserver.Utility.Data;
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
	public List<DayAttack> Attacks { get; }
	public string DamageDisplay { get; }

	public PhaseShellingAttackViewModel(BattleFleets fleets, PhaseShellingAttack attack)
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

		DamageDisplay =
			$"[{ElectronicObserverTypes.Attacks.DayAttack.AttackDisplay(AttackType)}] " +
			$"{string.Join(", ", Attacks.Select(AttackDisplay))}";

		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage));

		if (Defender.HPCurrent > 0 && Defender.HPCurrent != hpAfterAttacks)
		{
			DamageDisplay += $" ({Defender.HPCurrent} → {hpAfterAttacks})";
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
		true => $"<{BattleRes.Protected}> ",
		_ => "",
	};

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
