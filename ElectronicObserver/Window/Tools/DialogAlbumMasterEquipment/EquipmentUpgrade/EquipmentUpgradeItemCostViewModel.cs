using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class EquipmentUpgradeItemCostViewModel
{
	public List<EquipmentUpgradePlanCostEquipmentViewModel> RequiredEquipments { get; }
	public List<EquipmentUpgradePlanCostConsumableViewModel> RequiredConsumables { get; }

	public EquipmentUpgradeItemCostViewModel(EquipmentUpgradeImprovementCostDetail model)
	{
		RequiredEquipments = model.EquipmentDetail.Select(item => new EquipmentUpgradePlanCostEquipmentViewModel(new()
		{
			Id = item.Id,
			Required = item.Count
		})).ToList();

		RequiredConsumables = model.ConsumableDetail.Select(item => new EquipmentUpgradePlanCostConsumableViewModel(new()
		{
			Id = item.Id,
			Required = item.Count
		})).ToList();
	}
	
	public void UnsubscribeFromApis()
	{
		RequiredEquipments.ForEach(viewModel => viewModel.UnsubscribeFromApis());
		RequiredConsumables.ForEach(viewModel => viewModel.UnsubscribeFromApis());
	}
}
