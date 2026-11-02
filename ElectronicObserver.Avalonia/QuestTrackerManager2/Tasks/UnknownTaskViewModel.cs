namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public sealed class UnknownTaskViewModel(UnknownTaskModel model) : TaskViewModel
{
	public override UnknownTaskModel Model { get; } = model;
	public override string Display => QuestTrackerManagerResources.UnknownTask;
	public override int Count { get; set; }
	public override int Progress { get; set; }
}
