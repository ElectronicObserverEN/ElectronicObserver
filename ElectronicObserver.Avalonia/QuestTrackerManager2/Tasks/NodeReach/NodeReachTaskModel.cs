namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.NodeReach;

public class NodeReachTaskModel : ITaskModel
{
	public MapInfoModel Map { get; set; } = new();
	public int Count { get; set; } = 1;
	public List<int> NodeIds { get; set; } = [];
	public string? Name { get; set; }
}
