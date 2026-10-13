using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types;

public interface IUseItem : IIdentifiable, IResponseWrapper
{
	/// <summary>
	/// アイテムID
	/// </summary>
	int ItemID { get; }

	/// <summary>
	/// 個数
	/// </summary>
	int Count { get; set; }

	IUseItemMaster MasterUseItem { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }
}
