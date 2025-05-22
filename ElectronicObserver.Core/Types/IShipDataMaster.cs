using System.Collections.Generic;
using System.Drawing;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types;

public interface IShipDataMaster : IIdentifiable
{
	/// <summary>
	/// 艦船ID
	/// </summary>
	int ShipID { get; }

	/// <summary>
	/// 艦船ID
	/// </summary>
	public ShipId ShipId { get; }

	/// <summary>
	/// 図鑑番号
	/// </summary>
	int AlbumNo { get; }

	/// <summary>
	/// 母港ソート順
	/// </summary>
	int SortID { get; }

	/// <summary>
	/// 名前
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Name in romaji
	/// </summary>
	string NameEN { get; }

	/// <summary>
	/// 読み
	/// </summary>
	string NameReading { get; }

	public string NameReadingEN { get; }

	/// <summary>
	/// 艦種
	/// </summary>
	ShipTypes ShipType { get; }

	/// <summary>
	/// 艦型
	/// </summary>
	int ShipClass { get; }

	ShipClass ShipClassTyped { get; }

	/// <summary>
	/// 改装Lv.
	/// </summary>
	int RemodelAfterLevel { get; }

	/// <summary>
	/// 改装後の艦船ID
	/// 0=なし
	/// </summary>
	int RemodelAfterShipID { get; }

	/// <summary>
	/// 改装後の艦船
	/// </summary>
	IShipDataMaster? RemodelAfterShip { get; }

	/// <summary>
	/// 改装前の艦船ID
	/// 0=なし
	/// </summary>
	int RemodelBeforeShipID { get; set; }

	/// <summary>
	/// 改装前の艦船
	/// </summary>
	IShipDataMaster? RemodelBeforeShip { get; }

	/// <summary>
	/// 改装に必要な弾薬
	/// </summary>
	int RemodelAmmo { get; }

	/// <summary>
	/// 改装に必要な鋼材
	/// </summary>
	int RemodelSteel { get; }

	/// <summary>
	/// 改装に改装設計図が必要かどうか
	/// </summary>
	int NeedBlueprint { get; set; }

	/// <summary>
	/// 改装に試製甲板カタパルトが必要かどうか
	/// </summary>
	int NeedCatapult { get; set; }

	/// <summary>
	/// 改装に戦闘詳報が必要かどうか
	/// </summary>
	int NeedActionReport { get; set; }

	/// <summary>
	/// 改装に必要な 新型航空兵装資材 の個数
	/// </summary>
	public int NeedAviationMaterial { get; set; }

	/// <summary>
	/// 改装に必要な 新型兵装資材 の個数
	/// </summary>
	public int NeedArmamentMaterial { get; set; }

	/// <summary>
	/// 耐久初期値
	/// </summary>
	int HPMin { get; }

	/// <summary>
	/// 耐久最大値
	/// </summary>
	int HPMax { get; }

	/// <summary>
	/// 装甲初期値
	/// </summary>
	int ArmorMin { get; }

	/// <summary>
	/// 装甲最大値
	/// </summary>
	int ArmorMax { get; }

	/// <summary>
	/// 火力初期値
	/// </summary>
	int FirepowerMin { get; }

	/// <summary>
	/// 火力最大値
	/// </summary>
	int FirepowerMax { get; }

	/// <summary>
	/// 雷装初期値
	/// </summary>
	int TorpedoMin { get; }

	/// <summary>
	/// 雷装最大値
	/// </summary>
	int TorpedoMax { get; }

	/// <summary>
	/// 対空初期値
	/// </summary>
	int AAMin { get; }

	/// <summary>
	/// 対空最大値
	/// </summary>
	int AAMax { get; }

	/// <summary>
	/// 対潜
	/// </summary>
	IParameter ASW { get; }

	/// <summary>
	/// 回避
	/// </summary>
	IParameter Evasion { get; }

	/// <summary>
	/// 索敵
	/// </summary>
	IParameter LOS { get; }

	/// <summary>
	/// 運初期値
	/// </summary>
	int LuckMin { get; }

	/// <summary>
	/// 運最大値
	/// </summary>
	int LuckMax { get; }

	/// <summary>
	/// 速力
	/// 0=陸上基地, 5=低速, 10=高速
	/// </summary>
	int Speed { get; }

	/// <summary>
	/// 射程
	/// </summary>
	int Range { get; }

	/// <summary>
	/// 装備スロットの数
	/// </summary>
	int SlotSize { get; }

	/// <summary>
	/// 各スロットの航空機搭載数
	/// </summary>
	IList<int> Aircraft { get; }

	/// <summary>
	/// 搭載
	/// </summary>
	int AircraftTotal { get; }

	/// <summary>
	/// 初期装備のID
	/// </summary>
	IList<int>? DefaultSlot { get; }

	/// <summary>
	/// 特殊装備カテゴリ　指定がない場合は null
	/// </summary>
	IEnumerable<int> SpecialEquippableCategories { get; set; }

