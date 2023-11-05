using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Services;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeHelpersViewModel : ObservableObject
{
	public static DayOfWeek[] DaysOfWeek { get; } = new DayOfWeek[]
	{
		DayOfWeek.Monday,
		DayOfWeek.Tuesday,
		DayOfWeek.Wednesday,
		DayOfWeek.Thursday,
		DayOfWeek.Friday,
		DayOfWeek.Saturday,
		DayOfWeek.Sunday
	};

	public List<EquipmentUpgradeHelperViewModel> Helpers { get; set; } = new();

	public List<EquipmentUpgradeHelpersDayViewModel> Days { get; set; } = new();

	private TimeChangeService TimeService { get; } = Ioc.Default.GetService<TimeChangeService>()!;

	public bool CanHelpToday => Days.First(day => day.DayValue == TimeService.CurrentDayOfWeekJST).IsHelperDay;

	public EquipmentUpgradeHelpersViewModel(EquipmentUpgradeHelpersModel model)
	{
		Helpers = model.ShipIds.Select(ship => new EquipmentUpgradeHelperViewModel(ship)).ToList();

		Days = DaysOfWeek.Select(day => new EquipmentUpgradeHelpersDayViewModel(day, model.CanHelpOnDays.Contains(day))).ToList();

		TimeService.DayChanged += () => OnPropertyChanged(nameof(CanHelpToday));
	}

	public void UnsubscribeFromApis()
	{
		Helpers.ForEach(viewModel => viewModel.UnsubscribeFromApis());
	}
}
