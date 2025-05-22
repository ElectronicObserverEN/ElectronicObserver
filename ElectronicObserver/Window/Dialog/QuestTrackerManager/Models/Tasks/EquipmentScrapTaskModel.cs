using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class EquipmentScrapTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public EquipmentId Id { get; set; } = EquipmentId.MainGunSmall_12cmSingleGun;
	[Key(1)] public int Count { get; set; }
	[IgnoreMember] public int Progress { get; set; }

	public EquipmentScrapTaskModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Progress) or nameof(Count))) return;

			Progress = Math.Clamp(Progress, 0, Count);
			Count = Math.Clamp(Count, 1, int.MaxValue);
		};
	}
}
