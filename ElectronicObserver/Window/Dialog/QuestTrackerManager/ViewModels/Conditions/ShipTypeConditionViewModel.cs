using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class ShipTypeConditionViewModel : ObservableObject, IConditionViewModel
{
	public ShipTypeConditionModel Model { get; set; }

	public ShipTypes SelectedType { get; set; } = ShipTypes.Destroyer;
	public IEnumerable<ShipTypes> AllTypes { get; }

	public ComparisonType ComparisonType { get; set; }
	public IEnumerable<ComparisonType> ComparisonTypes { get; }

	public bool MustBeFlagship { get; set; }

	public string Display => $"({CountConditionDisplay}) {ComparisonTypeDisplay} {Model.Count}{FlagshipConditionDisplay}";

	private string CountConditionDisplay => string.Join($" {QuestTrackerManagerResources.Operator_Or} ",
		Model.Types.Select(s => s.Display()));

	private string ComparisonTypeDisplay => ComparisonType.Display();

	private string FlagshipConditionDisplay => MustBeFlagship switch
	{
		true => $"({QuestTrackerManagerResources.Flagship})",
		_ => ""
	};

	public ShipTypeConditionViewModel(ShipTypeConditionModel model)
	{
		Model = model;

		MustBeFlagship = Model.MustBeFlagship;
		ComparisonType = Model.ComparisonType;

		AllTypes = Enum.GetValues<ShipTypes>().Where(t => t is not ShipTypes.Unknown);
		ComparisonTypes = Enum.GetValues<ComparisonType>();

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		Model.Types.CollectionChanged += (sender, args) =>
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
		Model.Types.Add(SelectedType);
	}

	[RelayCommand]
	private void RemoveType(ShipTypes s)
	{
		Model.Types.Remove(s);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		List<IShipData> ships = fleet.MembersInstance.Where(s => s is not null).ToList();

		bool flagshipCondition = !Model.MustBeFlagship ||
			Model.Types.Contains(ships[0].MasterShip.ShipType);

		int shipCount = ships.Count(s => Model.Types.Contains(s.MasterShip.ShipType));

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
