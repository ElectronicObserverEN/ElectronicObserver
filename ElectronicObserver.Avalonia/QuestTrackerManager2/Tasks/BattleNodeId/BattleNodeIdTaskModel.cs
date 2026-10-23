namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BattleNodeId;

public class BattleNodeIdTaskModel : ITaskModel
{
	public MapInfoModel Map { get; set; } = new();
	public BattleRank Rank { get; set; } = BattleRank.S;
	public int Count { get; set; } = 1;
	public List<int> NodeIds { get; set; } = [];
	public string? Name { get; set; }
}
