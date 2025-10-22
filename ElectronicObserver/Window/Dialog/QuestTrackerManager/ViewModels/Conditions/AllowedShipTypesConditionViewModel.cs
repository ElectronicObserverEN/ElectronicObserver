using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class AllowedShipTypesConditionViewModel : ObservableObject, IConditionViewModel
{
	public AllowedShipTypesConditionModel Model { get; set; }

	public ShipTypes SelectedType { get; set; } = ShipTypes.Destroyer;
	public IEnumerable<ShipTypes> AllTypes { get; }

	public string Display => $"({QuestTrackerManagerResources.ConditionType_AllowedShipTypes}：{CountConditionDisplay})";

	private string CountConditionDisplay => string.Join("・", Model.Types.Select(s => s.Display()));

	public AllowedShipTypesConditionViewModel(AllowedShipTypesConditionModel model)
	{
		Model = model;

		AllTypes = Enum.GetValues<ShipTypes>().Where(t => t is not ShipTypes.Unknown);

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		Model.Types.CollectionChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};
	}

	[RelayCommand]
	private void AddType()
	{
		Model.Types.Add(SelectedType);
	}

	[RelayCommand]
	private void RemoveType(ShipTypes s)
	{
		Model.Types.Remove(s);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		return !fleet.MembersInstance
			.Where(s => s is not null)
			.Any(s => !Model.Types.Contains(s.MasterShip.ShipType));
	}
}
