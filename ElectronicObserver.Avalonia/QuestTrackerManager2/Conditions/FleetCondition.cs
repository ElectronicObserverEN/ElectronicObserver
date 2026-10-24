using System.Text.Json.Serialization;
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

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(GroupCondition), (int)ConditionType.Group)]
[JsonDerivedType(typeof(NotCondition), (int)ConditionType.Not)]
[JsonDerivedType(typeof(ReferenceCondition), (int)ConditionType.Reference)]
[JsonDerivedType(typeof(ShipIdCondition), (int)ConditionType.ShipId)]
[JsonDerivedType(typeof(ShipTypeCondition), (int)ConditionType.ShipType)]
[JsonDerivedType(typeof(ShipClassCondition), (int)ConditionType.ShipClass)]
[JsonDerivedType(typeof(PositionCondition), (int)ConditionType.ShipPosition)]
[JsonDerivedType(typeof(ShipCountCondition), (int)ConditionType.ShipCount)]
[JsonDerivedType(typeof(ShipLevelCondition), (int)ConditionType.ShipLevel)]
[JsonDerivedType(typeof(ShipNationalityCondition), (int)ConditionType.ShipNationality)]
public abstract class FleetCondition
{
}
