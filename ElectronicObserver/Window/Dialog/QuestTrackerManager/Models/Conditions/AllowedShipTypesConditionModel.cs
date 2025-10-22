using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class AllowedShipTypesConditionModel : ObservableObject, ICondition
{
	[Key(0)] public ObservableCollection<ShipTypes> Types { get; set; } = new() { ShipTypes.Destroyer };
}
