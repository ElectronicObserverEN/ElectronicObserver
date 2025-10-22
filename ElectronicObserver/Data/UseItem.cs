using ElectronicObserver.Core.Types;

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
	public int Count { get; set; }


	public IUseItemMaster MasterUseItem => KCDatabase.Instance.MasterUseItems[ItemID];


	public int ID => ItemID;
	public override string ToString() => $"[{ItemID}] {MasterUseItem.NameTranslated} x {Count}";

	public override void LoadFromResponse(string apiname, dynamic data)
	{
		Count = (int)data.api_count;

		base.LoadFromResponse(apiname, (object)data);
	}
}
