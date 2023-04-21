using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou;

public class marriage : APIBase
{
	public override void OnResponseReceived(dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;
		int id = (int)data.api_id;
		ShipData? ship = db.Ships[id];

		// should never be null but...
		int luckBeforeMarriage = ship?.LuckTotal ?? 0;

		if (ship != null)
		{
			ship.LoadFromResponse(APIName, data);
		}
		else
		{
			// todo: how would this ever happen?
			ShipData a = new();
			a.LoadFromResponse(APIName, data);
			db.Ships.Add(a);
			ship = db.Ships[id];
		}

		int luckAfterMarriage = ship.LuckTotal;

		Utility.Logger.Add(2, string.Format
		(
			LoggerRes.JustMarried,
			ship.Name,
			luckAfterMarriage - luckBeforeMarriage,
			ship.LuckBase, 
			ship.MasterShip.LuckMax
		));

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_kaisou/marriage";
}
