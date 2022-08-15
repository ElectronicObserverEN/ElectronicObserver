using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElectronicObserver.ReplayJSON;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserverTypes;
using Newtonsoft.Json;

namespace ElectronicObserver.Data;

public class ReplayManager : ResponseWrapper
{
	public static readonly string ReplayLogPath = "Replays";

	//TODO:fill in remaining values
	ReplayRoot Battle_replay = new ReplayRoot()
	{
		Id = 1,
		Support1 = 0,
		Support2 = 0,
		Combined = 0,
		Defeat_count = 0,
		Now_maphp = 0,
		Max_maphp = 0,
		Fleetnum = 1,
		World = 0,
		Mapnum = 0,
		Fleet1 = new List<Fleet1>(),
		Fleet2 = new List<Fleet2>(),
		Fleet3 = new List<Fleet3>(),
		Fleet4 = new List<Fleet4>(),
		Lbas = new List<Lba>(),
		Battles = new List<ReplayJSON.Battle>(1)
	};

	public Fleet1 Fleet1Data { get; set; }
	public Fleet2 Fleet2Data { get; set; }
	public Fleet3 Fleet3Data { get; set; }
	public Fleet4 Fleet4Data { get; set; }
	public Lba LbaData { get; set; }
	public Plane PlaneData { get; set; }
	public ReplayJSON.Range RangeData { get; set; }

	public dynamic DayData = null;
	public dynamic YasenData = null;
	public bool IsPractice { get; set; }
	public bool IsIgnored { get; set; }

	/// <summary>
	///response wrapper for getting API data
	/// </summary>
	/// <param name="apiname">apiname from battle</param>
	/// <param name="data">api_data or RawData</param>
	public override void LoadFromResponse(string apiname, dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;
		if (Battle_replay.Battles.Count < 1)
		{
			Battle_replay.Battles.Add(new ReplayJSON.Battle());
		}
		switch (apiname)
		{
			case "api_req_map/start":
			case "api_req_map/next":
				int sortied_fleet = db.Fleet.Fleets.Where(fleet => fleet.Value.IsInSortie).Select(fleet => fleet.Value.FleetID).Min();
				Battle_replay.Battles[0].Node = db.Battle.Compass.Destination;
				Battle_replay.Mapnum = db.Battle.Compass.MapInfoID;
				Battle_replay.World = db.Battle.Compass.MapAreaID;
				Battle_replay.Battles[0].SortieId = 1;
				Battle_replay.Battles[0].Id = 1;
				Battle_replay.Battles[0].BaseEXP = 0;
				Battle_replay.Battles[0].HqEXP = 0;
				Battle_replay.Fleetnum = db.Fleet.Fleets.Where(fleet => fleet.Value.IsInSortie).Select(fleet => fleet.Value.FleetID).Min();
				IsPractice = false;
				Battle_replay.Battles[0].Data = null;
				Battle_replay.Battles[0].Yasen = new object();
				YasenData = null;
				DayData = null;
				Battle_replay.Combined = sortied_fleet == 1 ? db.Fleet.CombinedFlag : 0;
				break;

			case "api_req_sortie/battle": // normal day 
				DayData = data;
				break;

			case "api_req_battle_midnight/battle": // normal night
				YasenData = data;
				break;

			case "api_req_battle_midnight/sp_midnight": // night node
				DayData = data;
				break;

			case "api_req_sortie/airbattle": // single air raid
				DayData = data;
				break;

			case "api_req_sortie/ld_airbattle": // single air raid
				DayData = data;
				break;

			case "api_req_sortie/night_to_day": // single night to day
				DayData = data;

				break;

			case "api_req_sortie/ld_shooting": // single fleet radar ambush
				DayData = data;
				break;

			case "api_req_combined_battle/battle": //combined normal
				DayData = data;
				break;

			case "api_req_combined_battle/midnight_battle": // combined day to night					
				YasenData = data;
				break;

			case "api_req_combined_battle/sp_midnight":// combined night battle
				DayData = data;
				break;

			case "api_req_combined_battle/airbattle": // combined air exchange ?
				DayData = data;
				break;

			case "api_req_combined_battle/battle_water": // CTF TCF Combined battle
				DayData = data;
				break;

			case "api_req_combined_battle/ld_airbattle": // air raid
				DayData = data;
				break;

			case "api_req_combined_battle/ec_battle": // CTF enemy combined battle
				DayData = data;
				break;

			case "api_req_combined_battle/ec_midnight_battle": // combined normal night battle
				YasenData = data;
				break;

			case "api_req_combined_battle/ec_night_to_day": // enemy combined night to day
				DayData = data;
				break;

			case "api_req_combined_battle/each_battle": //STF combined vs combined
				DayData = data;
				break;

			case "api_req_combined_battle/each_battle_water": // STF combined
				DayData = data;
				break;

			case "api_req_combined_battle/ld_shooting": // combined radar ambush
				DayData = data;
				break;

			case "api_req_practice/battle": //pvp day
				IsPractice = true;
				Battle_replay.World = 0;
				Battle_replay.Mapnum = 0;
				Battle_replay.Battles[0].Node = 0;
				Battle_replay.Battles[0].Yasen = new object();
				DayData = data;
				break;

			case "api_req_practice/midnight_battle": // pvp night
				IsPractice = true;
				YasenData = data;
				break;

			case "api_req_sortie/battleresult":
			case "api_req_combined_battle/battleresult":
			case "api_req_practice/battle_result":
				Battle_replay.Battles[0].Drop = db.Battle.Result.DroppedShipID;
				Battle_replay.Time = DateTime.Now.Ticks / 1000;
				Battle_replay.Battles[0].Rating = db.Battle.Result.Rank;
				Battle_replay.Battles[0].Time = DateTime.Now.Ticks / 1000;
				SaveReplay();

				break;

			case "api_port/port":
				IsIgnored = false;
				IsPractice = false;
				break;
		}
	}

