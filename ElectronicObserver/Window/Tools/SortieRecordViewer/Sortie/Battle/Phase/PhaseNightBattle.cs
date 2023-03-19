using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseNightBattle : PhaseBase
{
	private ApiHougeki ShellingData { get; }

	public string Title => BattleRes.BattlePhaseNightBattle;

	public List<PhaseNightBattleAttack> Attacks { get; } = new();
	public List<PhaseNightBattleAttackViewModel> AttackDisplays { get; } = new();

	public PhaseNightBattle(ApiHougeki shellingData)
	{
		static int ParseInt(object e) => e switch
		{
			JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			_ => throw new NotImplementedException(),
		};

		ShellingData = shellingData;

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
			switch (atk.AttackType)
			{
				/*
				case NightAttackKind.SpecialNelson:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						BattleIndex comboatk = new(atk.Attacker.Side, i * 2);       // #1, #3, #5
																					// BattleDetails.Add(new BattleNightDetail(Battle, comboatk, atk.Defenders[i].Defender, new[] { atk.Defenders[i].RawDamage }, new[] { atk.Defenders[i].CriticalFlag }, atk.AttackType, atk.EquipmentIDs, atk.NightAirAttackFlag, hps[atk.Defenders[i].Defender]));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
						damages[comboatk] += atk.Defenders[i].Damage;
					}
					break;

				case NightAttackKind.SpecialNagato:
				case NightAttackKind.SpecialMutsu:
				case NightAttackKind.SpecialYamato2Ships:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						var comboatk = new BattleIndex(atk.Attacker.Side, i / 2);       // #1, #1, #2
																						// BattleDetails.Add(new BattleNightDetail(Battle, comboatk, atk.Defenders[i].Defender, new[] { atk.Defenders[i].RawDamage }, new[] { atk.Defenders[i].CriticalFlag }, atk.AttackType, atk.EquipmentIDs, atk.NightAirAttackFlag, hps[atk.Defenders[i].Defender]));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
						damages[comboatk] += atk.Defenders[i].Damage;
					}
					break;
				*/
				case NightAttackKind.SpecialColorado:
				case NightAttackKind.SpecialKongou:
				case NightAttackKind.SpecialYamato3Ships:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						int fleetCount = KCDatabase.Instance.Fleet.Fleets.Values
							.Count(f => f.IsInSortie);

						PhaseNightBattleAttack comboAttack = atk with
						{
							// hack: Kongou night special attack index is messed up for combined fleet vs combined fleet
							// todo: need to check what happens in case only 1 of the fleets is combined
							// note: when testing via api replay you need a combined fleet in-game, else fleet data (count) won't be correct
							// #1, #2, #3
							Attacker = fleetCount switch
							{
								2 when i < 6 => new(i + 6, FleetFlag.Player),
								_ => new(i, FleetFlag.Player),
							},
							Defenders = new() { atk.Defenders[i] },
						};

						AttackDisplays.Add(new PhaseNightBattleAttackViewModel(battleFleets, comboAttack));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					}
					break;

				default:
					foreach (IGrouping<BattleIndex, PhaseNightBattleDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
					{
						// BattleDetails.Add(new BattleNightDetail(Battle, atk.Attacker, defs.Key, defs.Select(d => d.RawDamage).ToArray(), defs.Select(d => d.CriticalFlag).ToArray(), atk.AttackType, atk.EquipmentIDs, atk.NightAirAttackFlag, hps[defs.Key]));
						AttackDisplays.Add(new PhaseNightBattleAttackViewModel(battleFleets, atk));
						AddDamage(battleFleets, defs.Key, defs.Sum(d => d.Damage));
					}
					// damages[atk.Attacker] += atk.Defenders.Sum(d => d.Damage);
					break;
			}
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}
}
