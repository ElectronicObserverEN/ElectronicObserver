using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

/// <summary>
/// Link between two upgrade plan
/// </summary>
public class EquipmentConversionPlanItemViewModel(EquipmentUpgradePlanItemViewModel parent, List<EquipmentUpgradePlanItemViewModel> children) : IEquipmentPlanItemViewModel
{
	public EquipmentId EquipmentMasterDataId => EquipmentToUpgradePlan.EquipmentMasterDataId;

	public EquipmentUpgradePlanCostViewModel Cost => new(new());
	public List<EquipmentUpgradePlanItemViewModel> EquipmentRequiredForUpgradePlan { get; set; } = children;
	public EquipmentUpgradePlanItemViewModel EquipmentToUpgradePlan { get; set; } = parent;
}
