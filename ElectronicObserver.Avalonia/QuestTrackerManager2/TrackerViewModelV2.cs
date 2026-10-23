using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BattleNodeId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BossKill;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCardTypeScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCategoryScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentIconTypeScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentScrap;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Exercise;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Expedition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.MapFirstClear;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.NodeReach;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public partial class TrackerViewModelV2 : ObservableObject, ITrackerViewModel
{
	private IQuestProvider QuestProvider { get; }
	private TaskFactory TaskFactory { get; }
	private FleetEvaluationService FleetEvaluationService { get; }
	public TrackerModelV2 Model { get; }

	public ObservableCollection<TaskViewModel?> Tasks { get; }
	public FleetConditionSetViewModel Condition { get; }

	public List<TaskType> TaskTypes { get; } = [.. Enum.GetValues<TaskType>()];

	[ObservableProperty] public partial TaskType SelectedTaskType { get; set; } = TaskType.BossKill;

	public int QuestId => Model.QuestId;
	public int State => QuestProvider.TryGetQuest(QuestId)?.State ?? 0;
	public string? Code => QuestProvider.TryGetQuest(QuestId)?.Code;
	public string Name => QuestProvider.TryGetQuest(QuestId)?.Name ?? QuestTrackerManagerResources.UnknownQuest;
	public string? Description => QuestProvider.TryGetQuest(QuestId)?.Description;

	public QuestResetType QuestResetType =>
		QuestProvider.TryGetQuest(QuestId)?.GetProgressResetType() ?? QuestResetType.Unknown;

	public string QuestDisplay => $"{Code}: {Name} (ID: {QuestId})";

	public string ClearCondition => string.Join("\n", Tasks.Select(t => t?.Display));
	public string ConditionDisplay => Condition.Display;

	public int TaskCount => Tasks.Count;

	[ObservableProperty] public partial bool IsExpanded { get; set; }

	public TrackerViewModelV2(
		IQuestProvider questProvider,
		ConditionFactory conditionFactory,
		ConditionDisplayerService conditionDisplayerService,
		TaskFactory taskViewModelFactory,
		FleetEvaluationService fleetEvaluationService,
		TrackerModelV2 model)
	{
		QuestProvider = questProvider;
		TaskFactory = taskViewModelFactory;
		FleetEvaluationService = fleetEvaluationService;
		Model = model;
		Tasks = [];
		Condition = new(conditionFactory, conditionDisplayerService, model.Conditions);

		foreach (ITaskModel? taskModel in model.Tasks)
		{
			AddTaskViewModel(taskModel);
		}

		Condition.ConditionChanged += () =>
		{
			OnPropertyChanged(string.Empty);
			QuestProvider.NotifyQuestUpdated();
		};
	}

	private void AddTaskViewModel(ITaskModel? taskModel)
	{
		TaskViewModel? task = TaskFactory.CreateViewModel(taskModel);

		task?.PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(TaskViewModel.Display)) return;

			QuestProvider.NotifyQuestUpdated();
		};

		Tasks.Add(task);
	}

	[RelayCommand]
	private void AddTask()
	{
		ITaskModel? taskModel = TaskFactory.CreateModel(SelectedTaskType);

		if (taskModel is null) return;

		Model.Tasks.Add(taskModel);
		AddTaskViewModel(taskModel);
		QuestProvider.NotifyQuestUpdated();
	}

	[RelayCommand]
	private void RemoveTask(TaskViewModel? task)
	{
		if (task is null) return;

		Tasks.Remove(task);
		Model.Tasks.Remove(task.Model);
		QuestProvider.NotifyQuestUpdated();
	}

	/// <inheritdoc />
	public bool ConditionMet(IFleetData fleet) => fleet.MembersWithoutEscaped switch
	{
		null => false,
		_ => FleetEvaluationService.Evaluate(Model.Conditions, [.. fleet.MembersWithoutEscaped.OfType<IShipData>()]),
	};

	private static BattleRank ParseRank(string rank) => rank.ToUpper() switch
	{
		"E" => BattleRank.E,
		"D" => BattleRank.D,
		"C" => BattleRank.C,
		"B" => BattleRank.B,
		"A" => BattleRank.A,
		"S" => BattleRank.S,
		"SS" => BattleRank.SS,
		_ => BattleRank.Any,
	};

	/// <inheritdoc />
	public void Increment(IFleetData fleet, int compassMapAreaId, int compassMapInfoId, int nodeId)
	{
		if (!ConditionMet(fleet)) return;

		foreach (NodeReachTaskViewModel task in Tasks.OfType<NodeReachTaskViewModel>())
		{
			task.Increment(compassMapAreaId, compassMapInfoId, nodeId);
		}
	}

	/// <inheritdoc />
	public void Increment(IFleetData fleet, string resultRank, int compassMapAreaId, int compassMapInfoId, int nodeId)
	{
		if (!ConditionMet(fleet)) return;

		BattleRank rank = ParseRank(resultRank);

		foreach (BattleNodeIdTaskViewModel task in Tasks.OfType<BattleNodeIdTaskViewModel>())
		{
			task.Increment(rank, compassMapAreaId, compassMapInfoId, nodeId);
		}
	}

	/// <inheritdoc />
	public void Increment(IFleetData fleet, string resultRank, int compassMapAreaId, int compassMapInfoId)
	{
		if (!ConditionMet(fleet)) return;

		BattleRank rank = ParseRank(resultRank);

		foreach (BossKillTaskViewModel task in Tasks.OfType<BossKillTaskViewModel>())
		{
			task.Increment(rank, compassMapAreaId, compassMapInfoId);
		}
	}

	/// <inheritdoc />
	public void Increment(IFleetData fleet, int compassMapAreaId, int compassMapInfoId)
	{
		if (!ConditionMet(fleet)) return;

		foreach (MapFirstClearTaskViewModel task in Tasks.OfType<MapFirstClearTaskViewModel>())
		{
			task.Increment(compassMapAreaId, compassMapInfoId);
		}
	}

	/// <inheritdoc />
	public void Increment(IFleetData fleet, string winRank)
	{
		if (!ConditionMet(fleet)) return;

		BattleRank rank = ParseRank(winRank);

		foreach (ExerciseTaskViewModel task in Tasks.OfType<ExerciseTaskViewModel>())
		{
			task.Increment(rank);
		}
	}

	/// <inheritdoc />
	public void Increment(IFleetData fleet, int areaId)
	{
		if (!ConditionMet(fleet)) return;

		foreach (ExpeditionTaskViewModel task in Tasks.OfType<ExpeditionTaskViewModel>())
		{
			task.Increment(areaId);
		}
	}

	/// <inheritdoc />
	public void Increment(IEnumerable<EquipmentId> ids)
	{
		List<EquipmentId> idList = [.. ids];

		foreach (EquipmentScrapTaskViewModel task in Tasks.OfType<EquipmentScrapTaskViewModel>())
		{
			task.Increment(idList);
		}
	}

	/// <inheritdoc />
	public void Increment(IEnumerable<EquipmentTypes> categories)
	{
		List<EquipmentTypes> categoryList = [.. categories];

		foreach (EquipmentCategoryScrapTaskViewModel task in Tasks.OfType<EquipmentCategoryScrapTaskViewModel>())
		{
			task.Increment(categoryList);
		}
	}

	/// <inheritdoc />
	public void Increment(IEnumerable<EquipmentCardType> cardTypes)
	{
		List<EquipmentCardType> cardTypeList = [.. cardTypes];

		foreach (EquipmentCardTypeScrapTaskViewModel task in Tasks.OfType<EquipmentCardTypeScrapTaskViewModel>())
		{
			task.Increment(cardTypeList);
		}
	}

	/// <inheritdoc />
	public void Increment(IEnumerable<EquipmentIconType> iconTypes)
	{
		List<EquipmentIconType> iconTypeList = [.. iconTypes];

		foreach (EquipmentIconTypeScrapTaskViewModel task in Tasks.OfType<EquipmentIconTypeScrapTaskViewModel>())
		{
			task.Increment(iconTypeList);
		}
	}

	public void ResetTaskProgress()
	{
		foreach (TaskViewModel? task in Tasks)
		{
			task?.Progress = 0;
		}
	}

	public IEnumerable<int> GetProgress() => Tasks.Select(t => t?.Progress ?? 0);

	public void SetProgress(IEnumerable<int> progresses)
	{
		foreach ((TaskViewModel? task, int progress) in Tasks.Zip(progresses, (task, progress) => (task, progress)))
		{
			task?.Progress = progress;
		}
	}
}
