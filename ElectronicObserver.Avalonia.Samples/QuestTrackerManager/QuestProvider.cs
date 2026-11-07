using System.Collections.Generic;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Core;

namespace ElectronicObserver.Avalonia.Samples.QuestTrackerManager;

public sealed class QuestProvider : IQuestProvider
{
	/// <inheritdoc />
	public IQuestData TryGetQuest(int questId)
	{
		return new QuestDataMock { QuestID = questId };
	}

	/// <inheritdoc />
	public IEnumerable<IQuestData> GetAllQuests() =>
	[
		new QuestDataMock { QuestID = 1 },
		new QuestDataMock { QuestID = 2 },
	];

	/// <inheritdoc />
	public void NotifyQuestUpdated()
	{
	}
}
