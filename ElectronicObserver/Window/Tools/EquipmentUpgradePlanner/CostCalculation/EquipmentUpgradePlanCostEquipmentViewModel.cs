using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
public sealed class EquipmentUpgradePlanCostEquipmentViewModel : EquipmentUpgradePlanCostItemViewModel
{
	public IEquipmentDataMaster Equipment { get; set; }

	public EquipmentUpgradePlanCostEquipmentViewModel(EquipmentUpgradePlanCostItemModel model, bool shouldUpdate) : base(model, shouldUpdate)
	{
		Equipment = KCDatabase.Instance.MasterEquipments[model.Id];
		Update();
	}

	protected override void Update()
	{
		KCDatabase db = KCDatabase.Instance;
		Owned = db.Equipments.Count(eq => eq.Value?.EquipmentID == Equipment.EquipmentID && eq.Value.UpgradeLevel == UpgradeLevel.Zero);
	}

	public override void SubscribeToApis()
	{
		base.SubscribeToApis();

		APIObserver.Instance.ApiPort_Port.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived += OnApiResponseReceived;
	}

	public override void UnsubscribeFromApis()
	{
		base.UnsubscribeFromApis();

		APIObserver.Instance.ApiPort_Port.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived -= OnApiResponseReceived;
	}
}
