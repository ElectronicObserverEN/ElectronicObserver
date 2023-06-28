using ElectronicObserverTypes;

namespace ElectronicObserver.Data;

public static class Constants
{

	#region 艦船・装備

	/// <summary>
	/// 艦船の速力を表す文字列を取得します。
	/// </summary>
	public static string GetSpeed(int value)
	{
		switch (value)
		{
			case 0:
				return ConstantsRes.Land;
			case 5:
				return ConstantsRes.Slow;
			case 10:
				return ConstantsRes.Fast;
			case 15:
				return ConstantsRes.FastPlus;
			case 20:
				return ConstantsRes.Fastest;
			default:
				return ConstantsRes.Unknown;
		}
	}

	/// <summary>
	/// 射程を表す文字列を取得します。
	/// </summary>
	public static string GetRange(int value)
	{
		switch (value)
		{
			case 0:
				return ConstantsRes.None;
			case 1:
				return ConstantsRes.Short;
			case 2:
				return ConstantsRes.Medium;
			case 3:
				return ConstantsRes.Long;
			case 4:
				return ConstantsRes.VeryLong;
			case 5:
				return ConstantsRes.VeryLong + "+";
			default:
				return ConstantsRes.Unknown;
		}
	}

	/// <summary>
	/// 艦船のレアリティを表す文字列を取得します。
	/// </summary>
	public static string GetShipRarity(int value)
	{
		switch (value)
		{
			case 0:
				return ConstantsRes.Red;
			case 1:
				return ConstantsRes.Indigo;
			case 2:
				return ConstantsRes.Blue;
			case 3:
				return ConstantsRes.Aqua;
			case 4:
				return ConstantsRes.Silver;
			case 5:
				return ConstantsRes.Gold;
			case 6:
				return ConstantsRes.Rainbow;
			case 7:
				return ConstantsRes.SRainbow;
			case 8:
				return ConstantsRes.Sakura;
			default:
				return ConstantsRes.Unknown;
		}
	}

	/// <summary>
	/// 装備のレアリティを表す文字列を取得します。
	/// </summary>
	public static string GetEquipmentRarity(int value)
	{
		switch (value)
		{
			case 0:
				return ConstantsRes.Common;
			case 1:
				return ConstantsRes.Rare;
			case 2:
				return ConstantsRes.Holo;
			case 3:
				return ConstantsRes.SHolo;
			case 4:
				return ConstantsRes.SSHolo;
			case 5:
				return ConstantsRes.EXHolo;
			case 6:
				return ConstantsRes.SSHoloPlus;
			case 7:
				return ConstantsRes.SSPlusPlus;
			default:
				return ConstantsRes.Unknown;
		}
	}

	/// <summary>
	/// 装備のレアリティの画像インデックスを取得します。
	/// </summary>
	public static int GetEquipmentRarityID(int value)
	{
		switch (value)
		{
			case 0:
				return 1;
			case 1:
				return 2;
			case 2:
				return 3;
			case 3:
				return 4;
			case 4:
				return 5;
			case 5:
				return 6;
			case 6:
				return 7;
			case 7:
				return 8;
			default:
				return 0;
		}
	}


	/// <summary>
	/// 艦船のボイス設定フラグを表す文字列を取得します。
	/// </summary>
	public static string GetVoiceFlag(int value)
	{

		switch (value)
		{
			case 0:
				return "-";
			case 1:
				return ConstantsRes.Hourly;
			case 2:
				return ConstantsRes.Idle;
			case 3:
				return ConstantsRes.Idle + " + " + ConstantsRes.Hourly;
			case 4:
				return ConstantsRes.SpecialIdle;
			case 5:
				return ConstantsRes.Idle + " + " + ConstantsRes.SpecialIdle;
			case 6:
				return ConstantsRes.Hourly + " + " + ConstantsRes.SpecialIdle;
			case 7:
				return ConstantsRes.Idle + " + " + ConstantsRes.Hourly + " + " + ConstantsRes.SpecialIdle;
			default:
				return ConstantsRes.Unknown;
		}
	}


