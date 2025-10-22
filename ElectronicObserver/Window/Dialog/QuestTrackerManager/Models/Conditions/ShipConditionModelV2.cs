using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class ShipConditionModelV2 : ObservableObject, ICondition
{
	[Key(0)] public ShipId Id { get; set; }
	[Key(1)] public bool MustBeFlagship { get; set; }
	[Key(2)] public RemodelComparisonType RemodelComparisonType { get; set; } = RemodelComparisonType.Any;
	[Key(3)] public ShipClass ShipClass { get; set; }
}
