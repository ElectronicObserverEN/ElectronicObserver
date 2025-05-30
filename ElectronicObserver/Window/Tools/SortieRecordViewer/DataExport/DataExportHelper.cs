﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElectronicObserver.Common.ContentDialogs.ExportFilter;
using ElectronicObserver.Common.ContentDialogs.ExportProgress;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using BattleAirRaid = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleAirRaid;
using BattleBaseAirRaid = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleBaseAirRaid;
using BattleCombinedAirRaid = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleCombinedAirRaid;
using BattleCombinedRadar = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleCombinedRadar;
using BattleData = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleData;
using BattleIndex = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleIndex;
using BattleNormalRadar = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.BattleNormalRadar;
using DayAttack = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase.DayAttack;
using NightAttack = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase.NightAttack;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public class DataExportHelper(ElectronicObserverContext db, ToolService toolService)
{
	private ElectronicObserverContext Db { get; } = db;
	private ToolService ToolService { get; } = toolService;

	private async Task<List<T>> ProcessData<T>(
		Func<int?, SortieDetailViewModel, ExportFilterViewModel?, List<T>> dataProcessingFunction,
		IReadOnlyCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default
	)
	{
		exportProgress.Total = sorties.Count;

		List<SortieRecord> sortieRecords = await sorties
			.Select(s => s.Model)
			.WithApiFiles(Db, cancellationToken);

		int? admiralLevel = null;

		return sortieRecords
			.AsParallel()
			.SelectMany(r =>
			{
				SortieDetailViewModel? sortieDetail = ToolService.GenerateSortieDetailViewModel(Db, r);

				if (sortieDetail is null) return [];

				// getting the admiral level is slow as shit
				// since records are ordered chronologically, the level can't change after 120
				if (admiralLevel is not 120)
				{
					admiralLevel = r.GetAdmiralLevel(Db, cancellationToken).Result;
				}

				exportProgress.Progress++;

				return dataProcessingFunction(admiralLevel, sortieDetail, exportFilter);
			})
			.Select((d, i) =>
			{
				// AB raids don't have this data
				if (d is IExportModel exportModel)
				{
					exportModel.CommonData.No = i + 1;
				}

				return d;
			})
			.ToList();
	}

	public async Task<List<ShellingBattleExportModel>> ShellingBattle(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		return await ProcessData(ShellingBattle, sorties, exportFilter, exportProgress, cancellationToken);
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Splitting this up would cause more problems that it would solve")]
	private static List<ShellingBattleExportModel> ShellingBattle(
		int? admiralLevel,
		SortieDetailViewModel sortieDetail,
		ExportFilterViewModel? exportFilter)
	{
		List<ShellingBattleExportModel> dayShellingData = [];
		ApiOffshoreSupply? offshoreSupply = null;

		foreach (SortieNode node in sortieDetail.Nodes.Where(n => exportFilter?.MatchesFilter(n) ?? true))
		{
			offshoreSupply ??= node.ApiOffshoreSupply;

			if (node is not BattleNode battleNode) continue;

			List<BattleData?> battles =
			[
				battleNode.FirstBattle,
				battleNode.SecondBattle,
			];

			BattleFleets fleetsAfterBattle = battleNode.LastBattle.FleetsAfterBattle;

			foreach (BattleData? battle in battles)
			{
				if (battle is null) continue;

				List<PhaseBase> phases = battle.Phases.ToList();

				PhaseInitial? initial = phases.OfType<PhaseInitial>().FirstOrDefault();
				PhaseSearching? searching = phases.OfType<PhaseSearching>().FirstOrDefault();
				PhaseAirBattle? airBattle = phases.OfType<PhaseAirBattle>().FirstOrDefault();
				BattleFleets? fleets = initial?.FleetsAfterPhase;
				IFleetData? playerFleet = initial?.FleetsAfterPhase?.Fleet;

				if (initial is null) continue;
				if (searching is null) continue;
				if (airBattle is null) continue;
				if (fleets is null) continue;
				if (playerFleet is null) continue;

				foreach (PhaseShelling shelling in phases.OfType<PhaseShelling>())
				{
					DayAttackKind previousSpecial = DayAttackKind.Unknown;
					int specialIndex = 0;

					foreach (PhaseShellingAttackViewModel attackDisplay in shelling.AttackDisplays)
					{
						IFleetData? attackerFleet = fleets.GetFleet(attackDisplay.AttackerIndex);
						IShipData? attackerAfterBattle = fleetsAfterBattle.GetShip(attackDisplay.AttackerIndex);
						IShipData? defenderAfterBattle = fleetsAfterBattle.GetShip(attackDisplay.DefenderIndex);

						if (attackerFleet is null) continue;

						foreach ((DayAttack attack, int attackIndex) in attackDisplay.Attacks.Select((a, i) => (a, i)))
						{
							if (exportFilter?.IgnoreMisses is true && attack.CriticalFlag is HitType.Miss) continue;

							int actualAttackIndex = attackIndex;

							if (attack.AttackKind.IsSpecialAttack())
							{
								if (previousSpecial != attack.AttackKind)
								{
									previousSpecial = attack.AttackKind;
									specialIndex = 0;
								}
								else
								{
									specialIndex++;
								}

								actualAttackIndex = specialIndex;
							}

							dayShellingData.Add(new()
							{
								CommonData = MakeCommonData(battleNode, IsFirstNode(sortieDetail.Nodes, battleNode), sortieDetail, admiralLevel, airBattle, searching),
								BattleType = CsvExportResources.ShellingBattle,
								ShipName1 = attackerFleet.MembersInstance.Skip(0).FirstOrDefault()?.Name,
								ShipName2 = attackerFleet.MembersInstance.Skip(1).FirstOrDefault()?.Name,
								ShipName3 = attackerFleet.MembersInstance.Skip(2).FirstOrDefault()?.Name,
								ShipName4 = attackerFleet.MembersInstance.Skip(3).FirstOrDefault()?.Name,
								ShipName5 = attackerFleet.MembersInstance.Skip(4).FirstOrDefault()?.Name,
								ShipName6 = attackerFleet.MembersInstance.Skip(5).FirstOrDefault()?.Name,
								PlayerFleetType = GetPlayerFleet(initial.FleetsAfterPhase!, attackDisplay.AttackerIndex, attackDisplay.DefenderIndex),
								BattlePhase = GetPhaseString(shelling),
								AttackerSide = attackDisplay.AttackerIndex.FleetFlag switch
								{
									FleetFlag.Player => CsvExportResources.Player,
									_ => CsvExportResources.Enemy,
								},
								AttackType = CsvDayAttackKind(attack.AttackKind),
								AttackIndex = actualAttackIndex,
								DisplayedEquipment1 = attackDisplay.DisplayEquipment.Skip(0).FirstOrDefault()?.NameEN,
								DisplayedEquipment2 = attackDisplay.DisplayEquipment.Skip(1).FirstOrDefault()?.NameEN,
								DisplayedEquipment3 = attackDisplay.DisplayEquipment.Skip(2).FirstOrDefault()?.NameEN,
								HitType = (int)attack.CriticalFlag,
								Damage = attack.Damage,
								Protected = attack.GuardsFlagship switch
								{
									true => 1,
									_ => 0,
								},
								Attacker = MakeShip(attack.Attacker, attackDisplay.AttackerIndex, attackDisplay.AttackerHpBeforeAttack, attackerAfterBattle),
								Defender = MakeShip(attack.Defender, attackDisplay.DefenderIndex, attackDisplay.DefenderHpBeforeAttacks[attackIndex], defenderAfterBattle),
								FleetType = Constants.GetCombinedFleet(playerFleet.FleetType),
								EnemyFleetType = GetEnemyFleetType(initial.IsEnemyCombinedFleet),
								SortieItems = MakeSortieItems(battleNode, initial, searching, fleets, offshoreSupply),
								SmokeType = searching.SmokeCount,
								Balloon = MakeBalloonModel(fleets, searching),
								ArmorBreak = initial.ApiXal01,
							});
						}
					}
				}
			}
		}

		return dayShellingData;
	}

	public async Task<List<NightBattleExportModel>> NightBattle(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		return await ProcessData(NightBattle, sorties, exportFilter, exportProgress, cancellationToken);
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Splitting this up would cause more problems that it would solve")]
	private static List<NightBattleExportModel> NightBattle(
		int? admiralLevel,
		SortieDetailViewModel sortieDetail,
		ExportFilterViewModel? exportFilter)
	{
		List<NightBattleExportModel> nightShellingData = [];

		foreach (SortieNode node in sortieDetail.Nodes.Where(n => exportFilter?.MatchesFilter(n) ?? true))
		{
			if (node is not BattleNode battleNode) continue;

			List<BattleData?> battles =
			[
				battleNode.FirstBattle,
				battleNode.SecondBattle,
			];

			BattleFleets fleetsAfterBattle = battleNode.LastBattle.FleetsAfterBattle;
			PhaseInitial? initial = null;
			PhaseSearching? searching = null;

			foreach (BattleData? battle in battles)
			{
				if (battle is null) continue;

				List<PhaseBase> phases = battle.Phases.ToList();

				PhaseNightInitial? nightInitial = phases.OfType<PhaseNightInitial>().FirstOrDefault();
				initial ??= phases.OfType<PhaseInitial>().FirstOrDefault();
				searching ??= phases.OfType<PhaseSearching>().FirstOrDefault();
				BattleFleets? fleets = searching?.FleetsAfterPhase;
				IFleetData? playerFleet = nightInitial?.FleetsAfterPhase?.Fleet;

				if (nightInitial is null) continue;
				if (initial is null) continue;
				if (searching is null) continue;
				if (fleets is null) continue;
				if (playerFleet is null) continue;

				foreach (PhaseNightBattle nightBattle in phases.OfType<PhaseNightBattle>())
				{
					NightAttackKind previousSpecial = NightAttackKind.Unknown;
					int specialIndex = 0;

					foreach (PhaseNightBattleAttackViewModel attackDisplay in nightBattle.AttackDisplays)
					{
						IFleetData? attackerFleet = fleets.GetFleet(attackDisplay.AttackerIndex);
						IShipData? attackerAfterBattle = fleetsAfterBattle.GetShip(attackDisplay.AttackerIndex);
						IShipData? defenderAfterBattle = fleetsAfterBattle.GetShip(attackDisplay.DefenderIndex);

						if (attackerFleet is null) continue;

						foreach ((NightAttack attack, int attackIndex) in attackDisplay.Attacks.Select((a, i) => (a, i)))
						{
							if (exportFilter?.IgnoreMisses is true && attack.CriticalFlag is HitType.Miss) continue;

							int actualAttackIndex = attackIndex;

							if (attack.AttackKind.IsSpecialAttack())
							{
								if (previousSpecial != attack.AttackKind)
								{
									previousSpecial = attack.AttackKind;
									specialIndex = 0;
								}
								else
								{
									specialIndex++;
								}

								actualAttackIndex = specialIndex;
							}

							nightShellingData.Add(new()
							{
								CommonData = MakeCommonData(battleNode, IsFirstNode(sortieDetail.Nodes, node), sortieDetail, admiralLevel, nightInitial, searching),
								BattleType = CsvExportResources.NightBattle,
								ShipName1 = attackerFleet.MembersInstance.Skip(0).FirstOrDefault()?.Name,
								ShipName2 = attackerFleet.MembersInstance.Skip(1).FirstOrDefault()?.Name,
								ShipName3 = attackerFleet.MembersInstance.Skip(2).FirstOrDefault()?.Name,
								ShipName4 = attackerFleet.MembersInstance.Skip(3).FirstOrDefault()?.Name,
								ShipName5 = attackerFleet.MembersInstance.Skip(4).FirstOrDefault()?.Name,
								ShipName6 = attackerFleet.MembersInstance.Skip(5).FirstOrDefault()?.Name,
								PlayerFleetType = GetPlayerFleet(nightInitial.FleetsAfterPhase!, attackDisplay.AttackerIndex, attackDisplay.DefenderIndex),
								Start = GetStartingBattle(battleNode),
								AttackerSide = attackDisplay.AttackerIndex.FleetFlag switch
								{
									FleetFlag.Player => CsvExportResources.Player,
									_ => CsvExportResources.Enemy,
								},
								AttackType = CsvNightAttackKind(attack.AttackKind),
								AttackIndex = actualAttackIndex,
								DisplayedEquipment1 = attackDisplay.DisplayEquipment.Skip(0).FirstOrDefault()?.NameEN,
								DisplayedEquipment2 = attackDisplay.DisplayEquipment.Skip(1).FirstOrDefault()?.NameEN,
								DisplayedEquipment3 = attackDisplay.DisplayEquipment.Skip(2).FirstOrDefault()?.NameEN,
								HitType = (int)attack.CriticalFlag,
								Damage = attack.Damage,
								Protected = attack.GuardsFlagship switch
								{
									true => 1,
									_ => 0,
								},
								Attacker = MakeShip(attack.Attacker, attackDisplay.AttackerIndex, attackDisplay.AttackerHpBeforeAttack, attackerAfterBattle),
								Defender = MakeShip(attack.Defender, attackDisplay.DefenderIndex, attackDisplay.DefenderHpBeforeAttacks[attackIndex], defenderAfterBattle),
								FleetType = Constants.GetCombinedFleet(playerFleet.FleetType),
								EnemyFleetType = GetEnemyFleetType(fleets.EnemyEscortFleet is not null),
								PlayerSearchlightShipIndex = SearchlightIndex(nightInitial.SearchlightIndexFriend),
								PlayerSearchlightEquipmentId = (int?)nightInitial.SearchlightEquipmentFriend?.EquipmentId,
								EnemySearchlightShipIndex = SearchlightIndex(nightInitial.SearchlightIndexEnemy),
								EnemySearchlightEquipmentId = (int?)nightInitial.SearchlightEquipmentEnemy?.EquipmentId,
								ArmorBreak = initial.ApiXal01,
							});
						}
					}
				}
			}
		}

		return nightShellingData;
	}

	public async Task<List<TorpedoBattleExportModel>> TorpedoBattle(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		return await ProcessData(TorpedoBattle, sorties, exportFilter, exportProgress, cancellationToken);
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Splitting this up would cause more problems that it would solve")]
	private static List<TorpedoBattleExportModel> TorpedoBattle(
		int? admiralLevel,
		SortieDetailViewModel sortieDetail,
		ExportFilterViewModel? exportFilter)
	{
		List<TorpedoBattleExportModel> torpedoData = [];
		ApiOffshoreSupply? offshoreSupply = null;

		foreach (SortieNode node in sortieDetail.Nodes.Where(n => exportFilter?.MatchesFilter(n) ?? true))
		{
			offshoreSupply ??= node.ApiOffshoreSupply;

			if (node is not BattleNode battleNode) continue;

			List<BattleData?> battles =
			[
				battleNode.FirstBattle,
				battleNode.SecondBattle,
			];

			BattleFleets fleetsAfterBattle = battleNode.LastBattle.FleetsAfterBattle;

			foreach (BattleData? battle in battles)
			{
				if (battle is null) continue;

				List<PhaseBase> phases = battle.Phases.ToList();

				PhaseInitial? initial = phases.OfType<PhaseInitial>().FirstOrDefault();
				PhaseSearching? searching = phases.OfType<PhaseSearching>().FirstOrDefault();
				PhaseAirBattle? airBattle = phases.OfType<PhaseAirBattle>().FirstOrDefault();
				BattleFleets? fleets = initial?.FleetsAfterPhase;
				IFleetData? playerFleet = initial?.FleetsAfterPhase?.Fleet;

				if (initial is null) continue;
				if (searching is null) continue;
				if (airBattle is null) continue;
				if (fleets is null) continue;
				if (playerFleet is null) continue;

				foreach (PhaseTorpedo torpedo in phases.OfType<PhaseTorpedo>())
				{
					foreach (PhaseTorpedoAttackViewModel attackDisplay in torpedo.AttackDisplays)
					{
						int attackerHpBeforeAttack = torpedo.FleetsBeforePhase!
							.GetShip(attackDisplay.AttackerIndex)!
							.HPCurrent;

						int defenderHpBeforeAttacks = torpedo.FleetsBeforePhase!
							.GetShip(attackDisplay.DefenderIndex)!
							.HPCurrent;

						IFleetData? attackerFleet = fleets.GetFleet(attackDisplay.AttackerIndex);
						IShipData? attackerAfterBattle = fleetsAfterBattle.GetShip(attackDisplay.AttackerIndex);
						IShipData? defenderAfterBattle = fleetsAfterBattle.GetShip(attackDisplay.DefenderIndex);

						if (attackerFleet is null) continue;

						foreach (DayAttack attack in attackDisplay.Attacks)
						{
							if (exportFilter?.IgnoreMisses is true && attack.CriticalFlag is HitType.Miss) continue;

							torpedoData.Add(new()
							{
								CommonData = MakeCommonData(battleNode, IsFirstNode(sortieDetail.Nodes, node), sortieDetail, admiralLevel, airBattle, searching),
								BattleType = CsvExportResources.TorpedoBattle,
								PlayerFleetType = GetPlayerFleet(initial.FleetsAfterPhase!, attackDisplay.AttackerIndex, attackDisplay.DefenderIndex),
								BattlePhase = torpedo switch
								{
									PhaseOpeningTorpedo => "開幕",
									PhaseClosingTorpedo => "閉幕",
									_ => throw new NotImplementedException(),
								},
								AttackerSide = attackDisplay.AttackerIndex.FleetFlag switch
								{
									FleetFlag.Player => CsvExportResources.Player,
									_ => CsvExportResources.Enemy,
								},
								AttackType = null,
								DisplayedEquipment1 = null,
								DisplayedEquipment2 = null,
								DisplayedEquipment3 = null,
								HitType = (int)attack.CriticalFlag,
								Damage = attack.Damage,
								Protected = attack.GuardsFlagship switch
								{
									true => 1,
									_ => 0,
								},
								Attacker = MakeShip(attack.Attacker, attackDisplay.AttackerIndex, attackerHpBeforeAttack, attackerAfterBattle),
								Defender = MakeShip(attack.Defender, attackDisplay.DefenderIndex, defenderHpBeforeAttacks, defenderAfterBattle),
								FleetType = Constants.GetCombinedFleet(playerFleet.FleetType),
								EnemyFleetType = GetEnemyFleetType(fleets.EnemyEscortFleet is not null),
								SmokeType = searching.SmokeCount,
								Balloon = MakeBalloonModel(fleets, searching),
								ArmorBreak = initial.ApiXal01,
							});
						}
					}
				}
			}
		}

		return torpedoData;
	}

	public async Task<List<AirBattleExportModel>> AirBattle(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		return await ProcessData(AirBattle, sorties, exportFilter, exportProgress, cancellationToken);
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Splitting this up would cause more problems that it would solve")]
	private static List<AirBattleExportModel> AirBattle(
		int? admiralLevel,
		SortieDetailViewModel sortieDetail,
		ExportFilterViewModel? exportFilter)
	{
		List<AirBattleExportModel> airBattleData = [];

		foreach (BattleNode node in sortieDetail.Nodes.OfType<BattleNode>().Where(n => exportFilter?.MatchesFilter(n) ?? true))
		{
			List<PhaseBase> phases = node.FirstBattle.Phases
				.Concat(node.SecondBattle switch
				{
					null => [],
					_ => node.SecondBattle.Phases,
				}).ToList();

			PhaseInitial? initial = phases.OfType<PhaseInitial>().FirstOrDefault();
			PhaseSearching? searching = phases.OfType<PhaseSearching>().FirstOrDefault();
			BattleFleets? fleets = initial?.FleetsAfterPhase;

			if (initial is null) continue;
			if (searching is null) continue;
			if (fleets is null) continue;

			foreach (PhaseAirBattle airBattle in phases.OfType<PhaseAirBattle>())
			{
				void ProcessData(IFleetData fleet, IFleetData attackerFleet, FleetFlag fleetFlag, int indexOffset)
				{
					foreach ((IShipData? ship, int index) in fleet.MembersWithoutEscaped!.Select((s, i) => (s, i)))
					{
						if (ship is null) continue;

						BattleIndex defenderIndex = new(index + indexOffset, fleetFlag);
						AirBattleAttackViewModel? attackDisplay = airBattle.AttackDisplays
							.FirstOrDefault(a => a.DefenderIndex == defenderIndex);

						if (exportFilter?.IgnoreMisses is true && attackDisplay?.HitType is HitType.Miss) continue;

						airBattleData.Add(MakeAirBattleExport(node,
							sortieDetail, admiralLevel, airBattle, initial, searching, attackerFleet,
							attackDisplay, ship, defenderIndex, ship.HPCurrent, fleets));
					}
				}

				ProcessData(fleets.EnemyFleet!, fleets.Fleet, FleetFlag.Enemy, 0);

				if (fleets.EnemyEscortFleet is not null)
				{
					ProcessData(fleets.EnemyEscortFleet, fleets.Fleet, FleetFlag.Enemy, 6);
				}

				ProcessData(fleets.Fleet, fleets.EnemyFleet!, FleetFlag.Player, 0);

				if (fleets.EscortFleet is not null)
				{
					ProcessData(fleets.EscortFleet, fleets.EnemyFleet!, FleetFlag.Player, 6);
				}
			}
		}

		return airBattleData;
	}

	public async Task<List<AirBaseBattleExportModel>> AirBaseBattle(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		return await ProcessData(AirBaseBattle, sorties, exportFilter, exportProgress, cancellationToken);
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "Splitting this up would cause more problems that it would solve")]
	private static List<AirBaseBattleExportModel> AirBaseBattle(
		int? admiralLevel,
		SortieDetailViewModel sortieDetail,
		ExportFilterViewModel? exportFilter)
	{
		List<AirBaseBattleExportModel> airBattleData = [];

		foreach (BattleNode node in sortieDetail.Nodes.OfType<BattleNode>().Where(n => exportFilter?.MatchesFilter(n) ?? true))
		{
			List<PhaseBase> phases = node.FirstBattle.Phases
				.Concat(node.SecondBattle switch
				{
					null => [],
					_ => node.SecondBattle.Phases,
				}).ToList();

			PhaseInitial? initial = phases.OfType<PhaseInitial>().FirstOrDefault();
			PhaseSearching? searching = phases.OfType<PhaseSearching>().FirstOrDefault();
			IFleetData? playerFleet = initial?.FleetsAfterPhase?.Fleet;

			if (initial is null) continue;
			if (searching is null) continue;
			if (playerFleet is null) continue;

			foreach (PhaseBaseAirAttack airBaseBattle in phases.OfType<PhaseBaseAirAttack>())
			{
				foreach (PhaseBaseAirAttackUnit airAttackUnit in airBaseBattle.Units)
				{
					BattleFleets? fleets = airAttackUnit.FleetsBeforePhase;

					if (fleets is null) continue;

					void ProcessData(IFleetData fleet, IFleetData attackerFleet, FleetFlag fleetFlag, int indexOffset)
					{
						foreach ((IShipData? ship, int index) in fleet.MembersWithoutEscaped!.Select((s, i) => (s, i)))
						{
							if (ship is null) continue;

							BattleIndex defenderIndex = new(index + indexOffset, fleetFlag);
							AirBattleAttackViewModel? attackDisplay = airAttackUnit.AttackDisplays
								.FirstOrDefault(a => a.DefenderIndex == defenderIndex);

							if (exportFilter?.IgnoreMisses is true && attackDisplay?.HitType is HitType.Miss) continue;

							airBattleData.Add(new()
							{
								CommonData = MakeCommonData(node, IsFirstNode(sortieDetail.Nodes, node), sortieDetail, admiralLevel, airAttackUnit, searching),
								SquadronId = airAttackUnit.AirBaseId,
								SquadronAttackIndex = airAttackUnit.WaveIndex + 1,
								AirBasePlayerContact = airAttackUnit.TouchAircraftFriend,
								AirBaseEnemyContact = airAttackUnit.TouchAircraftEnemy,
								AirBaseSquadron1EquipmentName = airAttackUnit.Squadrons.Skip(0).FirstOrDefault()?.Equipment?.NameEN,
								AirBaseSquadron1Aircraft = airAttackUnit.Squadrons.Skip(0).FirstOrDefault()?.AircraftCount,
								AirBaseSquadron2EquipmentName = airAttackUnit.Squadrons.Skip(1).FirstOrDefault()?.Equipment?.NameEN,
								AirBaseSquadron2Aircraft = airAttackUnit.Squadrons.Skip(1).FirstOrDefault()?.AircraftCount,
								AirBaseSquadron3EquipmentName = airAttackUnit.Squadrons.Skip(2).FirstOrDefault()?.Equipment?.NameEN,
								AirBaseSquadron3Aircraft = airAttackUnit.Squadrons.Skip(2).FirstOrDefault()?.AircraftCount,
								AirBaseSquadron4EquipmentName = airAttackUnit.Squadrons.Skip(3).FirstOrDefault()?.Equipment?.NameEN,
								AirBaseSquadron4Aircraft = airAttackUnit.Squadrons.Skip(3).FirstOrDefault()?.AircraftCount,
								Stage1 = new()
								{
									PlayerAircraftTotal = airAttackUnit.Stage1FCount,
									PlayerAircraftLost = airAttackUnit.Stage1FLostcount,
									EnemyAircraftTotal = airAttackUnit.Stage1ECount,
									EnemyAircraftLost = airAttackUnit.Stage1ELostcount,
								},
								Stage2 = new()
								{
									PlayerAircraftTotal = airAttackUnit.Stage2FCount,
									PlayerAircraftLost = airAttackUnit.Stage2FLostcount,
									EnemyAircraftTotal = airAttackUnit.Stage2ECount,
									EnemyAircraftLost = airAttackUnit.Stage2ELostcount,
								},
								Attacker1 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(0).FirstOrDefault()),
								Attacker2 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(1).FirstOrDefault()),
								Attacker3 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(2).FirstOrDefault()),
								Attacker4 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(3).FirstOrDefault()),
								Attacker5 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(4).FirstOrDefault()),
								Attacker6 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(5).FirstOrDefault()),
								Attacker7 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(6).FirstOrDefault()),
								TotalTorpedoFlags = airAttackUnit.AttackDisplays.Sum(d => d.AttackType switch
								{
									AirAttack.Torpedo or AirAttack.TorpedoBombing => 1,
									_ => 0,
								}),
								TotalBomberFlags = airAttackUnit.AttackDisplays.Sum(d => d.AttackType switch
								{
									AirAttack.Bombing or AirAttack.TorpedoBombing => 1,
									_ => 0,
								}),
								TorpedoFlag = attackDisplay?.AttackType switch
								{
									AirAttack.Torpedo or AirAttack.TorpedoBombing => 1,
									_ => 0,
								},
								BomberFlag = attackDisplay?.AttackType switch
								{
									AirAttack.Bombing or AirAttack.TorpedoBombing => 1,
									_ => 0,
								},
								HitType = attackDisplay?.HitType switch
								{
									HitType.Critical => 1,
									_ => 0,
								},
								Damage = attackDisplay?.Damage ?? 0,
								Protected = attackDisplay?.GuardsFlagship switch
								{
									true => 1,
									_ => 0,
								},
								Defender = MakeShip(ship, defenderIndex, Math.Max(0, ship.HPCurrent), null),
								ArmorBreak = initial.ApiXal01,
							});
						}
					}

					ProcessData(fleets.EnemyFleet!, fleets.Fleet, FleetFlag.Enemy, 0);

					if (fleets.EnemyEscortFleet is not null)
					{
						ProcessData(fleets.EnemyEscortFleet, fleets.Fleet, FleetFlag.Enemy, 6);
					}
				}
			}
		}

		return airBattleData;
	}

	public async Task<List<AirBaseAirDefenseExportModel>> AirBaseAirDefense(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		return await ProcessData(AirBaseAirDefense, sorties, exportFilter, exportProgress, cancellationToken);
	}

	private static List<AirBaseAirDefenseExportModel> AirBaseAirDefense(
		int? admiralLevel,
		SortieDetailViewModel sortieDetail,
		ExportFilterViewModel? exportFilter)
	{
		List<AirBaseAirDefenseExportModel> airBattleData = [];

		foreach (SortieNode node in sortieDetail.Nodes.Where(n => exportFilter?.MatchesFilter(n) ?? true))
		{
			if (node.AirBaseRaid is null) continue;

			List<PhaseBase> phases = node.AirBaseRaid.Phases.ToList();

			PhaseInitial? initial = phases.OfType<PhaseInitial>().FirstOrDefault();
			PhaseSearching? searching = phases.OfType<PhaseSearching>().FirstOrDefault();
			List<IBaseAirCorpsData>? airBases = initial?.FleetsAfterPhase?.AirBases;

			if (initial is null) continue;
			if (searching is null) continue;
			if (airBases is null) continue;

			foreach (PhaseBaseAirRaid airBaseRaid in phases.OfType<PhaseBaseAirRaid>())
			{
				BattleFleets? fleets = airBaseRaid.FleetsBeforePhase;

				if (fleets is null) continue;

				airBattleData.Add(new()
				{
					Date = sortieDetail.StartTime!.Value.ToLocalTime(),
					World = KCDatabase.Instance.MapInfo[sortieDetail.World * 10 + sortieDetail.Map]?.NameEN ?? "",
					Square = AirDefenseSquareString(sortieDetail, node),
					PlayerFormation = Constants.GetFormation(searching.PlayerFormationType),
					EnemyFormation = Constants.GetFormation(searching.EnemyFormationType),
					Engagement = Constants.GetEngagementForm(searching.EngagementType),
					AirBaseDamage = GetAirBaseDamage(airBaseRaid.ApiLostKind),
					PlayerAircraft = airBaseRaid.Stage1FCount,
					PlayerAircraftLost = airBaseRaid.Stage1FLostcount,
					EnemyAircraft = airBaseRaid.Stage1ECount,
					EnemyAircraftLost = airBaseRaid.Stage1ELostcount,
					AirState = GetAirState(airBaseRaid),
					PlayerContact = airBaseRaid.TouchAircraftFriend ?? "なし",
					EnemyContact = airBaseRaid.TouchAircraftEnemy ?? "なし",
					AirBase1 = MakeAirBase(fleets.AirBases.Skip(0).FirstOrDefault()),
					AirBase2 = MakeAirBase(fleets.AirBases.Skip(1).FirstOrDefault()),
					AirBase3 = MakeAirBase(fleets.AirBases.Skip(2).FirstOrDefault()),
					EnemyShip1 = MakeAirDefenseShip(fleets.EnemyFleet!.MembersInstance.Skip(0).FirstOrDefault()),
					EnemyShip2 = MakeAirDefenseShip(fleets.EnemyFleet!.MembersInstance.Skip(1).FirstOrDefault()),
					EnemyShip3 = MakeAirDefenseShip(fleets.EnemyFleet!.MembersInstance.Skip(2).FirstOrDefault()),
					EnemyShip4 = MakeAirDefenseShip(fleets.EnemyFleet!.MembersInstance.Skip(3).FirstOrDefault()),
					EnemyShip5 = MakeAirDefenseShip(fleets.EnemyFleet!.MembersInstance.Skip(4).FirstOrDefault()),
					EnemyShip6 = MakeAirDefenseShip(fleets.EnemyFleet!.MembersInstance.Skip(5).FirstOrDefault()),

					PlayerTorpedoFlags1 = airBaseRaid.PlayerTorpedoFlags.Skip(0).FirstOrDefault(),
					PlayerTorpedoFlags2 = airBaseRaid.PlayerTorpedoFlags.Skip(1).FirstOrDefault(),
					PlayerTorpedoFlags3 = airBaseRaid.PlayerTorpedoFlags.Skip(2).FirstOrDefault(),

					EnemyTorpedoFlags1 = airBaseRaid.EnemyTorpedoFlags.Skip(0).FirstOrDefault(),
					EnemyTorpedoFlags2 = airBaseRaid.EnemyTorpedoFlags.Skip(1).FirstOrDefault(),
					EnemyTorpedoFlags3 = airBaseRaid.EnemyTorpedoFlags.Skip(2).FirstOrDefault(),
					EnemyTorpedoFlags4 = airBaseRaid.EnemyTorpedoFlags.Skip(3).FirstOrDefault(),
					EnemyTorpedoFlags5 = airBaseRaid.EnemyTorpedoFlags.Skip(4).FirstOrDefault(),
					EnemyTorpedoFlags6 = airBaseRaid.EnemyTorpedoFlags.Skip(5).FirstOrDefault(),

					PlayerBomberFlags1 = airBaseRaid.PlayerBomberFlags.Skip(0).FirstOrDefault(),
					PlayerBomberFlags2 = airBaseRaid.PlayerBomberFlags.Skip(1).FirstOrDefault(),
					PlayerBomberFlags3 = airBaseRaid.PlayerBomberFlags.Skip(2).FirstOrDefault(),

					EnemyBomberFlags1 = airBaseRaid.EnemyBomberFlags.Skip(0).FirstOrDefault(),
					EnemyBomberFlags2 = airBaseRaid.EnemyBomberFlags.Skip(1).FirstOrDefault(),
					EnemyBomberFlags3 = airBaseRaid.EnemyBomberFlags.Skip(2).FirstOrDefault(),
					EnemyBomberFlags4 = airBaseRaid.EnemyBomberFlags.Skip(3).FirstOrDefault(),
					EnemyBomberFlags5 = airBaseRaid.EnemyBomberFlags.Skip(4).FirstOrDefault(),
					EnemyBomberFlags6 = airBaseRaid.EnemyBomberFlags.Skip(5).FirstOrDefault(),

					PlayerHitFlags1 = (int)airBaseRaid.PlayerHitFlags.Skip(0).FirstOrDefault(),
					PlayerHitFlags2 = (int)airBaseRaid.PlayerHitFlags.Skip(1).FirstOrDefault(),
					PlayerHitFlags3 = (int)airBaseRaid.PlayerHitFlags.Skip(2).FirstOrDefault(),

					EnemyHitFlags1 = (int)airBaseRaid.EnemyHitFlags.Skip(0).FirstOrDefault(),
					EnemyHitFlags2 = (int)airBaseRaid.EnemyHitFlags.Skip(1).FirstOrDefault(),
					EnemyHitFlags3 = (int)airBaseRaid.EnemyHitFlags.Skip(2).FirstOrDefault(),
					EnemyHitFlags4 = (int)airBaseRaid.EnemyHitFlags.Skip(3).FirstOrDefault(),
					EnemyHitFlags5 = (int)airBaseRaid.EnemyHitFlags.Skip(4).FirstOrDefault(),
					EnemyHitFlags6 = (int)airBaseRaid.EnemyHitFlags.Skip(5).FirstOrDefault(),

					PlayerDamage1 = airBaseRaid.PlayerDamage.Skip(0).FirstOrDefault(),
					PlayerDamage2 = airBaseRaid.PlayerDamage.Skip(1).FirstOrDefault(),
					PlayerDamage3 = airBaseRaid.PlayerDamage.Skip(2).FirstOrDefault(),

					EnemyDamage1 = airBaseRaid.EnemyDamage.Skip(0).FirstOrDefault(),
					EnemyDamage2 = airBaseRaid.EnemyDamage.Skip(1).FirstOrDefault(),
					EnemyDamage3 = airBaseRaid.EnemyDamage.Skip(2).FirstOrDefault(),
					EnemyDamage4 = airBaseRaid.EnemyDamage.Skip(3).FirstOrDefault(),
					EnemyDamage5 = airBaseRaid.EnemyDamage.Skip(4).FirstOrDefault(),
					EnemyDamage6 = airBaseRaid.EnemyDamage.Skip(5).FirstOrDefault(),
				});
			}
		}

		return airBattleData;
	}

	[SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "I don't see a better way currently")]
	private static AirBattleExportModel MakeAirBattleExport(BattleNode node,
		SortieDetailViewModel sortieDetail, int? admiralLevel, PhaseAirBattle airBattle, PhaseInitial initial,
		PhaseSearching searching, IFleetData attackerFleet, AirBattleAttackViewModel? attackDisplay,
		IShipData defender, BattleIndex defenderIndex, int defenderHpBeforeAttack, BattleFleets fleets)
		=> new()
		{
			CommonData = MakeCommonData(node, IsFirstNode(sortieDetail.Nodes, node), sortieDetail, admiralLevel, airBattle, searching),
			Stage1 = new()
			{
				PlayerAircraftTotal = airBattle.Stage1FCount,
				PlayerAircraftLost = airBattle.Stage1FLostcount,
				EnemyAircraftTotal = airBattle.Stage1ECount,
				EnemyAircraftLost = airBattle.Stage1ELostcount,
			},
			Stage2 = new()
			{
				PlayerAircraftTotal = airBattle.Stage2FCount,
				PlayerAircraftLost = airBattle.Stage2FLostcount,
				EnemyAircraftTotal = airBattle.Stage2ECount,
				EnemyAircraftLost = airBattle.Stage2ELostcount,
			},
			AntiAirCutIn = new()
			{
				Ship = airBattle.ApiAirFire?.ApiIdx,
				Id = airBattle.ApiAirFire?.ApiKind,
				DisplayedEquipment1 = EquipmentFromId(airBattle.ApiAirFire?.ApiUseItems.Skip(0).FirstOrDefault())?.NameEN,
				DisplayedEquipment2 = EquipmentFromId(airBattle.ApiAirFire?.ApiUseItems.Skip(1).FirstOrDefault())?.NameEN,
				DisplayedEquipment3 = EquipmentFromId(airBattle.ApiAirFire?.ApiUseItems.Skip(2).FirstOrDefault())?.NameEN,
			},
			Attacker1 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(0).FirstOrDefault()),
			Attacker2 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(1).FirstOrDefault()),
			Attacker3 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(2).FirstOrDefault()),
			Attacker4 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(3).FirstOrDefault()),
			Attacker5 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(4).FirstOrDefault()),
			Attacker6 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(5).FirstOrDefault()),
			Attacker7 = MakeShip(attackerFleet.MembersWithoutEscaped?.Skip(6).FirstOrDefault()),
			TorpedoFlag = attackDisplay?.AttackType switch
			{
				AirAttack.Torpedo or AirAttack.TorpedoBombing => 1,
				_ => 0,
			},
			BomberFlag = attackDisplay?.AttackType switch
			{
				AirAttack.Bombing or AirAttack.TorpedoBombing => 1,
				_ => 0,
			},
			HitType = attackDisplay?.HitType switch
			{
				HitType.Critical => 1,
				_ => 0,
			},
			Damage = attackDisplay?.Damage ?? 0,
			Protected = attackDisplay?.GuardsFlagship switch
			{
				true => 1,
				_ => 0,
			},
			Defender = MakeShip(attackDisplay?.Defender ?? defender, attackDisplay?.DefenderIndex ?? defenderIndex, attackDisplay?.DefenderHpBeforeAttack ?? defenderHpBeforeAttack, null),
			SmokeType = searching.SmokeCount,
			Balloon = MakeBalloonModel(fleets, searching),
			ArmorBreak = initial.ApiXal01,
		};

	public async Task<List<BattleRanksExportModel>> BattleRank(
		ObservableCollection<SortieRecordViewModel> sorties,
		ExportFilterViewModel? exportFilter,
		ExportProgressViewModel exportProgress,
		CancellationToken cancellationToken = default)
	{
		return await ProcessData(BattleRank, sorties, exportFilter, exportProgress, cancellationToken);
	}

	private static List<BattleRanksExportModel> BattleRank(
		int? admiralLevel,
		SortieDetailViewModel sortieDetail,
		ExportFilterViewModel? exportFilter)
	{
		List<BattleRanksExportModel> rankData = [];

		foreach (BattleNode node in sortieDetail.Nodes.Where(n => exportFilter?.MatchesFilter(n) ?? true).OfType<BattleNode>())
		{
			if (node.FirstBattle.FleetsBeforeBattle.EnemyFleet is null) continue;
			if (node.LastBattle.FleetsAfterBattle.EnemyFleet is null) continue;

			BattleRankPrediction prediction = node.FirstBattle switch
			{
				BattleAirRaid or BattleCombinedAirRaid or BattleNormalRadar or BattleCombinedRadar => new AirRaidBattleRankPrediction()
				{
					FriendlyMainFleetBefore = node.FirstBattle.FleetsBeforeBattle.Fleet,
					FriendlyMainFleetAfter = node.LastBattle.FleetsAfterBattle.Fleet,

					FriendlyEscortFleetBefore = node.FirstBattle.FleetsBeforeBattle.EscortFleet,
					FriendlyEscortFleetAfter = node.LastBattle.FleetsAfterBattle.EscortFleet,

					EnemyMainFleetBefore = node.FirstBattle.FleetsBeforeBattle.EnemyFleet,
					EnemyMainFleetAfter = node.LastBattle.FleetsAfterBattle.EnemyFleet,

					EnemyEscortFleetBefore = node.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
					EnemyEscortFleetAfter = node.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
				},
				BattleBaseAirRaid => new BaseAirRaidBattleRankPrediction()
				{
					AirBaseBeforeAfter = node.AirBaseRaid?.AirBaseBeforeAfter ?? [],

					EnemyMainFleetBefore = node.FirstBattle.FleetsBeforeBattle.EnemyFleet,
					EnemyMainFleetAfter = node.LastBattle.FleetsAfterBattle.EnemyFleet,

					EnemyEscortFleetBefore = node.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
					EnemyEscortFleetAfter = node.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
				},
				_ => new NormalBattleRankPrediction()
				{
					FriendlyMainFleetBefore = node.FirstBattle.FleetsBeforeBattle.Fleet,
					FriendlyMainFleetAfter = node.LastBattle.FleetsAfterBattle.Fleet,

					FriendlyEscortFleetBefore = node.FirstBattle.FleetsBeforeBattle.EscortFleet,
					FriendlyEscortFleetAfter = node.LastBattle.FleetsAfterBattle.EscortFleet,

					EnemyMainFleetBefore = node.FirstBattle.FleetsBeforeBattle.EnemyFleet,
					EnemyMainFleetAfter = node.LastBattle.FleetsAfterBattle.EnemyFleet,

					EnemyEscortFleetBefore = node.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet,
					EnemyEscortFleetAfter = node.LastBattle.FleetsAfterBattle.EnemyEscortFleet,
				},
			};

			rankData.Add(new()
			{
				Date = sortieDetail.StartTime!.Value.ToLocalTime(),
				World = KCDatabase.Instance.MapInfo[sortieDetail.World * 10 + sortieDetail.Map]?.NameEN ?? "",
				Square = SquareString(sortieDetail, node),
				ActualRank = node.RealWinRank ?? "???",
				ExpectedRank = Constants.GetWinRank((int)prediction.PredictRank()),
			});
		}

		return rankData;
	}

	/// <summary>
	/// Makes data that's common between all csv exports.
	/// This could potentially be split into metadata and battle metadata.
	/// </summary>
	/// <param name="node">Battle node data.</param>
	/// <param name="isFirstNode">First node flag.</param>
	/// <param name="sortieDetail">Sortie detail data.</param>
	/// <param name="admiralLevel">Admiral level.</param>
	/// <param name="contactPhase">AirBattle for day battles, NightInitial for night battles.</param>
	/// <param name="searching">Searching phase.</param>
	/// <returns>Common data model.</returns>
	private static CommonDataExportModel MakeCommonData(BattleNode node, bool isFirstNode,
		SortieDetailViewModel sortieDetail, int? admiralLevel, PhaseBase contactPhase,
		PhaseSearching searching) => new()
		{
			// it's not possible to get the correct record index here
			// because the data processing is running in parallel
			No = 0,
			Date = sortieDetail.StartTime!.Value.ToLocalTime(),
			World = KCDatabase.Instance.MapInfo[sortieDetail.World * 10 + sortieDetail.Map]?.NameEN ?? "",
			Square = SquareString(sortieDetail, node),
			Sortie = (node.IsBoss, isFirstNode) switch
			{
				(true, true) => $"{CsvExportResources.Sortie}&{CsvExportResources.BossNode}",
				(_, true) => CsvExportResources.Sortie,
				(true, _) => CsvExportResources.BossNode,
				_ => "",
			},
			Rank = WinRank(node.BattleResult?.WinRank),
			EnemyFleet = node.BattleResult?.EnemyFleetName,
			AdmiralLevel = admiralLevel,
			PlayerFormation = Constants.GetFormation(searching.PlayerFormationType),
			EnemyFormation = Constants.GetFormation(searching.EnemyFormationType),
			PlayerSearch = contactPhase switch
			{
				// day search result should be ignored for night battles
				PhaseNightInitial => null,
				_ => GetSearchingResult(searching.PlayerDetectionType),
			},
			EnemySearch = contactPhase switch
			{
				PhaseNightInitial => null,
				_ => GetSearchingResult(searching.EnemyDetectionType),
			},
			AirState = GetAirState(contactPhase),
			Engagement = Constants.GetEngagementForm(searching.EngagementType),
			PlayerContact = contactPhase switch
			{
				PhaseAirBattle airBattle => airBattle.TouchAircraftFriend,
				PhaseNightInitial initial => initial.TouchAircraftFriend?.NameEN,
				_ => null,
			},
			EnemyContact = contactPhase switch
			{
				PhaseAirBattle airBattle => airBattle.TouchAircraftEnemy,
				PhaseNightInitial initial => initial.TouchAircraftEnemy?.NameEN,
				_ => null,
			},
			PlayerFlare = contactPhase switch
			{
				PhaseNightInitial initial => FlareIndex(initial.FlareIndexFriend),
				_ => null,
			},
			EnemyFlare = contactPhase switch
			{
				PhaseNightInitial initial => FlareIndex(initial.FlareIndexEnemy),
				_ => null,
			},
		};

	private static ShipExportModel MakeShip(IShipData ship, BattleIndex index, int hpBeforeAttack,
		IShipData? shipAfterBattle) => new()
		{
			Index = index.Index + 1,
			Id = ship.ShipID,
			Name = ship.Name,
			ShipType = ship.MasterShip.ShipType switch
			{
				ShipTypes.Transport => "補給艦",
				ShipTypes type => type.Display(),
			},
			Condition = NullForAbyssals(ship.Condition, ship),
			ConditionAfterBattle = NullForAbyssals(shipAfterBattle?.Condition, shipAfterBattle),
			HpCurrent = hpBeforeAttack,
			HpMax = ship.HPMax,
			DamageState = GetDamageState(hpBeforeAttack, ship.HPMax),
			FuelAfterBattle = NullForAbyssals(shipAfterBattle?.Fuel, shipAfterBattle),
			FuelCurrent = NullForAbyssals(ship.Fuel, ship),
			FuelMax = ship.FuelMax,
			AmmoAfterBattle = NullForAbyssals(shipAfterBattle?.Ammo, shipAfterBattle),
			AmmoCurrent = NullForAbyssals(ship.Ammo, ship),
			AmmoMax = ship.AmmoMax,
			Level = ship.Level,
			Speed = Constants.GetSpeed(ship.Speed),
			Firepower = ship.FirepowerTotal,
			Torpedo = ship.TorpedoTotal,
			AntiAir = ship.AATotal,
			Armor = ship.ArmorTotal,
			Evasion = ship.EvasionTotal,
			AntiSubmarine = ship.ASWTotal,
			Search = ship.LOSTotal,
			Luck = ship.LuckTotal,
			Range = Constants.GetRange(ship.Range),
			Equipment1 = MakeEquipment(ship, 0, shipAfterBattle),
			Equipment2 = MakeEquipment(ship, 1, shipAfterBattle),
			Equipment3 = MakeEquipment(ship, 2, shipAfterBattle),
			Equipment4 = MakeEquipment(ship, 3, shipAfterBattle),
			Equipment5 = MakeEquipment(ship, 4, shipAfterBattle),
			Equipment6 = MakeEquipment(ship, 5, shipAfterBattle),
		};

	private static AirBattleShipExportModel MakeShip(IShipData? ship) => new()
	{
		Id = ship?.ShipID,
		Name = ship?.Name,
		Level = ship?.Level,
	};

	private static EquipmentExportModel MakeEquipment(IShipData ship, int index,
		IShipData? shipAfterBattle) => new()
		{
			Name = ship.AllSlotInstance.Skip(index).FirstOrDefault()?.Name,
			Level = NullForAbyssals(ship.AllSlotInstance.Skip(index).FirstOrDefault()?.Level, ship),
			AircraftLevel = NullForAbyssals(ship.AllSlotInstance.Skip(index).FirstOrDefault()?.AircraftLevel, ship),
			Aircraft = ship.Aircraft.Take(ship.SlotSize).Skip(index).Cast<int?>().FirstOrDefault(),
			AircraftAfterBattle = NullForAbyssals(shipAfterBattle?.Aircraft.Take(ship.SlotSize).Skip(index).Cast<int?>().FirstOrDefault(), shipAfterBattle),
		};

	private static AirBaseExportModel MakeAirBase(IBaseAirCorpsData? ab) => new()
	{
		Hp = ab switch
		{
			null => null,
			_ => $"{ab.HPCurrent}/{ab.HPMax}",
		},
		Squadron1 = MakeAirBaseSquadron(AirDefenseSquadron(ab?.ActionKind, ab?.Squadrons.Values.Skip(0).FirstOrDefault())),
		Squadron2 = MakeAirBaseSquadron(AirDefenseSquadron(ab?.ActionKind, ab?.Squadrons.Values.Skip(1).FirstOrDefault())),
		Squadron3 = MakeAirBaseSquadron(AirDefenseSquadron(ab?.ActionKind, ab?.Squadrons.Values.Skip(2).FirstOrDefault())),
		Squadron4 = MakeAirBaseSquadron(AirDefenseSquadron(ab?.ActionKind, ab?.Squadrons.Values.Skip(3).FirstOrDefault())),
	};

	private static IBaseAirCorpsSquadron? AirDefenseSquadron(AirBaseActionKind? actionKind, IBaseAirCorpsSquadron? squadron) =>
		actionKind switch
		{
			AirBaseActionKind.AirDefense => squadron,
			_ => null,
		};

	private static AirBaseSquadronExportModel MakeAirBaseSquadron(IBaseAirCorpsSquadron? squadron) => new()
	{
		Name = squadron?.EquipmentInstance?.Name,
		Level = squadron?.EquipmentInstance?.Level,
		AircraftLevel = squadron?.EquipmentInstance?.AircraftLevel,
		Condition = AirBaseConditionDisplay(squadron?.Condition),
		Aircraft = squadron?.AircraftCurrent,
	};

	private static AirBaseAirDefenseShipExportModel MakeAirDefenseShip(IShipData? ship) => new()
	{
		Id = ship?.ShipID,
		Name = ship?.Name,
		Level = ship?.Level,
		Hp = ship switch
		{
			null => null,
			_ => $"{ship.HPCurrent}/{ship.HPMax}",
		},
		Equipment1Name = ship?.AllSlotInstance.Skip(0).FirstOrDefault()?.Name,
		Equipment2Name = ship?.AllSlotInstance.Skip(1).FirstOrDefault()?.Name,
		Equipment3Name = ship?.AllSlotInstance.Skip(2).FirstOrDefault()?.Name,
		Equipment4Name = ship?.AllSlotInstance.Skip(3).FirstOrDefault()?.Name,
		Equipment5Name = ship?.AllSlotInstance.Skip(4).FirstOrDefault()?.Name,
	};

	private static bool IsFirstNode(IEnumerable<SortieNode> nodes, SortieNode node)
		=> nodes.OfType<BattleNode>().FirstOrDefault() == node;

	private static string SquareString(SortieDetailViewModel sortieDetail, SortieNode node) =>
		$"{CsvExportResources.Map}:{sortieDetail.World}-{sortieDetail.Map} {CsvExportResources.Cell}:{node.Cell}";

	private static string WinRank(string? rank) => rank switch
	{
		"SS" => "完全勝利!!S",
		"S" => "勝利S",
		"A" => "勝利A",
		"B" => "戦術的勝利B",
		"C" => "戦術的敗北C",
		"D" => "敗北D",
		"E" => "敗北E",

		_ => "",
	};

	private static string? GetSearchingResult(DetectionType id) => id switch
	{
		DetectionType.Success => "発見!",
		DetectionType.SuccessNoReturn => "発見!索敵機未帰還機あり",
		DetectionType.NoReturn => "発見できず…索敵機未帰還機あり",
		DetectionType.Failure => "発見できず…",
		DetectionType.SuccessNoPlane => "発見!(索敵機なし)",
		DetectionType.FailureNoPlane => "なし",
		DetectionType.Unknown => null,
		_ => $"不明({id})",
	};

	private static string? GetAirState(PhaseBase contactPhase)
	{
		AirState airState = contactPhase switch
		{
			PhaseAirBattleBase airBattle => airBattle.AirState,
			PhaseBaseAirRaid airRaid => airRaid.AirState,
			_ => AirState.Unknown,
		};

		return airState switch
		{
			AirState.Parity => "航空互角",
			AirState.Unknown => null,
			_ => Constants.GetAirSuperiority(airState),
		};
	}

	private static int? FlareIndex(int index) => index switch
	{
		-1 => null,
		_ => index + 1,
	};

	private static int? SearchlightIndex(int index) => index switch
	{
		-1 => null,
		_ => index + 1,
	};

	private static string GetPlayerFleet(BattleFleets fleets, BattleIndex attackerIndex, BattleIndex defenderIndex)
	{
		BattleIndex index = attackerIndex.FleetFlag switch
		{
			FleetFlag.Player => attackerIndex,
			_ => defenderIndex,
		};

		IFleetData? fleet = fleets.GetFleet(index);

		if (fleet is null) throw new NotImplementedException();

		if (fleet == fleets.EscortFleet) return "連合第2艦隊";
		if (fleets.EscortFleet is null) return "通常艦隊";
		if (fleet == fleets.Fleet) return "連合第1艦隊";

		throw new NotImplementedException();
	}

	private static int CsvDayAttackKind(DayAttackKind attack) => attack switch
	{
		< DayAttackKind.Shelling => (int)attack,
		_ => 0,
	};

	private static int CsvNightAttackKind(NightAttackKind attack) => attack switch
	{
		< NightAttackKind.Shelling => (int)attack,
		_ => 0,
	};

	private static string GetDamageState(int hpCurrent, int hpMax) => ((double)hpCurrent / hpMax) switch
	{
		<= 0 => "轟沈",
		> 0.75 => "小破未満",

		double hpRate => Constants.GetDamageState(hpRate),
	};

	private static string GetPhaseString(PhaseBase phase) => phase switch
	{
		PhaseShelling { DayShellingPhase: DayShellingPhase.First } => "1",
		PhaseShelling { DayShellingPhase: DayShellingPhase.Second } => "2",
		PhaseShelling { DayShellingPhase: DayShellingPhase.Third } => "3",

		_ => phase.Title,
	};

	private static string GetEnemyFleetType(bool isCombined) => isCombined switch
	{
		true => CsvExportResources.CombinedFleet,
		_ => ConstantsRes.NormalFleet,
	};

	private static SortieItemsExportModel MakeSortieItems(BattleNode node, PhaseInitial initial,
		PhaseSearching searching, BattleFleets fleets, ApiOffshoreSupply? apiOffshoreSupply) => new()
		{
			SmokerFlag = node.Request?.ApiSmokeFlag,
			SmokerType = searching.SmokeCount ?? 0,
			SupplyShip = fleets.GetShipByDropId(apiOffshoreSupply?.ApiSupplyShip)?.Name,
			GivenShip = fleets.GetShipByDropId(apiOffshoreSupply?.ApiGivenShip)?.Name,
			UseNum = apiOffshoreSupply?.ApiUseNum,
			ApiCombatRation = initial.ApiCombatRation,
			ApiCombatRationCombined = initial.ApiCombatRationCombined,
		};

	private static BalloonExportModel MakeBalloonModel(BattleFleets fleets, PhaseSearching searching) => new()
	{
		BalloonCell = searching.BalloonCell,
		BalloonCount = BalloonCount(fleets.SortieShips()),
		EnemyBalloonCount = BalloonCount(fleets.EnemyShips()),
	};

	private static int BalloonCount(List<IShipData?> ships) => ships
		.OfType<IShipData>()
		.SelectMany(s => s.AllSlotInstance)
		.OfType<IEquipmentData>()
		.Count(e => e.MasterEquipment.IconTypeTyped is EquipmentIconType.BarrageBalloon);

	private static int? NullForAbyssals(int? value, IShipData? ship) => ship?.MasterShip.IsAbyssalShip switch
	{
		true => null,
		_ => value,
	};

	private static string GetStartingBattle(BattleNode node) => node.FirstBattle switch
	{
		NightBattleData => "夜戦開始",
		_ => "昼戦開始",
	};

	private static IEquipmentDataMaster? EquipmentFromId(int? id) => id switch
	{
		int i => KCDatabase.Instance.MasterEquipments[i],
		_ => null,
	};

	private static string AirDefenseSquareString(SortieDetailViewModel sortieDetail, SortieNode node) =>
		$"{CsvExportResources.Map}:{sortieDetail.World}-{sortieDetail.Map} {CsvExportResources.Cell}:{node.Cell} ({GetEventKind(node.ApiEventId, node.ApiEventKind)})";

	private static string GetEventKind(int eventId, int eventKind) => eventId switch
	{
		0 => "初期地点",
		// case 1: return "不明(1)";
		2 => "資源獲得",
		3 => "渦潮",
		4 => "戦闘",
		5 => "ボス",
		6 => "気のせい",
		7 => eventKind switch
		{
			0 => "航空偵察",
			4 => "航空戦",
			_ => $"不明(7/{eventKind})",
		},
		8 => "船団護衛成功",
		9 => "揚陸地点",
		10 => "長距離空襲戦",
		_ => $"不明({eventId})",
	};

	private static string GetAirBaseDamage(int kind) => kind switch
	{
		1 => "資源損害",
		2 => "資源・航空",
		3 => "航空隊損害",
		4 => "損害なし",
		_ => $"不明({kind})",
	};

	private static string? AirBaseConditionDisplay(AirBaseCondition? condition) => condition switch
	{
		null => null,
		AirBaseCondition.Normal => "通常",
		AirBaseCondition.Tired => "橙疲労",
		AirBaseCondition.VeryTired => "赤疲労",
		AirBaseCondition cond => $"不明({(int)cond})",
	};
}
