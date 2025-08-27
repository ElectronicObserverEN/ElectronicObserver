using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeDaysViewModel
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

	public List<EquipmentUpgradeDayViewModel> Days { get; set; } = new();

	public EquipmentUpgradeDaysViewModel(List<EquipmentUpgradeHelpersModel> models)
	{
		KCDatabase db = KCDatabase.Instance;

		Days = DaysOfWeek
			.Select(day => new EquipmentUpgradeDayViewModel(day, models.Where(helpers => helpers.CanHelpOnDays.Contains(day)).SelectMany(helpers => helpers.ShipIds).ToList()))
			.ToList();
	}

	public void UnsubscribeFromApis()
	{
		Days.ForEach(day => day.UnsubscribeFromApis());
	}
}
