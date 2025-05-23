using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Data;

/// <summary>
/// 基地航空隊のデータを扱います。
/// </summary>
public class BaseAirCorpsData : APIWrapper, IIdentifiable, IBaseAirCorpsData
{


	/// <summary>
	/// 飛行場が存在する海域ID
	/// </summary>
	public int MapAreaID => RawData.api_area_id() ? (int)RawData.api_area_id : -1;


	/// <summary>
	/// 航空隊ID
	/// </summary>
	public int AirCorpsID => (int)RawData.api_rid;


	/// <summary>
	/// 航空隊名
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// 戦闘行動半径 base distance
	/// </summary>
	public int Distance { get; private set; }

	///<summary>
	/// bonus distance
	///</summary>
	public int BonusDistance { get; private set; }

	///<summary>
	/// base distance
	///</summary>
	public int BaseDistance { get; private set; }

	/// <summary>
	/// 行動指示
	/// 0=待機, 1=出撃, 2=防空, 3=退避, 4=休息
	/// </summary>
	public AirBaseActionKind ActionKind { get; private set; }

	/// <summary>
	/// List of points (edge ?) the LBAS will strike
	/// </summary>
	public List<int> StrikePoints { get; private set; } = new List<int>();


	/// <summary>
	/// 航空中隊情報
	/// </summary>
	public IDictionary<int, IBaseAirCorpsSquadron> Squadrons { get; private set; }

	public IBaseAirCorpsSquadron this[int i] => Squadrons[i];




	public BaseAirCorpsData()
	{
		Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>();
	}


	public override void LoadFromRequest(string apiname, Dictionary<string, string> data)
	{
		base.LoadFromRequest(apiname, data);

		switch (apiname)
		{
			case "api_req_air_corps/change_name":
				Name = data["api_name"];
				break;

			case "api_req_air_corps/set_action":
			{

				int[] ids = data["api_base_id"].Split(",".ToCharArray()).Select(s => int.Parse(s)).ToArray();
				int[] actions = data["api_action_kind"].Split(",".ToCharArray()).Select(s => int.Parse(s)).ToArray();

				int index = Array.IndexOf(ids, AirCorpsID);

				if (index >= 0)
				{
					ActionKind = (AirBaseActionKind)actions[index];
				}

			}
			break;

			case "api_req_map/start_air_base":
			{
				SetStrikePoints(data);
			}
			break;
		}
	}

	public override void LoadFromResponse(string apiname, dynamic data)
	{

		switch (apiname)
		{
			case "api_get_member/base_air_corps":
			default:
				base.LoadFromResponse(apiname, (object)data);

				Name = (string)data.api_name;
				Distance = (int)data.api_distance.api_base + (int)data.api_distance.api_bonus;
				ActionKind = (AirBaseActionKind)data.api_action_kind;
				BaseDistance = (int)data.api_distance.api_base;
				BonusDistance = (int)data.api_distance.api_bonus;
				SetSquadrons(apiname, data.api_plane_info);
				break;

			case "api_req_air_corps/change_deployment_base":
				Distance = (int)data.api_distance.api_base + (int)data.api_distance.api_bonus;
				BaseDistance = (int)data.api_distance.api_base;
				BonusDistance = (int)data.api_distance.api_bonus;
				SetSquadrons(apiname, data.api_plane_info);
				break;

			case "api_req_air_corps/set_plane":
			{
				var prev = Squadrons.Values.Select(sq => sq != null && sq.State == 1 ? sq.EquipmentMasterID : 0).ToArray();
				SetSquadrons(apiname, data.api_plane_info);

				foreach (var deleted in prev.Except(Squadrons.Values.Select(sq => sq != null && sq.State == 1 ? sq.EquipmentMasterID : 0)))
				{
					var eq = KCDatabase.Instance.Equipments[deleted];

					if (eq != null)
					{
						KCDatabase.Instance.RelocatedEquipments.Add(new RelocationData(deleted, DateTime.Now));
					}
				}

				Distance = (int)data.api_distance.api_base + (int)data.api_distance.api_bonus;
				BaseDistance = (int)data.api_distance.api_base;
				BonusDistance = (int)data.api_distance.api_bonus;
			}
			break;

			case "api_req_air_corps/supply":
				SetSquadrons(apiname, data.api_plane_info);
				break;


			case "api_port/port":
				// Reset Strike points after the sortie
				StrikePoints.Clear();
				break;
		}
	}

	private void SetSquadrons(string apiname, dynamic data)
	{

		foreach (var elem in data)
		{

			int id = (int)elem.api_squadron_id;

			if (!Squadrons.ContainsKey(id))
			{
				var a = new BaseAirCorpsSquadron();
				a.LoadFromResponse(apiname, elem);
				Squadrons.Add(id, a);

			}
			else
			{
				Squadrons[id].LoadFromResponse(apiname, elem);
			}
		}
	}

	private void SetStrikePoints(Dictionary<string, string> data)
	{
		// --- The request doesn't specify the map area ID, so we'll need to rely on the data from the start API
		int currentArea = KCDatabase.Instance.Battle.Compass.MapAreaID;

		if (currentArea != MapAreaID) return;

		string key = $"api_strike_point_{AirCorpsID}";

		if (!data.ContainsKey(key)) return;

		// --- Points are sent as edges separated by a comma (,)
		string rawPoints = data[key];
		StrikePoints = rawPoints.Split(",").Select(pointAsString => int.Parse(pointAsString)).ToList();
	}

	public override string ToString() => $"[{MapAreaID}:{AirCorpsID}] {Name}";



	public int ID => GetID(RawData);

	// the api doesn't contain this data, so these are never used from this class
	public int HPCurrent { get; set; }
	public int HPMax { get; set; }


	public static int GetID(int mapAreaID, int airCorpsID) => mapAreaID * 10 + airCorpsID;

	public static int GetID(Dictionary<string, string> request)
		=> GetID(int.Parse(request["api_area_id"]), int.Parse(request["api_base_id"]));

	public static int GetID(dynamic response)
		=> GetID(response.api_area_id() ? (int)response.api_area_id : -1, (int)response.api_rid);


}
