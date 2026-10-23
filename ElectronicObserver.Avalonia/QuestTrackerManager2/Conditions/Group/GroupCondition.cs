namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;

public class GroupCondition : FleetCondition
{
	public LogicalOperator Operator { get; set; }
	public List<FleetCondition> Conditions { get; init; } = [];
}