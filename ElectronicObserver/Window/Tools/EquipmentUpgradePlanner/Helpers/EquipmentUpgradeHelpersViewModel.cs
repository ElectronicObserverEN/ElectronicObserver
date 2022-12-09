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
	/// <summary>
	/// Should this be here ?
	/// </summary>
	public static List<DayOfWeek> DaysSortedDependingOnCulture =>
		Enum.GetValues<DayOfWeek>()
		.OrderBy(day => day < CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek ? day + 7 : day)
		.ToList();

	public List<IShipDataMaster> Helpers { get; set; } = new();

	public List<EquipmentUpgradeHelpersDayViewModel> Days { get; set; } = new();

	public EquipmentUpgradeHelpersViewModel(EquipmentUpgradeHelpersModel model)
	{
		KCDatabase db = KCDatabase.Instance;

		Helpers = model.ShipIds.Select(ship => db.MasterShips[ship]).ToList();

		Days = DaysSortedDependingOnCulture.Select(day => new EquipmentUpgradeHelpersDayViewModel(day, model.CanHelpOnDays.Contains(day))).ToList();
	}
}
