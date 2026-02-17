using System.Collections.Generic;
using ElectronicObserver.Core.Types.Quests;
using ElectronicObserver.Core.Types.Serialization.Quests;

namespace ElectronicObserver.Core.Types.Extensions;

public static class QuestDataExtensions
{
	public static QuestResetType GetProgressResetType(this IQuestData quest, Dictionary<int, QuestMetadata> questsMetadata)
	{
		if (questsMetadata.TryGetValue(quest.QuestID, out QuestMetadata? metadata) && metadata.QuestProgressResetType is { } resetType)
		{
			return resetType;
		}

		return quest.Type switch
		{
			1 => QuestResetType.Daily,
			2 => QuestResetType.Weekly,
			3 => QuestResetType.Monthly,
			4 => QuestResetType.Never,
			5 => quest.LabelType switch
			{
				// 2, 3, 6 should never happen
				2 => QuestResetType.Daily,
				3 => QuestResetType.Weekly,
				6 => QuestResetType.Monthly,
				7 => quest.QuestID switch
				{
					// BD4 - 3 carriers
					211 => QuestResetType.Daily,
					// BD6 - 5 transports
					212 => QuestResetType.Daily,

					// this will be incorrect if they add any new odd daily quests
					_ => QuestResetType.Quarterly
				},
				101 => QuestResetType.January,
				102 => QuestResetType.February,
				103 => QuestResetType.March,
				104 => QuestResetType.April,
				105 => QuestResetType.May,
				106 => QuestResetType.June,
				107 => QuestResetType.July,
				108 => QuestResetType.August,
				109 => QuestResetType.September,
				110 => QuestResetType.October,
				111 => QuestResetType.November,
				112 => QuestResetType.December,

				_ => QuestResetType.Unknown
			},

			_ => QuestResetType.Unknown
		};
	}
}