	/// <summary>
	/// 艦船の損傷度合いを表す文字列を取得します。
	/// </summary>
	/// <param name="hprate">現在HP/最大HPで表される割合。</param>
	/// <param name="isPractice">演習かどうか。</param>
	/// <param name="isLandBase">陸上基地かどうか。</param>
	/// <param name="isEscaped">退避中かどうか。</param>
	/// <returns></returns>
	public static string GetDamageState(double hprate, bool isPractice = false, bool isLandBase = false, bool isEscaped = false)
	{

		if (isEscaped)
			return ConstantsRes.Retreated;
		else if (hprate <= 0.0)
			return isPractice ? ConstantsRes.Withdrawn : (!isLandBase ? ConstantsRes.Sunk : ConstantsRes.Destroyed);
		else if (hprate <= 0.25)
			return !isLandBase ? ConstantsRes.CriticalDamage : ConstantsRes.Damaged;
		else if (hprate <= 0.5)
			return !isLandBase ? ConstantsRes.ModerateDamage : ConstantsRes.Injured;
		else if (hprate <= 0.75)
			return !isLandBase ? ConstantsRes.LightDamage : ConstantsRes.Disorder;
		else if (hprate < 1.0)
			return ConstantsRes.Healthy;
		else
			return ConstantsRes.Unhurt;

	}


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
	public static string GetShipClass(int id, ShipId shipId = ShipId.Unknown) => KCDatabase.Instance.Translation.Ship.Class(GetShipClassUntranslated(id, shipId));

