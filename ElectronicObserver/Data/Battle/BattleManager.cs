using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcNightToDay;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdShooting;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.NightToDay;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.BattleDetail;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Data.Battle;

/// <summary>
/// 戦闘関連の処理を統括して扱います。
/// </summary>
public class BattleManager : APIWrapper
{
	public static string BattleLogPath => "BattleLog";

	/// <summary>
	/// Ship will have the level before level up
	/// </summary>
	/// <param name="ship"></param>
	/// <param name="nextLevel"></param>
	public delegate void ShipLevelUpHandler(IShipData ship, int nextLevel);

	public event ShipLevelUpHandler? ShipLevelUp;

	/// <summary>
	/// 羅針盤データ
	/// </summary>
	public CompassData? Compass { get; private set; }

	/// <summary>
	/// 昼戦データ
	/// </summary>
	public FirstBattleData? FirstBattle { get; private set; }

	/// <summary>
	/// 夜戦データ
	/// </summary>
	public BattleData? SecondBattle { get; private set; }

	/// <summary>
	/// 戦闘結果データ
	/// </summary>
	public BattleResult? Result { get; private set; }

	/// <summary>
	/// The battle result api doesn't report SS, so we need to evaluate it manually.
	/// </summary>
	public string? PredictedBattleRank { get; set; }

	// In the api, heavy base air raid is implemented as 3 different air raid battles
	// If we decide to collapse it down into 1 battle, this should be deleted
	// and heavy base air raid moved to BattleDay like regular BattleBaseAirRaid
	public List<BattleBaseAirRaid> HeavyBaseAirRaids { get; } = [];

	/// <summary>
	/// 連合艦隊戦かどうか
	/// </summary>
	public bool IsCombinedBattle => FirstBattle is { IsCombined: true };

	/// <summary>
	/// 演習かどうか
	/// </summary>
	public bool IsPractice => FirstBattle is { IsPractice: true };

	/// <summary>
	/// 敵が連合艦隊かどうか
	/// </summary>
	public bool IsEnemyCombined => FirstBattle is { IsEnemyCombined: true };

	/// <summary>
	/// 基地空襲戦かどうか
	/// </summary>
	public bool IsBaseAirRaid => FirstBattle is { IsBaseAirRaid: true };

	public bool IsAirRaid => FirstBattle is { IsAirRaid: true };

	/// <summary>
	/// 出撃中に入手した艦船数
	/// </summary>
	public int DroppedShipCount { get; private set; }

	/// <summary>
	/// 出撃中に入手した装備数
	/// </summary>
	public int DroppedEquipmentCount { get; private set; }

	/// <summary>
	/// 出撃中に入手したアイテム - ID と 個数 のペア
	/// </summary>
	private Dictionary<int, int> DroppedItemCount { get; } = [];


	/// <summary>
	/// 演習の敵提督名
	/// </summary>
	private string? EnemyAdmiralName { get; set; }

	/// <summary>
	/// 演習の敵提督階級
	/// </summary>
	private string? EnemyAdmiralRank { get; set; }

	/// <summary>
	/// True if Resupply was used before the battle
	/// </summary>
	public bool ResupplyUsed { get; private set; }

	/// <summary>
	/// True if ration was used before the battle
	/// </summary>
	public bool RationUsed { get; private set; }


	/// <summary>
	/// 特殊攻撃発動回数
	/// </summary>
	public Dictionary<int, int> SpecialAttackCount { get; } = [];

	/// <summary>
	/// 記録する特殊攻撃
	/// </summary>
	private List<int> TracedSpecialAttack { get; } = [100, 101, 102, 103, 104, 105, 300, 301, 302, 400, 401];

	public override void LoadFromRequest(string apiname, Dictionary<string, string> data)
	{
		switch (apiname)
		{
			case "api_req_sortie/battle":
				ResupplyUsed = data.ContainsKey("api_supply_flag") && data["api_supply_flag"] == "1";
				RationUsed = data.ContainsKey("api_ration_flag") && data["api_ration_flag"] == "1";
				break;
		}

	}

	private BattleFactory BattleFactory => Ioc.Default.GetRequiredService<BattleFactory>();

