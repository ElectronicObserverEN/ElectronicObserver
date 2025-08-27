using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_air_corps;

public class supply : APIBase
{

	private int _aircorpsID;


	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		_aircorpsID = BaseAirCorpsData.GetID(data);

		base.OnRequestReceived(data);
	}


	public override void OnResponseReceived(dynamic data)
	{

		var corps = KCDatabase.Instance.BaseAirCorps;

		if (corps.ContainsKey(_aircorpsID))
			corps[_aircorpsID].LoadFromResponse(APIName, data);


		int fuel = KCDatabase.Instance.Material.Fuel;
		int baux = KCDatabase.Instance.Material.Bauxite;

		KCDatabase.Instance.Material.LoadFromResponse(APIName, data);

		fuel -= KCDatabase.Instance.Material.Fuel;
		baux -= KCDatabase.Instance.Material.Bauxite;

		if (corps.ContainsKey(_aircorpsID))
		{
			Utility.Logger.Add(2, string.Format(NotifierRes.AirBaseResupplyCost,
				corps[_aircorpsID].MapAreaID, corps[_aircorpsID].Name, fuel, baux));
		}

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_air_corps/supply";
}
