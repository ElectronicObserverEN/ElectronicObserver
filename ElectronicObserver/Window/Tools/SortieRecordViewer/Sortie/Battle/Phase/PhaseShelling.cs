﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.KancolleApi.Types.Models;

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

	private IKCDatabase KcDatabase { get; }
	private ApiHougeki1 ShellingData { get; }
	public DayShellingPhase DayShellingPhase { get; }

	private List<PhaseShellingAttack> Attacks { get; } = new();
	public List<PhaseShellingAttackViewModel> AttackDisplays { get; } = new();

	public PhaseShelling(IKCDatabase kcDatabase, ApiHougeki1 shellingData, DayShellingPhase dayShellingPhase)
	{
		KcDatabase = kcDatabase;
		ShellingData = shellingData;
		DayShellingPhase = dayShellingPhase;

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
				DisplayEquipments = attackEquipments[i]
					.Select(id => KcDatabase.MasterEquipments[id])
					.ToList(),
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
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		foreach (PhaseShellingAttack atk in Attacks)
		{
			if (atk.AttackType.IsSpecialAttack())
			{
				List<int> attackers = atk.AttackType.SpecialAttackIndexes();

				for (int i = 0; i < atk.Defenders.Count; i++)
				{
					PhaseShellingAttack comboAttack = atk with
					{
						Attacker = new(attackers[i], FleetFlag.Player),
						Defenders = new() { atk.Defenders[i] },
					};

					AttackDisplays.Add(new PhaseShellingAttackViewModel(FleetsAfterPhase, comboAttack));
					AddDamage(FleetsAfterPhase, atk.Defenders[i].Defender, atk.Defenders[i].Damage);
				}
			}
			else
			{
				foreach (IGrouping<BattleIndex, PhaseShellingDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
				{
					AttackDisplays.Add(new PhaseShellingAttackViewModel(FleetsAfterPhase, atk));
					AddDamage(FleetsAfterPhase, defs.Key, defs.Sum(d => d.Damage));
				}
			}
		}

		return FleetsAfterPhase.Clone();
	}
}
