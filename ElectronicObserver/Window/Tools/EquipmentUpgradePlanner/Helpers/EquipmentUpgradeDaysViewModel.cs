using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Common;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeDaysViewModel : CanBeUpdatedByApiViewModel
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

	public EquipmentUpgradeDaysViewModel(List<EquipmentUpgradeHelpersModel> models, bool shouldUpdate) : base(shouldUpdate)
	{
		Days = DaysOfWeek
			.Select(day => new EquipmentUpgradeDayViewModel(day, models.Where(helpers => helpers.CanHelpOnDays.Contains(day)).SelectMany(helpers => helpers.ShipIds).ToList(), shouldUpdate))
			.ToList();
	}
}
