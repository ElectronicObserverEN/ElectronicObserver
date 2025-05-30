﻿using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_mission;

public class result : APIBase
{

	private int _fleetID;


	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		_fleetID = int.Parse(data["api_deck_id"]);

		base.OnRequestReceived(data);
	}

	public override void OnResponseReceived(dynamic data)
	{

		var fleet = KCDatabase.Instance.Fleet[_fleetID];

		Utility.Logger.Add(2, string.Format(NotifierRes.ExpeditionText,
			fleet.FleetID,
			fleet.Name,
			KCDatabase.Instance.Mission[fleet.ExpeditionDestination].DisplayID,
			KCDatabase.Instance.Translation.Mission.Name(data.api_quest_name)));


		// 獲得資源表示
		if (Utility.Configuration.Config.Log.ShowSpoiler)
		{

			var sb = new LinkedList<string>();

			//materials
			if (!(data.api_get_material is double))
			{       //(強制帰還などで)ないときに -1 になる
				int[] materials = (int[])data.api_get_material;
				for (int i = 0; i < 4; i++)
				{
					if (materials[i] > 0)
					{
						sb.AddLast(Constants.GetMaterialName(i + 1) + "+" + materials[i]);
					}
				}

			}

			//items
			{
				for (int i = 0; i < 2; i++)
				{

					int kind = (int)data.api_useitem_flag[i];

					if (kind > 0)
					{

						int id = (int)data["api_get_item" + (i + 1)].api_useitem_id;
						int count = (int)data["api_get_item" + (i + 1)].api_useitem_count;

						switch (kind)
						{
							case 1:
								sb.AddLast(ConstantsRes.Bucket + "+" + count);
								break;
							case 2:
								sb.AddLast(ConstantsRes.Flamethrower + "+" + count);
								break;
							case 3:
								sb.AddLast(ConstantsRes.DevMat + "+" + count);
								break;
							case 4:
								sb.AddLast(KCDatabase.Instance.MasterUseItems[id].NameTranslated + "+" + count);
								break;
							case 5:
								sb.AddLast(ConstantsRes.FurnitureCoin + "+" + count);
								break;
						}

					}
				}
			}

			//exp
			{
				int admiralExp = (int)data.api_get_exp;
				if (admiralExp > 0)
				{
					sb.AddLast(NotifierRes.AdmiralExp + admiralExp);
				}

				int shipExp = ((int[])data.api_get_ship_exp).Min();
				if (shipExp > 0)
				{
					sb.AddLast(NotifierRes.ShipExp + shipExp);
				}
			}

			Utility.Logger.Add(2, NotifierRes.ExpeditionResult + Constants.GetExpeditionResult((int)data.api_clear_result) + ": " + (sb.Count == 0 ? NotifierRes.NoResourcesGained : string.Join(", ", sb)));
		}


		// レベルアップ表示
		{
			int[] exps = new int[6];
			var src = (int[])data.api_get_ship_exp;
			Array.Copy(src, exps, src.Length);

			var lvup = new List<int[]>();
			foreach (var elem in data.api_get_exp_lvup)
			{
				lvup.Add((int[])elem);
			}

			for (int i = 0; i < lvup.Count; i++)
			{
				if (lvup[i].Length >= 2 && lvup[i][1] > 0 && lvup[i][0] + exps[i] >= lvup[i][1])
				{
					var ship = fleet.MembersInstance[i];
					int increment = Math.Max(lvup[i].Length - 2, 1);

					Utility.Logger.Add(2, string.Format(NotifierRes.ShipLevelUp, ship.Name, ship.Level + increment));
				}
			}
		}


		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_mission/result";
}
