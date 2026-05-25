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
