using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseTorpedo : PhaseBase
{
	public override string Title => Phase switch
	{
		TorpedoPhase.Opening => BattleRes.BattlePhaseOpeningTorpedo,
		TorpedoPhase.Closing => BattleRes.BattlePhaseClosingTorpedo,

		_ => "???",
	};

	private ApiRaigekiClass BattleApiOpeningAtack { get; }
	private TorpedoPhase Phase { get; }

	private List<PhaseShellingAttack> Attacks { get; } = new();
	public List<PhaseShellingAttackViewModel> AttackDisplays { get; } = new();

	public PhaseTorpedo(ApiRaigekiClass battleApiOpeningAtack, TorpedoPhase phase)
	{
		BattleApiOpeningAtack = battleApiOpeningAtack;
		Phase = phase;
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();

		ProcessPlayerAttacks();
		ProcessEnemyAttacks();

		foreach (PhaseShellingAttack attack in Attacks)
		{
			AttackDisplays.Add(new(battleFleets, attack));
			AddDamage(battleFleets, attack.Defenders.First().Defender, attack.Defenders.First().Damage);
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}

	private void ProcessPlayerAttacks()
	{
		for (int i = 0; i < BattleApiOpeningAtack.ApiFrai.Count; i++)
		{
			if (BattleApiOpeningAtack.ApiFrai[i] < 0) continue;
			if (BattleApiOpeningAtack.ApiFdam.Count <= i) break;
			if (BattleApiOpeningAtack.ApiFydam.Count <= i) break;
			if (BattleApiOpeningAtack.ApiFcl.Count <= i) break;

			BattleIndex attacker = new(i, FleetFlag.Player);
			BattleIndex defender = new(BattleApiOpeningAtack.ApiFrai[i], FleetFlag.Enemy);

			PhaseShellingAttack attack = new()
			{
				Attacker = attacker,
				AttackType = DayAttackKind.Torpedo,
				Defenders = new()
				{
					new()
					{
						RawDamage = BattleApiOpeningAtack.ApiFydam[i],
						Defender = defender,
						CriticalFlag = BattleApiOpeningAtack.ApiFcl[i] switch
						{
							2 => HitType.Critical,
							1 => HitType.Hit,
							_ => HitType.Miss,
						},
					},
				},
			};

			Attacks.Add(attack);
		}
	}

	private void ProcessEnemyAttacks()
	{
		for (int i = 0; i < BattleApiOpeningAtack.ApiErai.Count; i++)
		{
			if (BattleApiOpeningAtack.ApiErai[i] < 0) continue;
			if (BattleApiOpeningAtack.ApiEdam.Count <= i) break;
			if (BattleApiOpeningAtack.ApiEydam.Count <= i) break;
			if (BattleApiOpeningAtack.ApiEcl.Count <= i) break;

			BattleIndex attacker = new(i, FleetFlag.Enemy);
			BattleIndex defender = new(BattleApiOpeningAtack.ApiErai[i], FleetFlag.Player);

			PhaseShellingAttack attack = new()
			{
				Attacker = attacker,
				AttackType = DayAttackKind.Torpedo,
				Defenders = new()
				{
					new()
					{
						RawDamage = BattleApiOpeningAtack.ApiEydam[i],
						Defender = defender,
						CriticalFlag = BattleApiOpeningAtack.ApiEcl[i] switch
						{
							2 => HitType.Critical,
							1 => HitType.Hit,
							_ => HitType.Miss,
						},
					},
				},
			};

			Attacks.Add(attack);
		}
	}
}
