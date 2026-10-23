using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Core;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public sealed partial class QuestTrackerManager2ViewModel : ObservableObject, IQuestTrackerManager
{
	private IQuestProvider QuestProvider { get; }
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	private TaskFactory TaskViewModelFactory { get; }
	private FleetEvaluationService FleetEvaluationService { get; }

	public ObservableCollection<TrackerViewModelV2> Trackers { get; } = [];

	IEnumerable<ITrackerViewModel> IQuestTrackerManager.Trackers => Trackers;

	public DateTime LastQuestListUpdate { get; set; } = new(2000, 1, 1);
	public DateTime LastTrackerSave { get; set; } = DateTime.Now;

	// Deferred on purpose: quests are only populated once the game data has actually loaded,
	// which can happen well after this view model is constructed.
	public IEnumerable<IQuestData> Quests => QuestProvider.GetAllQuests();

	[ObservableProperty] public partial IQuestData? SelectedQuest { get; set; }

	private static string TrackerPath => Path.Combine("Record", "QuestTrackersV2.json");
	private static string ProgressPath => Path.Combine("Record", "QuestProgressV2.json");

	public QuestTrackerManager2ViewModel(
		IQuestProvider questProvider,
		ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		TaskFactory taskViewModelFactory,
		FleetEvaluationService fleetEvaluationService)
	{
		QuestProvider = questProvider;
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		TaskViewModelFactory = taskViewModelFactory;
		FleetEvaluationService = fleetEvaluationService;

		Load();
	}

	private TrackerViewModelV2 ToViewModel(TrackerModelV2 model)
		=> new(QuestProvider, ConditionFactory, ConditionDisplayerService, TaskViewModelFactory, FleetEvaluationService, model);

	[RelayCommand]
	private void AddTracker()
	{
		if (SelectedQuest is not { } quest) return;
		if (Trackers.Any(t => t.QuestId == quest.QuestID)) return;

		Trackers.Add(ToViewModel(new TrackerModelV2(quest.QuestID)));
		QuestProvider.NotifyQuestUpdated();
	}

	public void ManageTracker(int questId, TrackerModelV2? template = null, IEnumerable<int>? templateProgress = null)
	{
		foreach (TrackerViewModelV2 existingTracker in Trackers)
		{
			existingTracker.IsExpanded = false;
		}

		if (Trackers.FirstOrDefault(t => t.QuestId == questId) is { } existing)
		{
			existing.IsExpanded = true;
			return;
		}

		TrackerViewModelV2 tracker = ToViewModel(template ?? new TrackerModelV2(questId));
		tracker.IsExpanded = true;

		if (templateProgress is not null)
		{
			tracker.SetProgress(templateProgress);
		}

		Trackers.Add(tracker);
		QuestProvider.NotifyQuestUpdated();
	}

	[RelayCommand]
	private void RemoveTracker(TrackerViewModelV2? tracker)
	{
		if (tracker is null) return;

		Trackers.Remove(tracker);
		QuestProvider.NotifyQuestUpdated();
	}

	public void Load()
	{
		Trackers.Clear();

		foreach (TrackerModelV2 model in LoadTrackerModels())
		{
			Trackers.Add(ToViewModel(model));
		}

		LoadProgress();
	}

	private static List<TrackerModelV2> LoadTrackerModels()
	{
		if (!File.Exists(TrackerPath)) return [];

		try
		{
			string json = File.ReadAllText(TrackerPath);
			return JsonSerializer.Deserialize<List<TrackerModelV2>>(json) ?? [];
		}
		catch
		{
			return [];
		}
	}

	private void LoadProgress()
	{
		if (!File.Exists(ProgressPath)) return;

		try
		{
			string json = File.ReadAllText(ProgressPath);
			QuestProgressRecordV2? progress = JsonSerializer.Deserialize<QuestProgressRecordV2>(json);

			if (progress is null) return;

			foreach (QuestTrackerProgressRecordV2 trackerProgress in progress.TrackerProgresses)
			{
				Trackers.FirstOrDefault(t => t.QuestId == trackerProgress.QuestId)?.SetProgress(trackerProgress.Progresses);
			}

			LastQuestListUpdate = progress.LastQuestListUpdate;
		}
		catch
		{
		}
	}

	public void Save()
	{
		SaveTrackers();
		SaveProgress();
	}

	private void SaveTrackers()
	{
		try
		{
			Directory.CreateDirectory("Record");

			string json = JsonSerializer.Serialize(Trackers.Select(t => t.Model));
			File.WriteAllText(TrackerPath, json);
		}
		catch
		{
		}
	}

	private void SaveProgress()
	{
		try
		{
			Directory.CreateDirectory("Record");

			QuestProgressRecordV2 progress = new(
				LastQuestListUpdate,
				Trackers.Select(t => new QuestTrackerProgressRecordV2(t.QuestId, t.GetProgress())));

			string json = JsonSerializer.Serialize(progress);
			File.WriteAllText(ProgressPath, json);
		}
		catch
		{
		}
	}

	public ITrackerViewModel? GetTrackerById(int id) => Trackers.FirstOrDefault(t => t.QuestId == id);
}
