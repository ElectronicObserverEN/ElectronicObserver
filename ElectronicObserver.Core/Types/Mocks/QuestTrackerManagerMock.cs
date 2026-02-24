using System;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Core.Types.Mocks;

public class QuestTrackerManagerMock : IQuestTrackerManager
{
	public DateTime LastQuestListUpdate { get; set; }

	public bool DailyProgressReset { get; set; } 
	public bool WeeklyProgressReset { get; set; } 
	public bool MonthlyProgressReset { get; set; }
	public bool QuaterlyProgressReset { get; set; }

	public void TimerSave()
	{
		if (!DateTimeHelper.IsCrossedHour(LastQuestListUpdate)) return;

		LastQuestListUpdate = DateTime.Now;
	}

	public void QuestUpdated()
	{
		DailyProgressReset = (this as IQuestTrackerManager).ShouldQuestReset(QuestResetType.Daily);
		WeeklyProgressReset = (this as IQuestTrackerManager).ShouldQuestReset(QuestResetType.Weekly);
		MonthlyProgressReset = (this as IQuestTrackerManager).ShouldQuestReset(QuestResetType.Monthly);
		QuaterlyProgressReset = (this as IQuestTrackerManager).ShouldQuestReset(QuestResetType.Quarterly);

		LastQuestListUpdate = DateTime.Now;
	}
}
