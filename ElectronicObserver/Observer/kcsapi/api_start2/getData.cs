﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.MapData;
using ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

namespace ElectronicObserver.Observer.kcsapi.api_start2;

public class getData : APIBase
{


	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;
		using ElectronicObserverContext context = new();


		//特別置換処理
		data.api_mst_stype[7].api_name = "巡洋戦艦";


		//api_mst_ship
		foreach (var elem in data.api_mst_ship)
		{

			int id = (int)elem.api_id;
			if (db.MasterShips[id] == null)
			{
				var ship = new ShipDataMaster();
				ship.LoadFromResponse(APIName, elem);
				db.MasterShips.Add(ship);
			}
			else
			{
				db.MasterShips[id].LoadFromResponse(APIName, elem);
			}
		}

		//改装関連のデータ設定
		foreach (var ship in db.MasterShips)
		{
			int remodelID = ship.Value.RemodelAfterShipID;
			if (remodelID != 0)
			{
				db.MasterShips[remodelID].RemodelBeforeShipID = ship.Key;
			}
		}


		//api_mst_slotitem_equiptype
		foreach (var elem in data.api_mst_slotitem_equiptype)
		{

			int id = (int)elem.api_id;
			if (db.EquipmentTypes[id] == null)
			{
				var eqt = new EquipmentType();
				eqt.LoadFromResponse(APIName, elem);
				db.EquipmentTypes.Add(eqt);
			}
			else
			{
				db.EquipmentTypes[id].LoadFromResponse(APIName, elem);
			}
		}


		//api_mst_stype
		foreach (var elem in data.api_mst_stype)
		{

			int id = (int)elem.api_id;
			if (db.ShipTypes[id] == null)
			{
				var spt = new ShipType();
				spt.LoadFromResponse(APIName, elem);
				db.ShipTypes.Add(spt);
			}
			else
			{
				db.ShipTypes[id].LoadFromResponse(APIName, elem);
			}
		}


		//api_mst_slotitem
		foreach (var elem in data.api_mst_slotitem)
		{

			int id = (int)elem.api_id;
			if (db.MasterEquipments[id] == null)
			{
				var eq = new EquipmentDataMaster();
				eq.LoadFromResponse(APIName, elem);
				db.MasterEquipments.Add(eq);
			}
			else
			{
				db.MasterEquipments[id].LoadFromResponse(APIName, elem);
			}
		}


		//api_mst_useitem
		foreach (var elem in data.api_mst_useitem)
		{

			int id = (int)elem.api_id;
			if (db.MasterUseItems[id] == null)
			{
				var item = new UseItemMaster();
				item.LoadFromResponse(APIName, elem);
				db.MasterUseItems.Add(item);
			}
			else
			{
				db.MasterUseItems[id].LoadFromResponse(APIName, elem);
			}
		}

		//api_mst_maparea
		foreach (var elem in data.api_mst_maparea)
		{
			int id = (int)elem.api_id;
			if (db.MapArea[id] == null)
			{
				var item = new MapAreaData();
				item.LoadFromResponse(APIName, elem);
				db.MapArea.Add(item);
			}
			else
			{
				db.MapArea[id].LoadFromResponse(APIName, elem);
			}

			WorldModel? world = context.Worlds.Find(id);

			if (world is null)
			{
				context.Worlds.Add(new()
				{
					Id = id,
				});
			}
			else
			{
				// update any world related data if we ever add any
			}
		}

		//api_mst_mapinfo
		foreach (var elem in data.api_mst_mapinfo)
		{

			int id = (int)elem.api_id;
			if (db.MapInfo[id] == null)
			{
				var item = new MapInfoData();
				item.LoadFromResponse(APIName, elem);
				db.MapInfo.Add(item);
			}
			else
			{
				db.MapInfo[id].LoadFromResponse(APIName, elem);
			}

			MapModel? map = context.Maps.Find(id);

			if (map is null)
			{
				context.Maps.Add(new()
				{
					Id = id,
					WorldId = (int)elem.api_maparea_id,
					MapId = (int)elem.api_no,
				});
			}
			else
			{
				// update any map related data if we ever add any
			}
		}


