using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Core;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public sealed partial class QuestTrackerManager2ViewModel : ObservableObject, IQuestTrackerManager
{
	private IQuestProvider QuestProvider { get; }
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	private TaskFactory TaskViewModelFactory { get; }
	private FleetEvaluationService FleetEvaluationService { get; }
	private IClipboardService ClipboardService { get; }

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
		FleetEvaluationService fleetEvaluationService,
		IClipboardService clipboardService)
	{
		QuestProvider = questProvider;
		ConditionFactory = conditionFactory;
		ConditionDisplayerService = conditionDisplayerService;
		TaskViewModelFactory = taskViewModelFactory;
		FleetEvaluationService = fleetEvaluationService;
		ClipboardService = clipboardService;

		Load();
	}

	private TrackerViewModelV2 ToViewModel(TrackerModelV2 model)
		=> new(QuestProvider, ConditionFactory, ConditionDisplayerService, TaskViewModelFactory, FleetEvaluationService, ClipboardService, model);

	[RelayCommand]
	private void AddTracker()
	{
		if (SelectedQuest is not { } quest) return;
		if (Trackers.Any(t => t.QuestId == quest.QuestID)) return;

		Trackers.Add(ToViewModel(new TrackerModelV2(quest.QuestID)));
		QuestProvider.NotifyQuestUpdated();
	}

	[RelayCommand]
	private void CopyTrackersToClipboard()
	{
		string json = TrackerSerializer.SerializeTrackers(Trackers.Select(t => t.Model));
		ClipboardService.SetTextAndLogErrors(json);
	}

	[RelayCommand]
	private void LoadTrackerFromClipboard()
	{
		try
		{
			string? json = ClipboardService.GetTextAndLogErrors();

			if (json is null) return;

			List<TrackerModelV2> trackers = TrackerSerializer.DeserializeTrackers(json);
			MergeTrackers(trackers);
		}
		catch
		{
			// ignored
		}
	}

	private void MergeTrackers(List<TrackerModelV2> trackers)
	{
		IEnumerable<int> existingQuestIds = Trackers.Select(t => t.QuestId);
		List<TrackerModelV2> clashingTrackers = trackers.Where(t => existingQuestIds.Contains(t.QuestId)).ToList();

		foreach (TrackerModelV2 tracker in trackers.Where(t => !existingQuestIds.Contains(t.QuestId)))
		{
			Trackers.Add(ToViewModel(tracker));
		}

		if (clashingTrackers.Any())
		{
			string warning = string.Format(QuestTrackerManagerResources.TrackerOverwrite, string.Join("\n", Trackers
				.Where(t => clashingTrackers.Select(x => x.QuestId).Contains(t.QuestId))
				.Select(t => t.Model.QuestId)));

			// todo: implement a proper dialog service for this
			// if (MessageBox.Show(warning, QuestTrackerManagerResources.Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				foreach (TrackerModelV2 tracker in clashingTrackers)
				{
					Trackers.Remove(Trackers.First(t => t.QuestId == tracker.QuestId));
					Trackers.Add(ToViewModel(tracker));
				}
			}
		}

		global::Avalonia.Application.Current?.Dispatcher?.Invoke(QuestProvider.NotifyQuestUpdated);
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

		foreach (TrackerModelV2 model in TrackerSerializer.LoadTrackerModels(TrackerPath))
		{
			Trackers.Add(ToViewModel(model));
		}

		if (TrackerSerializer.LoadProgress(Trackers, ProgressPath) is DateTime lastUpdate)
		{
			LastQuestListUpdate = lastUpdate;
		}
	}

	public void Save()
	{
		TrackerSerializer.SaveTrackers(Trackers, TrackerPath);
		TrackerSerializer.SaveProgress(Trackers, ProgressPath, LastQuestListUpdate);
	}

	public ITrackerViewModel? GetTrackerById(int id) => Trackers.FirstOrDefault(t => t.QuestId == id);
}
