using System.Linq;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public class EquipmentUpgradePlanCostConsumableViewModel : EquipmentUpgradePlanCostItemViewModel
{
	public UseItemMaster Consumable { get; set; }

	public EquipmentUpgradePlanCostConsumableViewModel(EquipmentUpgradePlanCostItemModel model) : base(model)
	{
		KCDatabase db = KCDatabase.Instance;

		UseItem? item = db.UseItems[model.Id];

		Owned = item?.Count ?? 0;

		Consumable = KCDatabase.Instance.MasterUseItems[model.Id];
	}
}
