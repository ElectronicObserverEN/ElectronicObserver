using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

[Obsolete("Use PartialShipConditionViewModelV2")]
public partial class PartialShipConditionViewModel : ObservableObject, IConditionViewModel
{
	private ShipSelectorFactory ShipSelectorFactory { get; }

	public PartialShipConditionModel Model { get; }
	public IEnumerable<ShipConditionViewModel> Conditions { get; set; }

	public string Display => $"({Ships}) >= {Model.Count}";

	private string Ships => string.Join("+", Conditions.Select(c => c.Display));

	public PartialShipConditionViewModel(PartialShipConditionModel model, ShipSelectorFactory shipSelectorViewModel)
	{
		ShipSelectorFactory = shipSelectorViewModel;

		Model = model;
		Conditions = CreateViewModels(Model);

		Model.PropertyChanged += (_, _) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		Model.Conditions.CollectionChanged += (_, _) =>
		{
			Conditions = CreateViewModels(Model);
		};
	}

	private List<ShipConditionViewModel> CreateViewModels(PartialShipConditionModel model)
	{
		List<ShipConditionViewModel> conditions = model.Conditions
			.Select(s => new ShipConditionViewModel(s, ShipSelectorFactory))
			.ToList();

		foreach (IConditionViewModel condition in conditions)
		{
			condition.PropertyChanged += (_, args) =>
			{
				if (args.PropertyName is not nameof(IConditionViewModel.Display)) return;

				OnPropertyChanged(nameof(Display));
			};
		}

		return conditions;
	}

	[RelayCommand]
	private void AddCondition()
	{
		Model.Conditions.Add(new ShipConditionModel());
	}

	[RelayCommand]
	private void RemoveCondition(ShipConditionModel? condition)
	{
		if (condition is null) return;

		Model.Conditions.Remove(condition);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		return Conditions.Count(c => c.ConditionMet(fleet)) >= Model.Count;
	}
}
