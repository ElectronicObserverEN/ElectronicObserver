﻿using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[Union(0, typeof(BossKillTaskModel))]
[Union(1, typeof(ExpeditionTaskModel))]
[Union(2, typeof(BattleNodeIdTaskModel))]
[Union(3, typeof(EquipmentScrapTaskModel))]
[Union(4, typeof(EquipmentCategoryScrapTaskModel))]
[Union(5, typeof(EquipmentCardTypeScrapTaskModel))]
[Union(6, typeof(EquipmentIconTypeScrapTaskModel))]
[Union(7, typeof(NodeReachTaskModel))]
[Union(8, typeof(MapFirstClearTaskModel))]
[Union(9, typeof(ExerciseTaskModel))]
public interface IQuestTask
{
	int Progress { get; set; }
	int Count { get; }
}
