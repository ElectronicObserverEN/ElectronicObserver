using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
public class EquipmentUpgradePlanCostEquipmentViewModel : EquipmentUpgradePlanCostItemViewModel
{
	public IEquipmentDataMaster Equipment { get; set; }

	public EquipmentUpgradePlanCostEquipmentViewModel(EquipmentUpgradePlanCostItemModel model) : base(model)
	{
		KCDatabase db = KCDatabase.Instance;

		Owned = db.Equipments.Where(eq => eq.Value?.EquipmentID == model.Id).Count();

		Equipment = KCDatabase.Instance.MasterEquipments[model.Id];
	}
}
