﻿using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;

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

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += (_, _) => Update();
	}
}
