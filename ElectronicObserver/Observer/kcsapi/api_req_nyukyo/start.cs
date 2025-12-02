using System;
using System.Collections.Generic;
using ElectronicObserver.Core;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Observer.kcsapi.api_req_nyukyo;

public class start : APIBase
{

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase db = KCDatabase.Instance;

		DockData dock = db.Docks[int.Parse(data["api_ndock_id"])];
		bool bucketUsed = data["api_highspeed"] == "1";

		int shipID = int.Parse(data["api_ship_id"]);
		ShipData ship = db.Ships[shipID];


		Utility.Logger.Add(2, string.Format(NotifierRes.StartedRepairingShip,
			dock.DockID, ship.NameWithLevel,
			ship.HPCurrent, ship.HPMax,
			ship.RepairFuel, ship.RepairSteel,
			bucketUsed ? NotifierRes.BucketUsed : (NotifierRes.ETA + DateTimeHelper.TimeToCSVString(DateTime.Now + TimeSpan.FromMilliseconds(ship.RepairTime)))
		));


		db.Material.Fuel -= ship.RepairFuel;
		db.Material.Steel -= ship.RepairSteel;


		if (bucketUsed)
		{
			ship.Repair();
			db.Material.InstantRepair--;
		}
		else if (ship.RepairTime <= 60000)
		{
			ship.Repair();
		}
		else
		{
			//この場合は直後に ndock が呼ばれるので自力で更新しなくてもよい
			/*
			dock.State = 1;
			dock.ShipID = shipID;
			dock.CompletionTime = DateTime.Now.AddMilliseconds( ship.RepairTime );
			*/
		}


		db.Fleet.LoadFromRequest(APIName, data);

		base.OnRequestReceived(data);
	}


	public override bool IsRequestSupported => true;
	public override bool IsResponseSupported => false;


	public override string APIName => "api_req_nyukyo/start";
}
