using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.Translation;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public sealed class SystemQuestTrackerManager2 : IQuestTrackerManager
{
	private IQuestProvider QuestProvider { get; }
	private ConditionFactory ConditionFactory { get; }
	private ConditionDisplayerService ConditionDisplayerService { get; }
	private TaskFactory TaskViewModelFactory { get; }
	private FleetEvaluationService FleetEvaluationService { get; }
	private IClipboardService ClipboardService { get; }

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

	public void Load()
	{
		List<TrackerModelV2> trackerModels = TrackerSerializer.LoadTrackerModels(TrackerPath);
		Trackers = [.. trackerModels.Select(ToViewModel)];

		if (TrackerSerializer.LoadProgress(Trackers, ProgressPath) is DateTime lastUpdate)
		{
			LastQuestListUpdate = lastUpdate;
		}
	}

	public (TrackerModelV2 Model, IEnumerable<int> Progress)? TryCopyTracker(int questId)
	{
		TrackerViewModelV2? tracker = Trackers.FirstOrDefault(t => t.QuestId == questId);

		return tracker switch
		{
			null => null,
			_ => (TrackerSerializer.CloneModel(tracker.Model), tracker.GetProgress())
		};
	}

	public void Save()
	{
		TrackerSerializer.SaveProgress(Trackers, ProgressPath, LastQuestListUpdate);
	}

	public ITrackerViewModel? GetTrackerById(int id) => Trackers.FirstOrDefault(t => t.QuestId == id);
}
