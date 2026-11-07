using System.Collections.Generic;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Core;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public sealed class QuestProviderAdapter : IQuestProvider
{
	public IQuestData? TryGetQuest(int questId)
		=> KCDatabase.Instance.Quest.Quests.TryGetValue(questId, out QuestData quest) switch
		{
			true => quest,
			_ => null
		};

	// Deferred on purpose: quests are only populated once the game data has actually loaded,
	// which can happen well after this is first accessed.
	public IEnumerable<IQuestData> GetAllQuests() => KCDatabase.Instance.Quest.Quests.Values;

	public void NotifyQuestUpdated() => KCDatabase.Instance.Quest.OnQuestUpdated();
}
