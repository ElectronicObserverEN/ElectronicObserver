using System.Text.Json;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public static class TrackerSerializer
{
	public static TrackerModelV2 CloneModel(TrackerModelV2 model)
	{
		string json = JsonSerializer.Serialize(model, TrackerJsonContext.Default.TrackerModelV2);
		return JsonSerializer.Deserialize(json, TrackerJsonContext.Default.TrackerModelV2)!;
	}

	public static List<TrackerModelV2> DeserializeTrackers(string json)
	{
		return JsonSerializer.Deserialize(json, TrackerJsonContext.Default.ListTrackerModelV2) ?? [];
	}

	public static List<TrackerModelV2> LoadTrackerModels(string trackerPath)
	{
		if (!File.Exists(trackerPath)) return [];

		try
		{
			string json = File.ReadAllText(trackerPath);
			return DeserializeTrackers(json);
		}
		catch
		{
			return [];
		}
	}

	/// <summary>
	/// Load tracker progress.
	/// </summary>
	/// <param name="trackers"></param>
	/// <param name="progressPath"></param>
	/// <returns>LastQuestListUpdate</returns>
	public static DateTime? LoadProgress(IEnumerable<TrackerViewModelV2> trackers, string progressPath)
	{
		if (!File.Exists(progressPath)) return null;

		try
		{
			string json = File.ReadAllText(progressPath);
			QuestProgressRecordV2? progress = JsonSerializer.Deserialize(json, TrackerJsonContext.Default.QuestProgressRecordV2);

			if (progress is null) return null;

			foreach (QuestTrackerProgressRecordV2 trackerProgress in progress.TrackerProgresses)
			{
				trackers
					.FirstOrDefault(t => t.QuestId == trackerProgress.QuestId)
					?.SetProgress(trackerProgress.Progresses);
			}

			return progress.LastQuestListUpdate;
		}
		catch
		{
			// safe to ignore?
		}

		return null;
	}

	public static string SerializeTrackers(IEnumerable<TrackerModelV2> trackers)
	{
		return JsonSerializer.Serialize(trackers, TrackerJsonContext.Default.IEnumerableTrackerModelV2);
	}

	public static void SaveTrackers(IEnumerable<TrackerViewModelV2> trackers, string trackerPath)
	{
		try
		{
			Directory.CreateDirectory("Record");

			string json = SerializeTrackers(trackers.Select(t => t.Model));
			File.WriteAllText(trackerPath, json);
		}
		catch
		{
			// safe to ignore?
		}
	}

	public static void SaveProgress(IEnumerable<TrackerViewModelV2> trackers, string progressPath, DateTime lastQuestListUpdate)
	{
		try
		{
			Directory.CreateDirectory("Record");

			QuestProgressRecordV2 progress = new(
				lastQuestListUpdate,
				trackers.Select(t => new QuestTrackerProgressRecordV2(t.QuestId, t.GetProgress())));

			string json = JsonSerializer.Serialize(progress, TrackerJsonContext.Default.QuestProgressRecordV2);
			File.WriteAllText(progressPath, json);
		}
		catch
		{
			// safe to ignore?
		}
	}
}
