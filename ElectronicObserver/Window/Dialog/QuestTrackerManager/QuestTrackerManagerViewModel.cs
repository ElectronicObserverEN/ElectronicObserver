using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public partial class QuestTrackerManagerViewModel : QuestTrackerManagerBase
{
	public QuestTrackerManagerTranslationViewModel Translation { get; }
	private ShipSelectorFactory ShipSelectorFactory { get; }

	public IEnumerable<QuestModel> Quests { get; }

	public QuestModel? Quest { get; set; }

	public bool DebugEnabled => Utility.Configuration.Config.Debug.EnableDebugMenu;

	public QuestTrackerManagerViewModel()
	{
		Translation = Ioc.Default.GetRequiredService<QuestTrackerManagerTranslationViewModel>();
		ShipSelectorFactory = Ioc.Default.GetRequiredService<ShipSelectorFactory>();

		Quests = KCDatabase.Instance.Quest.Quests.Values
			.Select(q => new QuestModel(q.ID));

		SubscribeToApis();
	}

	private static List<TrackerModel> DeserializeTrackers(string json)
	{
		try
		{
			byte[] data = MessagePackSerializer.ConvertFromJson(json);
			return MessagePackSerializer.Deserialize<List<TrackerModel>>(data);
		}
		catch
		{
			// ignored
		}

		return new();
	}

	[RelayCommand]
	private void CopyTrackersToClipboard()
	{
		List<TrackerModel> trackers = Trackers.Select(t => t.Model).ToList();
		byte[] data = MessagePackSerializer.Serialize(trackers.SortTrackers());
		Clipboard.SetDataObject(MessagePackSerializer.ConvertToJson(data));
	}

	[RelayCommand]
	private void LoadTrackerFromClipboard()
	{
		try
		{
			string json = Clipboard.GetText();
			byte[] data = MessagePackSerializer.ConvertFromJson(json);
			List<TrackerModel> trackers = MessagePackSerializer.Deserialize<List<TrackerModel>>(data);
			MergeTrackers(trackers);
		}
		catch
		{
			// ignored
		}
	}

	[RelayCommand]
	private void CopyAllTrackersToClipboard()
	{
		// Copy all trackers from quest tracker manager and system trackers.
		// In case of duplicates, the one from quest tracker manager will be copied.
		List<TrackerModel> trackers = Trackers
			.Union(KCDatabase.Instance.SystemQuestTrackerManager.Trackers)
			.Select(t => t.Model)
			.DistinctBy(t => t.Quest.Id)
			.ToList();
		byte[] data = MessagePackSerializer.Serialize(trackers.SortTrackers());
		Clipboard.SetDataObject(MessagePackSerializer.ConvertToJson(data));
	}

	private void MergeTrackers(List<TrackerModel> trackers)
	{
		IEnumerable<int> existingQuestIds = Trackers.Select(t => t.QuestId);
		List<TrackerModel> clashingTrackers = trackers.Where(t => existingQuestIds.Contains(t.Quest.Id)).ToList();

		foreach (TrackerModel tracker in trackers.Where(t => !existingQuestIds.Contains(t.Quest.Id)))
		{
			Trackers.Add(new TrackerViewModel(tracker, ShipSelectorFactory));
		}

		if (clashingTrackers.Any())
		{
			string warning = string.Format(Translation.TrackerOverwrite, string.Join("\n", Trackers
				.Where(t => clashingTrackers.Select(x => x.Quest.Id).Contains(t.QuestId))
				.Select(t => t.Model.Quest.Display)));

			if (MessageBox.Show(warning, Translation.Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				foreach (TrackerModel tracker in clashingTrackers)
				{
					Trackers.Remove(Trackers.First(t => t.QuestId == tracker.Quest.Id));
					Trackers.Add(new TrackerViewModel(tracker, ShipSelectorFactory));
				}
			}
		}

		App.Current?.Dispatcher?.Invoke(() =>
		{
			KCDatabase.Instance.Quest.OnQuestUpdated();
		});
	}

	[RelayCommand]
	private void AddTracker()
	{
		if (Quest is null) return;
		if (Trackers.Select(t => t.QuestId).Contains(Quest.Id)) return;

		TrackerViewModel tracker = new(new TrackerModel(Quest), ShipSelectorFactory)
		{
			ShowDetails = true,
		};

		Trackers.Add(tracker);

		KCDatabase.Instance.Quest.OnQuestUpdated();
	}

	[RelayCommand]
	private void RemoveTracker(TrackerViewModel? tracker)
	{
		if (tracker is null) return;

		Trackers.Remove(tracker);

		KCDatabase.Instance.Quest.OnQuestUpdated();
	}

	public void ManageTracker(int questId)
	{
		foreach (TrackerViewModel trackerViewModel in Trackers)
		{
			trackerViewModel.ShowDetails = false;
		}

		if (Trackers.FirstOrDefault(t => t.Model.Quest.Id == questId) is { } tracker)
		{
			tracker.ShowDetails = true;
		}
		else if (KCDatabase.Instance.SystemQuestTrackerManager.Trackers.FirstOrDefault(t => t.Model.Quest.Id == questId)
				 is { } systemTracker)
		{
			TrackerModel? copiedTracker = DeserializeTrackers(systemTracker.SerializeTracker()).FirstOrDefault();
			if (copiedTracker is null)
			{
				// log
				// no idea how copying could fail
				return;
			}

			TrackerViewModel vm = new(copiedTracker, ShipSelectorFactory) { ShowDetails = true };

			vm.SetProgress(systemTracker.GetProgress());

			Trackers.Add(vm);
		}
		else
		{
			TrackerViewModel vm = new(new TrackerModel(new QuestModel(questId)), ShipSelectorFactory)
			{
				ShowDetails = true,
			};

			Trackers.Add(vm);
			KCDatabase.Instance.Quest.OnQuestUpdated();
		}
	}

	public override void Save()
	{
		SaveExistingTrackers();
		SaveProgress();
	}

	public void Load()
	{
		LoadExistingTrackers();
		LoadProgress();
	}

	private string CustomTrackerPath => Path.Combine("Record", "QuestTrackers.json");
	private string ProgressPath => Path.Combine("Record", "QuestProgress.json");

	private void SaveExistingTrackers()
	{
		try
		{
			IEnumerable<TrackerModel> trackers = Trackers.Select(t => t.Model);
			byte[] trackerBytes = MessagePackSerializer.Serialize(trackers);
			File.WriteAllText(CustomTrackerPath, MessagePackSerializer.ConvertToJson(trackerBytes));
		}
		catch
		{
		}
	}

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
