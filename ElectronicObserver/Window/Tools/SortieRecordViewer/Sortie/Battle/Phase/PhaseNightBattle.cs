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
	public override string Title => BattleRes.BattlePhaseNightBattle;

	private ApiHougeki ShellingData { get; }

	private List<PhaseNightBattleAttack> Attacks { get; } = new();
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
				case NightAttackKind.SpecialNelson:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						BattleIndex comboAttack = atk.Attacker with { Index = i * 2 };
						AttackDisplays.Add(new PhaseNightBattleAttackViewModel(battleFleets, comboAttack, atk.Defenders.First().Defender, atk.AttackType, atk.Defenders));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					}
					break;

				case NightAttackKind.SpecialNagato:
				case NightAttackKind.SpecialMutsu:
				case NightAttackKind.SpecialYamato2Ships:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						BattleIndex comboAttack = atk.Attacker with { Index = i / 2 };
						AttackDisplays.Add(new PhaseNightBattleAttackViewModel(battleFleets, comboAttack, atk.Defenders.First().Defender, atk.AttackType, atk.Defenders));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					}
					break;

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

						AttackDisplays.Add(new PhaseNightBattleAttackViewModel(battleFleets,
							comboAttack.Attacker, comboAttack.Defenders.First().Defender, 
							comboAttack.AttackType, comboAttack.Defenders));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					}
					break;

				default:
					foreach (IGrouping<BattleIndex, PhaseNightBattleDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
					{
						AttackDisplays.Add(new PhaseNightBattleAttackViewModel(battleFleets, atk.Attacker, defs.Key, atk.AttackType, defs.ToList()));
						AddDamage(battleFleets, defs.Key, defs.Sum(d => d.Damage));
					}
					break;
			}
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}
}
