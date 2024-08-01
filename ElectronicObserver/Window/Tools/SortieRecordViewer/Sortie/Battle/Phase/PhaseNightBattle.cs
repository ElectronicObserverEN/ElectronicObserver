using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseNightBattle : AttackPhaseBase
{
	public override string Title => BattleRes.BattlePhaseNightBattle;

	private IKCDatabase KcDatabase { get; }
	private List<PhaseNightBattleAttack> Attacks { get; } = [];
	public override List<PhaseNightBattleAttackViewModel> AttackDisplays { get; } = [];

	public PhaseNightBattle(IKCDatabase kcDatabase, List<FleetFlag> apiAtEflag, List<int> apiAtList,
		List<List<HitType>> apiClList, List<List<double>> apiDamage, List<List<int>> apiDfList,
		List<int> apiNMotherList, List<List<object>> apiSiList, List<NightAttackKind> apiSpList)
	{
		static int ParseInt(object e) => e switch
		{
			JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			_ => throw new NotImplementedException(),
		};

		KcDatabase = kcDatabase;

		List<FleetFlag> fleetflag = apiAtEflag;
		List<int> attackers = apiAtList;
		List<int> nightAirAttackFlags = apiNMotherList;
		List<NightAttackKind> attackTypes = apiSpList;
		List<List<int>> defenders = apiDfList.Select(elem => elem.Where(e => e != -1).ToList()).ToList();
		List<List<int>> attackEquipments = apiSiList.Select(elem => elem.Select(ParseInt).ToList()).ToList();
		List<List<HitType>> criticals = apiClList.Select(elem => elem.Where(e => e != HitType.Invalid).ToList()).ToList();
		List<List<double>> rawDamages = apiDamage.Select(elem => elem.Where(e => e >= 0).ToList()).ToList();

		for (int i = 0; i < attackers.Count; i++)
		{
			PhaseNightBattleAttack attack = new()
			{
				Attacker = new BattleIndex(attackers[i], fleetflag[i]),
				NightAirAttackFlag = nightAirAttackFlags[i] == -1,
				AttackType = attackTypes[i],
				DisplayEquipments = attackEquipments[i]
					.Select(id => KcDatabase.MasterEquipments[id])
					.OfType<IEquipmentDataMaster>()
					.ToList(),
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

	public override BattleFleets EmulateBattle(BattleFleets battleFleets, List<int> damages)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		foreach (PhaseNightBattleAttack atk in Attacks)
		{
			if (atk.AttackType.IsSpecialAttack())
			{
				List<int> attackers = atk.AttackType switch
				{
					NightAttackKind.CutinZuiun => Enumerable.Repeat(atk.Attacker.Index, 2).ToList(),
					_ => atk.AttackType.SpecialAttackIndexes(),
				};

				int fleetCount = KCDatabase.Instance.Fleet.Fleets.Values
					.Count(f => f.IsInSortie);

				for (int i = 0; i < atk.Defenders.Count; i++)
				{
					int attackerIndex = attackers[i];

					PhaseNightBattleAttack comboAttack = atk with
					{
						Attacker = fleetCount switch
						{
							2 => new(attackerIndex + 6, FleetFlag.Player),
							_ => new(attackerIndex, FleetFlag.Player),
						},
						Defenders = [atk.Defenders[i]],
					};

					AttackDisplays.Add(new PhaseNightBattleAttackViewModel(FleetsAfterPhase, comboAttack, comboAttack.Defenders.First().Defender));
					AddDamage(FleetsAfterPhase, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					damages[comboAttack.Attacker.ToFlatIndex()] += comboAttack.Defenders.Sum(d => d.Damage);
				}
			}
			else
			{
				foreach (IGrouping<BattleIndex, PhaseNightBattleDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
				{
					AttackDisplays.Add(new PhaseNightBattleAttackViewModel(FleetsAfterPhase, atk, atk.Defenders.First().Defender));
					AddDamage(FleetsAfterPhase, defs.Key, defs.Sum(d => d.Damage));
					damages[atk.Attacker.ToFlatIndex()] += defs.Sum(d => d.Damage);
				}
			}
		}

		return FleetsAfterPhase.Clone();
	}
}
