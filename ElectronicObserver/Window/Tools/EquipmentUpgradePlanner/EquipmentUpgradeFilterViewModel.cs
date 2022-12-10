using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Common;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradeFilterViewModel : ObservableObject
{

	public bool DisplayFinished { get; set; } = true;

	public List<EquipmentUpgradeFilterDayViewModel> Days { get; set; } = new();

	public DayOfWeek? SelectedDay { get; set; }
	public bool SelectAllDay { get; set; }

	public EquipmentUpgradeFilterViewModel()
	{
		Days = EquipmentUpgradeHelpersViewModel.DaysOfWeek.Select(day => new EquipmentUpgradeFilterDayViewModel(day)).ToList();

		foreach (CheckBoxEnumViewModel day in Days)
		{
			day.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(day.IsChecked)) return;
				if (sender is not CheckBoxEnumViewModel { IsChecked: bool isChecked, Value: DayOfWeek dayValue }) return;

				if (isChecked)
				{
					SelectedDay = dayValue;
				}
				else if (SelectedDay == dayValue)
				{
					SelectedDay = null;
				}
			};
		}

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedDay)) return;

			foreach (CheckBoxEnumViewModel format in Days)
			{
				if (format.Value is not DayOfWeek dayValue) return;

				format.IsChecked = dayValue == SelectedDay;
			}

			SelectAllDay = SelectedDay is null;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectAllDay)) return;

			if (SelectAllDay) SelectedDay = null;
			else if (SelectedDay is null) SelectAllDay = true;
		};

		SelectedDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Today, "Tokyo Standard Time").DayOfWeek;
	}

	public bool MeetsFilterCondition(EquipmentUpgradePlanItemViewModel plan)
	{
		if (!DisplayFinished && plan.Finished) return false;

		if (SelectedDay is DayOfWeek selectedDay)
		{
			List<DayOfWeek> daysOfThePlan = plan.HelperViewModels
				.SelectMany(helpers => helpers.Days)
				.Where(helperDays => helperDays.IsHelperDay)
				.Select(helperDays => helperDays.DayValue)
				.Distinct()
				.ToList();

			return daysOfThePlan.Contains(selectedDay);
		}

		return true;
	}
}
