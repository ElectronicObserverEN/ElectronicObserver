namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public sealed class UnknownConditionViewModel(UnknownFleetCondition model) : ConditionViewModel
{
	public override UnknownFleetCondition Model { get; } = model;

	public override string Display => QuestTrackerManagerResources.UnknownCondition;
}
