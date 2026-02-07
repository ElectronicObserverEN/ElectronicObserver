using System.Collections.Generic;
using ElectronicObserver.Core.Types.Quests;
using ElectronicObserver.Core.Types.Serialization.Quests;

namespace ElectronicObserver.Core.Types.Extensions;

public static class QuestIdentifierExtensions
{
	public static bool ResetsDaily(this IQuestIdentifier questData, List<TimeLimitedQuestData> timeLimitedQuests)
	{
		// Dailies
		if (questData.QuestResetType is QuestResetType.Daily) return true;

		// PvP quests
		return questData.QuestID switch
		{
			// Quests that are not daily but only appear on some days : 
			211 => true, // 空母3
			212 => true, // 輸送5

			// PVP quests
			311 => true,
			330 => true,
			337 => true,
			339 => true,
			341 => true,
			342 => true,
			348 => true,
			// I think we can assume all PVP quest resets daily but for some reason EO is only listing the above IDs as reseting daily ?
			// For time limited quests, we are using a data json file to handle special cases
			_ => timeLimitedQuests.Find(quest => quest.ApiId == questData.QuestID)?.ProgressResetsDaily is true,
		};
	}
}
