using System.Runtime.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 入渠任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressDocking")]
public class ProgressDocking : ProgressData
{

	public ProgressDocking(QuestData quest, int maxCount)
		: base(quest, maxCount)
	{
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Repair + ProgressMax;
	}
}
