﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.FleetImageGenerator;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseInitial : PhaseBase
{
	public override string Title => BattleRes.Participant;

	private IKCDatabase KcDatabase { get; }

	public bool IsEnemyCombinedFleet { get; }
	public int? ApiXal01 { get; }
	private bool IsBossDamaged => ApiXal01 > 0;
	public List<int>? ApiCombatRation { get; }
	public List<int>? ApiCombatRationCombined { get; }
	private List<int> FriendInitialHPs { get; }
	private List<int> FriendMaxHPs { get; }

	private List<IShipData?> EnemyMembersInstance { get; }
	private List<IShipData?>? EnemyMembersEscortInstance { get; }

	private string PlayerMainFleetTitle => FleetsAfterPhase?.EscortFleet switch
	{
		null => ConstantsRes.BattleDetail_FriendFleet,
		_ => ConstantsRes.BattleDetail_FriendMainFleet,
	};

	private string EnemyMainFleetTitle => FleetsAfterPhase?.EnemyEscortFleet switch
	{
		null => ConstantsRes.BattleDetail_EnemyFleet,
		_ => ConstantsRes.BattleDetail_EnemyMainFleet,
	};

	public string? PlayerMainFleetDisplay => FleetDisplay(FleetsAfterPhase?.Fleet, false, PlayerMainFleetTitle);
	public string? PlayerEscortFleetDisplay => FleetDisplay(FleetsAfterPhase?.EscortFleet, true, ConstantsRes.BattleDetail_FriendEscortFleet);
	public string? EnemyMainFleetDisplay => EnemyFleetDisplay(FleetsAfterPhase?.EnemyFleet, false, EnemyMainFleetTitle, IsBossDamaged);
	public string? EnemyEscortFleetDisplay => EnemyFleetDisplay(FleetsAfterPhase?.EnemyEscortFleet, true, ConstantsRes.BattleDetail_EnemyEscortFleet, IsBossDamaged);

	public string? AirBaseDisplay => HasAirBaseAttack switch
	{
		true => MakeAirBaseString(),
		_ => null,
	};

	public string? AirRaidAirBaseDisplay => IsAirBaseRaid switch
	{
		true => MakeAirRaidAirBaseString(),
		_ => null,
	};

	private static int GetFleetIndex(int index, bool isEscort) => isEscort switch
	{
		true => index + 7,
		_ => index + 1,
	};

	private static string? FleetDisplay(IFleetData? fleet, bool isEscort, string title) => fleet switch
	{
		null => null,
		_ => $"{FleetHeader(fleet, title)}\n" +
			string.Join("\n", fleet.MembersInstance.Select((ship, i) =>
				ShipDisplay(fleet, ship, GetFleetIndex(i, isEscort)))),
	};

	private static string FleetHeader(IFleetData fleet, string fleetTitle) =>
		$"{fleetTitle} " +
		$"{BattleRes.AirSuperiority} {Calculator.GetAirSuperiorityRangeString(fleet)} " +
		$"{GetLosString(fleet)}";

	private static string? ShipDisplay(IFleetData fleet, IShipData? ship, int displayIndex) => ship switch
	{
		null => null,

		_ => $"#{displayIndex}: {ship.MasterShip.ShipTypeName} {ship.NameWithLevel} " +
			$"HP: {ship.HPCurrent}/{ship.HPMax} - " +
			$"{GeneralRes.Firepower}:{ship.FirepowerBase}, " +
			$"{GeneralRes.Torpedo}:{ship.TorpedoBase}, " +
			$"{GeneralRes.AntiAir}:{ship.AABase}, " +
			$"{GeneralRes.Armor}:{ship.ArmorBase}" +
			$"{EscapedText(fleet.EscapedShipList.Contains(fleet.MembersInstance.IndexOf(ship)))}" +
			$"\n" +
			$" {string.Join(", ", ship.AllSlotInstance.Zip(ship.IsExpansionSlotAvailable switch
			{
				true => ship.Aircraft.Concat(new[] { 0 }),
				_ => ship.Aircraft,
			},
				(eq, aircraft) => eq switch
				{
					null => null,
					{ MasterEquipment.IsAircraft: true } => $"[{aircraft}] {eq.NameWithLevel}",
					_ => eq.NameWithLevel,
				}
			).Where(str => str != null))}",
	};

	private static string EscapedText(bool isEscaped) => isEscaped switch
	{
		true => $" ({BattleRes.Escaped})",
		_ => "",
	};

	private static string? EnemyFleetDisplay(IFleetData? fleet, bool isEscort, string title, bool isBossDamaged) => fleet switch
	{
		null => null,
		_ => $"{EnemyFleetHeader(fleet, title, isBossDamaged)}\n" +
			string.Join("\n", fleet.MembersInstance.Select((ship, i) =>
				EnemyShipDisplay(ship, GetFleetIndex(i, isEscort)))),
	};

	private static string EnemyFleetHeader(IFleetData fleet, string fleetTitle, bool isBossDamaged)
		=> isBossDamaged switch
		{
			true => $"{EnemyFleetHeader(fleet, fleetTitle)}{BattleRes.BossDebuffed}",
			_ => EnemyFleetHeader(fleet, fleetTitle),
		};

	private static string EnemyFleetHeader(IFleetData fleet, string fleetTitle) =>
		$"{fleetTitle} " +
		$"{string.Format(BattleRes.AirBaseAirPower, Calculator.GetAirSuperiority(fleet), AirBaseAirPower(fleet))}";

	private static int AirBaseAirPower(IFleetData fleet) => fleet.MembersInstance
		.Where(s => s is not null)
		.Sum(s => s!.AllSlotInstance.Zip(s.Aircraft, (eq, size) => (Equipment: eq, Size: size))
			.Where(slot => slot.Equipment?.MasterEquipment.IsAircraft ?? false)
			.Sum(slot => Calculator.GetAirSuperiority(slot.Equipment!.EquipmentID, slot.Size, 0, 0, AirBaseActionKind.Mission)));

	private static string? EnemyShipDisplay(IShipData? ship, int index) => ship switch
	{
		null => null,

		_ => $"#{index}: ID:{ship.MasterShip.ShipID} {ship.MasterShip.ShipTypeName} {ship.NameWithLevel} " +
			$"HP: {ship.HPCurrent}/{ship.HPMax} - " +
			$"{GeneralRes.Firepower}:{ship.FirepowerBase}, " +
			$"{GeneralRes.Torpedo}:{ship.TorpedoBase}, " +
			$"{GeneralRes.AntiAir}:{ship.AABase}, " +
			$"{GeneralRes.Armor}:{ship.ArmorBase}" +
			$"\n" +
			$" {string.Join(", ", ship.AllSlotInstance.Zip(ship.ExpansionSlot switch
			{
				> 0 => ship.Aircraft.Concat(new[] { 0 }),
				_ => ship.Aircraft,
			},
				(eq, aircraft) => eq switch
				{
					null => null,
					{ MasterEquipment.IsAircraft: true } => $"ID:{eq.MasterEquipment.EquipmentID} [{aircraft}] {eq.NameWithLevel}",
					_ => $"ID:{eq.MasterEquipment.EquipmentID} {eq.NameWithLevel}",
				}
			).Where(str => str != null))}",
	};

	public bool IsAirBaseRaid { get; }

	public bool HasAirBaseAttack { get; }

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, IBattleApiResponse battle)
	{
		KcDatabase = kcDatabase;
		FleetsBeforePhase = fleets;

		if (battle is IAirBaseBattle { ApiAirBaseAttack: not null } abb)
		{
			HasAirBaseAttack = true;

			foreach (ApiAirBaseAttack baseAttack in abb.ApiAirBaseAttack)
			{
				IBaseAirCorpsData? ab = FleetsBeforePhase.AirBases
					.Skip(baseAttack.ApiBaseId - 1)
					.FirstOrDefault();

				if (ab is null) continue;

				IEnumerable<BaseAirCorpsSquadronMock> squadrons = ab.Squadrons.Values
					.Where(s => s.EquipmentInstance is not null)
					.OfType<BaseAirCorpsSquadronMock>();

				foreach ((ApiSquadronPlane apiSquadron, BaseAirCorpsSquadronMock squadron) in baseAttack.ApiSquadronPlane.Zip(squadrons))
				{
					squadron.AircraftCurrent = apiSquadron.ApiCount;
				}
			}
		}

		ApiXal01 = battle.ApiXal01;
		ApiCombatRation = battle.ApiCombatRation;
		FriendInitialHPs = battle.ApiFNowhps;
		FriendMaxHPs = battle.ApiFMaxhps;

		SetShipParameters(fleets.Fleet.MembersInstance, FriendInitialHPs, battle.ApiFParam);

		Escape(fleets.Fleet, battle.ApiEscapeIdx);

		EnemyMembersInstance = battle.ApiShipKe
			.Zip(battle.ApiShipLv, (id, level) => (Id: id, Level: level))
			.Zip(battle.ApiESlot, (t, equipment) => (t.Id, t.Level, Equipment: equipment))
			.Zip(battle.ApiENowhps, (t, hp) => (t.Id, t.Level, t.Equipment, Hp: hp))
			.Select(t => t.Id switch
			{
				> 0 => new ShipDataMock(KcDatabase.MasterShips[t.Id])
				{
					HPCurrent = t.Hp switch
					{
						JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
						JsonElement { ValueKind: JsonValueKind.String } => KcDatabase.MasterShips[t.Id].HPMin,
						_ => throw new NotImplementedException(),
					},
					Level = t.Level,
					Condition = 49,
					SlotInstance = t.Equipment
						.Select(id => id switch
						{
							> 0 => new EquipmentDataMock(KcDatabase.MasterEquipments[id]),
							_ => null,
						})
						.Cast<IEquipmentData?>()
						.ToList(),
					CanBeTargeted = t.Hp switch
					{
						JsonElement { ValueKind: JsonValueKind.Number } => true,
						JsonElement { ValueKind: JsonValueKind.String } => false,
						_ => throw new NotImplementedException(),
					},
				},
				_ => null,
			})
			.Cast<IShipData?>()
			.ToList();

		SetEnemyRange(EnemyMembersInstance);
	}

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, IPlayerCombinedFleetBattle battle)
		: this(kcDatabase, fleets, (IBattleApiResponse)battle)
	{
		ApiCombatRationCombined = battle.ApiCombatRationCombined;
		SetEscortFleetParameters(fleets, battle);
		Escape(fleets.EscortFleet, battle.ApiEscapeIdxCombined);
	}

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, IEnemyCombinedFleetBattle battle)
		: this(kcDatabase, fleets, (IBattleApiResponse)battle)
	{
		IsEnemyCombinedFleet = true;
		EnemyMembersEscortInstance = MakeEnemyEscortFleet(battle);
		SetEnemyRange(EnemyMembersEscortInstance);
	}

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, ICombinedBattleApiResponse battle)
		: this(kcDatabase, fleets, (IBattleApiResponse)battle)
	{
		ApiCombatRationCombined = battle.ApiCombatRationCombined;
		SetEscortFleetParameters(fleets, battle);
		Escape(fleets.EscortFleet, battle.ApiEscapeIdxCombined);

		IsEnemyCombinedFleet = true;
		EnemyMembersEscortInstance = MakeEnemyEscortFleet(battle);
		SetEnemyRange(EnemyMembersEscortInstance);
	}

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, ICombinedNightBattleApiResponse battle)
		: this(kcDatabase, fleets, (IBattleApiResponse)battle)
	{
		SetEscortFleetParameters(fleets, battle);
		EnemyMembersEscortInstance = MakeEnemyEscortFleet(battle);
		SetEnemyRange(EnemyMembersEscortInstance);
	}

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, ApiDestructionBattle battle)
	{
		KcDatabase = kcDatabase;
		FleetsBeforePhase = fleets;
		IsAirBaseRaid = true;
		HasAirBaseAttack = true;

		FriendInitialHPs = battle.ApiFNowhps;
		FriendMaxHPs = battle.ApiFMaxhps;

		EnemyMembersInstance = battle.ApiShipKe
			.Zip(battle.ApiShipLv, (id, level) => (Id: id, Level: level))
			.Zip(battle.ApiESlot, (t, equipment) => (t.Id, t.Level, Equipment: equipment))
			.Zip(battle.ApiENowhps, (t, hp) => (t.Id, t.Level, t.Equipment, Hp: hp))
			.Select(t => t.Id switch
			{
				> 0 => new ShipDataMock(KcDatabase.MasterShips[t.Id])
				{
					HPCurrent = t.Hp switch
					{
						JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
						JsonElement { ValueKind: JsonValueKind.String } => KcDatabase.MasterShips[t.Id].HPMin,
						_ => throw new NotImplementedException(),
					},
					Level = t.Level,
					Condition = 49,
					SlotInstance = t.Equipment
						.Select(id => id switch
						{
							> 0 => new EquipmentDataMock(KcDatabase.MasterEquipments[id]),
							_ => null,
						})
						.Cast<IEquipmentData?>()
						.ToList(),
				},
				_ => null,
			})
			.Cast<IShipData?>()
			.ToList();
	}

	private static void SetEscortFleetParameters(BattleFleets fleets, IPlayerCombinedFleetBattle battle)
	{
		if (fleets.EscortFleet is null) return;

		SetShipParameters(fleets.EscortFleet.MembersInstance, battle.ApiFNowhpsCombined, battle.ApiFParamCombined);
	}

	private static void SetEscortFleetParameters(BattleFleets fleets, ICombinedNightBattleApiResponse battle)
	{
		if (fleets.EscortFleet is null) return;
		if (battle.ApiFNowhpsCombined is null) return;
		if (battle.ApiFParamCombined is null) return;

		SetShipParameters(fleets.EscortFleet.MembersInstance, battle.ApiFNowhpsCombined, battle.ApiFParamCombined);
	}

	private static void SetShipParameters(IEnumerable<IShipData?> ships, List<int> hpNowList, List<List<int>> param)
	{
		foreach ((IShipData? ship, int i) in ships.Select((s, i) => (s, i)))
		{
			if (ship is not ShipDataMock s) continue;

			s.HPCurrent = hpNowList[i];
			s.FirepowerBase = param[i][0];
			s.TorpedoBase = param[i][1];
			s.AABase = param[i][2];
			s.ArmorBase = param[i][3];
		}
	}

	private static void Escape(IFleetData? fleet, List<int>? escapeIndexes)
	{
		if (fleet is null) return;
		if (escapeIndexes is null) return;

		foreach (int index in escapeIndexes)
		{
			fleet.Escape(index);
		}
	}

	private List<IShipData?> MakeEnemyEscortFleet(IEnemyCombinedFleetBattle battle) => battle.ApiShipKeCombined
		.Zip(battle.ApiShipLvCombined, (id, level) => (Id: id, Level: level))
		.Zip(battle.ApiESlotCombined, (t, equipment) => (t.Id, t.Level, Equipment: equipment))
		.Zip(battle.ApiENowhpsCombined, (t, hp) => (t.Id, t.Level, t.Equipment, Hp: hp))
		.Select(t => t.Id switch
		{
			> 0 => new ShipDataMock(KcDatabase.MasterShips[t.Id])
			{
				HPCurrent = t.Hp,
				Level = t.Level,
				Condition = 49,
				SlotInstance = t.Equipment
					.Select(id => id switch
					{
						> 0 => new EquipmentDataMock(KcDatabase.MasterEquipments[id]),
						_ => null,
					})
					.Cast<IEquipmentData?>()
					.ToList(),
			},
			_ => null,
		})
		.Cast<IShipData?>()
		.ToList();

	private void SetEnemyRange(List<IShipData?> ships)
	{
		foreach (IShipData? ship in ships)
		{
			if (ship is not ShipDataMock s) continue;

			s.Range = Math.Max(s.MasterShip.Range, s.AllSlotInstance switch
			{
				[] => 0,
				_ => s.AllSlotInstance.Max(s => s?.MasterEquipment.Range ?? 0),
			});
		}
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		battleFleets = FleetsBeforePhase!.Clone();
		FleetsAfterPhase = battleFleets;

		FleetsAfterPhase.EnemyFleet = new FleetDataMock
		{
			FleetType = EnemyMembersEscortInstance switch
			{
				null => FleetType.Single,
				// todo: determine combined fleet type
				_ => FleetType.Carrier,
			},
			MembersInstance = new(EnemyMembersInstance),
		};

		if (EnemyMembersEscortInstance is not null)
		{
			FleetsAfterPhase.EnemyEscortFleet = new FleetDataMock
			{
				MembersInstance = new(EnemyMembersEscortInstance),
			};
		}

		if (IsAirBaseRaid)
		{
			foreach ((IBaseAirCorpsData? airBase, int hp) in FleetsAfterPhase.AirBases.Zip(FriendMaxHPs))
			{
				if (airBase is not BaseAirCorpsDataMock ab) continue;

				ab.HPMax = hp;
			}

			foreach ((IBaseAirCorpsData? airBase, int hp) in FleetsAfterPhase.AirBases.Zip(FriendInitialHPs))
			{
				if (airBase is not BaseAirCorpsDataMock ab) continue;

				ab.HPCurrent = hp;
			}
		}
		else
		{
			foreach ((IShipData? ship, int hp) in FleetsAfterPhase.Fleet.MembersInstance.Zip(FriendInitialHPs))
			{
				if (ship is not ShipDataMock s) continue;

				s.HPCurrent = hp;
			}
		}

		return FleetsAfterPhase.Clone();
	}

	private static string GetLosString(IFleetData fleet)
	{
		List<LosValue> los = GetLosValues(fleet);

		return string.Format(BattleRes.Los, los[0].Value, los[1].Value, los[2].Value, los[3].Value);
	}

	private static List<LosValue> GetLosValues(IFleetData fleet) => Enumerable.Range(1, 4)
		.Select(w => new LosValue(w, Math.Round(Calculator.GetSearchingAbility_New33(fleet, w), 2, MidpointRounding.ToNegativeInfinity)))
		.ToList();

	private string MakeAirBaseString()
	{
		StringBuilder sb = new();

		sb.AppendLine(ConstantsRes.BattleDetail_AirBase);

		foreach (IBaseAirCorpsData corps in FleetsBeforePhase!.AirBases)
		{
			sb.AppendFormat("{0} [{1}] " + BattleRes.AirSuperiority + " {2}\r\n　{3}\r\n",
				corps.Name, Constants.GetBaseAirCorpsActionKind(corps.ActionKind),
				Calculator.GetAirSuperiorityRangeString(corps),
				string.Join(", ", corps.Squadrons.Values
					.Where(sq => sq is { State: 1, EquipmentInstance: not null })
					.Select(sq => sq.EquipmentInstance!.NameWithLevel)));
		}

		return sb.ToString().TrimEnd();
	}

	private string MakeAirRaidAirBaseString()
	{
		StringBuilder sb = new();

		sb.AppendLine(ConstantsRes.BattleDetail_FriendFleet);

		for (int i = 0; i < FriendInitialHPs.Count; i++)
		{
			if (FriendMaxHPs[i] <= 0)
				continue;

			sb.AppendFormat(BattleRes.OutputFriendBase + "\r\n\r\n",
				i + 1,
				i + 1,
				FriendInitialHPs[i], FriendMaxHPs[i]);
		}

		return sb.ToString().TrimEnd();
	}
}
