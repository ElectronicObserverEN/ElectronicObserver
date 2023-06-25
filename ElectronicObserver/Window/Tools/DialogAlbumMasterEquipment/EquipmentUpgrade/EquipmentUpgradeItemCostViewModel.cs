using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Common;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class EquipmentUpgradeItemCostViewModel : CanBeUpdatedByApiViewModel
{
	public List<EquipmentUpgradePlanCostEquipmentViewModel> RequiredEquipments { get; }
	public List<EquipmentUpgradePlanCostConsumableViewModel> RequiredConsumables { get; }

	public EquipmentUpgradeItemCostViewModel(EquipmentUpgradeImprovementCostDetail model, bool shouldUpdate) : base(shouldUpdate)
	{
		RequiredEquipments = model.EquipmentDetail.Select(item => new EquipmentUpgradePlanCostEquipmentViewModel(new()
		{
			Id = item.Id,
			Required = item.Count
		}, shouldUpdate)).ToList();

		RequiredConsumables = model.ConsumableDetail.Select(item => new EquipmentUpgradePlanCostConsumableViewModel(new()
		{
			Id = item.Id,
			Required = item.Count
		}, shouldUpdate)).ToList();
	}
}
