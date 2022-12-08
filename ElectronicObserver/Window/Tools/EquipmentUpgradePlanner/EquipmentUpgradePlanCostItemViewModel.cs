namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public abstract class EquipmentUpgradePlanCostItemViewModel
{
	public int Required { get; set; }

	public int Owned { get; set; }

	protected EquipmentUpgradePlanCostItemViewModel(EquipmentUpgradePlanCostItemModel model)
	{
		Required = model.Required;
	}
}