	/// <summary>
	/// 艦型を表す文字列を取得します。
	/// </summary>
	private static string GetShipClassUntranslated(int id, ShipId shipId = ShipId.Unknown)
	{
		switch (id)
		{
			case 1: return "綾波型";
			case 2: return "伊勢型";
			case 3: return "加賀型";
			case 4: return "球磨型";
			case 5: return "暁型";
			case 6: return "金剛型";
			case 7: return "古鷹型";
			case 8: return "高雄型";
			case 9: return "最上型";
			case 10: return "初春型";
			case 11: return "祥鳳型";
			case 12: return "吹雪型";
			case 13: return "青葉型";
			case 14: return "赤城型";
			case 15: return "千歳型";
			case 16: return "川内型";
			case 17: return "蒼龍型";
			case 18: return "朝潮型";
			case 19: return "長門型";
			case 20: return "長良型";
			case 21: return "天龍型";
			case 22: return "島風型";
			case 23: return "白露型";
			case 24: return "飛鷹型";
			case 25: return "飛龍型";
			case 26: return "扶桑型";
			case 27: return "鳳翔型";
			case 28: return "睦月型";
			case 29: return "妙高型";
			case 30: return "陽炎型";
			case 31: return "利根型";
			case 32: return "龍驤型";
			case 33: return "翔鶴型";
			case 34: return "夕張型";
			case 35: return "海大VI型";
			case 36: return "巡潜乙型改二";
			case 37: return "大和型";
			case 38: return "夕雲型";
			case 39: return "巡潜乙型";
			case 40: return "巡潜3型";
			case 41: return "阿賀野型";
			case 42: return "「霧」";
			case 43: return "大鳳型";
			case 44: return "潜特型(伊400型潜水艦)";
			case 45: return "特種船丙型";
			case 46: return "三式潜航輸送艇";
			case 47: return "Bismarck級";
			case 48: return "Z1型";
			case 49: return "工作艦";
			case 50: return "大鯨型";
			case 51: return "龍鳳型";
			case 52: return "大淀型";
			case 53: return "雲龍型";
			case 54: return "秋月型";
			case 55: return "Admiral Hipper級";
			case 56: return "香取型";
			case 57: return "UボートIXC型";
			case 58: return "V.Veneto級";
			case 59: return "秋津洲型";
			case 60: return "改風早型";
			case 61: return "Maestrale級";
			case 62: return "瑞穂型";
			case 63: return "Graf Zeppelin級";
			case 64: return "Zara級";
			case 65: return "Iowa級";
			case 66: return "神風型";
			case 67: return "Queen Elizabeth級";
			case 68: return "Aquila級";
			case 69: return "Lexington級";
			case 70: return "C.Teste級";
			case 71: return "巡潜甲型改二";
			case 72: return "神威型";
			case 73: return "Гангут級";
			case 74: return "占守型";
			case 75: return "春日丸級";
			case 76: return "大鷹型";
			case 77: return "択捉型";
			case 78: return "Ark Royal級";
			case 79: return "Richelieu級";
			case 80: return "Guglielmo Marconi級";
			case 81: return "Ташкент級";
			case 82: return "J級";
			case 83: return "Casablanca級";
			case 84: return "Essex級";
			case 85: return "日振型";
			case 86: return "呂号潜水艦";            // "潜水艦" が艦種と被るので省くべき?
			case 87: return "John C.Butler級";
			case 88: return "Nelson級";
			case 89: return "Gotland級";
			case 90: return "日進型";
			case 91: return "Fletcher級";
			case 92: return "L.d.S.D.d.Abruzzi級";
			case 93: return "Colorado級";
			case 94: return "御蔵型";
			case 95: return "Northampton級";
			case 96: return "Perth級";
			case 97: return "陸軍特種船(R1)";
			case 98: return "De Ruyter級";
			case 99: return "Atlanta級";
			case 100: return "迅鯨型";
			case 101: return "松型";
			case 102: return "South Dakota級";
			case 103: return "巡潜丙型";
			case 104: return "丁型海防艦";       // 86 に同じ
			case 105: return "Yorktown級";
			case 106: return "St. Louis級";
			case 107: return "North Carolina級";
			case 108: return "Town級";
			case 109: return "潜高型";
			case 110: return "Brooklyn級";
			case 111 when shipId is ShipId.Souya699: return "耐氷型雑用運送艦";
			case 111 when shipId is ShipId.Souya645: return "LL01";
			case 111 when shipId is ShipId.Souya650: return "PL107";
			case 112: return "Illustrious級";
			case 113: return "Conte di Cavour級";
			case 114: return "Gato級";
			case 115: return "特2TL型";
			case 116: return "Independence級";
			case 117: return "鵜来型";
			case 118: return "Ranger級";
			case 119: return "特種船M丙型";
			case 120: return "第百一号型";
			case 121: return "New Orleans級";
			default: return "不明";
		}
	}

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
	public static int GetWinRank(string rank)
	{
		switch (rank.ToUpper())
		{
			case "E":
				return 1;
			case "D":
				return 2;
			case "C":
				return 3;
			case "B":
				return 4;
			case "A":
				return 5;
			case "S":
				return 6;
			case "SS":
				return 7;
			default:
				return 0;
		}
	}

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
	public static string GetQuestCategory(int id)
	{
		switch (id)
		{
			case 1:
				return ConstantsRes.Formation;
			case 2:
			case 8:
			case 9:
				return ConstantsRes.Sortie;
			case 3:
				return ConstantsRes.Practice;
			case 4:
				return ConstantsRes.Expedition;
			case 5:
				return ConstantsRes.Supply;        //入渠も含むが、文字数の関係
			case 6:
			case 11:
				return ConstantsRes.Construction;
			case 7:
				return ConstantsRes.Modernization;
			case 10:
				return ConstantsRes.Other;
			default:
				return ConstantsRes.Unknown;
		}
	}

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


	/// <summary>
	/// 連合艦隊の編成名を表す文字列を取得します。
	/// </summary>
	public static string GetCombinedFleet(int value)
	{
		switch (value)
		{
			case 0:
				return ConstantsRes.NormalFleet;
			case 1:
				return ConstantsRes.CarrierTF;
			case 2:
				return ConstantsRes.SurfaceTF;
			case 3:
				return ConstantsRes.TransportFleet;
			default:
				return ConstantsRes.Unknown;
		}
	}

	#endregion

	#region Servers
	public class KCServer
	{
		public int Num { get; set; }
		public string Name { get; set; }
		public string Jp { get; set; }
		public string Ip { get; set; }

