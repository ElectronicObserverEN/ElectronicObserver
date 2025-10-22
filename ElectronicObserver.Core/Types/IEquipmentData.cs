namespace ElectronicObserver.Core.Types;

public interface IEquipmentData
{
	/// <summary>
	/// 装備を一意に識別するID
	/// </summary>
	int MasterID { get; }

	/// <summary>
	/// 装備ID
	/// </summary>
	int EquipmentID { get; }

	/// <summary>
	/// 装備ID
	/// </summary>
	EquipmentId EquipmentId { get; }

	/// <summary>
	/// 保護ロック
	/// </summary>
	bool IsLocked { get; }

	/// <summary>
	/// 改修Level
	/// </summary>
	int Level { get; }

	/// <summary>
	/// 改修Level
	/// </summary>
	UpgradeLevel UpgradeLevel { get; }

	/// <summary>
	/// 艦載機熟練度
	/// </summary>
	int AircraftLevel { get; }

	/// <summary>
	/// 装備のマスターデータへの参照
	/// </summary>
	IEquipmentDataMaster MasterEquipment { get; }

	/// <summary>
	/// 装備名
	/// </summary>
	string Name { get; }

	/// <summary>
	/// 装備名(レベルを含む)
	/// </summary>
	string NameWithLevel { get; }

	/// <summary>
	/// 配置転換中かどうか
	/// </summary>
	bool IsRelocated { get; }

	int ID { get; }

	/// <summary>
	/// 生の受信データ(api_data)
	/// </summary>
	dynamic RawData { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	void LoadFromResponse(string apiname, dynamic data);
	string ToString();
}
