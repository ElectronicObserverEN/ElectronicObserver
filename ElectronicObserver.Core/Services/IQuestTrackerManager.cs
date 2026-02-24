using System;
using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Core.Services;

public interface IQuestTrackerManager
{
	public void TimerSave();
	public void QuestUpdated();

	public DateTime LastQuestListUpdate { get; set; }

	public bool ShouldQuestReset(QuestResetType resetType) => resetType switch
	{
		QuestResetType.Daily => DateTimeHelper.IsCrossedDailyQuestReset(LastQuestListUpdate),
		QuestResetType.Weekly => DateTimeHelper.IsCrossedWeeklyQuestReset(LastQuestListUpdate),
		QuestResetType.Monthly => DateTimeHelper.IsCrossedMonthlyQuestReset(LastQuestListUpdate),
		QuestResetType.Quarterly => DateTimeHelper.IsCrossedQuarterlyQuestReset(LastQuestListUpdate),

		QuestResetType.January => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 1),
		QuestResetType.February => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 2),
		QuestResetType.March => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 3),
		QuestResetType.April => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 4),
		QuestResetType.May => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 5),
		QuestResetType.June => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 6),
		QuestResetType.July => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 7),
		QuestResetType.August => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 8),
		QuestResetType.September => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 9),
		QuestResetType.October => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 10),
		QuestResetType.November => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 11),
		QuestResetType.December => DateTimeHelper.IsCrossedYearlyQuestReset(LastQuestListUpdate, 12),

		_ => false
	};
}
