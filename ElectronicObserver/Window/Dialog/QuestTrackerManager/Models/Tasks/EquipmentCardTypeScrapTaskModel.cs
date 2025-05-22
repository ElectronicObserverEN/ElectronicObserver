using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class EquipmentCardTypeScrapTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public EquipmentCardType CardType { get; set; } = EquipmentCardType.PrimaryArmament;
	[Key(1)] public int Count { get; set; }
	[IgnoreMember] public int Progress { get; set; }

	public EquipmentCardTypeScrapTaskModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Progress) or nameof(Count))) return;

			Progress = Math.Clamp(Progress, 0, Count);
			Count = Math.Clamp(Count, 1, int.MaxValue);
		};
	}
}
