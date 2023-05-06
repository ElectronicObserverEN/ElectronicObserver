using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseFriendlyShelling : PhaseBase
{
	private ApiHougeki ShellingData { get; }

	public string Title => BattleRes.BattlePhaseFriendlyShelling;

	private List<PhaseNightBattleAttack> Attacks { get; } = new();
	public List<PhaseFriendNightBattleAttackViewModel> AttackDisplays { get; } = new();

	public PhaseFriendlyShelling(ApiFriendlyBattle apiFriendlyBattle)
	{
		static int ParseInt(object e) => e switch
		{
			JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			_ => throw new NotImplementedException(),
		};

		ShellingData = apiFriendlyBattle.ApiHougeki;

		List<FleetFlag> fleetflag = ShellingData.ApiAtEflag;
		List<int> attackers = ShellingData.ApiAtList;
		List<int> nightAirAttackFlags = ShellingData.ApiNMotherList;
		List<NightAttackKind> attackTypes = ShellingData.ApiSpList;
		List<List<int>> defenders = ShellingData.ApiDfList.Select(elem => elem.Where(e => e != -1).ToList()).ToList();
		List<List<int>> attackEquipments = ShellingData.ApiSiList.Select(elem => elem.Select(ch => ParseInt(ch)).ToList()).ToList();
		List<List<HitType>> criticals = ShellingData.ApiClList.Select(elem => elem.Where(e => e != HitType.Invalid).ToList()).ToList();
		List<List<double>> rawDamages = ShellingData.ApiDamage.Select(elem => elem.Where(e => e != -1).ToList()).ToList();

		for (int i = 0; i < attackers.Count; i++)
		{
			PhaseNightBattleAttack attack = new()
			{
				Attacker = new BattleIndex(attackers[i], fleetflag[i]),
				NightAirAttackFlag = nightAirAttackFlags[i] == -1,
				AttackType = attackTypes[i],
				EquipmentIDs = attackEquipments[i],
			};

			static FleetFlag DefenderFlag(FleetFlag flag) => flag switch
			{
				FleetFlag.Player => FleetFlag.Enemy,
				_ => FleetFlag.Player,
			};

			for (int k = 0; k < defenders[i].Count; k++)
			{
				PhaseNightBattleDefender defender = new()
				{
					Defender = new BattleIndex(defenders[i][k], DefenderFlag(fleetflag[i])),
					CriticalFlag = criticals[i][k],
					RawDamage = rawDamages[i][k],
				};

				attack.Defenders.Add(defender);
			}

			Attacks.Add(attack);
		}
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();

		foreach (PhaseNightBattleAttack atk in Attacks)
		{
			foreach (IGrouping<BattleIndex, PhaseNightBattleDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
			{
				AttackDisplays.Add(new PhaseFriendNightBattleAttackViewModel(battleFleets, atk.Attacker, defs.Key, atk.AttackType, defs.ToList()));
				AddFriendDamage(battleFleets, defs.Key, defs.Sum(d => d.Damage));
			}
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}
}
