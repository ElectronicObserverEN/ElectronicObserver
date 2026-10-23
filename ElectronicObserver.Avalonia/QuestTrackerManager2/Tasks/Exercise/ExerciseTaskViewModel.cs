using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Exercise;

public partial class ExerciseTaskViewModel(
	TaskDisplayerService taskDisplayerService,
	ExerciseTaskModel model) : TaskViewModel
{
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	public override ExerciseTaskModel Model { get; } = model;

	public List<BattleRank> Ranks { get; } = [.. Enum.GetValues<BattleRank>()];

	[ObservableProperty] public partial BattleRank Rank { get; set; } = model.Rank;
	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

	public override string Display => TaskDisplayerService.ToDisplay(this);

	partial void OnRankChanged(BattleRank value)
	{
		Model.Rank = value;
		RaiseDisplayChanged();
	}

	partial void OnCountChanged(int value)
	{
		Count = Math.Clamp(value, 1, int.MaxValue);
		Model.Count = Count;
		OnProgressChanged(Progress);
	}

	partial void OnProgressChanged(int value)
	{
		Progress = Math.Clamp(value, 0, Count);
		RaiseDisplayChanged();
	}

	public void Increment(BattleRank resultRank)
	{
		if (resultRank < Rank) return;

		Progress++;
	}
}
