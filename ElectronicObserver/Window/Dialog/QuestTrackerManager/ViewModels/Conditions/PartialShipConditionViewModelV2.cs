using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class PartialShipConditionViewModelV2 : ObservableObject, IConditionViewModel
{
	private ShipSelectorFactory ShipSelectorFactory { get; }

	public PartialShipConditionModelV2 Model { get; }
	public IEnumerable<ShipConditionViewModelV2> Conditions { get; set; }

	public string Display => $"({Ships}) >= {Model.Count}";

	private string Ships => string.Join(" + ", Conditions.Select(c => c.Display));

	public PartialShipConditionViewModelV2(PartialShipConditionModelV2 model, ShipSelectorFactory shipSelectorViewModel)
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

	private List<ShipConditionViewModelV2> CreateViewModels(PartialShipConditionModelV2 model)
	{
		List<ShipConditionViewModelV2> conditions = model.Conditions
			.Select(s => new ShipConditionViewModelV2(s, ShipSelectorFactory))
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
		Model.Conditions.Add(new ShipConditionModelV2());
	}

	[RelayCommand]
	private void RemoveCondition(ShipConditionModelV2? condition)
	{
		if (condition is null) return;

		Model.Conditions.Remove(condition);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		IEnumerable<ShipConditionViewModelV2> classConditions = Conditions
			.Where(c => c.ShipClass is not 0 && c.Ship is null);

		int classMatchCount = fleet.MembersInstance
			.Select(s => new FleetDataMock
			{
				MembersInstance = new ReadOnlyCollection<IShipData?>(
				[
					s,
				]),
			})
			.Sum(f => classConditions.Count(c => c.ConditionMet(f)));

		int nonClassMatchCount = Conditions
			.Except(classConditions)
			.Count(c => c.ConditionMet(fleet));

		return classMatchCount + nonClassMatchCount >= Model.Count;
	}
}
