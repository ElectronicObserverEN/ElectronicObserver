using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class EquipmentUpgradeItemCostViewModel
{
	public EquipmentUpgradePlannerTranslationViewModel EquipmentUpgradePlanner { get; }

	public List<EquipmentUpgradePlanCostEquipmentViewModel> RequiredEquipments { get; set; } = new();
	public List<EquipmentUpgradePlanCostConsumableViewModel> RequiredConsumables { get; set; } = new();

	public EquipmentUpgradeItemCostViewModel(EquipmentUpgradeImprovementCostDetail model)
	{
		EquipmentUpgradePlanner = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		
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
}
