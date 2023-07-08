using System.Collections.Generic;

namespace ElectronicObserverTypes;

public interface IShipData
{
	/// <summary>
	/// 艦娘を一意に識別するID
	/// </summary>
	int MasterID { get; }

	/// <summary>
	/// 並べ替えの順番
	/// </summary>
	int SortID { get; }

	/// <summary>
	/// 艦船ID
	/// </summary>
	int ShipID { get; }

	/// <summary>
	/// レベル
	/// </summary>
	int Level { get; }

	/// <summary>
	/// 累積経験値
	/// </summary>
	int ExpTotal { get; }

	/// <summary>
	/// 次のレベルに達するために必要な経験値
	/// </summary>
	int ExpNext { get; }

	/// <summary>
	/// Progress to next level.
	/// </summary>
	public double ExpNextPercentage { get; }

	/// <summary>
	/// 耐久現在値
	/// </summary>
	int HPCurrent { get; }

	/// <summary>
	/// 耐久最大値
	/// </summary>
	int HPMax { get; }

	/// <summary>
	/// 速力
	/// </summary>
	int Speed { get; }

	/// <summary>
	/// 射程
	/// </summary>
	int Range { get; }

	/// <summary>
	/// 装備スロット(ID)
	/// </summary>
	IList<int> Slot { get; }

	/// <summary>
	/// 装備スロット(マスターID)
	/// </summary>
	IList<int> SlotMaster { get; }

	/// <summary>
	/// 装備スロット(装備データ)
	/// </summary>
	IList<IEquipmentData?> SlotInstance { get; }

	/// <summary>
	/// 装備スロット(装備マスターデータ)
	/// </summary>
	IList<IEquipmentDataMaster?> SlotInstanceMaster { get; }

	/// <summary>
	/// 補強装備スロット(ID)
	/// 0=未開放, -1=装備なし 
	/// </summary>
	int ExpansionSlot { get; }

	/// <summary>
	/// 補強装備スロット(マスターID)
	/// </summary>
	int ExpansionSlotMaster { get; }

	/// <summary>
	/// 補強装備スロット(装備データ)
	/// </summary>
	IEquipmentData? ExpansionSlotInstance { get; }

	/// <summary>
	/// 補強装備スロット(装備マスターデータ)
	/// </summary>
	IEquipmentDataMaster? ExpansionSlotInstanceMaster { get; }

	/// <summary>
	/// 全てのスロット(ID)
	/// </summary>
	IList<int> AllSlot { get; }

	/// <summary>
	/// 全てのスロット(マスターID)
	/// </summary>
	IList<int> AllSlotMaster { get; }

	public IList<int> AllSlotMasterReplay { get; }


	/// <summary>
	/// 全てのスロット(装備データ)
	/// </summary>
	IList<IEquipmentData?> AllSlotInstance { get; }

	/// <summary>
	/// 全てのスロット(装備マスターデータ)
	/// </summary>
	IList<IEquipmentDataMaster> AllSlotInstanceMaster { get; }

	/// <summary>
	/// 各スロットの航空機搭載量
	/// </summary>
	IList<int> Aircraft { get; }

	/// <summary>
	/// 現在の航空機搭載量
	/// </summary>
	int AircraftTotal { get; }

	/// <summary>
	/// 搭載燃料
	/// </summary>
	int Fuel { get; set; }

	/// <summary>
	/// 搭載弾薬
	/// </summary>
	int Ammo { get; set; }

	/// <summary>
	/// スロットのサイズ
	/// </summary>
	int SlotSize { get; }

	/// <summary>
	/// 入渠にかかる時間(ミリ秒)
	/// </summary>
	int RepairTime { get; }

	/// <summary>
	/// 入渠にかかる鋼材
	/// </summary>
	int RepairSteel { get; }

	/// <summary>
	/// 入渠にかかる燃料
	/// </summary>
	int RepairFuel { get; }

	/// <summary>
	/// コンディション
	/// </summary>
	int Condition { get; }

	public int[] Kyouka { get; }

	/// <summary>
	/// 火力強化値
	/// </summary>
	int FirepowerModernized { get; }

	/// <summary>
	/// 雷装強化値
	/// </summary>
	int TorpedoModernized { get; }

	/// <summary>
	/// 対空強化値
	/// </summary>
	int AAModernized { get; }

