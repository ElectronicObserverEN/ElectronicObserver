using System;
using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public abstract class PhaseTorpedo : PhaseBase
{
	public List<PhaseTorpedoAttackViewModel> AttackDisplays { get; } = [];

	protected static PhaseTorpedoAttack MakeAttack(int attackerIndex, int targetIndex, FleetFlag fleetFlag,
		List<int> targets, List<double> damages, List<int> attackDamages, List<int> criticalFlags)
	{
		FleetFlag enemyFlag = fleetFlag switch
		{
			FleetFlag.Player => FleetFlag.Enemy,
			_ => FleetFlag.Player,
		};

		BattleIndex attacker = new(attackerIndex, fleetFlag);
		BattleIndex defender = new(targets[targetIndex], enemyFlag);

		double protectedFlag = damages[defender.Index] - Math.Floor(damages[defender.Index]);

		return new()
		{
			Attacker = attacker,
			AttackType = DayAttackKind.Torpedo,
			Defenders =
			[
				new()
				{
					RawDamage = attackDamages[targetIndex] + protectedFlag,
					Defender = defender,
					CriticalFlag = criticalFlags[targetIndex] switch
					{
						2 => HitType.Critical,
						1 => HitType.Hit,
						_ => HitType.Miss,
					},
				},
			],
		};
	}

	protected static List<PhaseTorpedoAttack> ProcessPlayerAttacks(ApiRaigekiClass apiRaigekiClass)
	{
		List<PhaseTorpedoAttack> attacks = [];

		for (int i = 0; i < apiRaigekiClass.ApiFrai.Count; i++)
		{
			if (apiRaigekiClass.ApiFrai[i] < 0) continue;
			if (apiRaigekiClass.ApiEdam.Count <= apiRaigekiClass.ApiFrai[i]) break;
			if (apiRaigekiClass.ApiFydam.Count <= i) break;
			if (apiRaigekiClass.ApiFcl.Count <= i) break;

			PhaseTorpedoAttack attack = MakeAttack(i, i, FleetFlag.Player, apiRaigekiClass.ApiFrai,
				apiRaigekiClass.ApiEdam, apiRaigekiClass.ApiFydam, apiRaigekiClass.ApiFcl);

			attacks.Add(attack);
		}

		return attacks;
	}

	protected static List<PhaseTorpedoAttack> ProcessEnemyAttacks(ApiRaigekiClass apiRaigekiClass)
	{
		List<PhaseTorpedoAttack> attacks = [];

		for (int i = 0; i < apiRaigekiClass.ApiErai.Count; i++)
		{
			if (apiRaigekiClass.ApiErai[i] < 0) continue;
			if (apiRaigekiClass.ApiFdam.Count <= i) break;
			if (apiRaigekiClass.ApiEydam.Count <= i) break;
			if (apiRaigekiClass.ApiEcl.Count <= i) break;

			PhaseTorpedoAttack attack = MakeAttack(i, i, FleetFlag.Enemy, apiRaigekiClass.ApiErai,
				apiRaigekiClass.ApiFdam, apiRaigekiClass.ApiEydam, apiRaigekiClass.ApiEcl);

			attacks.Add(attack);
		}

		return attacks;
	}
}