		public KCServer(int num, string name, string jp, string ip)
		{
			this.Num = num;
			this.Name = name;
			this.Jp = jp;
			this.Ip = ip;
		}
	}

	public static KCServer getKCServer(int num)
	{
		switch (num)
		{
			case 1:
				return new KCServer(num, "Yokosuka Naval District", "横須賀鎮守府", "203.104.209.71");
			case 2:
				return new KCServer(num, "Kure Naval District", "呉鎮守府", "203.104.209.87");
			case 3:
				return new KCServer(num, "Sasebo Naval District", "佐世保鎮守府", "125.6.184.215");
			case 4:
				return new KCServer(num, "Maizuru Naval District", "舞鶴鎮守府", "203.104.209.183");
			case 5:
				return new KCServer(num, "Ominato Guard District", "大湊警備府", "203.104.209.150");
			case 6:
				return new KCServer(num, "Truk Anchorage", "トラック泊地", "203.104.209.134");
			case 7:
				return new KCServer(num, "Lingga Anchorage", "リンガ泊地", "203.104.209.167");
			case 8:
				return new KCServer(num, "Rabaul Naval Base", "ラバウル基地", "203.104.209.199");
			case 9:
				return new KCServer(num, "Shortland Anchorage", "ショートランド泊地", "125.6.189.7");
			case 10:
				return new KCServer(num, "Buin Naval Base", "ブイン基地", "125.6.189.39");
			case 11:
				return new KCServer(num, "Tawi-Tawi Anchorage", "タウイタウイ泊地", "125.6.189.71");
			case 12:
				return new KCServer(num, "Palau Anchorage", "パラオ泊地", "125.6.189.103");
			case 13:
				return new KCServer(num, "Brunei Anchorage", "ブルネイ泊地", "125.6.189.135");
			case 14:
				return new KCServer(num, "Hitokappu Bay Anchorage", "単冠湾泊地", "125.6.189.167");
			case 15:
				return new KCServer(num, "Paramushir Anchorage", "幌筵泊地", "125.6.189.215");
			case 16:
				return new KCServer(num, "Sukumo Bay Anchorage", "宿毛湾泊地", "125.6.189.247");
			case 17:
				return new KCServer(num, "Kanoya Airfield", "鹿屋基地", "203.104.209.23");
			case 18:
				return new KCServer(num, "Iwagawa Airfield", "岩川基地", "203.104.209.39");
			case 19:
				return new KCServer(num, "Saiki Bay Anchorage", "佐伯湾泊地", "203.104.209.55");
			case 20:
				return new KCServer(num, "Hashirajima Anchorage", "柱島泊地", "203.104.209.102");
			default:
				return new KCServer(0, "", "", "");
		}
	}

	public static KCServer getKCServer(string ip)
	{
		switch (ip)
		{
			case "203.104.209.71":
				return getKCServer(1);
			case "203.104.209.87":
				return getKCServer(2);
			case "125.6.184.215":
				return getKCServer(3);
			case "203.104.209.183":
				return getKCServer(4);
			case "203.104.209.150":
				return getKCServer(5);
			case "203.104.209.134":
				return getKCServer(6);
			case "203.104.209.167":
				return getKCServer(7);
			case "203.104.209.199":
				return getKCServer(8);
			case "125.6.189.7":
				return getKCServer(9);
			case "125.6.189.39":
				return getKCServer(10);
			case "125.6.189.71":
				return getKCServer(11);
			case "125.6.189.103":
				return getKCServer(12);
			case "125.6.189.135":
				return getKCServer(13);
			case "125.6.189.167":
				return getKCServer(14);
			case "125.6.189.215":
				return getKCServer(15);
			case "125.6.189.247":
				return getKCServer(16);
			case "203.104.209.23":
				return getKCServer(17);
			case "203.104.209.39":
				return getKCServer(18);
			case "203.104.209.55":
				return getKCServer(19);
			case "203.104.209.102":
				return getKCServer(20);
			default:
				return new KCServer(0, "", "", "");
		}
	}

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
