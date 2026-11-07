using ElectronicObserver.Avalonia.Dialogs;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
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
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public sealed class TaskFactory(
	IKCDatabase kcDatabase,
	TaskDisplayerService taskDisplayerService,
	DialogServiceBase dialogService,
	EquipmentSelectorFactory equipmentSelectorFactory)
{
	public ITaskModel? CreateModel(TaskType taskType) => taskType switch
	{
		TaskType.BossKill => new BossKillTaskModel(),
		TaskType.Expedition => new ExpeditionTaskModel(),
		TaskType.BattleNodeId => new BattleNodeIdTaskModel(),
		TaskType.NodeReach => new NodeReachTaskModel(),
		TaskType.MapFirstClear => new MapFirstClearTaskModel(),
		TaskType.Exercise => new ExerciseTaskModel(),
		TaskType.EquipmentScrap => new EquipmentScrapTaskModel(),
		TaskType.EquipmentCategoryScrap => new EquipmentCategoryScrapTaskModel(),
		TaskType.EquipmentCardTypeScrap => new EquipmentCardTypeScrapTaskModel(),
		TaskType.EquipmentIconTypeScrap => new EquipmentIconTypeScrapTaskModel(),

		TaskType.World1Map6ResourceNode => new NodeReachTaskModel
		{
			Map = new() { AreaId = 1, InfoId = 6 },
			Name = "N",
			NodeIds = [14, 17],
		},
		TaskType.World5Map6Boss1 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 5, InfoId = 6 },
			Name = "-1",
			NodeIds = [11],
		},
		TaskType.World5Map6Boss2 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 5, InfoId = 6 },
			Name = "-2",
			NodeIds = [27],
		},
		TaskType.World5Map6Boss3 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 5, InfoId = 6 },
			Name = "-3",
			NodeIds = [43],
		},
		TaskType.World7Map2Boss1 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 7, InfoId = 2 },
			Name = "-1",
			NodeIds = [7],
		},
		TaskType.World7Map2Boss2 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 7, InfoId = 2 },
			Name = "-2",
			NodeIds = [15],
		},
		TaskType.World7Map3Boss1 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 7, InfoId = 3 },
			Name = "-1",
			NodeIds = [5, 8],
		},
		TaskType.World7Map3Boss2 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 7, InfoId = 3 },
			Name = "-2",
			NodeIds = [18, 23, 24, 25],
		},
		TaskType.World7Map5Boss1 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 7, InfoId = 5 },
			Name = "-1",
			NodeIds = [11],
		},
		TaskType.World7Map5Boss2 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 7, InfoId = 5 },
			Name = "-2",
			NodeIds = [19],
		},
		TaskType.World7Map5Boss3 => new BattleNodeIdTaskModel
		{
			Map = new() { AreaId = 7, InfoId = 5 },
			Name = "-3",
			NodeIds = [24, 25],
		},

		_ => null,
	};

	public TaskViewModel? CreateViewModel(ITaskModel? task) => task switch
	{
		BossKillTaskModel b => new BossKillTaskViewModel(kcDatabase, taskDisplayerService, b),
		ExpeditionTaskModel e => new ExpeditionTaskViewModel(kcDatabase, taskDisplayerService, e),
		BattleNodeIdTaskModel b => new BattleNodeIdTaskViewModel(kcDatabase, taskDisplayerService, b),
		NodeReachTaskModel n => new NodeReachTaskViewModel(kcDatabase, taskDisplayerService, n),
		MapFirstClearTaskModel m => new MapFirstClearTaskViewModel(kcDatabase, taskDisplayerService, m),
		ExerciseTaskModel e => new ExerciseTaskViewModel(taskDisplayerService, e),
		EquipmentScrapTaskModel e => new EquipmentScrapTaskViewModel(kcDatabase, taskDisplayerService, dialogService, equipmentSelectorFactory, e),
		EquipmentCategoryScrapTaskModel e => new EquipmentCategoryScrapTaskViewModel(taskDisplayerService, e),
		EquipmentCardTypeScrapTaskModel e => new EquipmentCardTypeScrapTaskViewModel(taskDisplayerService, e),
		EquipmentIconTypeScrapTaskModel e => new EquipmentIconTypeScrapTaskViewModel(taskDisplayerService, e),
		UnknownTaskModel u => new UnknownTaskViewModel(u),

		_ => null,
	};
}