	private void GetBattleExp()
	{
		KCDatabase db = KCDatabase.Instance;
		Battle_replay.Battles[0].BaseEXP = db.Battle.Result.BaseExp;
		Battle_replay.Battles[0].HqEXP = db.Battle.Result.AdmiralExp;
	}

	private void GetMVP()
	{

		KCDatabase db = KCDatabase.Instance;
		Battle_replay.Battles[0].Mvp = new List<int>();
		Battle_replay.Battles[0].Mvp.Clear();
		if (db.Fleet.CombinedFlag > 0)
		{
			Battle_replay.Battles[0].Mvp = new List<int>(2){
				db.Battle.Result.MVPIndex,
				db.Battle.Result.MVPIndexCombined
			};
		}
		else
		{
			Battle_replay.Battles[0].Mvp = new List<int>(1)
			{
				db.Battle.Result.MVPIndex
			};
		}
	}
	private void GetNodeSupport()
	{
		KCDatabase db = KCDatabase.Instance;
		Battle_replay.Support1 = db.Fleet.NodeSupportFleet ?? 0;
	}

	private void GetBossSupport()
	{
		KCDatabase db = KCDatabase.Instance;
		Battle_replay.Support2 = db.Fleet.BossSupportFleet ?? 0;
	}

	private void GetLBAS()
	{
		Battle_replay.Lbas.Clear();
		KCDatabase db = KCDatabase.Instance;

		int[] bases = KCDatabase.Instance.BaseAirCorps.Keys.ToArray();
		int MatchedBases = 0; // add only if  bases match MapAreaID
		for (int baseIndex = 0; baseIndex < bases.Length; baseIndex++)
		{
			int baseID = bases[baseIndex];
			BaseAirCorpsData baseAirCorps = KCDatabase.Instance.BaseAirCorps[baseID];
			if (baseAirCorps.MapAreaID == db.Battle.Compass.MapAreaID)
			{
				Battle_replay.Lbas.Add(new Lba());
				Battle_replay.Lbas[MatchedBases].Rid = baseAirCorps.AirCorpsID;
				Battle_replay.Lbas[MatchedBases].Action = (int)baseAirCorps.ActionKind;
				Battle_replay.Lbas[MatchedBases].Range = new ReplayJSON.Range();
				Battle_replay.Lbas[MatchedBases].Range.ApiBase = baseAirCorps.Base_Distance;
				Battle_replay.Lbas[MatchedBases].Range.ApiBonus = baseAirCorps.Bonus_Distance;
				Battle_replay.Lbas[MatchedBases].Planes = new List<Plane>(4);
				if (baseAirCorps == null)
					continue;

				for (int squadronIndex = 0; squadronIndex < 4; squadronIndex++)
				{
					var squadron = baseAirCorps.Squadrons[squadronIndex + 1];
					var eq = squadron.EquipmentInstance;
					if (eq != null)
					{
						Battle_replay.Lbas[MatchedBases].Planes.Add(new Plane());
						Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].MstId = eq.EquipmentID;
						Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].State = squadron.State;
						Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].Count = squadron.AircraftCurrent;
						Battle_replay.Lbas[MatchedBases].Planes[squadronIndex].Morale = squadron.Condition;
					}
					else
					{
						Battle_replay.Lbas[MatchedBases].Planes.Add(new Plane());
					}
				}
				MatchedBases++;
			}
		}
	}

	public void SaveReplay()
	{
		GetFleet1();
		GetFleet2();
		GetFleet3();
		GetFleet4();
		GetBattleExp();
		GetMVP();
		if (!IsPractice)
		{
			GetMapHP();
			GetLBAS();
			GetNodeSupport();
			GetBossSupport();
		}
		JsonSerializerSettings KCjsonSerializer = new JsonSerializerSettings()
		{
			CheckAdditionalContent = true
		};
		if (YasenData != null)
		{
			string YasenData_str = YasenData.ToString();
			Battle_replay.Battles[0].Yasen = JsonConvert.DeserializeObject(YasenData_str, KCjsonSerializer);

		}
		if (DayData != null)
		{
			string DayData_str = DayData.ToString();
			Battle_replay.Battles[0].Data = JsonConvert.DeserializeObject(DayData_str, KCjsonSerializer);
		}

		try
		{
			string info;
			string parent = ReplayLogPath;
			if (!Directory.Exists(parent))
				Directory.CreateDirectory(parent);

			KCDatabase db = KCDatabase.Instance;
			if (!IsPractice)
			{
				info = $"{db.Battle.Compass.MapAreaID}-{db.Battle.Compass.MapInfoID}-{db.Battle.Compass.Destination}";
			}
			else
			{
				info = "practice";
			}
			string maps_file = @"Settings\ignored_maps.txt";

			if (File.Exists(maps_file) && !IsPractice)
			{
				foreach (var line in File.ReadLines(maps_file))
				{
					if ($"{db.Battle.Compass.MapAreaID}-{db.Battle.Compass.MapInfoID}" == line)
					{
						IsIgnored = true;
						break;
					}
				};
			}
			string path = $"{parent}\\{DateTimeHelper.GetTimeStamp()}@{info}-Replay.txt";
			if (!IsIgnored)
			{
				using (StreamWriter rep_file = File.CreateText(path))
				{
					JsonSerializer serializer = new JsonSerializer();
					serializer.Serialize(rep_file, Battle_replay);
				}
			}

		}

		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "戦闘ログの出力に失敗しました。");
		}
	}

	private void GetMapHP()
	{
		KCDatabase db = KCDatabase.Instance;
		var maps = db.MapInfo.Values;

		foreach (MapInfoData map in maps)
		{
			if ((map.MapAreaID == db.Battle.Compass.MapAreaID) && (map.MapInfoID == db.Battle.Compass.MapInfoID) && map.RequiredDefeatedCount != -1)
			{
				Battle_replay.Defeat_count = map.CurrentDefeatedCount;
			}
			else if ((map.MapAreaID == db.Battle.Compass.MapAreaID) && (map.MapInfoID == db.Battle.Compass.MapInfoID) && map.MapHPMax > 0)
			{
				Battle_replay.Max_maphp = map.MapHPMax;
				Battle_replay.Now_maphp = map.MapHPCurrent;
			}
		}
	}

	private void GetFleet1()
	{
		KCDatabase db = KCDatabase.Instance;
		FleetData fleet = db.Fleet[1]; //fleet 1
		Battle_replay.Fleet1.Clear();

		for (int i = 0; i < fleet.Members.Count; i++)
		{
			if (fleet[i] == -1)
				continue;

			IShipData ship = fleet.MembersInstance[i];
			List<int> kyouka_list = ship.Kyouka.ToList();
			Battle_replay.Fleet1.Add(new Fleet1()
			{
				MstId = ship.ShipID,
				Level = ship.Level,
				Morale = ship.Condition,
				Kyouka = kyouka_list,
				Equip = ship.AllSlotMasterReplay.ToList(),

			});

		}
	}

	private void GetFleet2()
	{
		KCDatabase db = KCDatabase.Instance;
		FleetData fleet = db.Fleet[2]; //fleet 2
		Battle_replay.Fleet2.Clear();
		if (fleet == null)
		{
			return;
		}
		for (int i = 0; i < fleet.Members.Count; i++)
		{
			if (fleet[i] == -1)
				continue;

			IShipData ship = fleet.MembersInstance[i];
			List<int> kyouka_list = ship.Kyouka.ToList();
			Battle_replay.Fleet2.Add(new Fleet2()
			{
				MstId = ship.ShipID,
				Level = ship.Level,
				Morale = ship.Condition,
				Kyouka = kyouka_list,
				Equip = ship.AllSlotMasterReplay.ToList(),

			});

		}
	}
	private void GetFleet3()
	{
		KCDatabase db = KCDatabase.Instance;
		FleetData fleet = db.Fleet[3]; //fleet 3
		Battle_replay.Fleet3.Clear();
		if (fleet == null)
		{
			return;
		}
		for (int i = 0; i < fleet.Members.Count; i++)
		{
			if (fleet[i] == -1)
				continue;

			IShipData ship = fleet.MembersInstance[i];
			List<int> kyouka_list = ship.Kyouka.ToList();
			Battle_replay.Fleet3.Add(new Fleet3()
			{
				MstId = ship.ShipID,
				Level = ship.Level,
				Morale = ship.Condition,
				Kyouka = kyouka_list,
				Equip = ship.AllSlotMasterReplay.ToList(),

			});

		}
	}
	private void GetFleet4()
	{
		KCDatabase db = KCDatabase.Instance;
		FleetData fleet = db.Fleet[4]; //fleet 4
		Battle_replay.Fleet4.Clear();
		if (fleet == null)
		{
			return;
		}
		for (int i = 0; i < fleet.Members.Count; i++)
		{
			if (fleet[i] == -1)
				continue;

			IShipData ship = fleet.MembersInstance[i];
			List<int> kyouka_list = ship.Kyouka.ToList();
			Battle_replay.Fleet4.Add(new Fleet4()
			{
				MstId = ship.ShipID,
				Level = ship.Level,
				Morale = ship.Condition,
				Kyouka = kyouka_list,
				Equip = ship.AllSlotMasterReplay.ToList(),

			});

		}
	}


}
