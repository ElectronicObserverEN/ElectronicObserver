using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class ExerciseTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public BattleRank Rank { get; set; } = BattleRank.S;
	[Key(1)] public int Count { get; set; }
	[IgnoreMember] public int Progress { get; set; }

	public ExerciseTaskModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Progress) or nameof(Count))) return;

			Progress = Math.Clamp(Progress, 0, Count);
			Count = Math.Clamp(Count, 1, int.MaxValue);
		};
	}
}
