using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradeFilterViewModel : ObservableObject
{

	public bool DisplayFinished { get; set; } = true;

	public List<EquipmentUpgradeFilterDayViewModel> Days { get; set; } = new();

	public DayOfWeek? SelectedDay { get; set; }
	public bool SelectAllDay { get; set; }

	public bool SelectToday { get; set; }

	private TimeChangeService TimeService { get; } = Ioc.Default.GetService<TimeChangeService>()!;

	public EquipmentUpgradePlannerTranslationViewModel Translations { get; set; } = new();

	public string TodayDisplay => string.Format(Translations.Today, CultureInfo.CurrentCulture.Name switch
	{
		"ja-JP" => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(TimeService.CurrentDayOfWeekJST)[..1],
		_ => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(TimeService.CurrentDayOfWeekJST)[..3],
	});


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
					SelectToday = false;
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

			SelectAllDay = SelectedDay is null && !SelectToday;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectAllDay)) return;

			if (SelectAllDay) SelectToday = false;
			else if (SelectedDay is null && !SelectToday) SelectAllDay = true;

			if (SelectAllDay) SelectedDay = null;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectToday)) return;

			if (SelectToday)
			{
				SelectAllDay = false;
				SelectedDay = null;
			}

			SelectAllDay = SelectedDay is null && !SelectToday;
		};

		SelectToday = true;

		TimeService.DayChanged += () => OnPropertyChanged("");

		Configuration.Instance.ConfigurationChanged += () => OnPropertyChanged(nameof(TodayDisplay));
	}

	public bool MeetsFilterCondition(EquipmentUpgradePlanItemViewModel plan)
	{
		if (!DisplayFinished && plan.Finished) return false;

		DayOfWeek? dayFilter = SelectToday switch
		{
			true => TimeService.CurrentDayOfWeekJST,
			_ => SelectedDay
		};

		if (dayFilter is DayOfWeek selectedDay)
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
