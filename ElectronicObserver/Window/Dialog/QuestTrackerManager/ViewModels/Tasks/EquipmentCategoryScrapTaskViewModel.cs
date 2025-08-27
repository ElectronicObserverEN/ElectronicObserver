using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class EquipmentCategoryScrapTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<EquipmentTypes> Categories { get; }

	public EquipmentCategoryScrapTaskModel Model { get; }

	public string Display => $"{Model.Progress}/{Model.Count} {ClearCondition}";
	public string? Progress => (Model.Progress >= Model.Count) switch
	{
		true => null,
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition() => KCDatabase.Instance.EquipmentTypes[(int)Model.Category].NameEN
		+ QuestTracking.Discard + Model.Count + QuestTracking.NumberOfPieces;

	public EquipmentCategoryScrapTaskViewModel(EquipmentCategoryScrapTaskModel model)
	{
		Categories = Enum.GetValues<EquipmentTypes>();

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

	public void Increment(IEnumerable<EquipmentTypes> categories)
	{
		Model.Progress += categories.Count(id => id == Model.Category);
	}
}
