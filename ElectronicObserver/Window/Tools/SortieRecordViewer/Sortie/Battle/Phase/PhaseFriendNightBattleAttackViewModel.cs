using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseFriendNightBattleAttackViewModel : AttackViewModelBase
{
	public BattleIndex AttackerIndex { get; }
	public IShipData Attacker { get; }
	public int AttackerHpBeforeAttack { get; }

	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	public List<int> DefenderHpBeforeAttacks { get; } = [];

	public List<IEquipmentDataMaster> DisplayEquipment { get; }
	private List<NightAttack> Attacks { get; }
	private IEquipmentData? UsedDamecon { get; }
	public string DamageDisplay { get; }

	public PhaseFriendNightBattleAttackViewModel(BattleFleets fleets, PhaseNightBattleAttack attack,
		BattleIndex defenderIndex)
	{
		AttackerIndex = attack.Attacker;
		Attacker = fleets.GetFriendShip(AttackerIndex)!;
		DefenderIndex = defenderIndex;
		Defender = fleets.GetFriendShip(DefenderIndex)!;
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
			$"[{Core.Types.Attacks.NightAttack.AttackDisplay(attack.AttackType, DisplayEquipment)}] " +
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
