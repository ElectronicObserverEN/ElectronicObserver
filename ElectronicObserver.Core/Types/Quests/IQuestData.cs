namespace ElectronicObserver.Core.Types.Quests;

/// <summary>
/// 任務のデータを保持します。
/// </summary>
public interface IQuestData
{
	/// <summary>
	/// 任務ID
	/// </summary>
	public int QuestID { get; }

	/// <summary>
	/// 任務出現タイプ
	/// 1=デイリー, 2=ウィークリー, 3=マンスリー, 4=単発, 5=他
	/// </summary>
	public int Type { get; }

	/// <summary>
	/// 周期アイコン種別
	/// 1=単発, 2=デイリー, 3=ウィークリー, 6=マンスリー, 7=他(輸送5と空母3,クォータリー), 100+x=イヤーリー(x月-)
	/// </summary>
	public int LabelType { get; }
}
