using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			if (apiPhaseOpeningTorpedo.ApiFraiListItems[i] is not List<int> targets) continue;
			if (apiPhaseOpeningTorpedo.ApiFydamListItems[i] is not List<int> attackDamages) continue;
			if (apiPhaseOpeningTorpedo.ApiFclListItems[i] is not List<int> criticalFlags) continue;

			for (int j = 0; j < targets.Count; j++)
			{
				if (targets[j] < 0) continue;

				Debug.Assert(apiPhaseOpeningTorpedo.ApiFdam.Count > j);
				Debug.Assert(attackDamages.Count > j);
				Debug.Assert(criticalFlags.Count > j);

				PhaseTorpedoAttack attack = MakeAttack(i, j, FleetFlag.Player,
					targets, apiPhaseOpeningTorpedo.ApiFdam, attackDamages, criticalFlags);

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
			if (apiPhaseOpeningTorpedo.ApiEraiListItems[i] is not List<int> targets) continue;
			if (apiPhaseOpeningTorpedo.ApiEydamListItems[i] is not List<int> attackDamages) continue;
			if (apiPhaseOpeningTorpedo.ApiEclListItems[i] is not List<int> criticalFlags) continue;

			for (int j = 0; j < targets.Count; j++)
			{
				if (targets[j] < 0) continue;

				Debug.Assert(apiPhaseOpeningTorpedo.ApiEdam.Count > j);
				Debug.Assert(attackDamages.Count > j);
				Debug.Assert(criticalFlags.Count > j);

				PhaseTorpedoAttack attack = MakeAttack(i, j, FleetFlag.Enemy,
					targets, apiPhaseOpeningTorpedo.ApiEdam, attackDamages, criticalFlags);

				attacks.Add(attack);
			}
		}

		return attacks;
	}
}
