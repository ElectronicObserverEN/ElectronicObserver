using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.Translation.Ship;

public sealed class ShipDataService(
	IConfigurationUi configurationUi,
	ISoftwareUpdaterService softwareUpdaterService,
	IEoLogger logger)
	: DataServiceBase(configurationUi, softwareUpdaterService, logger)
{
	protected override string FileName => "ship.json";
	protected override DataType DataType => DataType.Translation;

	private Dictionary<string, string> ShipList { get; set; } = [];
	private Dictionary<string, string> TypeList { get; set; } = [];
	private Dictionary<string, string> SuffixList { get; set; } = [];
	private Dictionary<string, string> ClassList { get; set; } = [];

	private Dictionary<string, string> NameCache { get; } = [];

	/// <inheritdoc />
	public override async Task Initialize()
	{
		NameCache.Clear();
		
		ShipList = [];
		TypeList = [];
		SuffixList = [];
		ClassList = [];

		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		ShipTranslationData? json = await Load<ShipTranslationData>(path);

		if (json is null) return;

		ShipList = json.Ship;
		TypeList = json.Stype;
		SuffixList = json.Suffix;
		ClassList = json.Class;
	}

	public string Name(string rawData)
	{
		if (ConfigurationUi.JapaneseShipName) return rawData;
		if (NameCache.TryGetValue(rawData, out string? value)) return value;

		value = TranslateName(rawData);
		NameCache.Add(rawData, value);

		return value;
	}

	private string TranslateName(string rawData)
	{
		// save current ship name to prevent suffix replacements that can show up in names
		// tre suffix can be found in Intrepid which gets you In Trepid
		string currentShipName = "";

		foreach ((string key, string value) in ShipList.OrderByDescending(s => s.Key.Length))
		{
			if (rawData.Equals(key)) return value;

			if (!rawData.StartsWith(key)) continue;

			int pos = rawData.IndexOf(key);
			rawData = rawData.Remove(pos, key.Length).Insert(pos, value);
			currentShipName = key;
		}

		string name = rawData; // prevent suffix from being replaced twice.

		foreach ((string key, string value) in SuffixList.OrderByDescending(sf => sf.Key.Length))
		{
			if (!rawData.Contains(key)) continue;

			int pos = rawData.IndexOf(key);

			if (pos < currentShipName.Length) continue;

			rawData = rawData.Remove(pos, key.Length).Insert(pos, new String('0', value.Length));
			name = name.Remove(pos, key.Length).Insert(pos, value);

			if (rawData.Substring(pos - 1, 1).Contains(' ')) continue;

			rawData = rawData.Insert(pos, " ");
			name = name.Insert(pos, " ");
		}

		return name;
	}

	public string Class(string rawData) => ConfigurationUi.JapaneseShipType switch
	{
		true => rawData,
		_ => ClassList.GetValueOrDefault(rawData, rawData),
	};

	public string TypeName(string rawData) => ConfigurationUi.JapaneseShipType switch
	{
		true => rawData,
		_ => TypeList.GetValueOrDefault(rawData, rawData),
	};
	
	public string TypeNameShort(ShipTypes shipType) => ConfigurationUi.JapaneseShipType switch
	{
		true => ShipTypeShort(shipType),
		_ => ShipTypeShortEnglish(shipType),
	};
	
	/// <summary>
	/// 艦型を表す文字列を取得します。
	/// </summary>
	public string GetShipClass(ShipClass id, ShipId shipId = ShipId.Souya645)
		=> GetShipClass((int)id, shipId);


	/// <summary>
	/// 艦型を表す文字列を取得します。
	/// </summary>
	public string GetShipClass(int id, ShipId shipId = ShipId.Unknown) => Class(GetShipClassUntranslated(id, shipId));

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
		133 => "Norge級",
		134 => "Courageous級",
		135 => "Glorious級",
		136 => "野埼型",
		137 => "Thonburi級",
		_ => "不明",
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
	private static string ShipTypeShort(ShipTypes shipType) => shipType switch
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
	private static string ShipTypeShortEnglish(ShipTypes shipType) => shipType switch
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
}