	private BattleFleets Fleets
	{
		get
		{
			IFleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values
				.First(f => f.IsInSortie);

			return new(fleet)
			{
				FleetId = fleet.FleetID,
				CombinedFlag = KCDatabase.Instance.Fleet.CombinedFlag,
				NodeSupportFleetId = KCDatabase.Instance.Fleet.NodeSupportFleet ?? 0,
				BossSupportFleetId = KCDatabase.Instance.Fleet.BossSupportFleet ?? 0,
			};
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "todo")]
	public override void LoadFromResponse(string apiname, dynamic data)
	{
		object? apiData = DeserializeResponse(apiname, data.ToString());

		HeavyBaseAirRaids.Clear();

		switch (apiname)
		{
			case "api_req_map/start":
			case "api_req_map/next":
			{
				FirstBattle = null;
				SecondBattle = null;
				Result = null;
				Compass = new CompassData();
				Compass.LoadFromResponse(apiname, data);

				if (apiData is ApiReqMapNextResponse { ApiDestructionBattle: { } battle })
				{
					FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
					BattleFinished();
				}
			}
			break;

			case "api_req_map/air_raid":
			{
				/* todo
				// BattleDay = new BattleHeavyBaseAirRaid();
				// BattleDay.LoadFromResponse(apiname, data.api_destruction_battle[0]);
				foreach (dynamic airraid in data.api_destruction_battle)
				{
					BattleBaseAirRaid raid = new();
					raid.LoadFromResponse(apiname, airraid);
					HeavyBaseAirRaids.Add(raid);
				}
				BattleFinished();
				*/
			}
			break;

			case "api_req_sortie/battle":
			{
				if (apiData is not ApiReqSortieBattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_battle_midnight/battle":
			{
				if (apiData is not ApiReqBattleMidnightBattleResponse battle) break;

				Debug.Assert(FirstBattle is not null);

				SecondBattle = BattleFactory.CreateBattle(battle, FirstBattle.FleetsAfterBattle);
			}
			break;

			case "api_req_battle_midnight/sp_midnight":
			{
				if (apiData is not ApiReqBattleMidnightSpMidnightResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_sortie/airbattle":
			{
				if (apiData is not ApiReqSortieAirbattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_sortie/ld_airbattle":
			{
				if (apiData is not ApiReqSortieLdAirbattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_sortie/night_to_day":
			{
				if (apiData is not ApiReqSortieNightToDayResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_sortie/ld_shooting":
			{
				if (apiData is not ApiReqSortieLdShootingResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/battle":
			{
				if (apiData is not ApiReqCombinedBattleBattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/midnight_battle":
			{
				if (apiData is not ApiReqCombinedBattleMidnightBattleResponse battle) break;

				Debug.Assert(FirstBattle is not null);

				SecondBattle = BattleFactory.CreateBattle(battle, FirstBattle.FleetsAfterBattle);
			}
			break;

			case "api_req_combined_battle/sp_midnight":
			{
				if (apiData is not ApiReqCombinedBattleSpMidnightResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/airbattle":
			{
				if (apiData is not ApiReqCombinedBattleAirbattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/battle_water":
			{
				if (apiData is not ApiReqCombinedBattleBattleWaterResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/ld_airbattle":
			{
				if (apiData is not ApiReqCombinedBattleLdAirbattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/ec_battle":
			{
				if (apiData is not ApiReqCombinedBattleEcBattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/ec_midnight_battle":
			{
				if (apiData is not ApiReqCombinedBattleEcMidnightBattleResponse battle) break;

				Debug.Assert(FirstBattle is not null);

				SecondBattle = BattleFactory.CreateBattle(battle, FirstBattle.FleetsAfterBattle);
			}
			break;

			case "api_req_combined_battle/ec_night_to_day":
			{
				if (apiData is not ApiReqCombinedBattleEcNightToDayResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/each_battle":
			{
				if (apiData is not ApiReqCombinedBattleEachBattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/each_battle_water":
			{
				if (apiData is not ApiReqCombinedBattleEachBattleWaterResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_combined_battle/ld_shooting":
			{
				if (apiData is not ApiReqCombinedBattleLdShootingResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_member/get_practice_enemyinfo":
				EnemyAdmiralName = data.api_nickname;
				EnemyAdmiralRank = Constants.GetAdmiralRank((int)data.api_rank);
				break;

			case "api_req_practice/battle":
			{
				if (apiData is not ApiReqPracticeBattleResponse battle) break;

				FirstBattle = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_practice/midnight_battle":
			{
				if (apiData is not ApiReqPracticeMidnightBattleResponse battle) break;

				Debug.Assert(FirstBattle is not null);

				SecondBattle = BattleFactory.CreateBattle(battle, FirstBattle.FleetsAfterBattle);
			}
			break;

			case "api_req_sortie/battleresult":
			case "api_req_combined_battle/battleresult":
			case "api_req_practice/battle_result":
			{
				Result = apiData switch
				{
					ApiReqCombinedBattleBattleresultResponse c => new(c),
					ISortieBattleResultApi s => new(s),
					ApiReqPracticeBattleResultResponse p => new(p),
					_ => null,
				};
				BattleFinished();
				break;
			}

			case "api_port/port":
				Compass = null;
				FirstBattle = null;
				SecondBattle = null;
				Result = null;
				PredictedBattleRank = "";
				DroppedShipCount = DroppedEquipmentCount = 0;
				DroppedItemCount.Clear();
				SpecialAttackCount.Clear();
				break;

			case "api_get_member/slot_item":
				DroppedEquipmentCount = 0;
				break;

		}
	}

	private static object? DeserializeResponse(string apiName, string json) => apiName switch
	{
		"api_req_map/start" => JsonSerializer.Deserialize<ApiReqMapStartResponse>(json),

		"api_req_sortie/battle" => JsonSerializer.Deserialize<ApiReqSortieBattleResponse>(json),
		"api_req_battle_midnight/sp_midnight" => JsonSerializer.Deserialize<ApiReqBattleMidnightSpMidnightResponse>(json),
		"api_req_sortie/airbattle" => JsonSerializer.Deserialize<ApiReqSortieAirbattleResponse>(json),
		"api_req_sortie/ld_airbattle" => JsonSerializer.Deserialize<ApiReqSortieLdAirbattleResponse>(json),
		"api_req_sortie/night_to_day" => throw new NotImplementedException(),
		"api_req_sortie/ld_shooting" => JsonSerializer.Deserialize<ApiReqSortieLdShootingResponse>(json),
		"api_req_combined_battle/battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleResponse>(json),
		"api_req_combined_battle/sp_midnight" => JsonSerializer.Deserialize<ApiReqCombinedBattleSpMidnightResponse>(json),
		"api_req_combined_battle/airbattle" => throw new NotImplementedException(),
		"api_req_combined_battle/battle_water" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleWaterResponse>(json),
		"api_req_combined_battle/ld_airbattle" => JsonSerializer.Deserialize<ApiReqCombinedBattleLdAirbattleResponse>(json),
		"api_req_combined_battle/ec_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEcBattleResponse>(json),
		"api_req_combined_battle/ec_night_to_day" => throw new NotImplementedException(),
		"api_req_combined_battle/each_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEachBattleResponse>(json),
		"api_req_combined_battle/each_battle_water" => JsonSerializer.Deserialize<ApiReqCombinedBattleEachBattleWaterResponse>(json),
		"api_req_combined_battle/ld_shooting" => JsonSerializer.Deserialize<ApiReqCombinedBattleLdShootingResponse>(json),

		"api_req_battle_midnight/battle" => JsonSerializer.Deserialize<ApiReqBattleMidnightBattleResponse>(json),
		"api_req_combined_battle/midnight_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleMidnightBattleResponse>(json),
		"api_req_combined_battle/ec_midnight_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEcMidnightBattleResponse>(json),

		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiReqSortieBattleresultResponse>(json),
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleresultResponse>(json),

		"api_req_practice/battle" => JsonSerializer.Deserialize<ApiReqPracticeBattleResponse>(json),
		"api_req_practice/battle_result" => JsonSerializer.Deserialize<ApiReqPracticeBattleResultResponse>(json),

		_ => null,
	};

	/// <summary>
	/// 戦闘終了時に各種データの収集を行います。
	/// </summary>
	private void BattleFinished()
	{
		//敵編成記録
		EnemyFleetRecord.EnemyFleetElement? enemyFleetData = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();

		if (enemyFleetData != null)
		{
			RecordManager.Instance.EnemyFleet.Update(enemyFleetData);
		}

		// ロギング
		if (IsPractice)
		{
			Debug.Assert(Result is not null);

			Utility.Logger.Add(2,
				string.Format(BattleRes.BattleFinishedPractice,
					EnemyAdmiralName, EnemyAdmiralRank, Result.EnemyFleetName, Result.Rank, Result.AdmiralExp, Result.BaseExp));
		}
		else if (IsBaseAirRaid)
		{
			Debug.Assert(Compass is not null);

			if (FirstBattle is BattleBaseAirRaid raid)
			{
				PhaseBaseAirRaid? airraid = raid.BaseAirRaid;
				List<int> initialHPs = FirstBattle.Initial.FriendInitialHPs.TakeWhile(hp => hp >= 0).ToList();
				int damage = initialHPs.Zip(FirstBattle.ResultHPs.Take(initialHPs.Count()), (initial, result) => initial - result).Sum();

				Utility.Logger.Add(2,
				string.Format(BattleRes.BattleFinishedBaseAirRaid,
					Compass.MapAreaID, Compass.MapInfoID, Compass.CellDisplay,
					Constants.GetAirSuperiority(airraid?.AirState ?? AirState.Unknown), damage, Constants.GetAirRaidDamage(Compass.AirRaidDamageKind)));
			}

			foreach (BattleBaseAirRaid battleBaseAirRaid in HeavyBaseAirRaids)
			{
				List<int> initialHPs = battleBaseAirRaid.Initial.FriendInitialHPs.TakeWhile(hp => hp >= 0).ToList();
				int damage = initialHPs.Zip(battleBaseAirRaid.ResultHPs.Take(initialHPs.Count()), (initial, result) => initial - result).Sum();
				PhaseBaseAirRaid? baseAirRaid = battleBaseAirRaid.BaseAirRaid;

				int airRaidDamageKind = baseAirRaid?.ApiLostKind ?? 0;

				Utility.Logger.Add(2,
					string.Format(BattleRes.BattleFinishedBaseAirRaid,
						Compass.MapAreaID, Compass.MapInfoID, Compass.CellDisplay,
						Constants.GetAirSuperiority(baseAirRaid?.AirState ?? AirState.Unknown), damage, Constants.GetAirRaidDamage(airRaidDamageKind)));
			}
		}
		else
		{
			Debug.Assert(Compass is not null);
			Debug.Assert(Result is not null);

			Utility.Logger.Add(2,
				string.Format(BattleRes.BattleFinishedSortie,
					Compass.MapAreaID, Compass.MapInfoID, Compass.CellDisplay, KCDatabase.Instance.Translation.Operation.FleetName(Result.EnemyFleetName), Result.Rank, Result.AdmiralExp, Result.BaseExp));
		}


		// Level up
		if (!IsBaseAirRaid)
		{
			Debug.Assert(FirstBattle is not null);
			Debug.Assert(Result is not null);

			List<int> exps = Result.ExpList;
			List<List<int>> lvup = Result.LevelUpList;
			for (int i = 0; i < lvup.Count; i++)
			{
				if (lvup[i].Count >= 2 && i < exps.Count && lvup[i][0] + exps[i] >= lvup[i][1])
				{
					IShipData ship = FirstBattle.FleetsBeforeBattle.Fleet.MembersInstance[i]!;
					int increment = Math.Max(lvup[i].Count - 2, 1);

					ShipLevelUp?.Invoke(ship, ship.Level + increment);
					Utility.Logger.Add(2, string.Format(BattleRes.HasLeveledUp, ship.Name, ship.Level + increment));
				}
			}

			if (IsCombinedBattle)
			{
				exps = Result.ExpListCombined!;
				lvup = Result.LevelUpListCombined!;

				for (int i = 0; i < lvup.Count; i++)
				{
					if (lvup[i].Count >= 2 && i < exps.Count && lvup[i][0] + exps[i] >= lvup[i][1])
					{
						IShipData ship = FirstBattle.FleetsBeforeBattle.EscortFleet!.MembersInstance[i]!;
						int increment = Math.Max(lvup[i].Count - 2, 1);

						ShipLevelUp?.Invoke(ship, ship.Level + increment);
						Utility.Logger.Add(2, string.Format(BattleRes.HasLeveledUp, ship.Name, ship.Level + increment));
					}
				}
			}
		}



		//ドロップ艦記録
		if (!IsPractice && !IsBaseAirRaid)
		{
			Debug.Assert(Compass is not null);
			Debug.Assert(Result is not null);

			//checkme: とてもアレな感じ

			int shipId = (int?)Result.DroppedShipId ?? -1;
			int itemId = Result.DroppedItemId ?? -1;
			int eqId = Result.DroppedEquipmentId ?? -1;
			bool showLog = Utility.Configuration.Config.Log.ShowSpoiler;

			if (KCDatabase.Instance.MasterShips[shipId] is IShipDataMaster ship)
			{
				DroppedShipCount++;

				IEnumerable<IEquipmentDataMaster?>? defaultSlot = ship.DefaultSlot?.Select(i => i switch
				{
					< 1 => null,
					_ => KCDatabase.Instance.MasterEquipments[i],
				});

				if (defaultSlot != null)
				{
					DroppedEquipmentCount += defaultSlot
						.Where(e => e is not null)
						.Count(e => e!.UsesSlotSpace());
				}

				if (showLog)
					Utility.Logger.Add(2, string.Format(LoggerRes.ShipAdded, ship.ShipTypeName, ship.NameWithClass));
			}

			if (itemId != -1)
			{
				DroppedItemCount.TryAdd(itemId, 0);
				DroppedItemCount[itemId]++;

				if (showLog)
				{
					IUseItem? item = KCDatabase.Instance.UseItems[itemId];
					IUseItemMaster? itemMaster = KCDatabase.Instance.MasterUseItems[itemId];
					Utility.Logger.Add(2, string.Format(LoggerRes.ItemObtained, itemMaster?.NameTranslated ?? (BattleRes.UnknownItem + itemId), (item?.Count ?? 0) + DroppedItemCount[itemId]));
				}
			}

			if (KCDatabase.Instance.MasterEquipments[eqId] is IEquipmentDataMaster eq)
			{
				if (eq.UsesSlotSpace())
				{
					DroppedEquipmentCount++;
				}

				if (showLog)
				{
					Utility.Logger.Add(2, string.Format(LoggerRes.EquipmentObtained, eq.CategoryTypeInstance.NameEN, eq.NameEN));
				}
			}


			// 満員判定
			if (shipId == -1 && (
				KCDatabase.Instance.Admiral.MaxShipCount - (KCDatabase.Instance.Ships.Count + DroppedShipCount) <= 0 ||
				KCDatabase.Instance.Admiral.MaxEquipmentCount - (KCDatabase.Instance.Equipments.Values.Count(e => e.MasterEquipment.UsesSlotSpace()) + DroppedEquipmentCount) <= 0))
			{
				shipId = -2;
			}

			Debug.Assert(enemyFleetData is not null);

			RecordManager.Instance.ShipDrop.Add(shipId, itemId, eqId, Compass.MapAreaID, Compass.MapInfoID, Compass.CellId, Compass.MapInfo.EventDifficulty, Compass.EventID == 5, enemyFleetData.FleetID, Result.Rank, KCDatabase.Instance.Admiral.Level);
		}


		void IncrementSpecialAttack(BattleData? bd)
		{
			if (bd == null) return;

			foreach (PhaseBase phase in bd.Phases)
			{
				switch (phase)
				{
					case PhaseShelling shelling:
					{
						foreach (PhaseShellingAttackViewModel attack in shelling.AttackDisplays
							.Where(a => a.AttackerIndex.FleetFlag is FleetFlag.Player)
							.Where(a => TracedSpecialAttack.Contains((int)a.AttackType)))
						{
							if (!SpecialAttackCount.TryAdd((int)attack.AttackType, 1))
							{
								SpecialAttackCount[(int)attack.AttackType]++;
							}
						}

						break;
					}

					case PhaseNightBattle night:
					{
						foreach (PhaseNightBattleAttackViewModel attack in night.AttackDisplays
							.Where(a => a.AttackerIndex.FleetFlag is FleetFlag.Player)
							.Where(a => TracedSpecialAttack.Contains((int)a.AttackType)))
						{
							if (!SpecialAttackCount.TryAdd((int)attack.AttackType, 1))
							{
								SpecialAttackCount[(int)attack.AttackType]++;
							}
						}

						break;
					}
				}
			}
		}

		IncrementSpecialAttack(FirstBattle);
		IncrementSpecialAttack(SecondBattle);

		WriteBattleLog();
	}

	/// <summary>
	/// 勝利ランクを予測します。
	/// </summary>
	/// <param name="friendRate">味方の損害率を出力します。</param>
	/// <param name="enemyRate">敵の損害率を出力します。</param>
	public int PredictWinRank(out double friendRate, out double enemyRate)
	{
		BattleData? activeBattle = SecondBattle ?? FirstBattle;

		// should never happen
		if (activeBattle is null)
		{
			friendRate = 0;
			enemyRate = 0;
			return -1;
		}

		BattleFleets fleetsBefore = activeBattle.FleetsBeforeBattle;

		List<int> hpsBefore = activeBattle.InitialHPs.ToList();
		List<int> hpsAfter = activeBattle.ResultHPs.ToList();

		Debug.Assert(fleetsBefore.EnemyFleet is not null);

		BattleRankPrediction prediction = activeBattle switch
		{
			{ IsAirRaid: true } or { IsRadar: true } => new AirRaidBattleRankPrediction()
			{
				FriendlyMainFleetBefore = fleetsBefore.Fleet,
				FriendlyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.Fleet, hpsAfter, BattleSides.FriendMain)!,

				FriendlyEscortFleetBefore = fleetsBefore.EscortFleet,
				FriendlyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EscortFleet, hpsAfter, BattleSides.FriendEscort),

				EnemyMainFleetBefore = fleetsBefore.EnemyFleet,
				EnemyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EnemyFleet, hpsAfter, BattleSides.EnemyMain)!,

				EnemyEscortFleetBefore = fleetsBefore.EnemyEscortFleet,
				EnemyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EnemyEscortFleet, hpsAfter, BattleSides.EnemyEscort),
			},
			{ IsBaseAirRaid: true } => new BaseAirRaidBattleRankPrediction()
			{
				AirBaseBeforeAfter = BaseAirRaidBattleRankPrediction.SimulateBaseAfterBattle(hpsBefore, hpsAfter),

				EnemyMainFleetBefore = fleetsBefore.EnemyFleet,
				EnemyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EnemyFleet, hpsAfter, BattleSides.EnemyMain)!,

				EnemyEscortFleetBefore = fleetsBefore.EnemyEscortFleet,
				EnemyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EnemyEscortFleet, hpsAfter, BattleSides.EnemyEscort),
			},
			_ => new NormalBattleRankPrediction()
			{
				FriendlyMainFleetBefore = fleetsBefore.Fleet,
				FriendlyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.Fleet, hpsAfter, BattleSides.FriendMain)!,

				FriendlyEscortFleetBefore = fleetsBefore.EscortFleet,
				FriendlyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EscortFleet, hpsAfter, BattleSides.FriendEscort),

				EnemyMainFleetBefore = fleetsBefore.EnemyFleet,
				EnemyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EnemyFleet, hpsAfter, BattleSides.EnemyMain)!,

				EnemyEscortFleetBefore = fleetsBefore.EnemyEscortFleet,
				EnemyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(fleetsBefore.EnemyEscortFleet, hpsAfter, BattleSides.EnemyEscort),
			},
		};

		BattleRank rank = prediction.PredictRank();

		friendRate = prediction.FriendHpRate;
		enemyRate = prediction.EnemyHpRate;

		return (int)rank;
	}

	/// <summary>
	/// 敵連合艦隊戦において、夜戦突入時に敵本隊と戦闘可能な戦況かどうか
	/// </summary>
	public bool WillNightBattleWithMainFleet()
	{
		if (!IsEnemyCombined) return false; // ? true?
		if (FirstBattle?.Initial is not { IsEnemyCombined: true } initial) return false; // ? true?

		int score = 0;
		List<int> resultHps = FirstBattle.ResultHPs.ToList();

		for (int i = 0; i < initial.EnemyInitialHPsEscort.Count; i++)
		{
			if (initial.EnemyMembersEscort[i] > 0)
			{
				double rate = (double)resultHps[BattleIndex.Get(BattleSides.EnemyEscort, i)] /
					initial.EnemyMaxHPsEscort[i];

				if (rate > 0.5)
				{
					score += 10;
				}
				else if (rate > 0.25)
				{
					score += 7;
				}

				if (i == 0 && rate > 0)
				{
					score += 10;
				}
			}
		}

		return score < 30;
	}


	private void WriteBattleLog()
	{
		if (!Utility.Configuration.Config.Log.SaveBattleLog) return;

		try
		{
			string parent = BattleLogPath;

			Directory.CreateDirectory(parent);

			string info = (IsPractice, Compass) switch
			{
				(true, _) => "practice",
				(_, not null) => $"{Compass.MapAreaID}-{Compass.MapInfoID}-{Compass.CellId}",

				// should never happen
				_ => "???",
			};

			string path = $"{parent}\\{DateTimeHelper.GetTimeStamp()}@{info}.txt";

			using StreamWriter sw = new(path, false, Utility.Configuration.Config.Log.FileEncoding);
			sw.Write(BattleDetailDescriptor.GetBattleDetail(this));
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, "戦闘ログの出力に失敗しました。");
		}
	}
}
