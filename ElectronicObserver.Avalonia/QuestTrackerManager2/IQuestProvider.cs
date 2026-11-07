using ElectronicObserver.Core;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public interface IQuestProvider
{
	IQuestData? TryGetQuest(int questId);

	IEnumerable<IQuestData> GetAllQuests();

	void NotifyQuestUpdated();
}
