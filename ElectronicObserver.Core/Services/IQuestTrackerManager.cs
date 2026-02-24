using System;
using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Core.Services;

public interface IQuestTrackerManager
{
	public void TimerSave();
	public void QuestUpdated();

	public DateTime LastQuestListUpdate { get; set; }
}
