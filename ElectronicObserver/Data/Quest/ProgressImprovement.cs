using System.Runtime.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 装備改修任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressImprovement")]
public class ProgressImprovement : ProgressData
{

	public ProgressImprovement(QuestData quest, int maxCount)
		: base(quest, maxCount)
	{
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Improvement + ProgressMax;
	}
}
