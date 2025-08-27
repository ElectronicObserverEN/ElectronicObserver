using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

[Obsolete("Use ShipConditionViewModelV2")]
public partial class ShipConditionViewModel : ObservableObject, IConditionViewModel
{
	private IKCDatabase Db { get; }
	private TransliterationService TransliterationService { get; }
	private ImageLoadService ImageLoadService { get; }

	private ShipSelectorViewModel? ShipSelectorViewModel { get; set; }

	[field: AllowNull, MaybeNull]
	public IShipDataMaster Ship
	{
		// bug: Ships don't get loaded till Kancolle loads
		get => field ??= Ships.FirstOrDefault(s => s.ShipId == Model.Id)
			?? throw new Exception("fix me: accessing this property before Kancolle gets loaded is a bug");
		set => SetProperty(ref field, value);
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
		Db = Ioc.Default.GetRequiredService<IKCDatabase>();
		TransliterationService = Ioc.Default.GetRequiredService<TransliterationService>();
		ImageLoadService = Ioc.Default.GetRequiredService<ImageLoadService>();

		RemodelComparisonTypes = Enum.GetValues<RemodelComparisonType>();

		Model = model;

		RemodelComparisonType = Model.RemodelComparisonType;
		MustBeFlagship = Model.MustBeFlagship;

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
			if (args.PropertyName is not nameof(MustBeFlagship)) return;

			Model.MustBeFlagship = MustBeFlagship;
		};
	}

	[RelayCommand]
	private void OpenShipPicker()
	{
		if (ShipSelectorViewModel is null)
		{
			List<IShipData> ships = Db.MasterShips.Values
				.Select(s => new ShipDataMock(s))
				.OfType<IShipData>()
				.ToList();

			ShipSelectorViewModel = new(TransliterationService, ImageLoadService, ships)
			{
				ShipFilter = { FinalRemodel = false, },
			};
		}

		ShipSelectorViewModel.ShowDialog();

		if (ShipSelectorViewModel.SelectedShip is null) return;

		Ship = ShipSelectorViewModel.SelectedShip.MasterShip;
	}

	public bool ConditionMet(IFleetData fleet)
	{
		IEnumerable<IShipData> ships = fleet.MembersInstance
			.OfType<IShipData>();

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
		IShipDataMaster conditionShip = Db.MasterShips.Values
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
