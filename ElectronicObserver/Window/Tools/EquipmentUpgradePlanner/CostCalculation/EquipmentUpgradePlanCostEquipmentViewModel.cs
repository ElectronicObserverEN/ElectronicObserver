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
		// Post sortie update (sunk ships, ...)
		APIObserver.Instance.ApiPort_Port.ResponseReceived += (_, _) => Update();

		// Lost equipments on ship scrap / modernisation
		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived += (_, _) => Update();
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived += (_, _) => Update();

		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived += (_, _) => Update();
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived += (_, _) => Update();
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived += (_, _) => Update();

		// Item is used => eg. xmas box can give new gear
		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived += (_, _) => Update();

		// After quest clear => equipments converted/aquired
		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived += (_, _) => Update();
	}
}
