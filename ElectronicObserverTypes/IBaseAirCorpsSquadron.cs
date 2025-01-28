using System;

namespace ElectronicObserverTypes;

public interface IBaseAirCorpsSquadron
{
	/// <summary>
	/// 中隊ID
	/// </summary>
	int SquadronID { get; }

	/// <summary>
	/// 状態
	/// 0=未配属, 1=配属済み, 2=配置転換中
	/// </summary>
	int State { get; }

	/// <summary>
	/// 装備固有ID
	/// </summary>
	int EquipmentMasterID { get; }

	/// <summary>
	/// 装備データ
	/// </summary>
	IEquipmentData? EquipmentInstance { get; }

	/// <summary>
	/// 装備ID
	/// </summary>
	int EquipmentID { get; }

	/// <summary>
	/// マスター装備データ
	/// </summary>
	IEquipmentDataMaster? EquipmentInstanceMaster { get; }

	/// <summary>
	/// 現在の稼働機数
	/// </summary>
	int AircraftCurrent { get; }

	/// <summary>
	/// 最大機数
	/// </summary>
	int AircraftMax { get; }

	/// <summary>
	/// コンディション
	/// 1=通常、2=橙疲労、3=赤疲労
	/// </summary>
	AirBaseCondition Condition { get; }

	/// <summary>
	/// 配置転換を開始した時刻
	/// </summary>
	DateTime RelocatedTime { get; }

	int ID { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	void LoadFromResponse(string apiname, object elem);
}
