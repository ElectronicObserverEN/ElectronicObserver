using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeHelpersViewModel
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

	public List<IShipDataMaster> Helpers { get; set; } = new();

	public List<EquipmentUpgradeHelpersDayViewModel> Days { get; set; } = new();

	public EquipmentUpgradeHelpersViewModel(EquipmentUpgradeHelpersModel model)
	{
		KCDatabase db = KCDatabase.Instance;

		Helpers = model.ShipIds.Select(ship => db.MasterShips[ship]).ToList();

		Days = DaysOfWeek.Select(day => new EquipmentUpgradeHelpersDayViewModel(day, model.CanHelpOnDays.Contains(day))).ToList();
	}
}
