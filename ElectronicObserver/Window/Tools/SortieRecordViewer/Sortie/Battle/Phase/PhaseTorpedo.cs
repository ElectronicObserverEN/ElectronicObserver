using System;
using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public abstract class PhaseTorpedo : PhaseBase
{
	public List<PhaseTorpedoAttackViewModel> AttackDisplays { get; } = [];

	protected static List<PhaseTorpedoAttack> ProcessPlayerAttacks(ApiRaigekiClass apiRaigekiClass)
	{
		List<PhaseTorpedoAttack> attacks = [];

		for (int i = 0; i < apiRaigekiClass.ApiFrai.Count; i++)
		{
			if (apiRaigekiClass.ApiFrai[i] < 0) continue;
			if (apiRaigekiClass.ApiFdam.Count <= i) break;
			if (apiRaigekiClass.ApiFydam.Count <= i) break;
			if (apiRaigekiClass.ApiFcl.Count <= i) break;

			BattleIndex attacker = new(i, FleetFlag.Player);
			BattleIndex defender = new(apiRaigekiClass.ApiFrai[i], FleetFlag.Enemy);

			double protectedFlag = apiRaigekiClass.ApiEdam[defender.Index] - Math.Floor(apiRaigekiClass.ApiEdam[defender.Index]);

			PhaseTorpedoAttack attack = new()
			{
				Attacker = attacker,
				AttackType = DayAttackKind.Torpedo,
				Defenders =
				[
					new()
					{
						RawDamage = apiRaigekiClass.ApiFydam[i] + protectedFlag,
						Defender = defender,
						CriticalFlag = apiRaigekiClass.ApiFcl[i] switch
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

		return attacks;
	}

	protected static List<PhaseTorpedoAttack> ProcessEnemyAttacks(ApiRaigekiClass apiRaigekiClass)
	{
		List<PhaseTorpedoAttack> attacks = [];

		for (int i = 0; i < apiRaigekiClass.ApiErai.Count; i++)
		{
			if (apiRaigekiClass.ApiErai[i] < 0) continue;
			if (apiRaigekiClass.ApiEdam.Count <= i) break;
			if (apiRaigekiClass.ApiEydam.Count <= i) break;
			if (apiRaigekiClass.ApiEcl.Count <= i) break;

			BattleIndex attacker = new(i, FleetFlag.Enemy);
			BattleIndex defender = new(apiRaigekiClass.ApiErai[i], FleetFlag.Player);

			double protectedFlag = apiRaigekiClass.ApiFdam[defender.Index] - Math.Floor(apiRaigekiClass.ApiFdam[defender.Index]);

			PhaseTorpedoAttack attack = new()
			{
				Attacker = attacker,
				AttackType = DayAttackKind.Torpedo,
				Defenders =
				[
					new()
					{
						RawDamage = apiRaigekiClass.ApiEydam[i] + protectedFlag,
						Defender = defender,
						CriticalFlag = apiRaigekiClass.ApiEcl[i] switch
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

		return attacks;
	}
}