	/// <summary>
	/// 装甲強化値
	/// </summary>
	int ArmorModernized { get; }

	/// <summary>
	/// 運強化値
	/// </summary>
	int LuckModernized { get; }

	/// <summary>
	/// 耐久強化値
	/// </summary>
	int HPMaxModernized { get; }

	/// <summary>
	/// 対潜強化値
	/// </summary>
	int ASWModernized { get; }

	/// <summary>
	/// 火力改修残り
	/// </summary>
	int FirepowerRemain { get; }

	/// <summary>
	/// 雷装改修残り
	/// </summary>
	int TorpedoRemain { get; }

	/// <summary>
	/// 対空改修残り
	/// </summary>
	int AARemain { get; }

	/// <summary>
	/// 装甲改修残り
	/// </summary>
	int ArmorRemain { get; }

	/// <summary>
	/// 運改修残り
	/// </summary>
	int LuckRemain { get; }

	/// <summary>
	/// 耐久改修残り
	/// </summary>
	int HPMaxRemain { get; }

	/// <summary>
	/// 対潜改修残り
	/// </summary>
	int ASWRemain { get; }

	/// <summary>
	/// 火力総合値
	/// </summary>
	int FirepowerTotal { get; }

	/// <summary>
	/// 雷装総合値
	/// </summary>
	int TorpedoTotal { get; }

	/// <summary>
	/// 対空総合値
	/// </summary>
	int AATotal { get; }

	/// <summary>
	/// 装甲総合値
	/// </summary>
	int ArmorTotal { get; }

	/// <summary>
	/// 回避総合値
	/// </summary>
	int EvasionTotal { get; }

	/// <summary>
	/// 対潜総合値
	/// </summary>
	int ASWTotal { get; }

	/// <summary>
	/// 索敵総合値
	/// </summary>
	int LOSTotal { get; }

	/// <summary>
	/// 運総合値
	/// </summary>
	int LuckTotal { get; }

	/// <summary>
	/// 爆装総合値
	/// </summary>
	int BomberTotal { get; }

	/// <summary>
	/// 命中総合値
	/// </summary>
	int AccuracyTotal { get; }

	/// <summary>
	/// 火力基本値
	/// </summary>
	int FirepowerBase { get; }

	/// <summary>
	/// 雷装基本値
	/// </summary>
	int TorpedoBase { get; }

	/// <summary>
	/// 対空基本値
	/// </summary>
	int AABase { get; }

	/// <summary>
	/// 装甲基本値
	/// </summary>
	int ArmorBase { get; }

	/// <summary>
	/// 回避基本値
	/// </summary>
	int EvasionBase { get; }

	/// <summary>
	/// 対潜基本値
	/// </summary>
	int ASWBase { get; }

	/// <summary>
	/// 索敵基本値
	/// </summary>
	int LOSBase { get; }

	/// <summary>
	/// 運基本値
	/// </summary>
	int LuckBase { get; }

	/// <summary>
	/// 回避最大値
	/// </summary>
	int EvasionMax { get; }

	/// <summary>
	/// 対潜最大値
	/// </summary>
	int ASWMax { get; }

	/// <summary>
	/// 索敵最大値
	/// </summary>
	int LOSMax { get; }

	/// <summary>
	/// Bonus items applied to that ship
	/// </summary>
	public List<SpecialEffectItem> SpecialEffectItems { get; }

	/// <summary>
	/// Bonus firepower from special items
	/// </summary>
	int SpecialEffectItemFirepower { get; }

	/// <summary>
	/// Bonus torpedo from special items
	/// </summary>
	int SpecialEffectItemTorpedo { get; }

	/// <summary>
	/// Bonus armor from special items
	/// </summary>
	int SpecialEffectItemArmor { get; }

	/// <summary>
	/// Bonus evasion from special items
	/// </summary>
	int SpecialEffectItemEvasion { get; }

	/// <summary>
	/// 保護ロックの有無
	/// </summary>
	bool IsLocked { get; }

	/// <summary>
	/// 装備による保護ロックの有無
	/// </summary>
	bool IsLockedByEquipment { get; }

	/// <summary>
	/// 出撃海域
	/// </summary>
	int SallyArea { get; }

	/// <summary>
	/// 艦船のマスターデータへの参照
	/// </summary>
	IShipDataMaster MasterShip { get; }

