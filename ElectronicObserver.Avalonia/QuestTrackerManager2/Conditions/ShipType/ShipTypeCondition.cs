using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipType;

public class ShipTypeCondition : FleetCondition
{
	public ShipTypes ShipType { get; set; } = ShipTypes.Destroyer;
}
