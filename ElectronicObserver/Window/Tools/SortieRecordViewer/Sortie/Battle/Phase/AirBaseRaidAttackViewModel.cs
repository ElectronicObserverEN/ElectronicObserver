using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class AirBaseRaidAttackViewModel : AttackViewModelBase
{
	private int WaveIndex { get; }

	public override BattleIndex? AttackerIndex => null;
	public override string AttackerDisplay => (WaveIndex, DefenderIndex.FleetFlag) switch
	{
		( > 0, _) => string.Format(BattleRes.AirSquadronWave, WaveIndex),
		(_, FleetFlag.Player) => BattleRes.EnemyAirSquadron,
		(_, FleetFlag.Enemy) => BattleRes.FriendlyAirSquadron,
		_ => "???",
	};

	public override BattleIndex DefenderIndex { get; }
	public IBaseAirCorpsData Defender { get; }
	private int DefenderHpBeforeAttack { get; }
	public override string DefenderDisplay { get; }

	public override double Damage { get; }
	private HitType HitType { get; }
	private AirAttack AttackType { get; }
	public override string DamageDisplay { get; }

	public AirBaseRaidAttackViewModel(BattleFleets fleets, int waveIndex, AirBattleAttack attack)
	{
		WaveIndex = waveIndex;
		Damage = attack.Defenders.First().Damage;
		HitType = attack.Defenders.First().CriticalFlag;

		DefenderIndex = attack.Defenders.First().Defender;
		Defender = fleets.GetAirBase(DefenderIndex)!;
		DefenderDisplay = $"{Defender.Name} {DefenderIndex.Display}";

		AttackType = attack.AttackType;

		DamageDisplay =
			$"[{GetAttackKind(AttackType)}] " +
			$"{AttackDisplay(attack.Defenders.First().GuardsFlagship, Damage, HitType)}";

		DefenderHpBeforeAttack = Defender.HPCurrent;

		int hpAfterAttacks = Math.Max(0, DefenderHpBeforeAttack - (int)Damage);

		if (DefenderHpBeforeAttack > 0 && DefenderHpBeforeAttack != hpAfterAttacks)
		{
			DamageDisplay += $" ({DefenderHpBeforeAttack} → {hpAfterAttacks})";
		}
	}

	private static string GetAttackKind(AirAttack airAttack) => airAttack switch
	{
		AirAttack.Torpedo => ConstantsRes.TorpedoAttack,
		AirAttack.Bombing => ConstantsRes.BombingAttack,
		AirAttack.TorpedoBombing => ConstantsRes.TorpBombingAttack,
		_ => ConstantsRes.Unknown,
	};
}
