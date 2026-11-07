using System.Runtime.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 装備開発任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressDevelopment")]
public class ProgressDevelopment : ProgressData
{

	public ProgressDevelopment(QuestData quest, int maxCount)
		: base(quest, maxCount)
	{
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Development + ProgressMax;
	}
}
