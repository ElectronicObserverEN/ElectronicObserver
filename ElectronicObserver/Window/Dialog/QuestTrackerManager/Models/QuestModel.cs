using ElectronicObserver.Core.Types.Quests;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

[MessagePackObject]
public record QuestModel([property: Key(0)] int Id)
{
	[IgnoreMember] public string Name => TryGetQuest()?.Name ?? QuestTrackerManagerResources.UnknownQuest;
	[IgnoreMember] public string? Description => TryGetQuest()?.Description;
	[IgnoreMember] public string? Code => TryGetQuest()?.Code;
	[IgnoreMember] public QuestCategory Category => (QuestCategory)(TryGetQuest()?.Category ?? 0);
	[IgnoreMember] public int State => TryGetQuest()?.State ?? 0;
	[IgnoreMember]
	public QuestResetType ResetType => TryGetQuest()?.Type switch
	{
		1 => QuestResetType.Daily,
		2 => QuestResetType.Weekly,
		3 => QuestResetType.Monthly,
		4 => QuestResetType.Never,
		5 => TryGetQuest()?.LabelType switch
		{
			// 2, 3, 6 should never happen
			2 => QuestResetType.Daily,
			3 => QuestResetType.Weekly,
			6 => QuestResetType.Monthly,
			7 => TryGetQuest()?.ID switch
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

	[IgnoreMember] public string Display => $"{Code}: {Name} (ID: {Id})";

	private QuestData? TryGetQuest()
	{
		if (!KCDatabase.Instance.Quest.Quests.ContainsKey(Id)) return null;

		return KCDatabase.Instance.Quest[Id];
	}
}
