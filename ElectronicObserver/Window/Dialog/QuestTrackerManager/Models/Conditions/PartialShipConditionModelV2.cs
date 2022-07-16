using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class PartialShipConditionModelV2 : ObservableObject, ICondition
{
	[Key(0)] public ObservableCollection<ShipConditionModelV2> Conditions { get; set; } = new();
	[Key(1)] public int Count { get; set; }
}
