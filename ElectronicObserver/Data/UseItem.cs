using ElectronicObserverTypes;

namespace ElectronicObserver.Data;

/// <summary>
/// 消費アイテムのデータを保持します。
/// </summary>
public class UseItem : ResponseWrapper, IUseItem
{

	/// <summary>
	/// アイテムID
	/// </summary>
	public int ItemID => (int)RawData.api_id;

	/// <summary>
	/// 個数
	/// </summary>
	public int Count => (int)RawData.api_count;


	public IUseItemMaster MasterUseItem => KCDatabase.Instance.MasterUseItems[ItemID];


	public int ID => ItemID;
	public override string ToString() => $"[{ItemID}] {MasterUseItem.NameTranslated} x {Count}";
}
