using ElectronicObserverTypes.Data;

namespace ElectronicObserverTypes;

public interface IUseItem : IIdentifiable
{
	/// <summary>
	/// アイテムID
	/// </summary>
	int ItemID { get; }

	/// <summary>
	/// 個数
	/// </summary>
	int Count { get; }

	IUseItemMaster MasterUseItem { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	/// <summary>
	/// Responseを読み込みます。
	/// </summary>
	/// <param name="apiname">読み込むAPIの名前。</param>
	/// <param name="data">受信したデータ。</param>
	void LoadFromResponse(string apiname, dynamic data);
}
