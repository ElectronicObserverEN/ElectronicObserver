using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class ShipPositionConditionViewModel : ObservableObject, IConditionViewModel
{
	private ShipSelectorFactory ShipSelectorFactory { get; }
	private ShipSelectorViewModel ShipSelectorViewModel => ShipSelectorFactory.QuestTrackerManager;

	[field: MaybeNull]
	public IShipDataMaster Ship
	{
		// bug: Ships don't get loaded till Kancolle loads
		get => field ??= Ships.FirstOrDefault(s => s.ShipId == Model.Id)
			?? throw new Exception("fix me: accessing this property before Kancolle gets loaded is a bug");
		set => SetProperty(ref field, value);
	}

	public IEnumerable<IShipDataMaster> Ships => KCDatabase.Instance.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.OrderBy(s => s.SortID);

	public RemodelComparisonType RemodelComparisonType { get; set; }
	public List<RemodelComparisonType> RemodelComparisonTypes { get; }

	public int Position { get; set; }

	public ShipPositionConditionModel Model { get; }

	public string Display => Ship switch
	{
		null => "",
		_ => $"{Ship.NameEN}({RemodelComparisonType.Display()})({PositionText})",
	};

	private string PositionText => $"{QuestTrackerManagerResources.Position}：{Position}";

	public ShipPositionConditionViewModel(ShipPositionConditionModel model, ShipSelectorFactory shipSelectorFactory)
	{
		ShipSelectorFactory = shipSelectorFactory;

		RemodelComparisonTypes = [.. Enum.GetValues<RemodelComparisonType>()];

		Model = model;

		RemodelComparisonType = Model.RemodelComparisonType;
		Position = Model.Position;

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(Ship)) return;

			Model.Id = Ship.ShipId;
		};

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(RemodelComparisonType)) return;

			Model.RemodelComparisonType = RemodelComparisonType;
		};

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(Position)) return;

			Model.Position = Position;
		};
	}

	[RelayCommand]
	private void OpenShipPicker()
	{
		ShipSelectorViewModel.ShowDialog();

		if (ShipSelectorViewModel.SelectedShip is null) return;

		Ship = ShipSelectorViewModel.SelectedShip.MasterShip;
	}

	public bool ConditionMet(IFleetData fleet)
	{
		List<IShipData> ships = [.. fleet.MembersInstance.OfType<IShipData>()];
		int index = Position - 1;

		if (ships.Count < index) return false;
		
		ships = [.. ships.Skip(index).Take(1)];

		return RemodelComparisonType switch
		{
			RemodelComparisonType.Any => ships.Any(AnyRemodelCheck),
			RemodelComparisonType.AtLeast => ships.Any(HigherRemodelCheck),
			RemodelComparisonType.Exact => ships.Any(s => s.MasterShip.ShipId == Model.Id),

			_ => false,
		};
	}

	private bool AnyRemodelCheck(IShipData ship)
	{
		IShipDataMaster conditionShip = KCDatabase.Instance.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		return ship.MasterShip.BaseShip().ShipId == conditionShip.BaseShip().ShipId;
	}

	private bool HigherRemodelCheck(IShipData ship)
	{
		IShipDataMaster conditionShip = KCDatabase.Instance.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		IShipDataMaster? masterShip = ship.MasterShip;

		while (masterShip is not null)
		{
			if (masterShip.ShipId == conditionShip.ShipId)
			{
				return true;
			}

			masterShip = masterShip.RemodelBeforeShip;
		}

		return false;
	}
}
