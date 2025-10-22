using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

/// <summary>
/// Sortie flow:
/// api_req_map/start - sortie start - map related data
/// battle or other nodes
/// api_get_member/ship_deck - updates ship state
/// api_req_map/next - after each node except the last one
/// api_port/port - after sortie ends
///
/// api_req_map/start
///		set all the map related data, sortie fleet id, fleet type (single, stf, ctf, tcf)
///		make a fleet snapshot (fleet before battle)
///
/// first battle (day, night only etc.)
///		set boss battle flag
///		save the raw battle data
///		add support to fleet before battle if it's present in the raw battle data
///
/// second battle (day, night only etc.)
///		save the raw battle data
///		add support to fleet before battle if it's present in the raw battle data
///
/// api_req_map/next
///		make a fleet snapshot (fleet after battle)
///		send the data to poi
///		clear the battle state 
///		make a fleet snapshot (fleet before battle)
/// 
/// api_port/port
///		make a fleet snapshot (fleet after battle)
///		send the data to poi
///		clear the whole state
/// </summary>
public class PoiDbBattleSubmissionService(
	KCDatabase kcDatabase,
	string version,
	PoiHttpClient poiHttpClient,
	Action<Exception> logError)
{
	private KCDatabase KcDatabase { get; } = kcDatabase;
	private string Version { get; } = version;
	private PoiHttpClient PoiHttpClient { get; } = poiHttpClient;
	private Action<Exception> LogError { get; } = logError;

	// cache all the battle data and send it all at the same time when
	// - you get back to port if no AB is used
	// - you get to the sortie screen (api_get_member/mapinfo) if AB is used
	private List<PoiDbBattleSubmissionData> SubmissionCache { get; } = [];

	// map state (stays the same for the whole sortie)
	private int? EventDifficulty { get; set; }
	private int? SortieFleetId { get; set; }
	private FleetType? FleetType { get; set; }
	private int? CellCount { get; set; }
	private int? World { get; set; }
	private int? Map { get; set; }

	// battle state (changes after each battle)
	private int? Cell { get; set; }
	private Fleet? FleetBeforeBattle { get; set; }
	private FleetAfter? FleetAfterBattle { get; set; }
	private JsonNode? FirstBattleData { get; set; }
	private JsonNode? SecondBattleData { get; set; }
	private bool? IsBossNode { get; set; }

	public void ApiReqMap_Start_ResponseReceived(string apiname, dynamic data)
	{
		ApiReqMapStartResponse response = JsonSerializer.Deserialize<ApiReqMapStartResponse>(data.ToString());

		CellCount = response.ApiCellData.Count;
		World = response.ApiMapareaId;
		Map = response.ApiMapinfoNo;
		Cell = response.ApiNo;
		EventDifficulty = KcDatabase.Battle.Compass.MapInfo.EventDifficulty;
		SortieFleetId = KcDatabase.Fleet.Fleets.Values
			.FirstOrDefault(f => f.IsInSortie)
			?.FleetID;
		FleetType = KcDatabase.Fleet.CombinedFlag;
		FleetBeforeBattle = MakeFleet(KcDatabase);
	}

	public void ProcessFirstBattle(string apiName, dynamic data)
	{
		IsBossNode = KcDatabase.Battle.Compass.IsBossNode;

		string firstBattleData = data.ToString();
		FirstBattleData = JsonNode.Parse(firstBattleData)!;

		AddPoiData(FirstBattleData, apiName);
		AddSupport(FirstBattleData);
	}

	public void ProcessSecondBattle(string apiName, dynamic data)
	{
		string secondBattleData = data.ToString();
		SecondBattleData = JsonNode.Parse(secondBattleData)!;

		AddPoiData(SecondBattleData, apiName);
		// I'm not sure if support ever happens in the second battle
		// air support for night to day battles maybe?
		AddSupport(SecondBattleData);
	}

	private static void AddPoiData(JsonNode battle, string apiName)
	{
		battle["poi_path"] = JsonValue.Create($"/kcsapi/{apiName}");
		battle["poi_time"] = JsonValue.Create(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
	}

	private void AddSupport(JsonNode battleData)
	{
		JsonNode? support = battleData["api_support_info"] ?? battleData["api_n_support_info"];

		if (FleetBeforeBattle is not null && support is not null)
		{
			FleetBeforeBattle.Support.Add(support);
		}
	}

	public void ApiReqMap_NextOnResponseReceived(string apiname, dynamic data)
	{
		if (FleetBeforeBattle is null) return;
		if (World is not int world) return;

		ApiReqMapNextResponse response = JsonSerializer.Deserialize<ApiReqMapNextResponse>(data.ToString());

		FleetAfterBattle = MakeFleetAfter(KcDatabase, world);

		PoiDbBattleSubmissionData? submissionData = MakeSubmissionData();

		if (submissionData is not null)
		{
			SubmissionCache.Add(submissionData);
		}

		ClearBattleState();

		Cell = response.ApiNo;
		FleetBeforeBattle = MakeFleet(KcDatabase);
	}

	public void ApiPort_Port_ResponseReceived(string apiname, dynamic data)
	{
		if (FleetBeforeBattle is not null && World is int world)
		{
			FleetAfterBattle = MakeFleetAfter(KcDatabase, world);

			PoiDbBattleSubmissionData? submissionData = MakeSubmissionData();

			if (submissionData is not null)
			{
				SubmissionCache.Add(submissionData);

				// if AB is used we need to wait until we get the final AB state before submitting data
				// this happens when you go to the sortie screen (api_get_member/mapinfo)
				if (submissionData.Data.Fleet.Lbac.Count is 0)
				{
					SubmitData();
				}
			}
		}

		ClearState();
	}

	/// <summary>
	/// This is when you get the AB state after the last sortie battle.
	/// </summary>
	public void ApiGetMember_MapInfo_ResponseReceived(string apiname, dynamic data)
	{
		if (SubmissionCache.Count is 0) return;
		if (World is not int world) return;

		List<PoiAirBase> airBases = KcDatabase.BaseAirCorps.Values
			.Where(a => a.MapAreaID == world)
			.Select(MakeAirBase)
			.ToList();

		PoiDbBattleSubmissionData lastSubmissionData = SubmissionCache[^1];

		lastSubmissionData.Data.FleetAfter.Lbac.Clear();
		lastSubmissionData.Data.FleetAfter.Lbac.AddRange(airBases);

		SubmitData();
	}

	private void ClearState()
	{
		EventDifficulty = null;
		SortieFleetId = null;
		FleetType = null;
		CellCount = null;

		ClearBattleState();
	}

	private void ClearBattleState()
	{
		Cell = null;
		FleetBeforeBattle = null;
		FleetAfterBattle = null;
		FirstBattleData = null;
		SecondBattleData = null;
		IsBossNode = null;
	}

	private void SubmitData()
	{
		try
		{
			string groupId = DateTimeOffset.UtcNow.ToUnixTimeMicroseconds().ToString();

			foreach (PoiDbBattleSubmissionData submissionData in SubmissionCache)
			{
				submissionData.Data.GroupId = groupId;

				Task.Run(async () =>
				{
					try
					{
						await PoiHttpClient.Battle(submissionData);
					}
					catch (Exception e)
					{
						LogError(e);
					}
				});
			}
		}
		catch (Exception e)
		{
			LogError(e);
			ClearState();
		}

		World = null;
		Map = null;
		SubmissionCache.Clear();
	}

	private PoiDbBattleSubmissionData? MakeSubmissionData()
	{
		if (FleetBeforeBattle is null) return null;
		if (FleetAfterBattle is null) return null;
		if (FirstBattleData is null) return null;
		if (CellCount is not int cellCount) return null;
		if (World is not int world) return null;
		if (Map is not int map) return null;
		if (Cell is not int cell) return null;
		if (EventDifficulty is not int eventDifficulty) return null;
		if (IsBossNode is not bool isBossNode) return null;

		List<JsonNode> battleData = [FirstBattleData];

		if (SecondBattleData is not null)
		{
			battleData.Add(SecondBattleData);
		}

		PoiDbBattleSubmissionData submissionData = new()
		{
			Data = new()
			{
				Fleet = FleetBeforeBattle,
				FleetAfter = FleetAfterBattle,
				Map = [world, map, cell],
				Packet = battleData,
				Type = isBossNode switch
				{
					true => "Boss",
					_ => "Normal",
				},
				Version = Version,
				ApiCellData = cellCount,
				MapLevel = eventDifficulty,
				Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
			}
		};

		return submissionData;
	}

	private Fleet? MakeFleet(KCDatabase kcDatabase)
	{
		if (World is not int world) return null;
		if (SortieFleetId is not int sortieFleetId) return null;

		FleetData? fleet = kcDatabase.Fleet.Fleets[sortieFleetId];

		if (fleet is null) return null;

		bool isCombinedFleetSortie = fleet.FleetID is 1 &&
			FleetType is not Core.Types.FleetType.Single;

		FleetData? escortFleet = isCombinedFleetSortie switch
		{
			true => kcDatabase.Fleet.Fleets[2],
			_ => null,
		};

		return new()
		{
			Type = kcDatabase.Fleet.CombinedFlag,
			Main = fleet.MembersInstance
				!.OfType<IShipData>()
				.Select(Extensions.MakeShip)
				.ToList(),
			Escort = escortFleet?.MembersInstance
				!.OfType<IShipData>()
				.Select(Extensions.MakeShip)
				.ToList(),
			Lbac = kcDatabase.BaseAirCorps.Values
				.Where(a => a.MapAreaID == world)
				.Select(MakeAirBase)
				.ToList(),
			Support = [ /* this part gets added when processing the battle */ ],
		};
	}

	private FleetAfter? MakeFleetAfter(KCDatabase kcDatabase, int world)
	{
		if (SortieFleetId is not int sortieFleetId) return null;

		FleetData? fleet = kcDatabase.Fleet.Fleets[sortieFleetId];

		if (fleet is null) return null;

		bool isCombinedFleetSortie = fleet.FleetID is 1 &&
			FleetType is not Core.Types.FleetType.Single;

		FleetData? escortFleet = isCombinedFleetSortie switch
		{
			true => kcDatabase.Fleet.Fleets[2],
			_ => null,
		};

		return new()
		{
			Main = fleet.MembersInstance
				!.OfType<IShipData>()
				.Select(Extensions.MakeShip)
				.ToList(),
			Escort = escortFleet?.MembersInstance
				!.OfType<IShipData>()
				.Select(Extensions.MakeShip)
				.ToList(),
			Lbac = kcDatabase.BaseAirCorps.Values
				.Where(a => a.MapAreaID == world)
				.Select(MakeAirBase)
				.ToList(),
		};
	}

	private static PoiAirBase MakeAirBase(BaseAirCorpsData ab) => new()
	{
		ApiActionKind = ab.ActionKind,
		ApiAreaId = ab.MapAreaID,
		ApiDistance = new()
		{
			ApiBase = ab.BaseDistance,
			ApiBonus = ab.BaseDistance,
		},
		ApiName = ab.Name,
		ApiRid = ab.ID,
		ApiPlaneInfo = ab.Squadrons.Values
			.Select(MakeAirBasePlane)
			.ToList(),
	};

	private static PoiPlaneInfo MakeAirBasePlane(IBaseAirCorpsSquadron s) => new()
	{
		PoiSlot = s.EquipmentInstance switch
		{
			IEquipmentData equip => new()
			{
				ApiAlv = equip.AircraftLevel,
				ApiLevel = equip.Level,
				ApiLocked = equip.IsLocked switch
				{
					true => 1,
					_ => 0,
				},
				ApiSlotitemId = equip.EquipmentId,
				ApiId = equip.MasterID,
			},
			_ => null,
		},
		ApiCond = s.Condition,
		ApiCount = s.AircraftCurrent,
		ApiMaxCount = s.AircraftMax,
		ApiSquadronId = s.SquadronID,
		ApiState = s.State,
	};
}
