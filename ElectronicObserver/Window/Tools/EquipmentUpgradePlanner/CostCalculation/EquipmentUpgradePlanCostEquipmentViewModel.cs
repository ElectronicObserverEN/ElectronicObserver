using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
public class EquipmentUpgradePlanCostEquipmentViewModel : EquipmentUpgradePlanCostItemViewModel
{
	public IEquipmentDataMaster Equipment { get; set; }

	public EquipmentUpgradePlanCostEquipmentViewModel(EquipmentUpgradePlanCostItemModel model) : base(model)
	{
		Equipment = KCDatabase.Instance.MasterEquipments[model.Id];

		SubscribeToApis();
		Update();
	}

	public void Update()
	{
		KCDatabase db = KCDatabase.Instance;
		Owned = db.Equipments.Where(eq => eq.Value?.EquipmentID == Equipment.EquipmentID).Count();
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += (_, _) => Update();
	}
}
