using System.Runtime.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 近代化改修任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressModernization")]
public class ProgressModernization : ProgressData
{

	public ProgressModernization(QuestData quest, int maxCount)
		: base(quest, maxCount)
	{
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Modernization + ProgressMax;
	}
}
