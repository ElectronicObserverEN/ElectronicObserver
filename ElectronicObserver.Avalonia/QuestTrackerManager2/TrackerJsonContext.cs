using System.Text.Json.Serialization;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Not;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipLevel;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipNationality;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipType;
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

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

[JsonSerializable(typeof(TrackerModelV2))]
[JsonSerializable(typeof(FleetCondition), TypeInfoPropertyName = "QuestFleetCondition")]
[JsonSerializable(typeof(UnknownFleetCondition))]
[JsonSerializable(typeof(GroupCondition))]
[JsonSerializable(typeof(NotCondition))]
[JsonSerializable(typeof(ReferenceCondition))]
[JsonSerializable(typeof(ShipIdCondition))]
[JsonSerializable(typeof(ShipTypeCondition))]
[JsonSerializable(typeof(ShipClassCondition))]
[JsonSerializable(typeof(PositionCondition))]
[JsonSerializable(typeof(ShipCountCondition))]
[JsonSerializable(typeof(ShipLevelCondition))]
[JsonSerializable(typeof(ShipNationalityCondition))]
[JsonSerializable(typeof(ITaskModel), TypeInfoPropertyName = "QuestTaskModel")]
[JsonSerializable(typeof(UnknownTaskModel))]
[JsonSerializable(typeof(BossKillTaskModel))]
[JsonSerializable(typeof(ExpeditionTaskModel))]
[JsonSerializable(typeof(BattleNodeIdTaskModel))]
[JsonSerializable(typeof(NodeReachTaskModel))]
[JsonSerializable(typeof(MapFirstClearTaskModel))]
[JsonSerializable(typeof(ExerciseTaskModel))]
[JsonSerializable(typeof(EquipmentScrapTaskModel))]
[JsonSerializable(typeof(EquipmentCategoryScrapTaskModel))]
[JsonSerializable(typeof(EquipmentCardTypeScrapTaskModel))]
[JsonSerializable(typeof(EquipmentIconTypeScrapTaskModel))]
[JsonSerializable(typeof(List<TrackerModelV2>))]
[JsonSerializable(typeof(IEnumerable<TrackerModelV2>))]
[JsonSerializable(typeof(QuestProgressRecordV2))]
[JsonSerializable(typeof(QuestTrackerProgressRecordV2))]
public partial class TrackerJsonContext : JsonSerializerContext;
