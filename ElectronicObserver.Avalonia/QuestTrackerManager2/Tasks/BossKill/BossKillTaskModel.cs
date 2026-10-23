namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.BossKill;

public class BossKillTaskModel : ITaskModel
{
	public MapInfoModel Map { get; set; } = new();
	public BattleRank Rank { get; set; } = BattleRank.S;
	public int Count { get; set; } = 1;
}
