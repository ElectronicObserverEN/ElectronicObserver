using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class AirBattleAttackViewModel : AttackViewModelBase
{
	public int WaveIndex { get; }

	public BattleIndex DefenderIndex { get; }
	public IShipData Defender { get; }
	public int DefenderHpBeforeAttack { get; }

	public int Damage { get; }
	public HitType HitType { get; }
	public AirAttack AttackType { get; }
	private IEquipmentData? UsedDamecon { get; }
	public string DamageDisplay { get; }
	public bool GuardsFlagship { get; }

	public string AttackerName => (WaveIndex, DefenderIndex.FleetFlag) switch
	{
		( > 0, _) => string.Format(BattleRes.AirSquadronWave, WaveIndex),
		(_, FleetFlag.Player) => BattleRes.EnemyAirSquadron,
		(_, FleetFlag.Enemy) => BattleRes.FriendlyAirSquadron,
		_ => "???",
	};

	public string AttackKind => GetAttackKind(AttackType);

	public AirBattleAttackViewModel(BattleFleets fleets, int waveIndex, AirBattleAttack attack)
	{
		WaveIndex = waveIndex;
		Damage = attack.Defenders.First().Damage;
		HitType = attack.Defenders.First().CriticalFlag;

		DefenderIndex = attack.Defenders.First().Defender;
		Defender = fleets.GetShip(DefenderIndex)!;
		AttackType = attack.AttackType;
		GuardsFlagship = attack.Defenders.First().GuardsFlagship;

		DefenderHpBeforeAttack = Defender.HPCurrent;

		int hpAfterAttacks = Math.Max(0, DefenderHpBeforeAttack - Damage);

		if (hpAfterAttacks <= 0 && GetDamecon(Defender) is { } damecon)
		{
			UsedDamecon = damecon;
		}

		DamageDisplay =
			$"[{GetAttackKind(AttackType)}] " +
			$"{AttackDisplay(GuardsFlagship, Damage, HitType)}";

		if (DefenderHpBeforeAttack > 0 && DefenderHpBeforeAttack != hpAfterAttacks)
		{
			DamageDisplay += $" ({DefenderHpBeforeAttack} → {hpAfterAttacks})";
		}

		DamageDisplay += UsedDamecon switch
		{
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairGoddess } => $"　{BattleRes.GoddessActivated} HP{Defender.HPMax}",
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairPersonnel } => $"　{BattleRes.DameconActivated} HP{(int)(Defender.HPMax * 0.2)}",
			_ => null,
		};
	}

	private static string GetAttackKind(AirAttack airAttack) => airAttack switch
	{
		AirAttack.Torpedo => ConstantsRes.TorpedoAttack,
		AirAttack.Bombing => ConstantsRes.BombingAttack,
		AirAttack.TorpedoBombing => ConstantsRes.TorpBombingAttack,
		_ => ConstantsRes.Unknown,
	};
}
