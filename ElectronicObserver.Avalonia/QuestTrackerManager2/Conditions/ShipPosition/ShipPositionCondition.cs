namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;

public class PositionCondition : FleetCondition
{
	public List<int> Positions { get; set; } = [];
	public FleetCondition? Condition { get; set; }
}
