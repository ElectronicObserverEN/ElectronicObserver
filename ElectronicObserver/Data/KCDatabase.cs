﻿using DynaJson;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.PoiDbSubmission;
using ElectronicObserver.Data.Quest;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Data.TsunDbSubmission;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Dialog.QuestTrackerManager;

namespace ElectronicObserver.Data;

/// <summary>
/// 艦これのデータを扱う中核です。
/// </summary>
public sealed class KCDatabase : IKCDatabase
{


	#region Singleton

	private static readonly KCDatabase instance = new KCDatabase();

	public static KCDatabase Instance => instance;

	#endregion

	/// <summary>
	/// 艦船のマスターデータ
	/// </summary>
	public IDDictionary<IShipDataMaster> MasterShips { get; private set; }

	/// <summary>
	/// 艦種データ
	/// </summary>
	public IDDictionary<ShipType> ShipTypes { get; private set; }

	/// <summary>
	/// 艦船グラフィックデータ
	/// </summary>
	public IDDictionary<ShipGraphicData> ShipGraphics { get; private set; }

	/// <summary>
	/// 装備のマスターデータ
	/// </summary>
	public IDDictionary<IEquipmentDataMaster> MasterEquipments { get; private set; }

	/// <summary>
	/// 装備種別
	/// </summary>
	public IDDictionary<EquipmentType> EquipmentTypes { get; private set; }


	/// <summary>
	/// 保有艦娘のデータ
	/// </summary>
	public IDDictionary<ShipData> Ships { get; private set; }

	/// <summary>
	/// 保有装備のデータ
	/// </summary>
	public IDDictionary<EquipmentData?> Equipments { get; private set; }


	/// <summary>
	/// 提督・司令部データ
	/// </summary>
	public AdmiralData Admiral { get; private set; }


	/// <summary>
	/// アイテムのマスターデータ
	/// </summary>
	public IDDictionary<IUseItemMaster> MasterUseItems { get; private set; }

	/// <summary>
	/// アイテムデータ
	/// </summary>
	public IDDictionary<IUseItem> UseItems { get; private set; }


	/// <summary>
	/// 工廠ドックデータ
	/// </summary>
	public IDDictionary<ArsenalData> Arsenals { get; private set; }

	/// <summary>
	/// 入渠ドックデータ
	/// </summary>
	public IDDictionary<DockData> Docks { get; private set; }

	/// <summary>
	/// 開発データ
	/// </summary>
	public DevelopmentData Development { get; private set; }


	/// <summary>
	/// 艦隊データ
	/// </summary>
	public FleetManager Fleet { get; private set; }


	/// <summary>
	/// 資源データ
	/// </summary>
	public MaterialData Material { get; private set; }


	/// <summary>
	/// 任務データ
	/// </summary>
	public QuestManager Quest { get; private set; }

	/// <summary>
	/// 任務進捗データ
	/// </summary>
	public QuestProgressManager QuestProgress { get; private set; }


	/// <summary>
	/// 戦闘データ
	/// </summary>
	public BattleManager Battle { get; private set; }


	/// <summary>
	/// 海域カテゴリデータ
	/// </summary>
	public IDDictionary<MapAreaData> MapArea { get; private set; }

	/// <summary>
	/// 海域データ
	/// </summary>
	public IDDictionary<IMapInfoData> MapInfo { get; private set; }


	/// <summary>
	/// 遠征データ
	/// </summary>
	public IDDictionary<MissionData> Mission { get; private set; }


	/// <summary>
	/// 艦船グループデータ
	/// </summary>
	public ShipGroupManager ShipGroup { get; private set; }


	/// <summary>
	/// 基地航空隊データ
	/// </summary>
	public IDDictionary<BaseAirCorpsData> BaseAirCorps { get; private set; }

	/// <summary>
	/// 配置転換中装備データ
	/// </summary>
	public IDDictionary<RelocationData> RelocatedEquipments { get; private set; }

