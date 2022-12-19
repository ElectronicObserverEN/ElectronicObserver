﻿using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.ResourceDisplay;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

public abstract class EquipmentUpgradePlanCostItemViewModel : EquipmentUpgradePlanResourceDisplayViewModel
{
	protected EquipmentUpgradePlanCostItemViewModel(EquipmentUpgradePlanCostItemModel model)
	{
		Required = model.Required;
	}
}
