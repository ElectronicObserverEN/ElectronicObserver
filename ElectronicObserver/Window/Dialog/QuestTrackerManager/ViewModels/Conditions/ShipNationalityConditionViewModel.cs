using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class ShipNationalityConditionViewModel : ObservableObject, IConditionViewModel
{
	public ShipNationalityConditionModel Model { get; set; }

	public ShipNationality SelectedNationality { get; set; } = ShipNationality.Japanese;
	public List<ShipNationality> AllNationalities { get; }

	public ComparisonType ComparisonType { get; set; }
	public List<ComparisonType> ComparisonTypes { get; }

	public bool MustBeFlagship { get; set; }

	public string Display => $"({ShipNationalityText}) {ComparisonTypeDisplay} {Model.Count}{FlagshipConditionDisplay}";

	private string ShipNationalityText => $"{NationalityText}{QuestTrackerManagerResources.Kanmusu}";

	private string NationalityText => Model.Nationalities switch
	{
		{ Count: 1 } => Model.Nationalities[0].Display(),
		_ => $"({string.Join($" {QuestTrackerManagerResources.Operator_Or} ", Model.Nationalities.Select(s => s.Display()))})"
	};

	private string ComparisonTypeDisplay => ComparisonType.Display();

	private string FlagshipConditionDisplay => MustBeFlagship switch
	{
		true => $"({QuestTrackerManagerResources.Flagship})",
		_ => ""
	};

	public ShipNationalityConditionViewModel(ShipNationalityConditionModel model)
	{
		Model = model;

		MustBeFlagship = Model.MustBeFlagship;
		ComparisonType = Model.ComparisonType;

		// skip unknown
		AllNationalities = Enum.GetValues<ShipNationality>().Skip(1).ToList();
		ComparisonTypes = Enum.GetValues<ComparisonType>().ToList();

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		Model.Nationalities.CollectionChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(MustBeFlagship)) return;

			Model.MustBeFlagship = MustBeFlagship;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ComparisonType)) return;

			Model.ComparisonType = ComparisonType;
		};
	}

	[RelayCommand]
	private void AddType()
	{
		Model.Nationalities.Add(SelectedNationality);
	}

	[RelayCommand]
	private void RemoveType(ShipNationality nationality)
	{
		Model.Nationalities.Remove(nationality);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		List<IShipData> ships = fleet.MembersInstance.Where(s => s is not null).ToList();

		bool flagshipCondition = !Model.MustBeFlagship ||
			Model.Nationalities.Contains(ships[0].MasterShip.Nationality());

		int shipCount = ships.Count(s => Model.Nationalities.Contains(s.MasterShip.Nationality()));

		bool countCondition = Compare(shipCount, Model.Count, Model.ComparisonType);

		return flagshipCondition && countCondition;
	}

	private static bool Compare(IComparable a, IComparable b, ComparisonType comparisonType) =>
		comparisonType switch
		{
			ComparisonType.Equal => a.CompareTo(b) == 0,
			ComparisonType.LessOrEqual => a.CompareTo(b) <= 0,

			_ => a.CompareTo(b) >= 0
		};
}
