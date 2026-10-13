namespace ElectronicObserver.Core.Types;

public interface IEquipmentType : IResponseWrapper
{
	/// <summary>
	/// 装備種別ID
	/// </summary>
	int TypeID { get; }

	/// <summary>
	/// 名前
	/// </summary>
	string Name { get; }

	string NameEN { get; }

	/// <summary>
	/// 装備種別ID
	/// </summary>
	EquipmentTypes Type { get; }

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
}
