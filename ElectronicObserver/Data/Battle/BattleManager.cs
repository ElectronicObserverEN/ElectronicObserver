using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElectronicObserver.Core;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data.Battle.Detail;
using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

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
	public BattleDay BattleDay { get; private set; }

	/// <summary>
	/// 夜戦データ
	/// </summary>
	public BattleNight BattleNight { get; private set; }

	/// <summary>
	/// 戦闘結果データ
	/// </summary>
	public BattleResultData Result { get; private set; }

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
	public BattleData FirstBattle => HeavyBaseAirRaids switch
	{
		// first battle gets used for things like engagement
		// remove this part if heavy air raids get moved to BattleDay
		{ Count: > 0 } => HeavyBaseAirRaids.Last(),
		_ => StartsFromDayBattle ? BattleDay : BattleNight
	};

	/// <summary>
	/// 2回目の戦闘
	/// </summary>
	public BattleData SecondBattle => StartsFromDayBattle ? (BattleData)BattleNight : BattleDay;


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
	public Dictionary<int, int> DroppedItemCount { get; internal set; }


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
	public Dictionary<int, int> SpecialAttackCount { get; private set; }

	/// <summary>
	/// 記録する特殊攻撃
	/// </summary>
	private int[] TracedSpecialAttack { get; } = [100, 101, 102, 103, 104, 105, 300, 301, 302, 400, 401];



	public BattleManager()
	{
		DroppedItemCount = new Dictionary<int, int>();
		SpecialAttackCount = new Dictionary<int, int>();
	}


	public override void LoadFromResponse(string apiname, dynamic data)
	{
		//base.LoadFromResponse( apiname, data );	//不要

		HeavyBaseAirRaids.Clear();

		switch (apiname)
		{
			case "api_req_map/start":
			case "api_req_map/next":
				BattleDay = null;
				BattleNight = null;
				Result = null;
				BattleMode = BattleModes.Undefined;
				Compass = new CompassData();
				Compass.LoadFromResponse(apiname, data);

				if (Compass.HasAirRaid)
				{
					BattleMode = BattleModes.BaseAirRaid;
					BattleDay = new BattleBaseAirRaid();
					BattleDay.LoadFromResponse(apiname, Compass.AirRaidData);
					BattleFinished();
				}
				break;

			case "api_req_map/air_raid":
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
				break;

			case "api_req_sortie/battle":
				BattleMode = BattleModes.Normal;
				BattleDay = new BattleNormalDay();
				BattleDay.LoadFromResponse(apiname, data);
				break;

			case "api_req_battle_midnight/battle":
				BattleNight = new BattleNormalNight();
				BattleNight.TakeOverParameters(BattleDay);
				BattleNight.LoadFromResponse(apiname, data);
				break;

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

			case "api_req_sortie/battleresult":
			case "api_req_combined_battle/battleresult":
			case "api_req_practice/battle_result":
				Result = new BattleResultData();
				Result.LoadFromResponse(apiname, data);
				BattleFinished();
				break;

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
			if (BattleDay is BattleBaseAirRaid { BaseAirRaid: { } airraid })
			{
				var initialHPs = BattleDay.Initial.FriendInitialHPs.TakeWhile(hp => hp >= 0);
				var damage = initialHPs.Zip(BattleDay.ResultHPs.Take(initialHPs.Count()), (initial, result) => initial - result).Sum();

				Utility.Logger.Add(2,
				string.Format(BattleRes.BattleFinishedBaseAirRaid,
					Compass.MapAreaID, Compass.MapInfoID, Compass.CellDisplay,
					Constants.GetAirSuperiority(airraid.IsAvailable ? airraid.AirSuperiority : -1), damage, Constants.GetAirRaidDamage(Compass.AirRaidDamageKind)));
			}

			foreach (BattleBaseAirRaid battleBaseAirRaid in HeavyBaseAirRaids)
			{
				List<int> initialHPs = battleBaseAirRaid.Initial.FriendInitialHPs.TakeWhile(hp => hp >= 0).ToList();
				int damage = initialHPs.Zip(battleBaseAirRaid.ResultHPs.Take(initialHPs.Count()), (initial, result) => initial - result).Sum();
				PhaseBaseAirRaid baseAirRaid = battleBaseAirRaid.BaseAirRaid;

				int airRaidDamageKind = (int)battleBaseAirRaid.RawData.api_lost_kind;

				Utility.Logger.Add(2,
					string.Format(BattleRes.BattleFinishedBaseAirRaid,
						Compass.MapAreaID, Compass.MapInfoID, Compass.CellDisplay,
						Constants.GetAirSuperiority(baseAirRaid.IsAvailable ? baseAirRaid.AirSuperiority : -1), damage, Constants.GetAirRaidDamage(airRaidDamageKind)));
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
			var exps = Result.ExpList;
			var lvup = Result.LevelUpList;
			for (int i = 0; i < lvup.Length; i++)
			{
				if (lvup[i].Length >= 2 && i < exps.Length && lvup[i][0] + exps[i] >= lvup[i][1])
				{
					var ship = FirstBattle.Initial.FriendFleet.MembersInstance[i];
					int increment = Math.Max(lvup[i].Length - 2, 1);

					ShipLevelUp?.Invoke(ship, ship.Level + increment);
					Utility.Logger.Add(2, string.Format(BattleRes.HasLeveledUp, ship.Name, ship.Level + increment));
				}
			}

			if (IsCombinedBattle)
			{
				exps = Result.ExpListCombined;
				lvup = Result.LevelUpListCombined;
				for (int i = 0; i < lvup.Length; i++)
				{
					if (lvup[i].Length >= 2 && i < exps.Length && lvup[i][0] + exps[i] >= lvup[i][1])
					{
						var ship = FirstBattle.Initial.FriendFleetEscort.MembersInstance[i];
						int increment = Math.Max(lvup[i].Length - 2, 1);

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

			int shipID = Result.DroppedShipID;
			int itemID = Result.DroppedItemID;
			int eqID = Result.DroppedEquipmentID;
			bool showLog = Utility.Configuration.Config.Log.ShowSpoiler;

			if (shipID != -1)
			{

				IShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
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

				if (!DroppedItemCount.ContainsKey(itemID))
					DroppedItemCount.Add(itemID, 0);
				DroppedItemCount[itemID]++;

				if (showLog)
				{
					var item = KCDatabase.Instance.UseItems[itemID];
					var itemmaster = KCDatabase.Instance.MasterUseItems[itemID];
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


		void IncrementSpecialAttack(BattleData bd)
		{
			if (bd == null)
				return;

			foreach (var phase in bd.GetPhases())
			{
				foreach (var detail in phase.BattleDetails)
				{
					int kind = detail.AttackType;

					if (detail.AttackerIndex.IsFriend && TracedSpecialAttack.Contains(kind))
					{
						if (SpecialAttackCount.ContainsKey(kind))
							SpecialAttackCount[kind]++;
						else
							SpecialAttackCount.Add(kind, 1);
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
		PhaseInitial firstInitial = FirstBattle.Initial;

		List<int> hpsAfter = activeBattle.ResultHPs.ToList();

		BattleRankPrediction prediction = (BattleMode & BattleModes.BattlePhaseMask) switch
		{
			BattleModes.AirRaid or BattleModes.Radar => new AirRaidBattleRankPrediction()
			{
				FriendlyMainFleetBefore = firstInitial.FriendFleet,
				FriendlyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.FriendFleet, hpsAfter, BattleSides.FriendMain)!,

				FriendlyEscortFleetBefore = firstInitial.FriendFleetEscort,
				FriendlyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.FriendFleetEscort, hpsAfter, BattleSides.FriendEscort),

				EnemyMainFleetBefore = firstInitial.EnemyFleet,
				EnemyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.EnemyFleet, hpsAfter, BattleSides.EnemyMain)!,

				EnemyEscortFleetBefore = firstInitial.EnemyFleetEscort,
				EnemyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.EnemyFleetEscort, hpsAfter, BattleSides.EnemyEscort),
			},
			BattleModes.BaseAirRaid => new BaseAirRaidBattleRankPrediction()
			{
				AirBaseBeforeAfter = BaseAirRaidBattleRankPrediction.SimulateBaseAfterBattle(firstInitial.FriendInitialHPs.ToList(), hpsAfter),

				EnemyMainFleetBefore = firstInitial.EnemyFleet,
				EnemyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.EnemyFleet, hpsAfter, BattleSides.EnemyMain)!,

				EnemyEscortFleetBefore = firstInitial.EnemyFleetEscort,
				EnemyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.EnemyFleetEscort, hpsAfter, BattleSides.EnemyEscort),
			},
			_ => new NormalBattleRankPrediction()
			{
				FriendlyMainFleetBefore = firstInitial.FriendFleet,
				FriendlyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.FriendFleet, hpsAfter, BattleSides.FriendMain)!,

				FriendlyEscortFleetBefore = firstInitial.FriendFleetEscort,
				FriendlyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.FriendFleetEscort, hpsAfter, BattleSides.FriendEscort),

				EnemyMainFleetBefore = firstInitial.EnemyFleet,
				EnemyMainFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.EnemyFleet, hpsAfter, BattleSides.EnemyMain)!,

				EnemyEscortFleetBefore = firstInitial.EnemyFleetEscort,
				EnemyEscortFleetAfter = BattleRankPrediction.SimulateFleetAfterBattle(firstInitial.EnemyFleetEscort, hpsAfter, BattleSides.EnemyEscort),
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
			for (int i = 0; i < initial.EnemyInitialHPsEscort.Length; i++)
			{
				if (initial.EnemyMembersEscort[i] > 0)
				{
					double rate = (double)BattleDay.ResultHPs[BattleIndex.Get(BattleSides.EnemyEscort, i)] / initial.EnemyMaxHPsEscort[i];

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
				sw.Write(BattleDetailDescriptor.GetBattleDetail(this));
			}

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "戦闘ログの出力に失敗しました。");
		}
	}

}
