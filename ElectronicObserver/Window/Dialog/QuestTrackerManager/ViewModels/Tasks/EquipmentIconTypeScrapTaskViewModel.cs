using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Quest;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class EquipmentIconTypeScrapTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<EquipmentIconType> IconTypes { get; }

	public EquipmentIconTypeScrapTaskModel Model { get; }

	public string Display => $"{Model.Progress}/{Model.Count} {ClearCondition}";
	public string? Progress => (Model.Progress >= Model.Count) switch
	{
		true => null,
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition() => $"{QuestTracking.Icon}[{Model.IconType.TranslatedName()}]"
		+ QuestTracking.Discard + Model.Count + QuestTracking.NumberOfPieces;

	public EquipmentIconTypeScrapTaskViewModel(EquipmentIconTypeScrapTaskModel model)
	{
		IconTypes = Enum.GetValues<EquipmentIconType>();

		Model = model;

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	public void Increment(IEnumerable<EquipmentIconType> iconTypes)
	{
		Model.Progress += iconTypes.Count(id => id == Model.IconType);
	}
}
