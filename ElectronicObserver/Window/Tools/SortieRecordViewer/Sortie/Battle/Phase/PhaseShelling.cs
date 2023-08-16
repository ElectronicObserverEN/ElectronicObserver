using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseShelling : PhaseBase
{
	public override string Title => DayShellingPhase switch
	{
		DayShellingPhase.First => BattleRes.BattlePhaseShellingFirst,
		DayShellingPhase.Second => BattleRes.BattlePhaseShellingSecond,
		DayShellingPhase.Third => BattleRes.BattlePhaseShellingThird,

		_ => "???",
	};

	private ApiHougeki1? ShellingData { get; }
	public DayShellingPhase DayShellingPhase { get; }

	private List<PhaseShellingAttack> Attacks { get; } = new();
	public List<PhaseShellingAttackViewModel> AttackDisplays { get; } = new();

	public PhaseShelling(ApiHougeki1? shellingData, DayShellingPhase dayShellingPhase)
	{
		ShellingData = shellingData;
		DayShellingPhase = dayShellingPhase;

		if (ShellingData is null) return;

		static int ParseInt(object e) => e switch
		{
			JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			_ => throw new NotImplementedException(),
		};

		List<FleetFlag> fleetflag = ShellingData.ApiAtEflag;
		List<int> attackers = ShellingData.ApiAtList;
		List<DayAttackKind> attackTypes = ShellingData.ApiAtType;
		List<List<int>> defenders = ShellingData.ApiDfList.Select(elem => elem.Where(e => e != -1).ToList()).ToList();
		List<List<int>> attackEquipments = ShellingData.ApiSiList.Select(elem => elem.Select(ParseInt).ToList()).ToList();
		List<List<HitType>> criticalFlags = ShellingData.ApiClList.Select(elem => elem.Where(e => e != HitType.Invalid).ToList()).ToList();
		List<List<double>> rawDamages = ShellingData.ApiDamage.Select(elem => elem.Where(e => e != -1).ToList()).ToList();

		for (int i = 0; i < attackers.Count; i++)
		{
			PhaseShellingAttack attack = new()
			{
				Attacker = new BattleIndex(attackers[i], fleetflag[i]),
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
				PhaseShellingDefender defender = new()
				{
					Defender = new BattleIndex(defenders[i][k], DefenderFlag(fleetflag[i])),
					CriticalFlag = criticalFlags[i][k],
					RawDamage = rawDamages[i][k],
				};

				attack.Defenders.Add(defender);
			}

			Attacks.Add(attack);
		}
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		if (ShellingData is null) return battleFleets;

		FleetsBeforePhase = battleFleets.Clone();

		foreach (PhaseShellingAttack atk in Attacks)
		{
			switch (atk.AttackType)
			{
				case DayAttackKind.SpecialNelson:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						PhaseShellingAttack comboAttack = atk with
						{
							// #1, #3, #5
							Attacker = new(i * 2, FleetFlag.Player),
							Defenders = new() { atk.Defenders[i] },
						};

						AttackDisplays.Add(new PhaseShellingAttackViewModel(battleFleets, comboAttack));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					}
					break;

				case DayAttackKind.SpecialNagato:
				case DayAttackKind.SpecialMutsu:
				case DayAttackKind.SpecialYamato2Ships:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						PhaseShellingAttack comboAttack = atk with
						{
							// #1, #1, #2
							Attacker = new(i / 2, FleetFlag.Player),
							Defenders = new() { atk.Defenders[i] },
						};

						AttackDisplays.Add(new PhaseShellingAttackViewModel(battleFleets, comboAttack));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					}
					break;

				case DayAttackKind.SpecialColorado:
				case DayAttackKind.SpecialKongo:
				case DayAttackKind.SpecialYamato3Ships:
					for (int i = 0; i < atk.Defenders.Count; i++)
					{
						PhaseShellingAttack comboAttack = atk with
						{
							// #1, #2, #3
							Attacker = new(i, FleetFlag.Player),
							Defenders = new() { atk.Defenders[i] },
						};

						AttackDisplays.Add(new PhaseShellingAttackViewModel(battleFleets, comboAttack));
						AddDamage(battleFleets, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
					}
					break;

				default:
					foreach (IGrouping<BattleIndex, PhaseShellingDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
					{
						AttackDisplays.Add(new PhaseShellingAttackViewModel(battleFleets, atk));
						AddDamage(battleFleets, defs.Key, defs.Sum(d => d.Damage));
					}
					break;
			}
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}
}
