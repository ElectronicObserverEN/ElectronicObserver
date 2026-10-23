namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;

public class ShipCountCondition : FleetCondition
{
	public int Count { get; set; }
	public ComparisonOperator Operator { get; set; }
	public FleetCondition? Condition { get; set; }
}
