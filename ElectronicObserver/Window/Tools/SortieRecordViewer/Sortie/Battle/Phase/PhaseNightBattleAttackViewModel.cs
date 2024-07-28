using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseNightBattleAttackViewModel : AttackViewModelBase
{
	public override BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public int AttackerHpBeforeAttack { get; }
	public override string AttackerDisplay { get; }

	public override BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	public List<int> DefenderHpBeforeAttacks { get; } = [];
	public override string DefenderDisplay { get; }

	public NightAttackKind AttackType { get; }
	public List<IEquipmentDataMaster> DisplayEquipment { get; }
	public List<NightAttack> Attacks { get; }
	private IEquipmentData? UsedDamecon { get; }

	public override string DamageDisplay { get; }

	public PhaseNightBattleAttackViewModel(BattleFleets fleets, PhaseNightBattleAttack attack,
		BattleIndex defenderIndex)
	{
		AttackerIndex = attack.Attacker;
		Attacker = fleets.GetShip(AttackerIndex)!;
		AttackerDisplay = $"{Attacker.Name} {AttackerIndex.Display}";

		DefenderIndex = defenderIndex;
		Defender = fleets.GetShip(DefenderIndex)!;
		DefenderDisplay = $"{Defender.Name} {DefenderIndex.Display}";

		AttackType = attack.AttackType;
		Attacks = attack.Defenders
			.Where(d => d.Defender == DefenderIndex)
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
