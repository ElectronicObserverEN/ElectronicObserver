﻿using System.Collections.Generic;
using System.Text;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou;

public class powerup : APIBase
{

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase db = KCDatabase.Instance;


		db.Fleet.LoadFromRequest(APIName, data);

		bool destroysEquipments = data.ContainsKey("api_slot_dest_flag") ? (data["api_slot_dest_flag"] != "0") : true;

		foreach (string id in data["api_id_items"].Split(",".ToCharArray()))
		{
			int shipID = int.Parse(id);

			ShipData ship = db.Ships[shipID];
			if (destroysEquipments)
			{
				for (int i = 0; i < ship.Slot.Count; i++)
				{
					if (ship.Slot[i] != -1)
						db.Equipments.Remove(ship.Slot[i]);
				}
			}

			Utility.Logger.Add(2, ship.NameWithLevel + LoggerRes.Decomissioned);
			db.Ships.Remove(shipID);

		}

		base.OnRequestReceived(data);
	}


	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;

		var ship = db.Ships[(int)data.api_ship.api_id];


		if (ship != null)
		{

			if (Utility.Configuration.Config.Log.ShowSpoiler)
			{
				if ((int)data.api_powerup_flag == 0)
				{
					Utility.Logger.Add(2, string.Format(LoggerRes.ImprovedFailure, ship.NameWithLevel));

				}
				else
				{
					var updated_ship = new ShipData();
					updated_ship.LoadFromResponse(APIName, data.api_ship);

					StringBuilder sb = new StringBuilder();
					sb.Append(ship.NameWithLevel + LoggerRes.ImprovedSuccess);

					var contents = new LinkedList<string>();


					int hp = updated_ship.HPMax - ship.HPMax;
					if (hp > 0)
						contents.AddLast(string.Format(EncycloRes.HP + "+{0}→{1}/{2}", hp, updated_ship.HPMax, updated_ship.HPMax + updated_ship.HPMaxRemain));

					int firepower = updated_ship.FirepowerBase - ship.FirepowerBase;
					if (firepower > 0)
						contents.AddLast(string.Format(GeneralRes.Firepower + "+{0}→{1}/{2}", firepower, updated_ship.FirepowerBase, updated_ship.MasterShip.FirepowerMax));

					int torpedo = updated_ship.TorpedoBase - ship.TorpedoBase;
					if (torpedo > 0)
						contents.AddLast(string.Format(GeneralRes.Torpedo + "+{0}→{1}/{2}", torpedo, updated_ship.TorpedoBase, updated_ship.MasterShip.TorpedoMax));

					int aa = updated_ship.AABase - ship.AABase;
					if (aa > 0)
						contents.AddLast(string.Format(GeneralRes.AntiAir + "+{0}→{1}/{2}", aa, updated_ship.AABase, updated_ship.MasterShip.AAMax));

					int armor = updated_ship.ArmorBase - ship.ArmorBase;
					if (armor > 0)
						contents.AddLast(string.Format(GeneralRes.Armor + "+{0}→{1}/{2}", armor, updated_ship.ArmorBase, updated_ship.MasterShip.ArmorMax));

					int asw = updated_ship.ASWBase - ship.ASWBase;
					if (asw > 0)
						contents.AddLast(string.Format(GeneralRes.ASW + "+{0}→{1}/{2}", asw, updated_ship.ASWBase, updated_ship.ASWMax + updated_ship.MasterShip.ASWModernizable));

					int luck = updated_ship.LuckBase - ship.LuckBase;
					if (luck > 0)
						contents.AddLast(string.Format(GeneralRes.Luck + "+{0}→{1}/{2}", luck, updated_ship.LuckBase, updated_ship.MasterShip.LuckMax));


					sb.Append(string.Join(", ", contents)).Append(")");
					Utility.Logger.Add(2, sb.ToString());
				}
			}
			ship.LoadFromResponse(APIName, data.api_ship);
		}

		db.Fleet.LoadFromResponse(APIName, data.api_deck);


		base.OnResponseReceived((object)data);
	}



	public override bool IsRequestSupported => true;
	public override bool IsResponseSupported => true;


	public override string APIName => "api_req_kaisou/powerup";
}
