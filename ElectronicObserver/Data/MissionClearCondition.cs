using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Data;

/// <summary>
/// 遠征可否判定を行います。
/// </summary>
public static class MissionClearCondition
{

	/// <summary>
	/// 遠征に成功する編成かどうかを判定します。
	/// </summary>
	/// <param name="missionId">遠征ID。</param>
	/// <param name="fleet">対象となる艦隊。達成条件を確認したい場合は null を指定します。</param>
	public static MissionClearConditionResult Check(int missionId, IFleetData? fleet)
	{
		MissionClearConditionResult result = new(fleet);

		result = CheckMissionCondition(result, missionId);

		if (result.ZeroSlotWarning)
		{
			result.Fail(DataRes.ZeroSlotAircraftWarning);
		}

		return result;
	}

	private static MissionClearConditionResult CheckMissionCondition(MissionClearConditionResult result,
		int missionId) => missionId switch
		{
			// 練習航海
			1 => result
				.CheckFlagshipLevel(1)
				.CheckShipCount(2),

			// 長距離練習航海
			2 => result
				.CheckFlagshipLevel(2)
				.CheckShipCount(4),

			// 警備任務
			3 => result
				.CheckFlagshipLevel(3)
				.CheckShipCount(3),

			// 対潜警戒任務
			4 => result
				.CheckFlagshipLevel(3)
				.CheckEscortFleet(),

			// 海上護衛任務
			5 => result
				.CheckFlagshipLevel(3)
				.CheckShipCount(4)
				.CheckEscortFleet(),

			// 防空射撃演習
			6 => result
				.CheckFlagshipLevel(4)
				.CheckShipCount(4),

			// 観艦式予行
			7 => result
				.CheckFlagshipLevel(5)
				.CheckShipCount(6),

			// 観艦式
			8 => result
				.CheckFlagshipLevel(6)
				.CheckShipCount(6),

			// 兵站強化任務
			100 => result
				.CheckFlagshipLevel(5)
				.CheckLevelSum(10)
				.CheckShipCount(4)
				.CheckSmallShipCount(3),

			// 海峡警備行動
			101 => result
				.CheckFlagshipLevel(20)
				.CheckSmallShipCount(4)
				.CheckFirepower(50)
				.CheckAA(70)
				.CheckASW(180),

			// 長時間対潜警戒
			102 => result
				.CheckFlagshipLevel(35)
				.CheckLevelSum(185)
				.CheckShipCount(5)
				.CheckEscortFleetDD3()
				.CheckAA(59)
				.CheckASW(280)
				.CheckLOS(60),

			// 南西方面連絡線哨戒
			103 => result
				.CheckFlagshipLevel(40)
				.CheckLevelSum(200)
				.CheckShipCount(5)
				.CheckEscortFleet()
				.CheckFirepower(300)
				.CheckAA(200)
				.CheckASW(200)
				.CheckLOS(120),

			// 小笠原沖哨戒線
			104 => result
				.CheckFlagshipLevel(45)
				.CheckLevelSum(230)
				.CheckShipCount(5)
				.CheckEscortFleetDD3()
				.CheckFirepower(280)
				.CheckAA(220)
				.CheckASW(240)
				.CheckLOS(150),

			// 小笠原沖戦闘哨戒
			105 => result
				.CheckFlagshipLevel(55)
				.CheckLevelSum(290)
				.CheckShipCount(6)
				.CheckEscortFleetDD5()
				.CheckFirepower(330)
				.CheckAA(300)
				.CheckASW(270)
				.CheckLOS(180),

			// タンカー護衛任務
			9 => result
				.CheckFlagshipLevel(3)
				.CheckShipCount(4)
				.CheckEscortFleet(),

			// 強行偵察任務
			10 => result
				.CheckFlagshipLevel(3)
				.CheckShipCount(3)
				.CheckShipCountByType(ShipTypes.LightCruiser, 2),

			// ボーキサイト輸送任務
			11 => result
				.CheckFlagshipLevel(6)
				.CheckShipCount(4)
				.CheckSmallShipCount(2),

			// 資源輸送任務
			12 => result
				.CheckFlagshipLevel(4)
				.CheckShipCount(4)
				.CheckSmallShipCount(2),

			// 鼠輸送作戦
			13 => result
				.CheckFlagshipLevel(5)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 4),

