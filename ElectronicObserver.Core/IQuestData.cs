using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Core;

public interface IQuestData
{
	/// <summary>
	/// 任務ID
	/// </summary>
	int QuestID { get; }

	/// <summary>
	/// 任務カテゴリ
	/// </summary>
	int Category { get; }

	/// <summary>
	/// 遂行状態
	/// 1=未受領, 2=遂行中, 3=達成
	/// </summary>
	int State { get; set; }

	string Code { get; }

	/// <summary>
	/// Name (Translated)
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Description (Translated)
	/// </summary>
	string Description { get; }

	QuestResetType GetProgressResetType();
}
