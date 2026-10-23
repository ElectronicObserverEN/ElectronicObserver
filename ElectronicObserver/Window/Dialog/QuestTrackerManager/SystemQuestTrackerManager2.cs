using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.Translation;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public sealed class SystemQuestTrackerManager2 : IQuestTrackerManager
{
	private IQuestProvider QuestProvider { get; }
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	private TaskFactory TaskViewModelFactory { get; }
	private FleetEvaluationService FleetEvaluationService { get; }

	private List<TrackerViewModelV2> Trackers { get; set; } = [];

	IEnumerable<ITrackerViewModel> IQuestTrackerManager.Trackers => Trackers;

	public DateTime LastQuestListUpdate { get; set; } = new(2000, 1, 1);
	public DateTime LastTrackerSave { get; set; } = DateTime.Now;

	private static string TrackerPath => Path.Join(DataConstants.DataFolder, "QuestTrackersV2.json");
	private static string ProgressPath => Path.Combine("Record", "SystemQuestProgressV2.json");

	public SystemQuestTrackerManager2(
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

	public void Load()
	{
		List<TrackerModelV2> trackerModels = LoadTrackerModels();
		Trackers = [..trackerModels.Select(ToViewModel)];

		LoadProgress();
	}

	public (TrackerModelV2 Model, IEnumerable<int> Progress)? TryCopyTracker(int questId)
	{
		TrackerViewModelV2? tracker = Trackers.FirstOrDefault(t => t.QuestId == questId);

		return tracker switch
		{
			null => null,
			_ => (CopyModel(tracker.Model), tracker.GetProgress())
		};
	}


	private static TrackerModelV2 CopyModel(TrackerModelV2 model)
	{
		string json = JsonSerializer.Serialize(model);
		return JsonSerializer.Deserialize<TrackerModelV2>(json)!;
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
				Trackers
					.FirstOrDefault(t => t.QuestId == trackerProgress.QuestId)
					?.SetProgress(trackerProgress.Progresses);
			}

			LastQuestListUpdate = progress.LastQuestListUpdate;
		}
		catch
		{
		}
	}

	public void Save()
	{
		try
		{
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
