using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdShooting;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Data.Battle;

/// <summary>
/// 戦闘関連の処理を統括して扱います。
/// </summary>
public class BattleManager : APIWrapper
{

	public static readonly string BattleLogPath = "BattleLog";

	public delegate void ShipLevelUpHandler(IShipData ship, int nextLevel);

	/// <summary>
	/// Ship will have the level before level up
	/// </summary>
	/// <param name="ship"></param>
	/// <param name="nextLevel"></param>
	public event ShipLevelUpHandler ShipLevelUp;

	/// <summary>
	/// 羅針盤データ
	/// </summary>
	public CompassData Compass { get; private set; }

	/// <summary>
	/// 昼戦データ
	/// </summary>
	public FirstBattleData BattleDay { get; private set; }

	/// <summary>
	/// 夜戦データ
	/// </summary>
	public BattleData? BattleNight { get; private set; }

	/// <summary>
	/// 戦闘結果データ
	/// </summary>
	public BattleResult Result { get; private set; }

	/// <summary>
	/// The battle result api doesn't report SS, so we need to evaluate it manually.
	/// </summary>
	public string PredictedBattleRank { get; set; }

	// In the api, heavy base air raid is implemented as 3 different air raid battles
	// If we decide to collapse it down into 1 battle, this should be deleted
	// and heavy base air raid moved to BattleDay like regular BattleBaseAirRaid
	public List<BattleBaseAirRaid> HeavyBaseAirRaids { get; } = new();

	[Flags]
	public enum BattleModes
	{
		Undefined,                      // 未定義
		Normal,                         // 昼夜戦(通常戦闘)
		NightOnly,                      // 夜戦
		NightDay,                       // 夜昼戦
		AirBattle,                      // 航空戦
		AirRaid,                        // 長距離空襲戦
		Radar,                          // レーダー射撃
		Practice,                       // 演習
		BaseAirRaid,                    // 基地空襲戦
		BattlePhaseMask = 0xFF,         // 戦闘形態マスク
		CombinedTaskForce = 0x100,      // 機動部隊
		CombinedSurface = 0x200,        // 水上部隊
		CombinedMask = 0xFF00,          // 連合艦隊仕様
		EnemyCombinedFleet = 0x10000,   // 敵連合艦隊
	}

	/// <summary>
	/// 戦闘種別
	/// </summary>
	public BattleModes BattleMode { get; private set; }


	/// <summary>
	/// 昼戦から開始する戦闘かどうか
	/// </summary>
	public bool StartsFromDayBattle => !StartsFromNightBattle;

	/// <summary>
	/// 夜戦から開始する戦闘かどうか
	/// </summary>
	public bool StartsFromNightBattle
	{
		get
		{
			var phase = BattleMode & BattleModes.BattlePhaseMask;
			return phase == BattleModes.NightOnly || phase == BattleModes.NightDay;
		}
	}

	/// <summary>
	/// 連合艦隊戦かどうか
	/// </summary>
	public bool IsCombinedBattle => (BattleMode & BattleModes.CombinedMask) != 0;

	/// <summary>
	/// 演習かどうか
	/// </summary>
	public bool IsPractice => (BattleMode & BattleModes.BattlePhaseMask) == BattleModes.Practice;

	/// <summary>
	/// 敵が連合艦隊かどうか
	/// </summary>
	public bool IsEnemyCombined => (BattleMode & BattleModes.EnemyCombinedFleet) != 0;

	/// <summary>
	/// 基地空襲戦かどうか
	/// </summary>
	public bool IsBaseAirRaid => (BattleMode & BattleModes.BattlePhaseMask) == BattleModes.BaseAirRaid;


	/// <summary>
	/// 1回目の戦闘
	/// </summary>
	public FirstBattleData FirstBattle => HeavyBaseAirRaids switch
	{
		// first battle gets used for things like engagement
		// remove this part if heavy air raids get moved to BattleDay
		{ Count: > 0 } => HeavyBaseAirRaids.Last(),
		_ => StartsFromDayBattle ? BattleDay : (FirstBattleData)BattleNight!
	};

	/// <summary>
	/// 2回目の戦闘
	/// </summary>
	public BattleData? SecondBattle => StartsFromDayBattle ? (BattleData)BattleNight : BattleDay;