			// 包囲陸戦隊撤収作戦
			14 => result
				.CheckFlagshipLevel(6)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 3),

			// 囮機動部隊支援作戦
			15 => result
				.CheckFlagshipLevel(8)
				.CheckShipCount(6)
				.CheckAircraftCarrierCount(2)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 艦隊決戦援護作戦
			16 => result
				.CheckFlagshipLevel(10)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 南西方面航空偵察作戦
			110 => result
				.CheckFlagshipLevel(40)
				.CheckLevelSum(150)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.SeaplaneTender, 1)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckSmallShipCount(2)
				.CheckAA(200)
				.CheckASW(200)
				.CheckLOS(140),

			// 敵泊地強襲反撃作戦
			111 => result
				.CheckFlagshipLevel(45)
				.CheckLevelSum(220)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.HeavyCruiser, 1)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 3)
				.CheckFirepower(360)
				.CheckAA(160)
				.CheckASW(160)
				.CheckLOS(140),

			// 南西諸島離島哨戒作戦
			112 => result
				.CheckFlagshipLevel(50)
				.CheckLevelSum(250)
				.CheckShipCountByType(ShipTypes.SeaplaneTender, 1)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckSmallShipCount(4)
				.CheckFirepower(400)
				.CheckAA(220)
				.CheckASW(220)
				.CheckLOS(190),

			// 南西諸島離島防衛作戦
			113 => result
				.CheckFlagshipLevel(55)
				.CheckLevelSum(300)
				.CheckShipCountByType(ShipTypes.HeavyCruiser, 2)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 2)
				.CheckSubmarineCount(1)
				.CheckFirepower(500)
				.CheckAA(280)
				.CheckASW(280)
				.CheckLOS(170),

			// 南西諸島捜索撃滅戦
			114 => result
				.CheckFlagshipLevel(60)
				.CheckLevelSum(330)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.SeaplaneTender, 1)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 2)
				.CheckFirepower(510)
				.CheckAA(400)
				.CheckASW(285)
				.CheckLOS(385),

			// 精鋭水雷戦隊夜襲
			115 => result
				.CheckFlagshipLevel(75)
				.CheckLevelSum(400)
				.CheckFlagshipType(ShipTypes.LightCruiser)
				.CheckShipCountByType(ShipTypes.Destroyer, 5)
				.CheckFirepower(410)
				.CheckAA(390)
				.CheckASW(410)
				.CheckLOS(340),

			// 敵地偵察作戦
			17 => result
				.CheckFlagshipLevel(20)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 3),

			// 航空機輸送作戦
			18 => result
				.CheckFlagshipLevel(15)
				.CheckShipCount(6)
				.CheckAircraftCarrierCount(3)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 北号作戦
			19 => result
				.CheckFlagshipLevel(20)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.AviationBattleship, 2)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 潜水艦哨戒任務
			20 => result
				.CheckFlagshipLevel(1)
				.CheckSubmarineCount(1)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1),

			// 北方鼠輸送作戦
			21 => result
				.CheckFlagshipLevel(15)
				.CheckLevelSum(30)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 4)
				.CheckEquippedShipCount(EquipmentTypes.TransportContainer, 3),

			// 艦隊演習
			22 => result
				.CheckFlagshipLevel(30)
				.CheckLevelSum(45)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.HeavyCruiser, 1)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 航空戦艦運用演習
			23 => result
				.CheckFlagshipLevel(50)
				.CheckLevelSum(200)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.AviationBattleship, 2)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 北方航路海上護衛
			24 => result
				.CheckFlagshipLevel(50)
				.CheckLevelSum(200)
				.CheckShipCount(6)
				.CheckFlagshipType(ShipTypes.LightCruiser)
				.CheckSmallShipCount(4),

			// 通商破壊作戦
			25 => result
				.CheckFlagshipLevel(25)
				.CheckShipCountByType(ShipTypes.HeavyCruiser, 2)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 敵母港空襲作戦
			26 => result
				.CheckFlagshipLevel(30)
				.CheckAircraftCarrierCount(1)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 潜水艦通商破壊作戦
			27 => result
				.CheckFlagshipLevel(1)
				.CheckSubmarineCount(2),

			// 潜水艦通商破壊作戦
			28 => result
				.CheckFlagshipLevel(30)
				.CheckSubmarineCount(3),

			// 潜水艦派遣演習
			29 => result
				.CheckFlagshipLevel(50)
				.CheckSubmarineCount(3),

			// 潜水艦派遣作戦
			30 => result
				.CheckFlagshipLevel(55)
				.CheckSubmarineCount(4),

			// 海外艦との接触
			31 => result
				.CheckFlagshipLevel(60)
				.CheckLevelSum(200)
				.CheckSubmarineCount(4),

			// 遠洋練習航海
			32 => result
				.CheckFlagshipLevel(5)
				.CheckFlagshipType(ShipTypes.TrainingCruiser)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 西方海域偵察作戦
			131 => result
				.CheckFlagshipLevel(50)
				.CheckLevelSum(200)
				.CheckShipCount(5)
				.CheckFlagshipType(ShipTypes.SeaplaneTender)
				.CheckShipCountByType(ShipTypes.Destroyer, 3)
				.CheckAA(240)
				.CheckASW(240)
				.CheckLOS(300),

			// 西方潜水艦作戦
			132 => result
				.CheckFlagshipLevel(55)
				.CheckLevelSum(270)
				.CheckShipCount(5)
				.CheckFlagshipType(ShipTypes.SubmarineTender)
				.CheckSubmarineCount(3)
				.CheckFirepower(60)
				.CheckAA(80)
				.CheckASW(50),

			// 欧州方面友軍との接触
			133 => result
				.CheckFlagshipLevel(65)
				.CheckLevelSum(350)
				.CheckFlagshipType(ShipTypes.SubmarineTender)
				.CheckSubmarineCount(3)
				.CheckShipCount(5)
				.CheckFirepower(115)
				.CheckAA(90)
				.CheckASW(70)
				.CheckLOS(95),

			// 前衛支援任務
			33 => result.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// 艦隊決戦支援任務
			34 => result.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// MO作戦
			35 => result
				.CheckFlagshipLevel(40)
				.CheckShipCount(6)
				.CheckAircraftCarrierCount(2)
				.CheckShipCountByType(ShipTypes.HeavyCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 1),

			// 水上機基地建設
			36 => result
				.CheckFlagshipLevel(30)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.SeaplaneTender, 2)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 1),

			// 東京急行
			37 => result
				.CheckFlagshipLevel(50)
				.CheckLevelSum(200)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 5)
				.CheckEquippedShipCount(EquipmentTypes.TransportContainer, 3)
				.CheckEquipmentCount(EquipmentTypes.TransportContainer, 4),

			// 東京急行(弐)
			38 => result
				.CheckFlagshipLevel(65)
				.CheckLevelSum(240)
				.CheckShipCount(6)
				.CheckShipCountByType(ShipTypes.Destroyer, 5)
				.CheckEquippedShipCount(EquipmentTypes.TransportContainer, 4)
				.CheckEquipmentCount(EquipmentTypes.TransportContainer, 8),

			// 遠洋潜水艦作戦
			39 => result
				.CheckFlagshipLevel(3)
				.CheckLevelSum(180)
				.CheckShipCountByType(ShipTypes.SubmarineTender, 1)
				.CheckSubmarineCount(4),

			// 水上機前線輸送
			40 => result
				.CheckFlagshipLevel(25)
				.CheckLevelSum(150)
				.CheckShipCount(6)
				.CheckFlagshipType(ShipTypes.LightCruiser)
				.CheckShipCountByType(ShipTypes.SeaplaneTender, 2)
				.CheckShipCountByType(ShipTypes.Destroyer, 2),

			// ラバウル方面艦隊進出
			141 => result
				.CheckFlagshipLevel(55)
				.CheckLevelSum(290)
				.CheckShipCount(6)
				.CheckFlagshipType(ShipTypes.HeavyCruiser)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 3)
				.CheckFirepower(450)
				.CheckAA(350)
				.CheckASW(330)
				.CheckLOS(250),

			// 強行鼠輸送作戦
			142 => result
				.CheckFlagshipLevel(70)
				.CheckLevelSum(320)
				.CheckShipCountByType(ShipTypes.Destroyer, 5)
				.CheckEquippedShipCount(EquipmentTypes.TransportContainer, 3)
				.CheckEquipmentCount(EquipmentTypes.TransportContainer, 4)
				.CheckFirepower(280)
				.CheckAA(240)
				.CheckASW(200)
				.CheckLOS(160),

			// ブルネイ泊地沖哨戒
			41 => result
				.CheckFlagshipLevel(30)
				.CheckLevelSum(100)
				.CheckSmallShipCount(3)
				.CheckFirepower(60)
				.CheckAA(80)
				.CheckASW(210),

			// ミ船団護衛(一号船団)
			42 => result
				.CheckFlagshipLevel(45)
				.CheckLevelSum(200)
				.CheckShipCount(4)
				.CheckEscortFleet(),

			// ミ船団護衛(二号船団)
			43 => result
				.CheckFlagshipLevel(55)
				.CheckLevelSum(300)
				.CheckShipCount(6)
				.CheckFlagshipType(ShipTypes.LightAircraftCarrier)
				.CheckEscortFleetDD4()
				.CheckFirepower(500)
				.CheckAA(280)
				.CheckASW(280)
				.CheckLOS(170),

			// 航空装備輸送任務
			44 => result
				.CheckFlagshipLevel(35)
				.CheckLevelSum(210)
				.CheckShipCount(6)
				.OrCondition(
					r => r
						.CheckShipCountByType(ShipTypes.SeaplaneTender, 2),
					r => r
						.CheckShipCountByType(ShipTypes.SeaplaneTender, 1)
						.CheckAircraftCarrierCount(1, false)
				)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckSmallShipCount(2)
				.CheckEquippedShipCount(EquipmentTypes.TransportContainer, 3)
				.CheckEquipmentCount(EquipmentTypes.TransportContainer, 6)
				.CheckAA(200)
				.CheckASW(200)
				.CheckLOS(150),

			// ボーキサイト船団護衛
			45 => result
				.CheckFlagshipLevel(50)
				.CheckLevelSum(240)
				.CheckFlagshipType(ShipTypes.LightAircraftCarrier)
				.CheckSmallShipCount(4)
				.CheckAA(240)
				.CheckASW(300)
				.CheckLOS(180),

			// 南西海域戦闘哨戒
			46 => result
				.CheckFlagshipLevel(60)
				.CheckLevelSum(300)
				.CheckShipCount(5)
				.CheckShipCountByType(ShipTypes.HeavyCruiser, 2)
				.CheckShipCountByType(ShipTypes.LightCruiser, 1)
				.CheckShipCountByType(ShipTypes.Destroyer, 2)
				.CheckFirepower(350)
				.CheckAA(250)
				.CheckASW(220)
				.CheckLOS(190),

			_ => DefaultCase(result, missionId),
		};

	private static MissionClearConditionResult DefaultCase(MissionClearConditionResult result, int missionId)
	{
		// イベント海域での支援遠征への対応
		MissionData mission = KCDatabase.Instance.Mission[missionId];

		if (mission is { Name: "前衛支援任務" or "艦隊決戦支援任務" })
		{
			return result
				.CheckShipCountByType(ShipTypes.Destroyer, 2);
		}

		return result
			.AddMessage(DataRes.MissionClearIncompatible + $"(ID:{missionId})");
	}

	/// <summary>
	/// 遠征可否判定の結果を保持します。
	/// </summary>
	public class MissionClearConditionResult
	{
		/// <summary>
		/// 遠征が成功するかどうか
		/// </summary>
		public bool IsSuceeded { get; private set; }

		private bool IsParameterCheckExpedition { get; set; }
		private bool HasZeroSlotAircraft { get; }
		public bool ZeroSlotWarning => IsParameterCheckExpedition && HasZeroSlotAircraft;

		public BattleExpeditionSuccessType SuccessType { get; private set; } = BattleExpeditionSuccessType.GreatSuccess;
		public List<string> SuccessPercent { get; }
		private List<string> failureReason;

		/// <summary>
		/// 遠征が失敗した理由 / 未対応遠征の場合のメッセージ
		/// </summary>
		public ReadOnlyCollection<string> FailureReason => failureReason.AsReadOnly();

		public IFleetData? TargetFleet { get; }
		private IEnumerable<IShipData> Members => (TargetFleet?.MembersInstance ?? Enumerable.Empty<IShipData?>())
			.Where(s => s is not null)
			.Cast<IShipData>();

		public MissionClearConditionResult(IFleetData? targetFleet)
		{
			TargetFleet = targetFleet;
			IsSuceeded = true;
			failureReason = new List<string>();
			SuccessPercent = new List<string>();

			HasZeroSlotAircraft = TargetFleet?.MembersInstance
				.Where(s => s is not null)
				.Any(s => s!.HasZeroSlotAircraft())
				?? false;
		}


		private void Assert(bool condition, Func<string> failedMessage)
		{
			if (!condition)
			{
				IsSuceeded = false;
				failureReason.Add(failedMessage());
			}
		}

		private string CurrentValue(int value) => TargetFleet != null ? (value.ToString() + "/") : "";

		public MissionClearConditionResult AddMessage(string message)
		{
			failureReason.Add(message);
			return this;
		}
		public MissionClearConditionResult SuppressWarnings()
		{
			failureReason.Add(DataRes.MissionClearUnverified);
			IsSuceeded = true;
			return this;
		}
		public MissionClearConditionResult Fail(string reason)
		{
			Assert(false, () => reason);
			return this;
		}


		public MissionClearConditionResult OrCondition(params Action<MissionClearConditionResult>[] conditions)
		{
			var conds = new MissionClearConditionResult[conditions.Length];
			for (int i = 0; i < conditions.Length; i++)
			{
				conds[i] = new MissionClearConditionResult(TargetFleet);
				conditions[i](conds[i]);
			}

			Assert(conds.Any(c => c.IsSuceeded), () => "(" + string.Join(") or (", conds.Select(c => string.Join(", ", c.FailureReason))) + ")");
			return this;
		}


		public MissionClearConditionResult CheckFlagshipLevel(int leastLevel)
		{
			int actualLevel = Members.FirstOrDefault()?.Level ?? 0;
			Assert(actualLevel >= leastLevel,
				() => DataRes.MissionClearFlagshipLv + $"{CurrentValue(actualLevel)}{leastLevel}");
			return this;
		}

		public MissionClearConditionResult CheckLevelSum(int leastSum)
		{
			int actualSum = Members.Sum(s => s.Level);
			Assert(actualSum >= leastSum,
				() => DataRes.MissionClearShipLvSum + $"{CurrentValue(actualSum)}{leastSum}");
			return this;
		}

		public MissionClearConditionResult CheckShipCount(int leastCount)
		{
			int actualCount = Members.Count();
			Assert(
				actualCount >= leastCount,
				() => string.Format(DataRes.MissionClearTotalShipCount, CurrentValue(actualCount), leastCount));
			return this;
		}


		public MissionClearConditionResult CheckShipCount(Func<IShipData, bool> predicate, int leastCount, string whatis)
		{
			int actualCount = Members.Count(predicate);
			Assert(
				actualCount >= leastCount,
				() => string.Format(DataRes.MissionClearShipCount, whatis, CurrentValue(actualCount), leastCount));
			return this;
		}

		public MissionClearConditionResult CheckShipCountByType(ShipTypes shipType, int leastCount) =>
			CheckShipCount(s => s.MasterShip.ShipType == shipType, leastCount, KCDatabase.Instance.ShipTypes[(int)shipType].NameEN);

		public MissionClearConditionResult CheckSmallShipCount(int leastCount) =>
			CheckShipCount(s => s.MasterShip.ShipType == ShipTypes.Destroyer || s.MasterShip.ShipType == ShipTypes.Escort, leastCount, DataRes.MissionClearSmallShipCount);

		public MissionClearConditionResult CheckAircraftCarrierCount(int leastCount, bool includesSeaplaneTender = true) =>
			CheckShipCount(s => s.MasterShip.IsAircraftCarrier || (includesSeaplaneTender && s.MasterShip.ShipType == ShipTypes.SeaplaneTender), leastCount, DataRes.MissionClearAircraftCarrierCount + (includesSeaplaneTender ? "" : DataRes.MissionClearExcludingSeaplaneTender));

		public MissionClearConditionResult CheckSubmarineCount(int leastCount) =>
			CheckShipCount(s => s.MasterShip.IsSubmarine, leastCount, DataRes.MissionClearSubmarineCount);

		public MissionClearConditionResult CheckEscortLeaderCount(int leastCount) =>
			CheckShipCount(s => s.MasterShip.ShipType == ShipTypes.LightCruiser || s.MasterShip.ShipType == ShipTypes.TrainingCruiser || s.MasterShip.IsEscortAircraftCarrier, leastCount, DataRes.MissionClearEscortLeaderCount);

		public MissionClearConditionResult CheckEscortFleet()
		{
			int lightCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser);
			int destroyer = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer);
			int trainingCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.TrainingCruiser);
			int escort = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Escort);
			int escortAircraftCarrier = Members.Count(s => s.MasterShip.IsEscortAircraftCarrier);

			Assert(
				(lightCruiser >= 1 && (destroyer + escort) >= 2) ||
				(escortAircraftCarrier >= 1 && (destroyer >= 2 || escort >= 2)) ||
				(destroyer >= 1 && escort >= 3) ||
				(trainingCruiser >= 1 && escort >= 2),
				//() => "[軽巡+(駆逐+海防)2 or 護衛空母+(駆逐2 or 海防2) or 駆逐+海防3 or 練巡+海防2]"       // 厳密だけど長いので
				() => DataRes.MissionClearEscortFleet
			);
			return this;
		}

		public MissionClearConditionResult CheckEscortFleetDD3()
		{
			int lightCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser);
			int destroyer = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer);
			int trainingCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.TrainingCruiser);
			int escort = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Escort);
			int escortAircraftCarrier = Members.Count(s => s.MasterShip.ShipType == ShipTypes.LightAircraftCarrier && s.ASWBase > 0);

			Assert(
				(lightCruiser >= 1 && (destroyer + escort) >= 3) ||
				(lightCruiser >= 1 && escort >= 2) ||
				(escortAircraftCarrier >= 1 && (destroyer >= 2 || escort >= 2)) ||
				(destroyer >= 1 && escort >= 3) ||
				(trainingCruiser >= 1 && escort >= 2),
				//() => "[軽巡+(駆逐+海防)3 or 軽巡+海防2 or 護衛空母+(駆逐2 or 海防2) or 駆逐+海防3 or 練巡+海防2]"       // 厳密だけど長いので
				() => DataRes.MissionClearEscortFleetDD3
			);
			return this;
		}

		public MissionClearConditionResult CheckEscortFleetDD4()
		{
			int lightCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser);
			int destroyer = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer);
			int trainingCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.TrainingCruiser);
			int escort = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Escort);
			int escortAircraftCarrier = Members.Count(s => s.MasterShip.ShipType == ShipTypes.LightAircraftCarrier && s.ASWBase > 0);

			Assert(
				(lightCruiser >= 1 && (destroyer + escort) >= 4) ||
				(lightCruiser >= 1 && escort >= 2) ||
				(escortAircraftCarrier >= 1 && (destroyer >= 2 || escort >= 2)) ||
				(destroyer >= 1 && escort >= 3) ||
				(trainingCruiser >= 1 && escort >= 2),
				//() => "[軽巡+(駆逐+海防)4 or 軽巡+海防2 or 護衛空母+(駆逐2 or 海防2) or 駆逐+海防3 or 練巡+海防2]"       // 厳密だけど長いので
				() => "護衛隊(軽巡1駆逐4他)"
			);
			return this;
		}

		public MissionClearConditionResult CheckEscortFleetDD5()
		{
			int lightCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser);
			int destroyer = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer);
			int trainingCruiser = Members.Count(s => s.MasterShip.ShipType == ShipTypes.TrainingCruiser);
			int escort = Members.Count(s => s.MasterShip.ShipType == ShipTypes.Escort);
			int escortAircraftCarrier = Members.Count(s => s.MasterShip.ShipType == ShipTypes.LightAircraftCarrier && s.ASWBase > 0);

			Assert(
				(lightCruiser >= 1 && destroyer >= 3) ||
				(escortAircraftCarrier >= 1 && (destroyer >= 2 || escort >= 2)) ||
				(destroyer >= 1 && escort >= 3) ||
				(trainingCruiser + lightCruiser >= 1 && escort >= 2),
				() => DataRes.MissionClearEscortFleetDD3
			);
			return this;
		}

		public MissionClearConditionResult CheckFlagshipType(ShipTypes shipType)
		{
			Assert(
				Members.FirstOrDefault()?.MasterShip?.ShipType == shipType,
				() => DataRes.MissionClearFlagshipType + $"{KCDatabase.Instance.ShipTypes[(int)shipType].NameEN}");
			return this;
		}

		public MissionClearConditionResult CheckFlagshipEscortAircraftCarrier()
		{
			Assert(
				Members.FirstOrDefault()?.MasterShip.IsEscortAircraftCarrier ?? false,
				() => DataRes.MissionClearFlagshipEscortAircraftCarrier);
			return this;
		}


		private MissionClearConditionResult CheckParameter(Func<IShipData, int> selector, int leastSum, string parameterName)
		{
			IsParameterCheckExpedition = true;
			int actualSum = Members.Sum(s => selector(s));
			var percentage = (double)actualSum / leastSum;
			string isStatExceeded = "★";

			// https://wikiwiki.jp/kancolle/%E9%81%A0%E5%BE%81#monthlyensei
			// https://twitter.com/yktd708/status/1880477339030888754
			if (percentage < 2.2)
			{
				SuccessType = BattleExpeditionSuccessType.Success;
				isStatExceeded = "";
			}

			SuccessPercent.Add(string.Format("{3}: {4} ({0:P2}) {1}/{2}", percentage, actualSum, leastSum,
				parameterName, isStatExceeded));

			Assert(
			actualSum >= leastSum,
			() => string.Format(DataRes.MissionClearParameter, parameterName, CurrentValue(actualSum), leastSum));
			return this;
		}

		public MissionClearConditionResult CheckFirepower(int leastSum) =>
			CheckParameter(s => s.ExpeditionFirepowerTotal, leastSum, DataRes.MissionClearFirepower);

		public MissionClearConditionResult CheckAA(int leastSum) =>
			CheckParameter(s => s.ExpeditionAATotal, leastSum, DataRes.MissionClearAa);

		public MissionClearConditionResult CheckLOS(int leastSum) =>
			CheckParameter(s => s.ExpeditionLOSTotal, leastSum, DataRes.MissionClearLos);


		public MissionClearConditionResult CheckASW(int leastSum) =>
			CheckParameter(s => s.ExpeditionASWTotal - s.AllSlotInstance.Sum(eq =>
			{
				if (eq == null) return 0;
				switch (eq.MasterEquipment.CategoryType)
				{
					case EquipmentTypes.SeaplaneRecon:
					case EquipmentTypes.SeaplaneBomber:
					case EquipmentTypes.FlyingBoat:
						return eq.MasterEquipment.ASW;
					default:
						return 0;
				}
			}), leastSum, DataRes.MissionClearAsw);


		public MissionClearConditionResult CheckEquipmentCount(Func<IEquipmentData, bool> predicate, int leastCount, string whatis)
		{
			int actualCount = Members.Sum(s => s.AllSlotInstance.Count(eq => eq != null && predicate(eq)));
			Assert(actualCount >= leastCount,
				() => string.Format(DataRes.MissionClearEquipmentCount, whatis, CurrentValue(actualCount), leastCount));
			return this;
		}

		public MissionClearConditionResult CheckEquipmentCount(EquipmentTypes equipmentType, int leastCount) =>
			CheckEquipmentCount(eq => eq.MasterEquipment.CategoryType == equipmentType, leastCount, KCDatabase.Instance.EquipmentTypes[(int)equipmentType].NameEN);


		public MissionClearConditionResult CheckEquippedShipCount(Func<IEquipmentData, bool> predicate, int leastCount, string whatis)
		{
			int actualCount = Members.Count(s => s.AllSlotInstance.Any(eq => eq != null && predicate(eq)));
			Assert(actualCount >= leastCount,
				() => string.Format(DataRes.MissionClearEquippedShipCount, whatis, CurrentValue(actualCount), leastCount));
			return this;
		}

		public MissionClearConditionResult CheckEquippedShipCount(EquipmentTypes equipmentType, int leastCount) =>
			CheckEquippedShipCount(eq => eq.MasterEquipment.CategoryType == equipmentType, leastCount, KCDatabase.Instance.EquipmentTypes[(int)equipmentType].NameEN);



		public override string ToString()
		{
			return (IsSuceeded ? DataRes.MissionClearSuccess : DataRes.MissionClearFailure) + (FailureReason.Count == 0 ? "" : (" - " + string.Join(", ", FailureReason)));
		}
	}

}
