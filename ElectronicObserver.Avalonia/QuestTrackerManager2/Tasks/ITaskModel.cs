using System.Text.Json.Serialization;
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

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(BossKillTaskModel),  (int)TaskType.BossKill)]
[JsonDerivedType(typeof(ExpeditionTaskModel), (int)TaskType.Expedition)]
[JsonDerivedType(typeof(BattleNodeIdTaskModel), (int)TaskType.BattleNodeId)]
[JsonDerivedType(typeof(NodeReachTaskModel), (int)TaskType.NodeReach)]
[JsonDerivedType(typeof(MapFirstClearTaskModel), (int)TaskType.MapFirstClear)]
[JsonDerivedType(typeof(ExerciseTaskModel), (int)TaskType.Exercise)]
[JsonDerivedType(typeof(EquipmentScrapTaskModel), (int)TaskType.EquipmentScrap)]
[JsonDerivedType(typeof(EquipmentCategoryScrapTaskModel), (int)TaskType.EquipmentCategoryScrap)]
[JsonDerivedType(typeof(EquipmentCardTypeScrapTaskModel), (int)TaskType.EquipmentCardTypeScrap)]
[JsonDerivedType(typeof(EquipmentIconTypeScrapTaskModel), (int)TaskType.EquipmentIconTypeScrap)]
public interface ITaskModel
{
	int Count { get; set; }
}
