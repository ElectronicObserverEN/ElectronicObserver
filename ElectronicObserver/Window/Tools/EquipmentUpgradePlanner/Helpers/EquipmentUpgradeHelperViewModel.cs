using System.Linq;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public sealed class EquipmentUpgradeHelperViewModel : CanBeUpdatedByApiViewModel
{
	public IShipDataMaster Helper { get; set; }

	public bool PlayerHasAtleastOne => KCDatabase.Instance.Ships.Values.Where(ship => ship.MasterShip.ShipId == Helper.ShipId).Any();

	public EquipmentUpgradeHelperViewModel(int helperId, bool shouldUpdate) : base(shouldUpdate)
	{
		KCDatabase db = KCDatabase.Instance;

		Helper = db.MasterShips[helperId];

		Update();
	}

	protected override void Update()
	{
		OnPropertyChanged(nameof(PlayerHasAtleastOne));
	}

	public override void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived += OnApiResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived += OnApiResponseReceived;

		APIObserver.Instance.ApiReqKaisou_Remodeling.ResponseReceived += OnApiResponseReceived;
	}

	public override void UnsubscribeFromApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqKousyou_DestroyShip.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKousyou_GetShip.ResponseReceived -= OnApiResponseReceived;
		APIObserver.Instance.ApiReqKaisou_PowerUp.ResponseReceived -= OnApiResponseReceived;

		APIObserver.Instance.ApiReqKaisou_Remodeling.ResponseReceived -= OnApiResponseReceived;
	}
}
