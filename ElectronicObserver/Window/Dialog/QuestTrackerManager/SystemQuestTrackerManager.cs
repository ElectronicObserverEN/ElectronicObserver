using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class SystemQuestTrackerManager : QuestTrackerManagerBase
{
	private bool IsInitialized { get; }

	public SystemQuestTrackerManager()
	{
		Load();
		SubscribeToApis();

		IsInitialized = true;
	}

	private void MergeTrackers(List<TrackerModel> trackers)
	{
		IEnumerable<int> existingQuestIds = Trackers.Select(t => t.QuestId);
		List<TrackerModel> clashingTrackers = trackers.Where(t => existingQuestIds.Contains(t.Quest.Id)).ToList();

		foreach (TrackerModel tracker in trackers.Where(t => !existingQuestIds.Contains(t.Quest.Id)))
		{
			Trackers.Add(new TrackerViewModel(tracker));
		}

		if (clashingTrackers.Any())
		{
			foreach (TrackerModel tracker in clashingTrackers)
			{
				TrackerViewModel oldTracker = Trackers.First(t => t.QuestId == tracker.Quest.Id);
				IEnumerable<int> progress = oldTracker.GetProgress();

				Trackers.Remove(oldTracker);

				TrackerViewModel newTracker = new(tracker);
				newTracker.SetProgress(progress);

				Trackers.Add(newTracker);
			}
		}

		if (IsInitialized)
		{
				KCDatabase.Instance.Quest.OnQuestUpdated();
		}
	}

	public override void Save()
	{
		SaveProgress();
	}

	public void Load()
	{
		if (Trackers.Any())
		{
			SaveProgress();
		}

		LoadExistingTrackers();
		LoadProgress();
	}

	private string CustomTrackerPath => Path.Join(DataAndTranslationManager.DataFolder, "QuestTrackers.json");
	private string ProgressPath => Path.Combine("Record", "SystemQuestProgress.json");

	private void LoadExistingTrackers()
	{
		if (!File.Exists(CustomTrackerPath)) return;

		try
		{
			byte[] data = MessagePackSerializer.ConvertFromJson(File.ReadAllText(CustomTrackerPath));
			List<TrackerModel> trackers = MessagePackSerializer.Deserialize<List<TrackerModel>>(data);
			App.Current?.Dispatcher?.Invoke(() =>
			{
				MergeTrackers(trackers);
			});
		}
		catch
		{

		}
	}

	private void SaveProgress()
	{
		try
		{
			QuestProgressRecord progresses = new(LastQuestListUpdate,
				Trackers.Select(t => new QuestTrackerProgressRecord(t.QuestId, t.GetProgress())));
			byte[] progressBytes = MessagePackSerializer.Serialize(progresses, DateTimeOptions);
			File.WriteAllText(ProgressPath, MessagePackSerializer.ConvertToJson(progressBytes));
		}
		catch
		{
		}
	}

	private void LoadProgress()
	{
		if (!File.Exists(ProgressPath)) return;

		try
		{
			string json = File.ReadAllText(ProgressPath);
			byte[] data = MessagePackSerializer.ConvertFromJson(json);
			QuestProgressRecord progress = MessagePackSerializer.Deserialize<QuestProgressRecord>(data, DateTimeOptions);
			foreach ((int questId, IEnumerable<int> progresses) in progress.TrackerProgresses)
			{
				Trackers.FirstOrDefault(t => t.QuestId == questId)?.SetProgress(progresses);
			}

			LastQuestListUpdate = progress.LastQuestListUpdate;
		}
		catch
		{

		}
	}
}