	/// <summary>
	/// 入渠中のドックID　非入渠時は-1
	/// </summary>
	int RepairingDockID { get; }

	/// <summary>
	/// 所属艦隊　-1=なし
	/// </summary>
	int Fleet { get; }

	/// <summary>
	/// 所属艦隊及びその位置
	/// ex. 1-3 (位置も1から始まる)
	/// 所属していなければ 空文字列
	/// </summary>
	string FleetWithIndex { get; }

	/// <summary>
	/// ケッコン済みかどうか
	/// </summary>
	bool IsMarried { get; }

	/// <summary>
	/// 次の改装まで必要な経験値
	/// </summary>
	int ExpNextRemodel { get; }

	/// <summary>
	/// 艦名
	/// </summary>
	string Name { get; }

	/// <summary>
	/// 艦名(レベルを含む)
	/// </summary>
	string NameWithLevel { get; }

	/// <summary>
	/// HP/HPmax
	/// </summary>
	double HPRate { get; }

	/// <summary>
	/// 最大搭載燃料
	/// </summary>
	int FuelMax { get; }

	/// <summary>
	/// 最大搭載弾薬
	/// </summary>
	int AmmoMax { get; }

	/// <summary>
	/// 燃料残量割合
	/// </summary>
	double FuelRate { get; }

	/// <summary>
	/// 弾薬残量割合
	/// </summary>
	double AmmoRate { get; }

	/// <summary>
	/// 補給で消費する燃料
	/// </summary>
	int SupplyFuel { get; }

	/// <summary>
	/// 補給で消費する弾薬
	/// </summary>
	int SupplyAmmo { get; }

	/// <summary>
	/// 搭載機残量割合
	/// </summary>
	IList<double> AircraftRate { get; }

	/// <summary>
	/// 搭載機残量割合
	/// </summary>
	double AircraftTotalRate { get; }

	/// <summary>
	/// 補強装備スロットが使用可能か
	/// </summary>
	bool IsExpansionSlotAvailable { get; }

	/// <summary>
	/// 航空戦威力
	/// 本来スロットごとのものであるが、ここでは最大火力を採用する
	/// </summary>
	int AirBattlePower { get; }

	/// <summary>
	/// 各スロットの航空戦威力
	/// </summary>
	IList<int> AirBattlePowers { get; }

	/// <summary>
	/// 砲撃威力
	/// </summary>
	int ShellingPower { get; }

	/// <summary>
	/// 空撃威力
	/// </summary>
	int AircraftPower { get; }

	/// <summary>
	/// 対潜威力
	/// </summary>
	int AntiSubmarinePower { get; }

	/// <summary>
	/// 雷撃威力
	/// </summary>
	int TorpedoPower { get; }

	/// <summary>
	/// 夜戦威力
	/// </summary>
	int NightBattlePower { get; }
	/// <summary>
	/// FirePower Total for Expedition
	/// </summary>
	int ExpeditionFirepowerTotal { get; }
	/// <summary>
	/// ASW Total for Expedition
	/// </summary>
	int ExpeditionASWTotal { get; }
	/// <summary>
	/// LOS Total for Expedition
	/// </summary>
	int ExpeditionLOSTotal { get; }
	/// <summary>
	/// AA Total for Expedition
	/// </summary>
	int ExpeditionAATotal { get; }
	/// <summary>
	/// 対潜攻撃可能か
	/// </summary>
	bool CanAttackSubmarine { get; }

	/// <summary>
	/// 開幕対潜攻撃可能か
	/// </summary>
	bool CanOpeningASW { get; }

	bool CanNoSonarOpeningAsw { get; }

	/// <summary>
	/// 夜戦攻撃可能か
	/// </summary>
	bool CanAttackAtNight { get; }

	/// <summary>
	/// 発動可能なダメコンのID -1=なし, 42=要員, 43=女神
	/// </summary>
	int DamageControlID { get; }

	int ID { get; }
	Dictionary<string, string> RequestData { get; }

	/// <summary>
	/// 生の受信データ(api_data)
	/// </summary>
	dynamic RawData { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	string ToString();
	void LoadFromResponse(string apiname, dynamic data);
	void LoadFromRequest(string apiname, Dictionary<string, string> data);
}
