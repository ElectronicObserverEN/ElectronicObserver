using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
public sealed class EquipmentUpgradePlanCostMaterialViewModel : EquipmentUpgradePlanResourceDisplayViewModel
{
	public UseItemId Type { get; set; }

	public EquipmentUpgradePlanCostMaterialViewModel(int required, UseItemId type, bool shouldUpdate) : base(shouldUpdate)
	{
		Required = required;
		Type = type;

		Update();
	}

	protected override void Update()
	{
		MaterialData db = KCDatabase.Instance.Material;

		Owned = Type switch
		{
			UseItemId.Fuel => db.Fuel,
			UseItemId.Ammo => db.Ammo,
			UseItemId.Steel => db.Steel,
			UseItemId.Bauxite => db.Bauxite,

			UseItemId.DevelopmentMaterial => db.DevelopmentMaterial,
			UseItemId.ImproveMaterial => db.ModdingMaterial,

			_ => 0
		};
	}

	public override void SubscribeToApis()
	{
		base.SubscribeToApis();

		APIObserver.Instance.ApiPort_Port.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiGetMember_Material.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqHokyu_Charge.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_CreateItem.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqAirCorps_Supply.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqAirCorps_SetPlane.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived += OnApiResponseReceived;

	}

	public override void UnsubscribeFromApis()
	{
		base.UnsubscribeFromApis();

		APIObserver.Instance.ApiPort_Port.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiGetMember_Material.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqHokyu_Charge.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_CreateItem.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqAirCorps_Supply.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqAirCorps_SetPlane.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived -= OnApiResponseReceived;

	}
}
