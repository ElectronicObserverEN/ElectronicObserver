using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public class FleetConditionSet
{
	public List<ReferenceDefinition> Definitions { get; init; } = [];
	public FleetCondition? Condition { get; set; }
}
