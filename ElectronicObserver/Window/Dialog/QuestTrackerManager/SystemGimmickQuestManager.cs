using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public static class SystemGimmickQuestManager
{
	private static int QuestId = 10000;

	private static bool IsInitialized = false;

	private static string GimmickQuestsPath => Path.Join(DataAndTranslationManager.DataFolder, "GimmickQuests.json");


	public static void Load()
	{
		if (IsInitialized) return;
		if (!File.Exists(GimmickQuestsPath)) return;

		try
		{
			byte[] data = MessagePackSerializer.ConvertFromJson(File.ReadAllText(GimmickQuestsPath));
			List<GimmickQuestData> quests = MessagePackSerializer.Deserialize<List<GimmickQuestData>>(data);

			int currentUnlockPartCount = 0;
			GimmickQuestData? previousQuest = null;

			foreach (GimmickQuestData quest in quests
				.OrderBy(quest => quest.MapArea)
				.ThenBy(quest => quest.MapNumber)
				.ThenBy(quest => quest.GaugeNumber)
				.ThenBy(quest => quest.Type))
			{
				// --- Assign quest id :
				int questID = ++QuestId;
				quest.QuestID = questID;
				if (quest.Tracker is not null) quest.Tracker.Quest = new QuestModel(questID);

				// --- Init GimmickNumber
				if (previousQuest != null && previousQuest.MapArea == quest.MapArea && previousQuest.MapNumber == quest.MapNumber && previousQuest.GaugeNumber == quest.GaugeNumber && previousQuest.Type == quest.Type)
				{
					if (previousQuest.GimmickNumber == 0)
					{
						previousQuest.GimmickNumber = ++currentUnlockPartCount;
					}

					quest.GimmickNumber = ++currentUnlockPartCount;
				}

				previousQuest = quest;
			}

			// Update Quests
			foreach (GimmickQuestData quest in quests.OrderBy(quest => quest.QuestID))
			{
				KCDatabase.Instance.Quest.Quests.Add(quest);
			}

			// Update trackers
			List<TrackerModel> trackerList = quests
				.Where(quest => quest.Tracker != null)
				.Select(quest => quest.Tracker)
				.ToList()!;

			KCDatabase.Instance.SystemQuestTrackerManager.MergeTrackers(trackerList);

			IsInitialized = true;
		}
		catch
		{

		}
	}
}
