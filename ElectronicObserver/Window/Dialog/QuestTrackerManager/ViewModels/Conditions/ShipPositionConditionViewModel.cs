using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class ShipPositionConditionViewModel : ObservableObject, IConditionViewModel
{
	private ShipPickerViewModel ShipPickerViewModel { get; }

	private IShipDataMaster? _ship;

	public IShipDataMaster Ship
	{
		// bug: Ships don't get loaded till Kancolle loads
		get => _ship ??= Ships.FirstOrDefault(s => s.ShipId == Model.Id)
						 ?? throw new Exception("fix me: accessing this property before Kancolle gets loaded is a bug");
		set => SetProperty(ref _ship, value);
	}

	public IEnumerable<IShipDataMaster> Ships => KCDatabase.Instance.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.OrderBy(s => s.SortID);

	public RemodelComparisonType RemodelComparisonType { get; set; }
	public IEnumerable<RemodelComparisonType> RemodelComparisonTypes { get; }

	public int Position { get; set; }

	public ShipPositionConditionModel Model { get; }

	public string Display => Ship switch
	{
		null => "",
		_ => $"{Ship.NameEN}({RemodelComparisonType.Display()})({PositionText})"
	};

	private string PositionText => $"{QuestTrackerManagerResources.Position}：{Position}";

	public ShipPositionConditionViewModel(ShipPositionConditionModel model)
	{
		ShipPickerViewModel = Ioc.Default.GetService<ShipPickerViewModel>()!;

		RemodelComparisonTypes = Enum.GetValues<RemodelComparisonType>();

		Model = model;

		RemodelComparisonType = Model.RemodelComparisonType;
		Position = Model.Position;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Ship)) return;

			Model.Id = Ship?.ShipId ?? ShipId.Kamikaze;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(RemodelComparisonType)) return;

			Model.RemodelComparisonType = RemodelComparisonType;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Position)) return;

			Model.Position = Position;
		};
	}

	[RelayCommand]
	private void OpenShipPicker()
	{
		ShipPickerView shipPicker = new(ShipPickerViewModel);
		if (shipPicker.ShowDialog(App.Current.MainWindow) is true)
		{
			Ship = shipPicker.PickedShip!;
		}
	}

	public bool ConditionMet(IFleetData fleet)
	{
		List<IShipData> ships = fleet.MembersInstance.Where(s => s is not null).ToList();
		int index = Position - 1;

		if (ships.Count < index) return false;
		
		ships = ships.Skip(index).Take(1).ToList();

		return RemodelComparisonType switch
		{
			RemodelComparisonType.Any => ships.Any(AnyRemodelCheck),
			RemodelComparisonType.AtLeast => ships.Any(HigherRemodelCheck),
			RemodelComparisonType.Exact => ships.Any(s => s.MasterShip.ShipId == Model.Id),

			_ => false
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
		IShipDataMaster? conditionShip = KCDatabase.Instance.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		while (conditionShip != null)
		{
			if (ship.MasterShip.ShipId == conditionShip.ShipId)
			{
				return true;
			}

			conditionShip = conditionShip.RemodelBeforeShip;
		}

		return false;
	}
}
