using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

[Obsolete("Use ShipConditionViewModelV2")]
public partial class ShipConditionViewModel : ObservableObject, IConditionViewModel
{
	private IKCDatabase Db { get; }

	private ShipPickerViewModel ShipPickerViewModel { get; }

	private IShipDataMaster? _ship;

	public IShipDataMaster Ship
	{
		// bug: Ships don't get loaded till Kancolle loads
		get => _ship ??= Ships.FirstOrDefault(s => s.ShipId == Model.Id)
						 ?? throw new Exception("fix me: accessing this property before Kancolle gets loaded is a bug");
		set => SetProperty(ref _ship, value);
	}

	public IEnumerable<IShipDataMaster> Ships => Db.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.OrderBy(s => s.SortID);

	public RemodelComparisonType RemodelComparisonType { get; set; }
	public IEnumerable<RemodelComparisonType> RemodelComparisonTypes { get; }

	public bool MustBeFlagship { get; set; }

	public ShipConditionModel Model { get; }

	public string Display => Ship switch
	{
		null => "",
		_ => $"{Ship.NameEN}({RemodelComparisonType.Display()}){FlagshipConditionDisplay}"
	};

	private string FlagshipConditionDisplay => MustBeFlagship switch
	{
		true => $"({QuestTrackerManagerResources.Flagship})",
		_ => ""
	};

	public ShipConditionViewModel(ShipConditionModel model)
	{
		Db = Ioc.Default.GetService<IKCDatabase>()!;
		ShipPickerViewModel = Ioc.Default.GetService<ShipPickerViewModel>()!;

		RemodelComparisonTypes = Enum.GetValues(typeof(RemodelComparisonType))
			.Cast<RemodelComparisonType>();

		Model = model;

		RemodelComparisonType = Model.RemodelComparisonType;
		MustBeFlagship = Model.MustBeFlagship;

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
			if (args.PropertyName is not nameof(MustBeFlagship)) return;

			Model.MustBeFlagship = MustBeFlagship;
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
		IEnumerable<IShipData> ships = fleet.MembersInstance.Where(s => s is not null);

		if (MustBeFlagship)
		{
			ships = ships.Take(1);
		}

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
		IShipDataMaster conditionShip = Db.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		return ship.MasterShip.BaseShip().ShipId == conditionShip.BaseShip().ShipId;
	}

	private bool HigherRemodelCheck(IShipData ship)
	{
		IShipDataMaster? conditionShip = Db.MasterShips.Values
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
