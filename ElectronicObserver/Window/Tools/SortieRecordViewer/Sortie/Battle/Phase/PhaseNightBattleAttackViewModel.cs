using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseNightBattleAttackViewModel : AttackViewModelBase
{
	public BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public int AttackerHpBeforeAttack { get; }

	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	public List<int> DefenderHpBeforeAttacks { get; } = new();

	public List<IEquipmentDataMaster> DisplayEquipment { get; }
	public List<NightAttack> Attacks { get; }
	private IEquipmentData? UsedDamecon { get; }
	public string DamageDisplay { get; }

	public PhaseNightBattleAttackViewModel(BattleFleets fleets, PhaseNightBattleAttack attack)
	{
		AttackerIndex = attack.Attacker;
		Attacker = fleets.GetShip(AttackerIndex)!;
		DefenderIndex = attack.Defenders.First().Defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		Attacks = attack.Defenders
			.Select(d => new NightAttack
			{
				Attacker = Attacker,
				Defender = fleets.GetShip(d.Defender)!,
				AttackKind = attack.AttackType,
				Damage = d.Damage,
				GuardsFlagship = d.GuardsFlagship,
				CriticalFlag = d.CriticalFlag,
			})
			.ToList();
		DisplayEquipment = attack.DisplayEquipments;

		AttackerHpBeforeAttack = Attacker.HPCurrent;
		DefenderHpBeforeAttacks.Add(Defender.HPCurrent);

		foreach (NightAttack nightAttack in Attacks)
		{
			DefenderHpBeforeAttacks.Add(Math.Max(0, DefenderHpBeforeAttacks[^1] - nightAttack.Damage));
		}

		int hpAfterAttacks = Math.Max(0, Defender.HPCurrent - Attacks.Sum(a => a.Damage));

		if (hpAfterAttacks <= 0 && GetDamecon(Defender) is { } damecon)
		{
			UsedDamecon = damecon;
		}

		DamageDisplay =
			$"[{ElectronicObserverTypes.Attacks.NightAttack.AttackDisplay(attack.AttackType)}] " +
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
