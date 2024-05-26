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
		Owned = db.Equipments.Count(eq => eq.Value?.EquipmentID == Equipment.EquipmentID && eq.Value.UpgradeLevel == UpgradeLevel.Zero);
	}

	private void UpdateOnResponseReceived(string apiname, dynamic data)
	{
		Update();
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiGetMember_SlotItem.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived += UpdateOnResponseReceived;
	}
	
	public void UnsubscribeFromApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiGetMember_SlotItem.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived -= UpdateOnResponseReceived;
	}
}
