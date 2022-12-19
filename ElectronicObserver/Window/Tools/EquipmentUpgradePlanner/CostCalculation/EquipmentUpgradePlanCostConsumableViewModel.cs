using ElectronicObserver.Data;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
public class EquipmentUpgradePlanCostConsumableViewModel : EquipmentUpgradePlanCostItemViewModel
{
	public UseItemMaster Consumable { get; set; }

	public EquipmentUpgradePlanCostConsumableViewModel(EquipmentUpgradePlanCostItemModel model) : base(model)
	{
		Consumable = KCDatabase.Instance.MasterUseItems[model.Id];

		SubscribeToApis();
		Update();
	}

	public void Update()
	{
		KCDatabase db = KCDatabase.Instance;

		UseItem? item = db.UseItems[Consumable.ID];

		Owned = item?.Count ?? 0;
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += (_,_) => Update();
	}
}
