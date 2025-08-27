using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

/// <summary>
/// Link between two upgrade plan
/// </summary>
public class EquipmentConversionPlanItemViewModel(EquipmentUpgradePlanItemViewModel parent, List<EquipmentUpgradePlanItemViewModel> children) : IEquipmentPlanItemViewModel
{
	public EquipmentId EquipmentMasterDataId => EquipmentRequiredForUpgradePlan.FirstOrDefault() switch
	{
		{ ShouldBeConvertedInto: not null } plan => (EquipmentId)plan.ShouldBeConvertedInto,
		_ => EquipmentId.Unknown,
	};

	public EquipmentUpgradePlanCostViewModel Cost => new(new());

	public List<EquipmentUpgradePlanItemViewModel> EquipmentRequiredForUpgradePlan { get; set; } = children;
	public EquipmentUpgradePlanItemViewModel EquipmentToUpgradePlan { get; set; } = parent;

	public List<IEquipmentPlanItemViewModel> GetPlanChildren() => EquipmentRequiredForUpgradePlan
		.Cast<IEquipmentPlanItemViewModel>()
		.ToList();

	public void UnsubscribeFromApis()
	{
		Cost.UnsubscribeFromApis();
	}
}
