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
		DailyProgressReset = DateTimeHelper.ShouldQuestReset(QuestResetType.Daily, LastQuestListUpdate);
		WeeklyProgressReset = DateTimeHelper.ShouldQuestReset(QuestResetType.Weekly, LastQuestListUpdate);
		MonthlyProgressReset = DateTimeHelper.ShouldQuestReset(QuestResetType.Monthly, LastQuestListUpdate);
		QuaterlyProgressReset = DateTimeHelper.ShouldQuestReset(QuestResetType.Quarterly, LastQuestListUpdate);

		LastQuestListUpdate = DateTime.Now;
	}
}
