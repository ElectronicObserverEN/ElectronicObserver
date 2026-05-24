using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou;

public class remodel_slot : APIBase
{

	private int _equipmentID;

	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		_equipmentID = int.Parse(data["api_slot_id"]);

		base.OnRequestReceived(data);
	}

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;

		db.Material.LoadFromResponse(APIName, data.api_after_material);


		if (data.api_after_slot())
		{   //改修成功時のみ存在
			EquipmentData eq = db.Equipments[(int)data.api_after_slot.api_id];
			if (eq != null)
			{
				eq.LoadFromResponse(APIName, data.api_after_slot);

				if (Utility.Configuration.Config.Log.ShowSpoiler &&
					Utility.Configuration.Config.Log.ShowEquipmentImprovementSpoiler)
					Utility.Logger.Add(2, string.Format(LoggerRes.ImprovedEqSuccess, eq.NameWithLevel));
			}

		}
		else if (Utility.Configuration.Config.Log.ShowSpoiler &&
			Utility.Configuration.Config.Log.ShowEquipmentImprovementSpoiler)
		{
			Utility.Logger.Add(2, string.Format(LoggerRes.ImprovedFailure, db.Equipments[_equipmentID].NameWithLevel));
		}


		if (data.api_use_slot_id())
		{
			foreach (int id in data.api_use_slot_id)
			{
				db.Equipments.Remove(id);
			}
		}

		base.OnResponseReceived((object)data);
	}


	public override string APIName => "api_req_kousyou/remodel_slot";
}
