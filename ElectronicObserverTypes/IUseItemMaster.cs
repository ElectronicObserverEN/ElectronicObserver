using ElectronicObserverTypes.Data;

namespace ElectronicObserverTypes;

public interface IUseItemMaster : IIdentifiable
{
	/// <summary>
	/// アイテムID
	/// </summary>
	UseItemId ItemID { get; }

	/// <summary>
	/// 使用形態
	/// 1=高速修復材, 2=高速建造材, 3=開発資材, 4=資源還元, その他
	/// </summary>
	int UseType { get; }

	/// <summary>
	/// カテゴリ
	/// </summary>
	int Category { get; }

	/// <summary>
	/// アイテム名
	/// </summary>
	string NameTranslated { get; }

	/// <summary>
	/// アイテム名
	/// </summary>
	string Name { get; }

	/// <summary>
	/// 説明
	/// </summary>
	string Description { get; }

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
