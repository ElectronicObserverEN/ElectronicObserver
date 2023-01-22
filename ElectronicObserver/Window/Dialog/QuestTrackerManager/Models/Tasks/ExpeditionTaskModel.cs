using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class ExpeditionTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public ExpeditionModel Expedition { get; set; } = new(1);
	[Key(1)] public int Count { get; set; }
	[IgnoreMember] public int Progress { get; set; }

	public ExpeditionTaskModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Progress) or nameof(Count))) return;

			Progress = Math.Clamp(Progress, 0, Count);
			Count = Math.Clamp(Count, 1, int.MaxValue);
		};
	}
}
