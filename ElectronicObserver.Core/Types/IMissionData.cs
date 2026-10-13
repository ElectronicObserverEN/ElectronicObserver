using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types;

public interface IMissionData : IIdentifiable, IApiWrapper
{
	/// <summary>
	/// 遠征ID
	/// </summary>
	int MissionID { get; }

	/// <summary>
	/// 表示される遠征ID
	/// </summary>
	string DisplayID { get; }

	/// <summary>
	/// 海域カテゴリID
	/// </summary>
	int MapAreaID { get; }

	/// <summary>
	/// Expedition IDs sorted by world ingame
	/// </summary>
	int SortID { get; }

	/// <summary>
	/// 遠征名
	/// </summary>
	string Name { get; }

	string NameEN { get; }

	/// <summary>
	/// 進捗リセットタイミング
	/// </summary>
	ResetType ResetType { get; }

	ExpeditionType ExpeditionType { get; }
}
