namespace ElectronicObserverTypes.Data;

public interface IKCDatabase
{
	/// <summary>
	/// 艦船のマスターデータ
	/// </summary>
	IDDictionary<IShipDataMaster> MasterShips { get; }
	/*
	/// <summary>
	/// 艦種データ
	/// </summary>
	IDDictionary<ShipType> ShipTypes { get; }

	/// <summary>
	/// 艦船グラフィックデータ
	/// </summary>
	IDDictionary<ShipGraphicData> ShipGraphics { get; }
	*/
	/// <summary>
	/// 装備のマスターデータ
	/// </summary>
	IDDictionary<IEquipmentDataMaster> MasterEquipments { get; }
	/*
	/// <summary>
	/// 装備種別
	/// </summary>
	IDDictionary<EquipmentType> EquipmentTypes { get; }

	/// <summary>
	/// 保有艦娘のデータ
	/// </summary>
	IDDictionary<ShipData> Ships { get; }

	/// <summary>
	/// 保有装備のデータ
	/// </summary>
	IDDictionary<EquipmentData> Equipments { get; }

	/// <summary>
	/// 提督・司令部データ
	/// </summary>
	AdmiralData Admiral { get; }
	*/
	/// <summary>
	/// アイテムのマスターデータ
	/// </summary>
	IDDictionary<IUseItemMaster> MasterUseItems { get; }

	/// <summary>
	/// アイテムデータ
	/// </summary>
	IDDictionary<IUseItem> UseItems { get; }
	/*
	/// <summary>
	/// 工廠ドックデータ
	/// </summary>
	IDDictionary<ArsenalData> Arsenals { get; }

	/// <summary>
	/// 入渠ドックデータ
	/// </summary>
	IDDictionary<DockData> Docks { get; }

	/// <summary>
	/// 開発データ
	/// </summary>
	DevelopmentData Development { get; }

	/// <summary>
	/// 艦隊データ
	/// </summary>
	FleetManager Fleet { get; }

	/// <summary>
	/// 資源データ
	/// </summary>
	MaterialData Material { get; }

	/// <summary>
	/// 任務データ
	/// </summary>
	QuestManager Quest { get; }

	/// <summary>
	/// 任務進捗データ
	/// </summary>
	QuestProgressManager QuestProgress { get; }

	/// <summary>
	/// 戦闘データ
	/// </summary>
	BattleManager Battle { get; }

	/// <summary>
	/// 海域カテゴリデータ
	/// </summary>
	IDDictionary<MapAreaData> MapArea { get; }

	/// <summary>
	/// 海域データ
	/// </summary>
	IDDictionary<MapInfoData> MapInfo { get; }

	/// <summary>
	/// 遠征データ
	/// </summary>
	IDDictionary<MissionData> Mission { get; }

	/// <summary>
	/// 艦船グループデータ
	/// </summary>
	ShipGroupManager ShipGroup { get; }

	/// <summary>
	/// 基地航空隊データ
	/// </summary>
	IDDictionary<BaseAirCorpsData> BaseAirCorps { get; }

	/// <summary>
	/// 配置転換中装備データ
	/// </summary>
	IDDictionary<RelocationData> RelocatedEquipments { get; }

	ReplayManager Replays { get; }
	TsunDbSubmissionManager TsunDbSubmission { get; }
	DataAndTranslationManager Translation { get; }

	/// <summary>
	/// Current server
	/// </summary>
	Constants.KCServer Server { get; set; }

	/// <summary>
	/// 艦隊編成プリセットデータ
	/// </summary>
	FleetPresetManager FleetPreset { get; }

	QuestTrackerManagerViewModel QuestTrackerManagers { get; }
	SystemQuestTrackerManager SystemQuestTrackerManager { get; }
	void Load();
	void Save();
	*/
}
