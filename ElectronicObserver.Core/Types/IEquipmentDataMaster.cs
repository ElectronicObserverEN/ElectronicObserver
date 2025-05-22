using System.Collections.Generic;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types;

public interface IEquipmentDataMaster : IIdentifiable
{
	/// <summary>
	/// 装備ID
	/// </summary>
	int EquipmentID { get; }

	/// <summary>
	/// EquipmentID with a type
	/// </summary>
	EquipmentId EquipmentId { get; }

	/// <summary>
	/// 図鑑番号
	/// </summary>
	int AlbumNo { get; }

	/// <summary>
	/// 名前
	/// </summary>
	string Name { get; }

	string NameEN { get; }

	bool IsTranslated { get; }

	/// <summary>
	/// 装備種別
	/// </summary>
	IList<int> EquipmentType { get; }

	/// <summary>
	/// 装甲
	/// </summary>
	int Armor { get; }

	/// <summary>
	/// 火力
	/// </summary>
	int Firepower { get; }

	/// <summary>
	/// 雷装
	/// </summary>
	int Torpedo { get; }

	/// <summary>
	/// 爆装
	/// </summary>
	int Bomber { get; }

	/// <summary>
	/// 対空
	/// </summary>
	int AA { get; }

	/// <summary>
	/// 対潜
	/// </summary>
	int ASW { get; }

	/// <summary>
	/// 命中 / 対爆
	/// </summary>
	int Accuracy { get; }

	/// <summary>
	/// 回避 / 迎撃
	/// </summary>
	int Evasion { get; }

	/// <summary>
	/// 索敵
	/// </summary>
	int LOS { get; }

	/// <summary>
	/// 運
	/// </summary>
	int Luck { get; }

	/// <summary>
	/// 射程
	/// </summary>
	int Range { get; }

	/// <summary>
	/// レアリティ
	/// </summary>
	int Rarity { get; }

	/// <summary>
	/// 廃棄資材
	/// </summary>
	IList<int> Material { get; }

	/// <summary>
	/// 図鑑説明
	/// </summary>
	string Message { get; }

	/// <summary>
	/// 基地航空隊：配置コスト
	/// </summary>
	int AircraftCost { get; }

	/// <summary>
	/// 基地航空隊：戦闘行動半径
	/// </summary>
	int AircraftDistance { get; }

	/// <summary>
	/// 深海棲艦専用装備かどうか
	/// </summary>
	bool IsAbyssalEquipment { get; }

	/// <summary>
	/// 図鑑に載っているか
	/// </summary>
	bool IsListedInAlbum { get; }

	/// <summary>
	/// 装備種別：小分類
	/// </summary>
	int CardType { get; }

	/// <summary>
	/// 装備種別：カテゴリ
	/// </summary>
	EquipmentTypes CategoryType { get; }

	/// <summary>
	/// 装備種別：カテゴリ
	/// </summary>
	IEquipmentType CategoryTypeInstance { get; }

	/// <summary>
	/// 装備種別：アイコン
	/// </summary>
	int IconType { get; }

	/// <inheritdoc />
	EquipmentIconType IconTypeTyped { get; }

	/// <summary>
	/// 拡張スロットに装備可能な艦船IDのリスト
	/// </summary>
	IEnumerable<ShipId> EquippableShipsAtExpansion { get; set; }

	IEnumerable<ShipTypes> EquippableShipTypesAtExpansion { get; set; }

	IEnumerable<ShipClass> EquippableShipClassesAtExpansion { get; set; }

	/// <summary> 砲系かどうか </summary>
	bool IsGun { get; }

	/// <summary> 主砲系かどうか </summary>
	bool IsMainGun { get; }

	/// <summary> 副砲系かどうか </summary>
	bool IsSecondaryGun { get; }

	/// <summary> 魚雷系かどうか </summary>
	bool IsTorpedo { get; }

	/// <summary> 高角砲かどうか </summary>
	bool IsHighAngleGun { get; }

	/// <summary> 高角砲+高射装置かどうか </summary>
	bool IsHighAngleGunWithAADirector { get; }

	/// <summary> 集中配備機銃かどうか </summary>
	bool IsConcentratedAAGun { get; }

	/// <summary> 航空機かどうか </summary>
	bool IsAircraft { get; }

	/// <summary> 戦闘に参加する航空機かどうか </summary>
	bool IsCombatAircraft { get; }

	/// <summary> 偵察機かどうか </summary>
	bool IsReconAircraft { get; }

	/// <summary> 対潜攻撃可能な航空機かどうか </summary>
	bool IsAntiSubmarineAircraft { get; }

	/// <summary> 夜間行動可能な航空機かどうか </summary>
	bool IsNightAircraft { get; }

	/// <summary> 夜間戦闘機かどうか </summary>
	bool IsNightFighter { get; }

	/// <summary> 夜間攻撃機かどうか </summary>
	bool IsNightAttacker { get; }

	/// <summary> Swordfish 系艦上攻撃機かどうか </summary>
	bool IsSwordfish { get; }

	/// <summary> 電探かどうか </summary>
	bool IsRadar { get; }

	/// <summary> 対空電探かどうか </summary>
	bool IsAirRadar { get; }

	/// <summary> 水上電探かどうか </summary>
	bool IsSurfaceRadar { get; }

	bool IsHighAccuracyRadar { get; }

	/// <summary> ソナーかどうか </summary>
	bool IsSonar { get; }

	/// <summary> 爆雷かどうか(投射機は含まない) </summary>
	bool IsDepthCharge { get; }

	/// <summary> 爆雷投射機かどうか(爆雷/対潜迫撃砲は含まない) </summary>
	bool IsDepthChargeProjector { get; }

	/// <summary> 夜間作戦航空要員かどうか </summary>
	bool IsNightAviationPersonnel { get; }

	/// <summary> 高高度局戦かどうか </summary>
	bool IsHightAltitudeFighter { get; }

	/// <summary> 対空噴進弾幕が発動可能なロケットランチャーかどうか </summary>
	bool IsAARocketLauncher { get; }

	int ID { get; }

	/// <summary>
	/// 生の受信データ(api_data)
	/// </summary>
	dynamic RawData { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	string ToString();

	/// <summary>
	/// Responseを読み込みます。
	/// </summary>
	/// <param name="apiname">読み込むAPIの名前。</param>
	/// <param name="data">受信したデータ。</param>
	void LoadFromResponse(string apiname, dynamic data);
}
