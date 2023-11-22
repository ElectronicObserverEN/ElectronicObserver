using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes.Serialization.FitBonus;

namespace ElectronicObserver.Observer.kcsapi.api_get_member;

public class ship3 : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;

		//api_ship_data
		foreach (var elem in data.api_ship_data)
		{
			int id = (int)elem.api_id;
			bool isRemodeled = false;

			ShipData ship = db.Ships[id];
			ship.LoadFromResponse(APIName, elem);

			for (int i = 0; i < ship.Slot.Count; i++)
			{
				if (ship.Slot[i] == -1) continue;
				if (!db.Equipments.ContainsKey(ship.Slot[i]))
				{
					//改装時に新装備を入手するが、追加される前にIDを与えられてしまうため
					EquipmentData eq = new EquipmentData();
					eq.LoadFromResponse(APIName, ship.Slot[i]);
					db.Equipments.Add(eq);
					isRemodeled = true;
				}
			}

			// 装備シナジー検出カッコカリ
			if (!isRemodeled)
			{
				FitBonusValue bonus = ship.GetFitBonus();

				if (bonus.HasBonus())
				{
					var sb = new StringBuilder();
					sb.Append(ObserverRes.DetectedSynergy);

					var a = new List<string>();

					if (bonus.Firepower != 0)
						a.Add(ObserverRes.Firepower + $"{bonus.Firepower:+#;-#;0}");

					if (bonus.Torpedo != 0)
						a.Add(ObserverRes.Torpedo + $"{bonus.Torpedo:+#;-#;0}");

					if (bonus.AntiAir != 0)
						a.Add(ObserverRes.AntiAir + $"{bonus.AntiAir:+#;-#;0}");

					if (bonus.Armor != 0)
						a.Add(ObserverRes.Armor + $"{bonus.Armor:+#;-#;0}");

					if (bonus.ASW != 0)
						a.Add(ObserverRes.Asw + $"{bonus.ASW:+#;-#;0}");

					if (bonus.Evasion != 0)
						a.Add(ObserverRes.Evasion + $"{bonus.Evasion:+#;-#;0}");

					if (bonus.LOS != 0)
						a.Add(ObserverRes.Los + $"{bonus.LOS:+#;-#;0}");

					if (bonus.Accuracy != 0)
						a.Add(ObserverRes.Accuracy + $"{bonus.Accuracy:+#;-#;0}");

					if (bonus.Range != 0)
						a.Add(ObserverRes.Range + $"{bonus.Range:+#;-#;0}");

					sb.Append(string.Join(", ", a));

					sb.AppendFormat(" ; {0} [{1}]",
						ship.NameWithLevel,
						string.Join(", ", ship.AllSlotInstance.Where(eq => eq != null).Select(eq => eq.NameWithLevel)));

					Utility.Logger.Add(2, sb.ToString());
				}
			}
		}

		//api_deck_data
		db.Fleet.LoadFromResponse(APIName, data.api_deck_data);



		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_get_member/ship3";
}