	/// <summary>
	/// 装備可能なカテゴリ
	/// </summary>
	IEnumerable<int> EquippableCategories { get; }

	IEnumerable<EquipmentTypes> EquippableCategoriesTyped { get; }

	/// <summary>
	/// 建造時間(分)
	/// </summary>
	int BuildingTime { get; }

	/// <summary>
	/// 解体資材
	/// </summary>
	IList<int> Material { get; }

	/// <summary>
	/// 近代化改修の素材にしたとき上昇するパラメータの量
	/// </summary>
	IList<int> PowerUp { get; }

	/// <summary>
	/// レアリティ
	/// </summary>
	int Rarity { get; }

	/// <summary>
	/// ドロップ/ログイン時のメッセージ
	/// </summary>
	string MessageGet { get; }

	/// <summary>
	/// 艦船名鑑でのメッセージ
	/// </summary>
	string MessageAlbum { get; }

	/// <summary>
	/// 搭載燃料
	/// </summary>
	int Fuel { get; }

	/// <summary>
	/// 搭載弾薬
	/// </summary>
	int Ammo { get; }

	/// <summary>
	/// ボイス再生フラグ
	/// </summary>
	int VoiceFlag { get; }

	/// <summary>
	/// グラフィック設定データへの参照
	/// </summary>
	IShipGraphicData GraphicData { get; }

	/// <summary>
	/// リソースのファイル/フォルダ名
	/// </summary>
	string ResourceName { get; }

	/// <summary>
	/// 画像リソースのバージョン
	/// </summary>
	string ResourceGraphicVersion { get; }

	/// <summary>
	/// ボイスリソースのバージョン
	/// </summary>
	string ResourceVoiceVersion { get; }

	/// <summary>
	/// 母港ボイスリソースのバージョン
	/// </summary>
	string ResourcePortVoiceVersion { get; }

	/// <summary>
	/// 衣替え艦：ベースとなる艦船ID
	/// </summary>
	int OriginalCostumeShipID { get; }

	/// <summary>
	/// ケッコンカッコカリ後のHP
	/// </summary>
	int HPMaxMarried { get; }

	/// <summary>
	/// HP改修可能値(未婚時)
	/// </summary>
	int HPMaxModernizable { get; }

	/// <summary>
	/// HP改修可能値(既婚時)
	/// </summary>
	int HPMaxMarriedModernizable { get; }

	/// <summary>
	/// 近代化改修後のHP(未婚時)
	/// </summary>
	int HPMaxModernized { get; }

	/// <summary>
	/// 近代化改修後のHP(既婚時)
	/// </summary>
	int HPMaxMarriedModernized { get; }

	/// <summary>
	/// 対潜改修可能値
	/// </summary>
	int ASWModernizable { get; }

	/// <summary>
	/// 深海棲艦かどうか
	/// </summary>
	bool IsAbyssalShip { get; }

	/// <summary>
	/// クラスも含めた艦名
	/// </summary>
	string NameWithClass { get; }

	/// <summary>
	/// 艦種インスタンス
	/// </summary>
	IShipType ShipTypeInstance { get; }

	/// <summary>
	/// 陸上基地かどうか
	/// </summary>
	bool IsLandBase { get; }

	/// <summary>
	/// 図鑑に載っているか
	/// </summary>
	bool IsListedInAlbum { get; }

	/// <summary>
	/// 改装段階
	/// 初期 = 0, 改 = 1, 改二 = 2, ...
	/// </summary>
	int RemodelTier { get; }

	/// <summary>
	/// 改装段階
	/// 初期 = 0, 改 = 1, 改二 = 2, ...
	/// </summary>
	RemodelTier RemodelTierTyped { get; }

	/// <summary>
	/// 艦種名
	/// </summary>
	string ShipTypeName { get; }

	/// <summary>
	/// 潜水艦系か (潜水艦/潜水空母)
	/// </summary>
	bool IsSubmarine { get; }

	/// <summary>
	/// 空母系か (軽空母/正規空母/装甲空母)
	/// </summary>
	bool IsAircraftCarrier { get; }

	/// <summary>
	/// Regular Carrier (CV/CVB)
	/// </summary>
	bool IsRegularCarrier { get; }

	/// <summary>
	/// 護衛空母か
	/// </summary>
	bool IsEscortAircraftCarrier { get; }

	/// <summary>
	/// PT imps
	/// </summary>
	bool IsPt { get; }

	int ID { get; }

	/// <summary>
	/// 生の受信データ(api_data)
	/// </summary>
	dynamic RawData { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	Color GetShipNameColor();
	IShipDataMaster BaseShip();
	string ToString();

	/// <summary>
	/// Responseを読み込みます。
	/// </summary>
	/// <param name="apiname">読み込むAPIの名前。</param>
	/// <param name="data">受信したデータ。</param>
	void LoadFromResponse(string apiname, dynamic data);
}
