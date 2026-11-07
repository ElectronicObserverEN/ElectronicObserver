namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Exercise;

public class ExerciseTaskModel : ITaskModel
{
	public BattleRank Rank { get; set; } = BattleRank.S;
	public int Count { get; set; } = 1;
}
