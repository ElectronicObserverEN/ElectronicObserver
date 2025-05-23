using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
public class EquipmentUpgradePlanCostMaterialViewModel : EquipmentUpgradePlanResourceDisplayViewModel
{
	public UseItemId Type { get; set; }

	public EquipmentUpgradePlanCostMaterialViewModel(int required, UseItemId type)
	{
		Required = required;
		Type = type;

		SubscribeToApis();
		Update();
	}

	public void Update()
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

	private void UpdateOnResponseReceived(string apiname, dynamic data)
	{
		Update();
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiGetMember_Material.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqHokyu_Charge.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_CreateItem.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqAirCorps_Supply.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqAirCorps_SetPlane.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived += UpdateOnResponseReceived;
	}

	public void UnsubscribeFromApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiGetMember_Material.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqHokyu_Charge.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_DestroyItem2.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_CreateItem.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_RemodelSlot.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqAirCorps_Supply.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqAirCorps_SetPlane.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqMember_ItemUse.ResponseReceived -= UpdateOnResponseReceived;
	}
}
