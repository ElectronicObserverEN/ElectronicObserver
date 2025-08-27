using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou;

public class destroyitem2 : APIBase
{


	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase db = KCDatabase.Instance;

		// 削除処理が終わってからだと装備データが取れないため
		db.QuestProgress.EquipmentDiscarded(APIName, data);
		db.QuestTrackerManagers.EquipmentDiscarded(APIName, data);
		db.SystemQuestTrackerManager.EquipmentDiscarded(APIName, data);

		Dictionary<string, int> itemsDestroyed = new Dictionary<string, int>();

		foreach (int id in data["api_slotitem_ids"].Split(",".ToCharArray()).Select(str => int.Parse(str)))
		{
			string name = KCDatabase.Instance.Equipments[id].NameWithLevel;
			itemsDestroyed.TryGetValue(name, out int amount);
			itemsDestroyed[name] = amount + 1;

			db.Equipments.Remove(id);
		}

		foreach (var item in itemsDestroyed)
		{
			Utility.Logger.Add(2, string.Format(NotifierRes.EquipmentHasBenScrapped, item.Key, item.Value > 1 ? " x " + item.Value : ""));
		}

		base.OnRequestReceived(data);
	}


	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Material.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}


	public override bool IsRequestSupported => true;
	public override bool IsResponseSupported => true;

	public override string APIName => "api_req_kousyou/destroyitem2";
}
