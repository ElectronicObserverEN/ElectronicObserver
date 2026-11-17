using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data;

public static class Constants
{
	#region 艦船・装備

	/// <summary>
	/// 艦船の速力を表す文字列を取得します。
	/// </summary>
	public static string GetSpeed(int value) => value switch
	{
		0 => ConstantsRes.Land,
		5 => ConstantsRes.Slow,
		10 => ConstantsRes.Fast,
		15 => ConstantsRes.FastPlus,
		20 => ConstantsRes.Fastest,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 射程を表す文字列を取得します。
	/// </summary>
	public static string GetRange(int value) => value switch
	{
		0 => ConstantsRes.None,
		1 => ConstantsRes.Short,
		2 => ConstantsRes.Medium,
		3 => ConstantsRes.Long,
		4 => ConstantsRes.VeryLong,
		5 => ConstantsRes.VeryLong + "+",
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 艦船のレアリティを表す文字列を取得します。
	/// </summary>
	public static string GetShipRarity(int value) => value switch
	{
		0 => ConstantsRes.Red,
		1 => ConstantsRes.Indigo,
		2 => ConstantsRes.Blue,
		3 => ConstantsRes.Aqua,
		4 => ConstantsRes.Silver,
		5 => ConstantsRes.Gold,
		6 => ConstantsRes.Rainbow,
		7 => ConstantsRes.SRainbow,
		8 => ConstantsRes.Sakura,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 装備のレアリティを表す文字列を取得します。
	/// </summary>
	public static string GetEquipmentRarity(int value) => value switch
	{
		0 => ConstantsRes.Common,
		1 => ConstantsRes.Rare,
		2 => ConstantsRes.Holo,
		3 => ConstantsRes.SHolo,
		4 => ConstantsRes.SSHolo,
		5 => ConstantsRes.EXHolo,
		6 => ConstantsRes.SSHoloPlus,
		7 => ConstantsRes.SSPlusPlus,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 装備のレアリティの画像インデックスを取得します。
	/// </summary>
	public static int GetEquipmentRarityID(int value) => value switch
	{
		0 => 1,
		1 => 2,
		2 => 3,
		3 => 4,
		4 => 5,
		5 => 6,
		6 => 7,
		7 => 8,
		_ => 0,
	};


	/// <summary>
	/// 艦船のボイス設定フラグを表す文字列を取得します。
	/// </summary>
	public static string GetVoiceFlag(int value) => value switch
	{
		0 => "-",
		1 => ConstantsRes.Hourly,
		2 => ConstantsRes.Idle,
		3 => $"{ConstantsRes.Idle} + {ConstantsRes.Hourly}",
		4 => ConstantsRes.SpecialIdle,
		5 => $"{ConstantsRes.Idle} + {ConstantsRes.SpecialIdle}",
		6 => $"{ConstantsRes.Hourly} + {ConstantsRes.SpecialIdle}",
		7 => $"{ConstantsRes.Idle} + {ConstantsRes.Hourly} + {ConstantsRes.SpecialIdle}",
		_ => ConstantsRes.Unknown,
	};


	/// <summary>
	/// 艦船の損傷度合いを表す文字列を取得します。
	/// </summary>
	/// <param name="hprate">現在HP/最大HPで表される割合。</param>
	/// <param name="isPractice">演習かどうか。</param>
	/// <param name="isInstallation">陸上基地かどうか。</param>
	/// <param name="isEscaped">退避中かどうか。</param>
	/// <returns></returns>
	public static string GetDamageState(double hprate, bool isPractice = false, bool isInstallation = false, bool isEscaped = false)
		=> hprate switch
		{
			_ when isEscaped => ConstantsRes.Retreated,

			<= 0.0 when isPractice => ConstantsRes.Withdrawn,
			<= 0.0 when isInstallation => ConstantsRes.Destroyed,
			<= 0.0 => ConstantsRes.Sunk,

			<= 0.25 when isInstallation => ConstantsRes.Damaged,
			<= 0.25 => ConstantsRes.CriticalDamage,

			<= 0.5 when isInstallation => ConstantsRes.Injured,
			<= 0.5 => ConstantsRes.ModerateDamage,

			<= 0.75 when isInstallation => ConstantsRes.Disorder,
			<= 0.75 => ConstantsRes.LightDamage,

			< 1.0 => ConstantsRes.Healthy,

			_ => ConstantsRes.Unhurt,
		};


	/// <summary>
	/// 基地航空隊の行動指示を表す文字列を取得します。
	/// </summary>
	public static string GetBaseAirCorpsActionKind(AirBaseActionKind value) => value switch
	{
		AirBaseActionKind.Standby => ConstantsRes.Standby,
		AirBaseActionKind.Mission => ConstantsRes.Mission,
		AirBaseActionKind.AirDefense => ConstantsRes.AirDefense,
		AirBaseActionKind.TakeCover => ConstantsRes.TakeCover,
		AirBaseActionKind.Rest => ConstantsRes.Rest,

		_ => ConstantsRes.Unknown,
	};

	#endregion


	#region 出撃

	/// <summary>
	/// マップ上のセルでのイベントを表す文字列を取得します。
	/// </summary>
	public static string GetMapEventID(int value) => value switch
	{
		0 => ConstantsRes.StartNode,
		1 => ConstantsRes.NoEvent,
		2 => ConstantsRes.Resources,
		3 => ConstantsRes.Maelstrom,
		4 => ConstantsRes.Battle,
		5 => ConstantsRes.Boss,
		6 => ConstantsRes.Imagination,
		7 => ConstantsRes.AirBattle,
		8 => ConstantsRes.LargeResourceNode,
		9 => ConstantsRes.LandNode,
		10 => ConstantsRes.Anchorage,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// マップ上のセルでのイベント種別を表す文字列を取得します。
	/// </summary>
	public static string GetMapEventKind(int value) => value switch
	{
		0 => ConstantsRes.NoBattle,
		1 => ConstantsRes.DayBattle,
		2 => ConstantsRes.NightBattle,
		3 => ConstantsRes.NightDayBattle, // 対通常?
		4 => ConstantsRes.AirBattle,
		5 => ConstantsRes.EnemyCombinedFleet,
		6 => ConstantsRes.AirRaid,
		7 => ConstantsRes.NightDayBattle, // 対連合
		8 => ConstantsRes.RadarAmbush,
		_ => ConstantsRes.Unknown,
	};


	/// <summary>
	/// 海域難易度を表す文字列を取得します。
	/// </summary>
	public static string GetDifficulty(int value) => value switch
	{
		-1 => ConstantsRes.NoNode,
		0 => ConstantsRes.Unselected,
		1 => ConstantsRes.CasualDifficulty,
		2 => ConstantsRes.EasyDifficulty,
		3 => ConstantsRes.MediumDifficulty,
		4 => ConstantsRes.HardDifficulty,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 海域難易度を表す数値を取得します。
	/// </summary>
	public static int GetDifficulty(string value) => value switch
	{
		"未選択" or "Unselected" => 0,
		"丁" or "Casual" => 1,
		"丙" or "Easy" => 2,
		"乙" or "Medium" => 3,
		"甲" or "Hard" => 4,
		_ => -1,
	};

	/// <summary>
	/// 空襲被害の状態を表す文字列を取得します。
	/// </summary>
	public static string GetAirRaidDamage(int value) => value switch
	{
		1 => ConstantsRes.ABResourcesDamaged,
		2 => ConstantsRes.ABResources_AndPlanesDamaged,
		3 => ConstantsRes.ABPlanesDamaged,
		4 => ConstantsRes.ABNoDamage,
		_ => ConstantsRes.ABNoAirRaid,
	};

	/// <summary>
	/// 空襲被害の状態を表す文字列を取得します。(短縮版)
	/// </summary>
	public static string GetAirRaidDamageShort(int value) => value switch
	{
		1 => ConstantsRes.ABShortResources,
		2 => ConstantsRes.ABShortResourcesPlanes,
		3 => ConstantsRes.ABShortPlanes,
		4 => ConstantsRes.ABShortNoDamage,
		_ => "-",
	};

	#endregion


	#region 戦闘

	public static string GetFormation(FormationType id) => GetFormation((int)id);

	/// <summary>
	/// 陣形を表す文字列を取得します。
	/// </summary>
	public static string GetFormation(int id) => id switch
	{
		1 => ConstantsRes.LineAhead,
		2 => ConstantsRes.DoubleLine,
		3 => ConstantsRes.Diamond,
		4 => ConstantsRes.Echelon,
		5 => ConstantsRes.LineAbreast,
		6 => ConstantsRes.Vanguard,
		11 => ConstantsRes.FirstPatrolFormation,
		12 => ConstantsRes.SecondPatrolFormation,
		13 => ConstantsRes.ThirdPatrolFormation,
		14 => ConstantsRes.FourthPatrolFormation,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 陣形を表す数値を取得します。
	/// </summary>
	public static int GetFormation(string value) => value switch
	{
		"単縦陣" or "Line Ahead" => 1,
		"複縦陣" or "Double Line" => 2,
		"輪形陣" or "Diamond" => 3,
		"梯形陣" or "Echelon" => 4,
		"単横陣" or "Line Abreast" => 5,
		"警戒陣" or "Vanguard" => 6,
		"第一警戒航行序列" or "First Cruising Formation" => 11,
		"第二警戒航行序列" or "Second Cruising Formation" => 12,
		"第三警戒航行序列" or "Third Cruising Formation" => 13,
		"第四警戒航行序列" or "Fourth Cruising Formation" => 14,
		_ => -1,
	};

	/// <summary>
	/// 陣形を表す文字列(短縮版)を取得します。
	/// </summary>
	public static string GetFormationShort(int id) => id switch
	{
		1 => ConstantsRes.LineAheadShort,
		2 => ConstantsRes.DoubleLineShort,
		3 => ConstantsRes.DiamondShort,
		4 => ConstantsRes.EchelonShort,
		5 => ConstantsRes.LineAbreastShort,
		6 => ConstantsRes.VanguardShort,
		11 => ConstantsRes.FirstPatrolFormationShort,
		12 => ConstantsRes.SecondPatrolFormationShort,
		13 => ConstantsRes.ThirdPatrolFormationShort,
		14 => ConstantsRes.FourthPatrolFormationShort,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 交戦形態を表す文字列を取得します。
	/// </summary>
	public static string GetEngagementForm(int id) => GetEngagementForm((EngagementType)id);

	/// <summary>
	/// 交戦形態を表す文字列を取得します。
	/// </summary>
	public static string GetEngagementForm(EngagementType id) => id switch
	{
		EngagementType.Parallel => ConstantsRes.Parallel,
		EngagementType.HeadOn => ConstantsRes.HeadOn,
		EngagementType.TAdvantage => ConstantsRes.GreenT,
		EngagementType.TDisadvantage => ConstantsRes.RedT,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 交戦形態を表す文字列を取得します。
	/// </summary>
	public static string GetEngagementFormShort(int id) => id switch
	{
		1 => ConstantsRes.Parallel,
		2 => ConstantsRes.HeadOn,
		3 => ConstantsRes.TAdvantage,
		4 => ConstantsRes.TDisadvantage,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 索敵結果を表す文字列を取得します。
	/// </summary>
	public static string GetSearchingResult(int id) => GetSearchingResult((DetectionType)id);

	/// <summary>
	/// 索敵結果を表す文字列を取得します。
	/// </summary>
	public static string GetSearchingResult(DetectionType id) => id switch
	{
		DetectionType.Success => ConstantsRes.Success,
		DetectionType.SuccessNoReturn => ConstantsRes.SuccessNoReturn,
		DetectionType.NoReturn => ConstantsRes.NoReturn,
		DetectionType.Failure => ConstantsRes.Failure,
		DetectionType.SuccessNoPlane => ConstantsRes.SuccessNoPlane,
		DetectionType.FailureNoPlane => ConstantsRes.FailureNoPlane,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 索敵結果を表す文字列(短縮版)を取得します。
	/// </summary>
	public static string GetSearchingResultShort(int id) => id switch
	{
		1 => ConstantsRes.Success,
		2 => ConstantsRes.Success + "△",
		3 => ConstantsRes.NoReturn,
		4 => ConstantsRes.Failure,
		5 => ConstantsRes.Success,
		6 => ConstantsRes.Failure,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 制空戦の結果を表す文字列を取得します。
	/// </summary>
	public static string GetAirSuperiority(int id) => GetAirSuperiority((AirState)id);

	/// <summary>
	/// 制空戦の結果を表す文字列を取得します。
	/// </summary>
	public static string GetAirSuperiority(AirState id) => id switch
	{
		AirState.Parity => ConstantsRes.AirParity,
		AirState.Supremacy => ConstantsRes.AirSupremacy,
		AirState.Superiority => ConstantsRes.AirSuperiority,
		AirState.Denial => ConstantsRes.AirDenialNew,
		AirState.Incapability => ConstantsRes.AirIncapability,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 勝利ランクを表すIDを取得します。
	/// </summary>
	public static BattleRank GetWinRank(string rank) => rank.ToUpper() switch
	{
		"E" => BattleRank.E,
		"D" => BattleRank.D,
		"C" => BattleRank.C,
		"B" => BattleRank.B,
		"A" => BattleRank.A,
		"S" => BattleRank.S,
		"SS" => BattleRank.SS,
		_ => BattleRank.Any,
	};

	/// <summary>
	/// 勝利ランクを表す文字列を取得します。
	/// </summary>
	public static string GetWinRank(BattleRank rank) => rank switch
	{
		BattleRank.E => "E",
		BattleRank.D => "D",
		BattleRank.C => "C",
		BattleRank.B => "B",
		BattleRank.A => "A",
		BattleRank.S => "S",
		BattleRank.SS => "SS",
		_ => ConstantsRes.Unknown,
	};

	#endregion


	#region その他

	/// <summary>
	/// 資源の名前を取得します。
	/// </summary>
	/// <param name="materialID">資源のID。</param>
	/// <returns>資源の名前。</returns>
	public static string GetMaterialName(int materialID) => materialID switch
	{
		1 => ConstantsRes.Fuel,
		2 => ConstantsRes.Ammo,
		3 => ConstantsRes.Steel,
		4 => ConstantsRes.Baux,
		5 => ConstantsRes.Flamethrower,
		6 => ConstantsRes.Bucket,
		7 => ConstantsRes.DevMat,
		8 => ConstantsRes.ImpMat,
		_ => ConstantsRes.Unknown,
	};


	/// <summary>
	/// 階級を表す文字列を取得します。
	/// </summary>
	public static string GetAdmiralRank(int id) => id switch
	{
		1 => ConstantsRes.FleetAdmiral,
		2 => ConstantsRes.FourStarAdmiral,
		3 => ConstantsRes.ViceAdmiral,
		4 => ConstantsRes.RearAdmiral,
		5 => ConstantsRes.Captain,
		6 => ConstantsRes.Commander,
		7 => ConstantsRes.NoviceCommander,
		8 => ConstantsRes.LtCmdr,
		9 => ConstantsRes.ViceLtCmdr,
		10 => ConstantsRes.NoviceLtCmdr,
		_ => ConstantsRes.Admiral,
	};

	/// <summary>
	/// 任務の発生タイプを表す文字列を取得します。
	/// </summary>
	public static string GetQuestType(int id) => id switch
	{
		1 => ConstantsRes.Daily, //デイリー
		2 => ConstantsRes.Weekly, //ウィークリー
		3 => ConstantsRes.Monthly, //マンスリー
		4 => ConstantsRes.QuestOneTime, //単発
		5 => ConstantsRes.QuestOther, //その他(輸送5/空母3)

		// 以下、厳密には LabelType だが面倒なので
		101 => ConstantsRes.QuestYearly + "1",
		102 => ConstantsRes.QuestYearly + "2",
		103 => ConstantsRes.QuestYearly + "3",
		104 => ConstantsRes.QuestYearly + "4",
		105 => ConstantsRes.QuestYearly + "5",
		106 => ConstantsRes.QuestYearly + "6",
		107 => ConstantsRes.QuestYearly + "7",
		108 => ConstantsRes.QuestYearly + "8",
		109 => ConstantsRes.QuestYearly + "9",
		110 => ConstantsRes.QuestYearly + "10",
		111 => ConstantsRes.QuestYearly + "11",
		112 => ConstantsRes.QuestYearly + "12",

		_ => ConstantsRes.Question
	};

	public static string GetQuestLabelType(int id) => id switch
	{
		1 => ConstantsRes.QuestOneTime,
		2 => ConstantsRes.Daily,
		3 => ConstantsRes.Weekly,
		6 => ConstantsRes.Monthly,
		7 => ConstantsRes.QuestOther,
		101 => ConstantsRes.QuestYearly + ConstantsRes.January + ConstantsRes.Reset,
		102 => ConstantsRes.QuestYearly + ConstantsRes.February + ConstantsRes.Reset,
		103 => ConstantsRes.QuestYearly + ConstantsRes.March + ConstantsRes.Reset,
		104 => ConstantsRes.QuestYearly + ConstantsRes.April + ConstantsRes.Reset,
		105 => ConstantsRes.QuestYearly + ConstantsRes.May + ConstantsRes.Reset,
		106 => ConstantsRes.QuestYearly + ConstantsRes.June + ConstantsRes.Reset,
		107 => ConstantsRes.QuestYearly + ConstantsRes.July + ConstantsRes.Reset,
		108 => ConstantsRes.QuestYearly + ConstantsRes.August + ConstantsRes.Reset,
		109 => ConstantsRes.QuestYearly + ConstantsRes.September + ConstantsRes.Reset,
		110 => ConstantsRes.QuestYearly + ConstantsRes.October + ConstantsRes.Reset,
		111 => ConstantsRes.QuestYearly + ConstantsRes.November + ConstantsRes.Reset,
		112 => ConstantsRes.QuestYearly + ConstantsRes.December + ConstantsRes.Reset,
		_ => id.ToString(),
	};


	/// <summary>
	/// 任務のカテゴリを表す文字列を取得します。
	/// </summary>
	public static string GetQuestCategory(int id) => id switch
	{
		1 => ConstantsRes.Formation,
		2 or 8 or 9 or 10 => ConstantsRes.Sortie,
		3 => ConstantsRes.Practice,
		4 => ConstantsRes.Expedition,
		5 => ConstantsRes.Supply, //入渠も含むが、文字数の関係
		6 or 11 => ConstantsRes.Construction,
		7 => ConstantsRes.Modernization,
		_ => ConstantsRes.Unknown,
	};

	/// <summary>
	/// 遠征の結果を表す文字列を取得します。
	/// </summary>
	public static string GetExpeditionResult(int value) => value switch
	{
		0 => ConstantsRes.Failure,
		1 => ConstantsRes.Success,
		2 => ConstantsRes.GreatSuccess,
		_ => ConstantsRes.Unknown,
	};

	public static string GetCombinedFleet(int value) => GetCombinedFleet((FleetType)value);

	/// <summary>
	/// 連合艦隊の編成名を表す文字列を取得します。
	/// </summary>
	public static string GetCombinedFleet(FleetType value) => value switch
	{
		FleetType.Single => ConstantsRes.NormalFleet,
		FleetType.Carrier => ConstantsRes.CarrierTF,
		FleetType.Surface => ConstantsRes.SurfaceTF,
		FleetType.Transport => ConstantsRes.TransportFleet,
		_ => ConstantsRes.Unknown,
	};

	#endregion

	#region Servers

	public static string GetRank(int num) => num switch
	{
		1 => "Marshal Admiral",
		2 => "Admiral",
		3 => "Vice-Admiral",
		4 => "Rear-Admiral",
		5 => "Captain",
		6 => "Commander",
		7 => "Commander (Novice)",
		8 => "Lieutenant-Commander",
		9 => "Vice Lieutenant-Commander",
		10 => "Lieutenant-Commander (Novice)",
		_ => "",
	};

	#endregion
}