		//api_mst_mission
		foreach (var elem in data.api_mst_mission)
		{

			int id = (int)elem.api_id;
			if (db.Mission[id] == null)
			{
				var item = new MissionData();
				item.LoadFromResponse(APIName, elem);
				db.Mission.Add(item);
			}
			else
			{
				db.Mission[id].LoadFromResponse(APIName, elem);
			}

		}


		//api_mst_shipupgrade
		Dictionary<int, int> upgradeLevels = new Dictionary<int, int>();
		foreach (var elem in data.api_mst_shipupgrade)
		{
			int idbefore = (int)elem.api_current_ship_id;
			int idafter = (int)elem.api_id;
			var shipbefore = db.MasterShips[idbefore];
			var shipafter = db.MasterShips[idafter];
			int level = (int)elem.api_upgrade_level;

			if (upgradeLevels.ContainsKey(idafter))
			{
				if (level < upgradeLevels[idafter])
				{
					shipafter.RemodelBeforeShipID = idbefore;
					upgradeLevels[idafter] = level;
				}
			}
			else
			{
				shipafter.RemodelBeforeShipID = idbefore;
				upgradeLevels.Add(idafter, level);
			}

			if (shipbefore != null)
			{
				shipbefore.NeedBlueprint = (int)elem.api_drawing_count;
				shipbefore.NeedCatapult = (int)elem.api_catapult_count;
				shipbefore.NeedActionReport = (int)elem.api_report_count;
				shipbefore.NeedAviationMaterial = (int)elem.api_aviation_mat_count;
				shipbefore.NeedArmamentMaterial = elem.api_arms_mat_count() ? (int)elem.api_arms_mat_count : 0;
			}
		}

		Dictionary<string, ApiMstEquipShip> specialEquippableCategories = JsonSerializer
			.Deserialize<Dictionary<string, ApiMstEquipShip>>(data.api_mst_equip_ship.ToString());

		foreach ((string key, ApiMstEquipShip value) in specialEquippableCategories)
		{
			int id = int.Parse(key);

			db.MasterShips[id].SpecialEquippableCategories = [.. value.ApiEquipType.Keys.Select(int.Parse)];
		}

		Dictionary<string, ApiMstEquipExslotShip> expansionSlotData = JsonSerializer
			.Deserialize<Dictionary<string, ApiMstEquipExslotShip>>(data.api_mst_equip_exslot_ship.ToString());

		foreach ((string key, ApiMstEquipExslotShip value) in expansionSlotData)
		{
			int id = int.Parse(key);

			if (value.ApiShipIds is not null)
			{
				db.MasterEquipments[id].EquippableShipsAtExpansion = value.ApiShipIds.Keys
					.Select(Enum.Parse<ShipId>);
			}

			if (value.ApiStypes is not null)
			{
				db.MasterEquipments[id].EquippableShipTypesAtExpansion = value.ApiStypes.Keys
					.Select(Enum.Parse<ShipTypes>);
			}

			if (value.ApiCtypes is not null)
			{
				db.MasterEquipments[id].EquippableShipClassesAtExpansion = value.ApiCtypes.Keys
					.Select(Enum.Parse<ShipClass>);
			}
		}


		//api_mst_shipgraph
		foreach (var elem in data.api_mst_shipgraph)
		{

			int id = (int)elem.api_id;
			if (db.ShipGraphics[id] == null)
			{
				var sgd = new ShipGraphicData();
				sgd.LoadFromResponse(APIName, elem);
				db.ShipGraphics.Add(sgd);
			}
			else
			{
				db.ShipGraphics[id].LoadFromResponse(APIName, elem);
			}
		}

		context.SaveChanges();

		Utility.Logger.Add(2, LoggerRes.GameStart);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_start2/getData";
}
