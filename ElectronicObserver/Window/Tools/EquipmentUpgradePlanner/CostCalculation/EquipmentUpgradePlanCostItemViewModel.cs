namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

public abstract class EquipmentUpgradePlanCostItemViewModel : EquipmentUpgradePlanResourceDisplayViewModel
{
	protected EquipmentUpgradePlanCostItemViewModel(EquipmentUpgradePlanCostItemModel model, bool shouldUpdate) : base(shouldUpdate)
	{
		Required = model.Required;
	}
}
