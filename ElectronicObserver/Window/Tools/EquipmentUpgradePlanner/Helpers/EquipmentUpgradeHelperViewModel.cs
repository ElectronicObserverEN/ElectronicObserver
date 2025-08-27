using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeHelperViewModel : ObservableObject
{
	public IShipDataMaster Helper { get; set; }

	public bool PlayerHasAtleastOne => KCDatabase.Instance.Ships.Values.Where(ship => ship.MasterShip.ShipId == Helper.ShipId).Any();

	public EquipmentUpgradeHelperViewModel(int helperId)
	{
		KCDatabase db = KCDatabase.Instance;

		Helper = db.MasterShips[helperId];

		SubscribeToApis();
		Update();
	}

	private void Update()
	{
		OnPropertyChanged(nameof(PlayerHasAtleastOne));
	}

	private void UpdateOnResponseReceived(string apiname, dynamic data)
	{
		Update();
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived += UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived += UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKaisou_Remodeling.ResponseReceived += UpdateOnResponseReceived;
	}

	public void UnsubscribeFromApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived -= UpdateOnResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived -= UpdateOnResponseReceived;

		APIObserver.Instance.ApiReqKaisou_Remodeling.ResponseReceived -= UpdateOnResponseReceived;
	}
}
