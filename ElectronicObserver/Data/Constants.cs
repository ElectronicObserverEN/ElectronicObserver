using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

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

	/// <summary>
	/// 海防艦　　　　→　海防 <br />
	/// 駆逐艦　　　　→　駆逐 <br />
	/// 軽巡洋艦　　　→　軽巡 <br />
	/// 重雷装巡洋艦　→　雷巡 <br />
	/// 重巡洋艦　　　→　重巡 <br />
	/// 航空巡洋艦　　→　航巡 <br />
	/// 軽空母　　　　→　軽空 <br />
	/// 巡洋戦艦　　　→　高戦 <br />
	/// 戦艦　　　　　→　戦艦 <br />
	/// 航空戦艦　　　→　航戦 <br />
	/// 正規空母　　　→　正空 <br />
	/// 弩級戦艦　　　→　戦艦 <br />
	/// 潜水艦　　　　→　潜水 <br />
	/// 潜水空母　　　→　潜空 <br />
	/// 補給艦　　　　→　補給 <br />
	/// 水上機母艦　　→　水母 <br />
	/// 揚陸艦　　　　→　揚陸 <br />
	/// 装甲空母　　　→　装空 <br />
	/// 工作艦　　　　→　工作 <br />
	/// 潜水母艦　　　→　潜母 <br />
	/// 練習巡洋艦　　→　練巡 <br />
	/// </summary>
	public static string ShipTypeShort(ShipTypes shiptype) => shiptype switch
	{
		ShipTypes.Escort => "海防",
		ShipTypes.Destroyer => "駆逐",
		ShipTypes.LightCruiser => "軽巡",
		ShipTypes.TorpedoCruiser => "雷巡",
		ShipTypes.HeavyCruiser => "重巡",
		ShipTypes.AviationCruiser => "航巡",
		ShipTypes.LightAircraftCarrier => "軽空",
		ShipTypes.Battlecruiser => "高戦",
		ShipTypes.Battleship => "戦艦",
		ShipTypes.AviationBattleship => "航戦",
		ShipTypes.AircraftCarrier => "正空",
		ShipTypes.SuperDreadnoughts => "戦艦",
		ShipTypes.Submarine => "潜水",
		ShipTypes.SubmarineAircraftCarrier => "潜空",
		ShipTypes.Transport => "補給",
		ShipTypes.SeaplaneTender => "水母",
		ShipTypes.AmphibiousAssaultShip => "揚陸",
		ShipTypes.ArmoredAircraftCarrier => "装空",
		ShipTypes.RepairShip => "工作",
		ShipTypes.SubmarineTender => "潜母",
		ShipTypes.TrainingCruiser => "練巡",
		ShipTypes.FleetOiler => "補給",
		_ => "IX",
	};

	/// <summary>
	/// 艦種略号を取得します。
	/// </summary>
	// todo: need to figure out how to do this via resx files
	public static string ShipTypeShortEnglish(ShipTypes shiptype) => shiptype switch
	{
		ShipTypes.Escort => "DE",
		ShipTypes.Destroyer => "DD",
		ShipTypes.LightCruiser => "CL",
		ShipTypes.TorpedoCruiser => "CLT",
		ShipTypes.HeavyCruiser => "CA",
		ShipTypes.AviationCruiser => "CAV",
		ShipTypes.LightAircraftCarrier => "CVL",
		ShipTypes.Battlecruiser => "BC",
		ShipTypes.Battleship => "BB",
		ShipTypes.AviationBattleship => "BBV",
		ShipTypes.AircraftCarrier => "CV",
		ShipTypes.SuperDreadnoughts => "BB",
		ShipTypes.Submarine => "SS",
		ShipTypes.SubmarineAircraftCarrier => "SSV",
		ShipTypes.Transport => "AP",
		ShipTypes.SeaplaneTender => "AV",
		ShipTypes.AmphibiousAssaultShip => "LHA",
		ShipTypes.ArmoredAircraftCarrier => "CVB",
		ShipTypes.RepairShip => "AR",
		ShipTypes.SubmarineTender => "AS",
		ShipTypes.TrainingCruiser => "CT",
		ShipTypes.FleetOiler => "AO",
		_ => "IX",
	};

	/// <summary>
	/// 艦型を表す文字列を取得します。
	/// </summary>
	public static string GetShipClass(ShipClass id, ShipId shipId = ShipId.Souya645)
		=> GetShipClass((int)id, shipId);


	/// <summary>
	/// 艦型を表す文字列を取得します。
	/// </summary>
	public static string GetShipClass(int id, ShipId shipId = ShipId.Unknown) =>
		KCDatabase.Instance.Translation.Ship.Class(GetShipClassUntranslated(id, shipId));

	/// <summary>
	/// 艦型を表す文字列を取得します。
	/// </summary>
	private static string GetShipClassUntranslated(int id, ShipId shipId = ShipId.Unknown) => id switch
	{
		1 => "綾波型",
		2 => "伊勢型",
		3 => "加賀型",
		4 => "球磨型",
		5 => "暁型",
		6 => "金剛型",
		7 => "古鷹型",
		8 => "高雄型",
		9 => "最上型",
		10 => "初春型",
		11 => "祥鳳型",
		12 => "吹雪型",
		13 => "青葉型",
		14 => "赤城型",
		15 => "千歳型",
		16 => "川内型",
		17 => "蒼龍型",
		18 => "朝潮型",
		19 => "長門型",
		20 => "長良型",
		21 => "天龍型",
		22 => "島風型",
		23 => "白露型",
		24 => "飛鷹型",
		25 => "飛龍型",
		26 => "扶桑型",
		27 => "鳳翔型",
		28 => "睦月型",
		29 => "妙高型",
		30 => "陽炎型",
		31 => "利根型",
		32 => "龍驤型",
		33 => "翔鶴型",
		34 => "夕張型",
		35 => "海大VI型",
		36 => "巡潜乙型改二",
		37 => "大和型",
		38 => "夕雲型",
		39 => "巡潜乙型",
		40 => "巡潜3型",
		41 => "阿賀野型",
		42 => "「霧」",
		43 => "大鳳型",
		44 => "潜特型(伊400型潜水艦)",
		45 => "特種船丙型",
		46 => "三式潜航輸送艇",
		47 => "Bismarck級",
		48 => "Z1型",
		49 => "工作艦",
		50 => "大鯨型",
		51 => "龍鳳型",
		52 => "大淀型",
		53 => "雲龍型",
		54 => "秋月型",
		55 => "Admiral Hipper級",
		56 => "香取型",
		57 => "UボートIXC型",
		58 => "V.Veneto級",
		59 => "秋津洲型",
		60 => "改風早型",
		61 => "Maestrale級",
		62 => "瑞穂型",
		63 => "Graf Zeppelin級",
		64 => "Zara級",
		65 => "Iowa級",
		66 => "神風型",
		67 => "Queen Elizabeth級",
		68 => "Aquila級",
		69 => "Lexington級",
		70 => "C.Teste級",
		71 => "巡潜甲型改二",
		72 => "神威型",
		73 => "Гангут級",
		74 => "占守型",
		75 => "春日丸級",
		76 => "大鷹型",
		77 => "択捉型",
		78 => "Ark Royal級",
		79 => "Richelieu級",
		80 => "Guglielmo Marconi級",
		81 => "Ташкент級",
		82 => "J級",
		83 => "Casablanca級",
		84 => "Essex級",
		85 => "日振型",
		86 => "呂号潜水艦",
		87 => "John C.Butler級",
		88 => "Nelson級",
		89 => "Gotland級",
		90 => "日進型",
		91 => "Fletcher級",
		92 => "L.d.S.D.d.Abruzzi級",
		93 => "Colorado級",
		94 => "御蔵型",
		95 => "Northampton級",
		96 => "Perth級",
		97 => "陸軍特種船(R1)",
		98 => "De Ruyter級",
		99 => "Atlanta級",
		100 => "迅鯨型",
		101 => "松型",
		102 => "South Dakota級",
		103 => "巡潜丙型",
		104 => "丁型海防艦",
		105 => "Yorktown級",
		106 => "St. Louis級",
		107 => "North Carolina級",
		108 => "Town級",
		109 => "潜高型",
		110 => "Brooklyn級",
		111 when shipId is ShipId.Souya699 => "耐氷型雑用運送艦",
		111 when shipId is ShipId.Souya645 => "LL01",
		111 when shipId is ShipId.Souya650 => "PL107",
		112 => "Illustrious級",
		113 => "Conte di Cavour級",
		114 => "Gato級",
		115 => "特2TL型",
		116 => "Independence級",
		117 => "鵜来型",
		118 => "Ranger級",
		119 => "特種船M丙型",
		120 => "二等輸送艦",
		121 => "New Orleans級",
		122 => "Salmon級",
		123 => "改敷島型",
		124 => "Marcello級",
		125 => "Nevada級",
		126 => "改氷川丸級",
		127 => "巡潜乙型改一",
		128 => "La Galissonnière級",
		129 => "Mogador級",
		130 => "大泊型",
		131 => "Киров級",
		132 => "特1TL型",
		_ => "不明",
	};

	#endregion


	#region 出撃

	/// <summary>
	/// マップ上のセルでのイベントを表す文字列を取得します。
	/// </summary>
	public static string GetMapEventID(int value)
	{

		switch (value)
		{

			case 0:
				return ConstantsRes.StartNode;
			case 1:
				return ConstantsRes.NoEvent;
			case 2:
				return ConstantsRes.Resources;
			case 3:
				return ConstantsRes.Maelstrom;
			case 4:
				return ConstantsRes.Battle;
			case 5:
				return ConstantsRes.Boss;
			case 6:
				return ConstantsRes.Imagination;
			case 7:
				return ConstantsRes.AirBattle;
			case 8:
				return ConstantsRes.LargeResourceNode;
			case 9:
				return ConstantsRes.LandNode;
			case 10:
				return ConstantsRes.Anchorage; // todo add to res
			default:
				return ConstantsRes.Unknown;
		}
	}

	/// <summary>
	/// マップ上のセルでのイベント種別を表す文字列を取得します。
	/// </summary>
	public static string GetMapEventKind(int value)
	{

		switch (value)
		{
			case 0:
				return ConstantsRes.NoBattle;
			case 1:
				return ConstantsRes.DayBattle;
			case 2:
				return ConstantsRes.NightBattle;
			case 3:
				return ConstantsRes.NightDayBattle;       // 対通常?
			case 4:
				return ConstantsRes.AirBattle;
			case 5:
				return ConstantsRes.EnemyCombinedFleet;
			case 6:
				return ConstantsRes.AirRaid;
			case 7:
				return ConstantsRes.NightDayBattle;       // 対連合
			case 8:
				return ConstantsRes.RadarAmbush;
			default:
				return ConstantsRes.Unknown;
		}
	}


	/// <summary>
	/// 海域難易度を表す文字列を取得します。
	/// </summary>
	public static string GetDifficulty(int value)
	{

		switch (value)
		{
			case -1:
				return ConstantsRes.NoNode;
			case 0:
				return ConstantsRes.Unselected;
			case 1:
				return ConstantsRes.CasualDifficulty;
			case 2:
				return ConstantsRes.EasyDifficulty;
			case 3:
				return ConstantsRes.MediumDifficulty;
			case 4:
				return ConstantsRes.HardDifficulty;
			default:
				return ConstantsRes.Unknown;
		}
	}

	/// <summary>
	/// 海域難易度を表す数値を取得します。
	/// </summary>
	public static int GetDifficulty(string value)
	{

		switch (value)
		{
			case "未選択":
			case "Unselected":
				return 0;
			case "丁":
			case "Casual":
				return 1;
			case "丙":
			case "Easy":
				return 2;
			case "乙":
			case "Medium":
				return 3;
			case "甲":
			case "Hard":
				return 4;
			default:
				return -1;
		}
	}

	/// <summary>
	/// 空襲被害の状態を表す文字列を取得します。
	/// </summary>
	public static string GetAirRaidDamage(int value)
	{
		switch (value)
		{
			case 1:
				return ConstantsRes.ABResourcesDamaged;
			case 2:
				return ConstantsRes.ABResources_AndPlanesDamaged;
			case 3:
				return ConstantsRes.ABPlanesDamaged;
			case 4:
				return ConstantsRes.ABNoDamage;
			default:
				return ConstantsRes.ABNoAirRaid;
		}
	}

	/// <summary>
	/// 空襲被害の状態を表す文字列を取得します。(短縮版)
	/// </summary>
	public static string GetAirRaidDamageShort(int value)
	{
		switch (value)
		{
			case 1:
				return ConstantsRes.ABShortResources;
			case 2:
				return ConstantsRes.ABShortResourcesPlanes;
			case 3:
				return ConstantsRes.ABShortPlanes;
			case 4:
				return ConstantsRes.ABShortNoDamage;
			default:
				return "-";
		}
	}


	#endregion


	#region 戦闘

	public static string GetFormation(FormationType id) => GetFormation((int)id);

	/// <summary>
	/// 陣形を表す文字列を取得します。
	/// </summary>
	public static string GetFormation(int id)
	{
		switch (id)
		{
			case 1:
				return ConstantsRes.LineAhead;
			case 2:
				return ConstantsRes.DoubleLine;
			case 3:
				return ConstantsRes.Diamond;
			case 4:
				return ConstantsRes.Echelon;
			case 5:
				return ConstantsRes.LineAbreast;
			case 6:
				return ConstantsRes.Vanguard;
			case 11:
				return ConstantsRes.FirstPatrolFormation;
			case 12:
				return ConstantsRes.SecondPatrolFormation;
			case 13:
				return ConstantsRes.ThirdPatrolFormation;
			case 14:
				return ConstantsRes.FourthPatrolFormation;
			default:
				return ConstantsRes.Unknown;
		}
	}

	/// <summary>
	/// 陣形を表す数値を取得します。
	/// </summary>
	public static int GetFormation(string value)
	{
		switch (value)
		{
			case "単縦陣":
			case "Line Ahead":
				return 1;
			case "複縦陣":
			case "Double Line":
				return 2;
			case "輪形陣":
			case "Diamond":
				return 3;
			case "梯形陣":
			case "Echelon":
				return 4;
			case "単横陣":
			case "Line Abreast":
				return 5;
			case "警戒陣":
			case "Vanguard":
				return 6;
			case "第一警戒航行序列":
			case "First Cruising Formation":
				return 11;
			case "第二警戒航行序列":
			case "Second Cruising Formation":
				return 12;
			case "第三警戒航行序列":
			case "Third Cruising Formation":
				return 13;
			case "第四警戒航行序列":
			case "Fourth Cruising Formation":
				return 14;
			default:
				return -1;
		}
	}

	/// <summary>
	/// 陣形を表す文字列(短縮版)を取得します。
	/// </summary>
	public static string GetFormationShort(int id)
	{
		switch (id)
		{
			case 1:
				return ConstantsRes.LineAheadShort;
			case 2:
				return ConstantsRes.DoubleLineShort;
			case 3:
				return ConstantsRes.DiamondShort;
			case 4:
				return ConstantsRes.EchelonShort;
			case 5:
				return ConstantsRes.LineAbreastShort;
			case 6:
				return ConstantsRes.VanguardShort;
			case 11:
				return ConstantsRes.FirstPatrolFormationShort;
			case 12:
				return ConstantsRes.SecondPatrolFormationShort;
			case 13:
				return ConstantsRes.ThirdPatrolFormationShort;
			case 14:
				return ConstantsRes.FourthPatrolFormationShort;
			default:
				return ConstantsRes.Unknown;
		}
	}

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
	public static string GetEngagementFormShort(int id)
	{
		switch (id)
		{
			case 1:
				return ConstantsRes.Parallel;
			case 2:
				return ConstantsRes.HeadOn;
			case 3:
				return ConstantsRes.TAdvantage;
			case 4:
				return ConstantsRes.TDisadvantage;
			default:
				return ConstantsRes.Unknown;
		}
	}

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
	public static string GetSearchingResultShort(int id)
	{
		switch (id)
		{
			case 1:
				return ConstantsRes.Success;
			case 2:
				return ConstantsRes.Success + "△";
			case 3:
				return ConstantsRes.NoReturn;
			case 4:
				return ConstantsRes.Failure;
			case 5:
				return ConstantsRes.Success;
			case 6:
				return ConstantsRes.Failure;
			default:
				return ConstantsRes.Unknown;
		}
	}

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
	public static string GetWinRank(int rank)
	{
		switch (rank)
		{
			case 1:
				return "E";
			case 2:
				return "D";
			case 3:
				return "C";
			case 4:
				return "B";
			case 5:
				return "A";
			case 6:
				return "S";
			case 7:
				return "SS";
			default:
				return ConstantsRes.Unknown;
		}
	}

	#endregion


	#region その他

	/// <summary>
	/// 資源の名前を取得します。
	/// </summary>
	/// <param name="materialID">資源のID。</param>
	/// <returns>資源の名前。</returns>
	public static string GetMaterialName(int materialID)
	{

		switch (materialID)
		{
			case 1:
				return ConstantsRes.Fuel;
			case 2:
				return ConstantsRes.Ammo;
			case 3:
				return ConstantsRes.Steel;
			case 4:
				return ConstantsRes.Baux;
			case 5:
				return ConstantsRes.Flamethrower;
			case 6:
				return ConstantsRes.Bucket;
			case 7:
				return ConstantsRes.DevMat;
			case 8:
				return ConstantsRes.ImpMat;
			default:
				return ConstantsRes.Unknown;
		}
	}


	/// <summary>
	/// 階級を表す文字列を取得します。
	/// </summary>
	public static string GetAdmiralRank(int id)
	{
		switch (id)
		{
			case 1:
				return ConstantsRes.FleetAdmiral;
			case 2:
				return ConstantsRes.FourStarAdmiral;
			case 3:
				return ConstantsRes.ViceAdmiral;
			case 4:
				return ConstantsRes.RearAdmiral;
			case 5:
				return ConstantsRes.Captain;
			case 6:
				return ConstantsRes.Commander;
			case 7:
				return ConstantsRes.NoviceCommander;
			case 8:
				return ConstantsRes.LtCmdr;
			case 9:
				return ConstantsRes.ViceLtCmdr;
			case 10:
				return ConstantsRes.NoviceLtCmdr;
			default:
				return ConstantsRes.Admiral;
		}
	}

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

	public static string GetQuestLabelType(int id)
	{
		switch (id)
		{
			case 1:
				return ConstantsRes.QuestOneTime;
			case 2:
				return ConstantsRes.Daily;
			case 3:
				return ConstantsRes.Weekly;
			case 6:
				return ConstantsRes.Monthly;
			case 7:
				return ConstantsRes.QuestOther;
			case 101:
				return ConstantsRes.QuestYearly + ConstantsRes.January + ConstantsRes.Reset;
			case 102:
				return ConstantsRes.QuestYearly + ConstantsRes.February + ConstantsRes.Reset;
			case 103:
				return ConstantsRes.QuestYearly + ConstantsRes.March + ConstantsRes.Reset;
			case 104:
				return ConstantsRes.QuestYearly + ConstantsRes.April + ConstantsRes.Reset;
			case 105:
				return ConstantsRes.QuestYearly + ConstantsRes.May + ConstantsRes.Reset;
			case 106:
				return ConstantsRes.QuestYearly + ConstantsRes.June + ConstantsRes.Reset;
			case 107:
				return ConstantsRes.QuestYearly + ConstantsRes.July + ConstantsRes.Reset;
			case 108:
				return ConstantsRes.QuestYearly + ConstantsRes.August + ConstantsRes.Reset;
			case 109:
				return ConstantsRes.QuestYearly + ConstantsRes.September + ConstantsRes.Reset;
			case 110:
				return ConstantsRes.QuestYearly + ConstantsRes.October + ConstantsRes.Reset;
			case 111:
				return ConstantsRes.QuestYearly + ConstantsRes.November + ConstantsRes.Reset;
			case 112:
				return ConstantsRes.QuestYearly + ConstantsRes.December + ConstantsRes.Reset;

			default:
				return id.ToString();
		}

	}


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
	public static string GetExpeditionResult(int value)
	{
		switch (value)
		{
			case 0:
				return ConstantsRes.Failure;
			case 1:
				return ConstantsRes.Success;
			case 2:
				return ConstantsRes.GreatSuccess;
			default:
				return ConstantsRes.Unknown;
		}
	}

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

	public static string getRank(int num)
	{
		switch (num)
		{
			case 1:
				return "Marshal Admiral";
			case 2:
				return "Admiral";
			case 3:
				return "Vice-Admiral";
			case 4:
				return "Rear-Admiral";
			case 5:
				return "Captain";
			case 6:
				return "Commander";
			case 7:
				return "Commander (Novice)";
			case 8:
				return "Lieutenant-Commander";
			case 9:
				return "Vice Lieutenant-Commander";
			case 10:
				return "Lieutenant-Commander (Novice)";
			default:
				return "";
		}
	}

	#endregion
}
