using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou;

public class createitem : APIBase
{
	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase.Instance.Development.LoadFromRequest(APIName, data);

		base.OnRequestReceived(data);
	}

	public override void OnResponseReceived(dynamic data)
	{

		var db = KCDatabase.Instance;
		var dev = db.Development;

		dev.LoadFromResponse(APIName, data);


		//logging
		if (Utility.Configuration.Config.Log.ShowSpoiler &&
			Utility.Configuration.Config.Log.ShowDevelopmentSpoiler)
		{
			//Utility.Logger.Add(2, $"開発結果: {string.Join(", ", dev.Results)} ({dev.Fuel}/{dev.Ammo}/{dev.Steel}/{dev.Bauxite} 秘書艦: {db.Fleet[1].MembersInstance[0].NameWithLevel})");

			foreach (var result in dev.Results)
			{
				if (result.IsSucceeded)
				{
					Utility.Logger.Add(2, string.Format(LoggerRes.CreatedItem,
						result.MasterEquipment.CategoryTypeInstance.NameEN,
						result.MasterEquipment.NameEN,
						dev.Fuel, dev.Ammo, dev.Steel, dev.Bauxite,
						db.Fleet[1].MembersInstance[0].NameWithLevel));
				}
				else
				{
					Utility.Logger.Add(2, string.Format(LoggerRes.ItemCreationFailed,
						dev.Fuel, dev.Ammo, dev.Steel, dev.Bauxite,
						db.Fleet[1].MembersInstance[0].NameWithLevel));
				}
			}
		}

		base.OnResponseReceived((object)data);
	}

	public override bool IsRequestSupported => true;
	public override bool IsResponseSupported => true;

	public override string APIName => "api_req_kousyou/createitem";
}
