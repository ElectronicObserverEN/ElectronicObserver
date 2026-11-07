using System.Runtime.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 艦船解体任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressDestruction")]
public class ProgressDestruction : ProgressData
{

	public ProgressDestruction(QuestData quest, int maxCount)
		: base(quest, maxCount)
	{
	}

	public void Increment(int amount)
	{
		for (int i = 0; i < amount; i++)
			Increment();
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Dismantlement + ProgressMax;
	}
}
