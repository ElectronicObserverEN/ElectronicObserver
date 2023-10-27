using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Utility.Data;
public static class EquipmentUpgradeExtensions
{
	public static List<EquipmentUpgradeDataModel> CanUpgradeEquipments(this IShipDataMaster ship, DayOfWeek day, List<EquipmentUpgradeDataModel> upgradesData)
		=> upgradesData.Where(upgrade => upgrade.Improvement
				.Any(improvement => improvement.Helpers
					.Any(helpers => helpers.ShipIds.Contains(ship.ShipID) && helpers.CanHelpOnDays.Contains(day))
				)
			)
			.ToList();
}