	/// <summary>
	/// 出撃中に入手した艦船数
	/// </summary>
	public int DroppedShipCount { get; internal set; }

	/// <summary>
	/// 出撃中に入手した装備数
	/// </summary>
	public int DroppedEquipmentCount { get; internal set; }

	/// <summary>
	/// 出撃中に入手したアイテム - ID と 個数 のペア
	/// </summary>
	public Dictionary<int, int> DroppedItemCount { get; } = [];


	/// <summary>
	/// 演習の敵提督名
	/// </summary>
	public string EnemyAdmiralName { get; internal set; }

	/// <summary>
	/// 演習の敵提督階級
	/// </summary>
	public string EnemyAdmiralRank { get; internal set; }

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

	public override void LoadFromResponse(string apiname, dynamic data)
	{
		IKCDatabase db = KCDatabase.Instance;

		//base.LoadFromResponse( apiname, data );	//不要

		object? apiData = DeserializeResponse(apiname, data.ToString());

		HeavyBaseAirRaids.Clear();

		switch (apiname)
		{
			case "api_req_map/start":
			case "api_req_map/next":
			{
				BattleDay = null;
				BattleNight = null;
				Result = null;
				BattleMode = BattleModes.Undefined;
				Compass = new CompassData();
				Compass.LoadFromResponse(apiname, data);

				if (apiData is ApiReqMapNextResponse { ApiDestructionBattle: { } battle })
				{
					BattleMode = BattleModes.BaseAirRaid;
					BattleDay = BattleFactory.CreateBattle(battle, Fleets);
					BattleFinished();
				}
			}
			break;
			/*
			case "api_req_map/air_raid":
			{
				BattleMode = BattleModes.BaseAirRaid;
				// BattleDay = new BattleHeavyBaseAirRaid();
				// BattleDay.LoadFromResponse(apiname, data.api_destruction_battle[0]);
				foreach (dynamic airraid in data.api_destruction_battle)
				{
					BattleBaseAirRaid raid = new();
					raid.LoadFromResponse(apiname, airraid);
					HeavyBaseAirRaids.Add(raid);
				}
				BattleFinished();
			}
			break;
			*/
			case "api_req_sortie/battle":
			{
				if (apiData is not ApiReqSortieBattleResponse battle) break;

				BattleMode = BattleModes.Normal;
				BattleDay = BattleFactory.CreateBattle(battle, Fleets);
			}
			break;

			case "api_req_battle_midnight/battle":
			{
				if (apiData is not ApiReqBattleMidnightBattleResponse battle) break;

				BattleNight = BattleFactory.CreateBattle(battle, BattleDay.FleetsAfterBattle);
			}
			break;

			/*
			case "api_req_battle_midnight/sp_midnight":
				BattleMode = BattleModes.NightOnly;
				BattleDay = null;
				BattleNight = new BattleNightOnly();
				BattleNight.LoadFromResponse(apiname, data);
				break;

			case "api_req_sortie/airbattle":
				BattleMode = BattleModes.AirBattle;
				BattleDay = new BattleAirBattle();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_sortie/ld_airbattle":
				BattleMode = BattleModes.AirRaid;
				BattleDay = new BattleAirRaid();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_sortie/night_to_day":
				BattleMode = BattleModes.NightDay;
				BattleNight = new BattleNormalDayFromNight();
				BattleNight.LoadFromResponse(apiname, data);
				break;

			case "api_req_sortie/ld_shooting":
				BattleMode = BattleModes.Radar;
				BattleDay = new BattleNormalRadar();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/battle":
				BattleMode = BattleModes.Normal | BattleModes.CombinedTaskForce;
				BattleDay = new BattleCombinedNormalDay();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/midnight_battle":
				BattleNight = new BattleCombinedNormalNight();
				//BattleNight.TakeOverParameters( BattleDay );		//checkme: 連合艦隊夜戦では昼戦での与ダメージがMVPに反映されない仕様？
				BattleNight.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/sp_midnight":
				BattleMode = BattleModes.NightOnly | BattleModes.CombinedMask;
				BattleDay = null;
				BattleNight = new BattleCombinedNightOnly();
				BattleNight.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/airbattle":
				BattleMode = BattleModes.AirBattle | BattleModes.CombinedTaskForce;
				BattleDay = new BattleCombinedAirBattle();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/battle_water":
				BattleMode = BattleModes.Normal | BattleModes.CombinedSurface;
				BattleDay = new BattleCombinedWater();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/ld_airbattle":
				BattleMode = BattleModes.AirRaid | BattleModes.CombinedTaskForce;
				BattleDay = new BattleCombinedAirRaid();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/ec_battle":
				BattleMode = BattleModes.Normal | BattleModes.EnemyCombinedFleet;
				BattleDay = new BattleEnemyCombinedDay();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/ec_midnight_battle":
				BattleNight = new BattleEnemyCombinedNight();
				BattleNight.TakeOverParameters(BattleDay);
				BattleNight.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/ec_night_to_day":
				BattleMode = BattleModes.NightDay | BattleModes.EnemyCombinedFleet;
				BattleNight = new BattleEnemyCombinedDayFromNight();
				BattleNight.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/each_battle":
				BattleMode = BattleModes.Normal | BattleModes.CombinedTaskForce | BattleModes.EnemyCombinedFleet;
				BattleDay = new BattleCombinedEachDay();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/each_battle_water":
				BattleMode = BattleModes.Normal | BattleModes.CombinedSurface | BattleModes.EnemyCombinedFleet;
				BattleDay = new BattleCombinedEachWater();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_combined_battle/ld_shooting":
				BattleMode = BattleModes.Radar | BattleModes.CombinedTaskForce;
				BattleDay = new BattleCombinedRadar();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_member/get_practice_enemyinfo":
				EnemyAdmiralName = data.api_nickname;
				EnemyAdmiralRank = Constants.GetAdmiralRank((int)data.api_rank);
				break;

			case "api_req_practice/battle":
				BattleMode = BattleModes.Practice;
				BattleDay = new BattlePracticeDay();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_practice/midnight_battle":
				BattleNight = new BattlePracticeNight();
				BattleNight.TakeOverParameters(BattleDay);
				BattleNight.LoadFromResponse(apiname, data);
				break;
			*/

			case "api_req_sortie/battleresult":
			case "api_req_combined_battle/battleresult":
			case "api_req_practice/battle_result":
			{
				if (apiData is not ISortieBattleResultApi result) break;

				Result = result switch
				{
					ApiReqCombinedBattleBattleresultResponse r => new(r),
					_ => new(result),
				};
				BattleFinished();
				break;
			}

			case "api_port/port":
				Compass = null;
				BattleDay = null;
				BattleNight = null;
				Result = null;
				BattleMode = BattleModes.Undefined;
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
		EnemyFleetRecord.EnemyFleetElement enemyFleetData = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();

		if (enemyFleetData != null)
			RecordManager.Instance.EnemyFleet.Update(enemyFleetData);


		// ロギング
		if (IsPractice)
		{
			Utility.Logger.Add(2,
				string.Format(BattleRes.BattleFinishedPractice,
					EnemyAdmiralName, EnemyAdmiralRank, Result.EnemyFleetName, Result.Rank, Result.AdmiralExp, Result.BaseExp));
		}
		else if (IsBaseAirRaid)
		{
			if (BattleDay is BattleBaseAirRaid raid)
			{
				PhaseBaseAirRaid? airraid = raid.BaseAirRaid;
				List<int> initialHPs = BattleDay.Initial.FriendInitialHPs.TakeWhile(hp => hp >= 0).ToList();
				int damage = initialHPs.Zip(BattleDay.ResultHPs.Take(initialHPs.Count()), (initial, result) => initial - result).Sum();

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
			Utility.Logger.Add(2,
				string.Format(BattleRes.BattleFinishedSortie,
					Compass.MapAreaID, Compass.MapInfoID, Compass.CellDisplay, KCDatabase.Instance.Translation.Operation.FleetName(Result.EnemyFleetName), Result.Rank, Result.AdmiralExp, Result.BaseExp));
		}


		// Level up
		if (!IsBaseAirRaid)
		{
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
			//checkme: とてもアレな感じ

			int shipID = (int?)Result.DroppedShipId ?? -1;
			int itemID = Result.DroppedItemId ?? -1;
			int eqID = Result.DroppedEquipmentId ?? -1;
			bool showLog = Utility.Configuration.Config.Log.ShowSpoiler;

			if (shipID != -1)
			{

				IShipDataMaster ship = KCDatabase.Instance.MasterShips[(int)shipID];
				DroppedShipCount++;

				IEnumerable<IEquipmentDataMaster?>? defaultSlot = ship.DefaultSlot?.Select(i => i switch
				{
					< 1 => null,
					_ => KCDatabase.Instance.MasterEquipments[i]
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

			if (itemID != -1)
			{
				DroppedItemCount.TryAdd(itemID, 0);
				DroppedItemCount[itemID]++;

				if (showLog)
				{
					IUseItem? item = KCDatabase.Instance.UseItems[itemID];
					IUseItemMaster? itemmaster = KCDatabase.Instance.MasterUseItems[itemID];
					Utility.Logger.Add(2, string.Format(LoggerRes.ItemObtained, itemmaster?.NameTranslated ?? (BattleRes.UnknownItem + itemID), (item?.Count ?? 0) + DroppedItemCount[itemID]));
				}
			}

			if (eqID != -1)
			{
				IEquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[eqID];
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
			if (shipID == -1 && (
				KCDatabase.Instance.Admiral.MaxShipCount - (KCDatabase.Instance.Ships.Count + DroppedShipCount) <= 0 ||
				KCDatabase.Instance.Admiral.MaxEquipmentCount - (KCDatabase.Instance.Equipments.Values.Count(e => e.MasterEquipment.UsesSlotSpace()) + DroppedEquipmentCount) <= 0))
			{
				shipID = -2;
			}

			RecordManager.Instance.ShipDrop.Add(shipID, itemID, eqID, Compass.MapAreaID, Compass.MapInfoID, Compass.CellId, Compass.MapInfo.EventDifficulty, Compass.EventID == 5, enemyFleetData.FleetID, Result.Rank, KCDatabase.Instance.Admiral.Level);
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
	/// <param name="friendrate">味方の損害率を出力します。</param>
	/// <param name="enemyrate">敵の損害率を出力します。</param>
	public int PredictWinRank(out double friendrate, out double enemyrate)
	{
		BattleData activeBattle = SecondBattle ?? FirstBattle;
		BattleFleets fleetsBefore = activeBattle.FleetsBeforeBattle;

		List<int> hpsBefore = activeBattle.InitialHPs.ToList();
		List<int> hpsAfter = activeBattle.ResultHPs.ToList();

		BattleRankPrediction prediction = (BattleMode & BattleModes.BattlePhaseMask) switch
		{
			BattleModes.AirRaid or BattleModes.Radar => new AirRaidBattleRankPrediction()
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
			BattleModes.BaseAirRaid => new BaseAirRaidBattleRankPrediction()
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

		friendrate = prediction.FriendHpRate;
		enemyrate = prediction.EnemyHpRate;

		return (int)rank;
	}

	/// <summary>
	/// 敵連合艦隊戦において、夜戦突入時に敵本隊と戦闘可能な戦況かどうか
	/// </summary>
	public bool WillNightBattleWithMainFleet()
	{
		if (StartsFromDayBattle && IsEnemyCombined)
		{
			var initial = BattleDay.Initial;
			int score = 0;
			for (int i = 0; i < initial.EnemyInitialHPsEscort.Count; i++)
			{
				if (initial.EnemyMembersEscort.ToList()[i] > 0)
				{
					double rate = (double)BattleDay.ResultHPs.ToList()[BattleIndex.Get(BattleSides.EnemyEscort, i)] / initial.EnemyMaxHPsEscort.ToList()[i];

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
		else return false;          // ? true?
	}


	private void WriteBattleLog()
	{

		if (!Utility.Configuration.Config.Log.SaveBattleLog)
			return;

		try
		{
			string parent = BattleLogPath;

			if (!Directory.Exists(parent))
				Directory.CreateDirectory(parent);

			string info;
			if (IsPractice)
				info = "practice";
			else
				info = $"{Compass.MapAreaID}-{Compass.MapInfoID}-{Compass.CellId}";

			string path = $"{parent}\\{DateTimeHelper.GetTimeStamp()}@{info}.txt";

			using (var sw = new StreamWriter(path, false, Utility.Configuration.Config.Log.FileEncoding))
			{
				// todo sw.Write(BattleDetailDescriptor.GetBattleDetail(this));
			}

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "戦闘ログの出力に失敗しました。");
		}
	}

}