	public TsunDbSubmissionManager TsunDbSubmission { get; private set; }
	public DataAndTranslationManager Translation { get; private set; }
	public PoiDbSubmissionService PoiDbSubmission { get; private set; }

	public ServerManager ServerManager { get; } = new();

	/// <summary>
	/// 艦隊編成プリセットデータ
	/// </summary>
	public FleetPresetManager FleetPreset { get; private set; }


	private QuestTrackerManagerViewModel? _questRequirements;
	public QuestTrackerManagerViewModel QuestTrackerManagers => _questRequirements ??= new();

	private SystemQuestTrackerManager? _systemQuestTrackerManager;
	public SystemQuestTrackerManager SystemQuestTrackerManager => _systemQuestTrackerManager ??= new();

	private KCDatabase()
	{

		MasterShips = new IDDictionary<IShipDataMaster>();
		ShipTypes = new IDDictionary<ShipType>();
		ShipGraphics = new IDDictionary<ShipGraphicData>();
		MasterEquipments = new IDDictionary<IEquipmentDataMaster>();
		EquipmentTypes = new IDDictionary<EquipmentType>();
		Ships = new IDDictionary<ShipData>();
		Equipments = new IDDictionary<EquipmentData>();
		Admiral = new AdmiralData();
		MasterUseItems = new IDDictionary<IUseItemMaster>();
		UseItems = new IDDictionary<IUseItem>();
		Arsenals = new IDDictionary<ArsenalData>();
		Docks = new IDDictionary<DockData>();
		Development = new DevelopmentData();
		Fleet = new FleetManager();
		Material = new MaterialData();
		Quest = new QuestManager();
		QuestProgress = new QuestProgressManager();
		Battle = new BattleManager();
		MapArea = new IDDictionary<MapAreaData>();
		MapInfo = new IDDictionary<IMapInfoData>();
		Mission = new IDDictionary<MissionData>();
		ShipGroup = new ShipGroupManager();
		BaseAirCorps = new IDDictionary<BaseAirCorpsData>();
		RelocatedEquipments = new IDDictionary<RelocationData>();
		TsunDbSubmission = new TsunDbSubmissionManager();
		FleetPreset = new FleetPresetManager();
		Translation = new DataAndTranslationManager();
		PoiDbSubmission = new(this);

#if DEBUG
		// data needed for loading old event battles via local api loader
		// the values don't really matter, they just need to exist
		for (int i = 56; i <= 59; i++)
		{
			AddWorld(i);
		}
#endif
	}

	private void AddWorld(int world)
	{
		string json = $$"""{"api_id":{{world}},"api_name":"","api_type":1}""";
		dynamic elem = JsonObject.Parse(json);

		MapAreaData item = new();
		item.LoadFromResponse("api_start2/getData", elem);
		MapArea.Add(item);

		for (int i = 1; i <= 7; i++)
		{
			AddMap(world, i);
		}
	}

	private void AddMap(int world, int map)
	{
		string json = $$"""{"api_id":{{world}}{{map}},"api_maparea_id":{{world}},"api_no":{{map}},"api_name":"","api_level":6,"api_opetext":"","api_infotext":"","api_item":[32,34,0,0],"api_max_maphp":300,"api_required_defeat_count":null,"api_sally_flag":[1,0,0]}""";
		dynamic elem = JsonObject.Parse(json);

		MapInfoData item = new();
		item.LoadFromResponse("api_start2/getData", elem);
		MapInfo.Add(item);
	}

	public void Load()
	{
		{
			var temp = (ShipGroupManager)ShipGroup.Load();
			if (temp != null)
				ShipGroup = temp;
		}
		{
			var temp = QuestProgress.Load();
			if (temp != null)
			{
				if (QuestProgress != null)
					QuestProgress.RemoveEvents();
				QuestProgress = temp;
			}
		}

		QuestTrackerManagers.Load();
		SystemQuestTrackerManager.Load();
	}

	public void Save()
	{
		ShipGroup.Save();
		QuestProgress.Save();
		QuestTrackerManagers.Save();
		SystemQuestTrackerManager.Save();
	}

}
