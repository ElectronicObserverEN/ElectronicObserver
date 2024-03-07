using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseOpeningTorpedo : PhaseTorpedo
{
	public override string Title => BattleRes.BattlePhaseOpeningTorpedo;

	private ApiRaigekiClass? ApiRaigekiClass { get; }
	private ApiPhaseOpeningTorpedo? ApiPhaseOpeningTorpedo { get; }

	private List<PhaseTorpedoAttack> Attacks { get; } = [];

	public PhaseOpeningTorpedo(ApiPhaseOpeningTorpedo apiPhaseOpeningTorpedo)
	{
		ApiPhaseOpeningTorpedo = apiPhaseOpeningTorpedo;
	}

	public PhaseOpeningTorpedo(ApiRaigekiClass apiRaigekiClass)
	{
		ApiRaigekiClass = apiRaigekiClass;
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		if (ApiRaigekiClass is not null)
		{
			Attacks.AddRange(ProcessPlayerAttacks(ApiRaigekiClass));
			Attacks.AddRange(ProcessEnemyAttacks(ApiRaigekiClass));
		}
		else if (ApiPhaseOpeningTorpedo is not null)
		{
			Attacks.AddRange(ProcessPlayerAttacks(ApiPhaseOpeningTorpedo));
			Attacks.AddRange(ProcessEnemyAttacks(ApiPhaseOpeningTorpedo));
		}

		foreach (PhaseTorpedoAttack attack in Attacks)
		{
			AttackDisplays.Add(new(FleetsAfterPhase, attack));
			AddDamage(FleetsAfterPhase, attack.Defenders.First().Defender, attack.Defenders.First().Damage);
		}

		return FleetsAfterPhase.Clone();
	}

	private static List<PhaseTorpedoAttack> ProcessPlayerAttacks(ApiPhaseOpeningTorpedo apiPhaseOpeningTorpedo)
	{
		List<PhaseTorpedoAttack> attacks = [];

		for (int i = 0; i < apiPhaseOpeningTorpedo.ApiFraiListItems.Count; i++)
		{
			if (apiPhaseOpeningTorpedo.ApiFraiListItems[i] is null) continue;

			for (int j = 0; j < apiPhaseOpeningTorpedo.ApiFraiListItems[i].Count; j++)
			{
				if (apiPhaseOpeningTorpedo.ApiFraiListItems[i][j] < 0) continue;
				if (apiPhaseOpeningTorpedo.ApiFdam.Count <= j) continue;
				if (apiPhaseOpeningTorpedo.ApiFydamListItems[i].Count <= j) continue;
				if (apiPhaseOpeningTorpedo.ApiFclListItems[i].Count <= j) continue;

				BattleIndex attacker = new(i, FleetFlag.Player);
				BattleIndex defender = new(apiPhaseOpeningTorpedo.ApiFraiListItems[i][j], FleetFlag.Enemy);

				double protectedFlag = apiPhaseOpeningTorpedo.ApiEdam[defender.Index] - Math.Floor(apiPhaseOpeningTorpedo.ApiEdam[defender.Index]);

				PhaseTorpedoAttack attack = new()
				{
					Attacker = attacker,
					AttackType = DayAttackKind.Torpedo,
					Defenders =
					[
						new()
						{
							RawDamage = apiPhaseOpeningTorpedo.ApiFydamListItems[i][j] + protectedFlag,
							Defender = defender,
							CriticalFlag = apiPhaseOpeningTorpedo.ApiFclListItems[i][j] switch
							{
								2 => HitType.Critical,
								1 => HitType.Hit,
								_ => HitType.Miss,
							},
						},
					],
				};

				attacks.Add(attack);
			}
		}

		return attacks;
	}

	private static List<PhaseTorpedoAttack> ProcessEnemyAttacks(ApiPhaseOpeningTorpedo apiPhaseOpeningTorpedo)
	{
		List<PhaseTorpedoAttack> attacks = [];

		for (int i = 0; i < apiPhaseOpeningTorpedo.ApiEraiListItems.Count; i++)
		{
			if (apiPhaseOpeningTorpedo.ApiEraiListItems[i] is null) continue;

			for (int j = 0; j < apiPhaseOpeningTorpedo.ApiEraiListItems[i].Count; j++)
			{
				if (apiPhaseOpeningTorpedo.ApiEraiListItems[i][j] < 0) continue;
				if (apiPhaseOpeningTorpedo.ApiEdam.Count <= j) continue;
				if (apiPhaseOpeningTorpedo.ApiEydamListItems[i].Count <= j) continue;
				if (apiPhaseOpeningTorpedo.ApiEclListItems[i].Count <= j) continue;

				BattleIndex attacker = new(i, FleetFlag.Enemy);
				BattleIndex defender = new(apiPhaseOpeningTorpedo.ApiEraiListItems[i][j], FleetFlag.Player);

				double protectedFlag = apiPhaseOpeningTorpedo.ApiFdam[defender.Index] - Math.Floor(apiPhaseOpeningTorpedo.ApiFdam[defender.Index]);

				PhaseTorpedoAttack attack = new()
				{
					Attacker = attacker,
					AttackType = DayAttackKind.Torpedo,
					Defenders =
					[
						new()
						{
							RawDamage = apiPhaseOpeningTorpedo.ApiEydamListItems[i][j] + protectedFlag,
							Defender = defender,
							CriticalFlag = apiPhaseOpeningTorpedo.ApiEclListItems[i][j] switch
							{
								2 => HitType.Critical,
								1 => HitType.Hit,
								_ => HitType.Miss,
							},
						},
					],
				};

				attacks.Add(attack);
			}
		}

		return attacks;
	}
}
