using System.Collections.Generic;
using ElectronicObserver.Core.Types.Quests;
using ElectronicObserver.Core.Types.Serialization.Quests;

namespace ElectronicObserver.Core.Types.Extensions;

public static class QuestIdentifierExtensions
{
	public static bool ProgressResetsDaily(this IQuestIdentifier questData, List<QuestMetadata> questsMetadata)
	{
		// Dailies
		if (questData.QuestResetType is QuestResetType.Daily) return true;

		return questData.QuestID switch
		{
			// Quests that are not daily but only appear on some days : 
			211 => true, // 空母3
			212 => true, // 輸送5

			// Some PVP quests
			311 => true,
			330 => true,
			337 => true,
			339 => true,
			341 => true,
			342 => true,
			348 => true,

			// Special cases
			_ => questsMetadata.Find(quest => quest.ApiId == questData.QuestID)?.QuestProgressResetType is QuestProgressResetType.Daily,
		};
	}
}
