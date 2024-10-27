using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

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
	Func<HttpClient> makeHttpClient,
	Action<Exception> logError)
{
	private KCDatabase KcDatabase { get; } = kcDatabase;
	private string Version { get; } = version;
	private Func<HttpClient> MakeHttpClient { get; } = makeHttpClient;
	private Action<Exception> LogError { get; } = logError;

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
	private string? FirstBattleData { get; set; }
	private string? SecondBattleData { get; set; }
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
		FirstBattleData = data.ToString();
		AddSupport(FirstBattleData);
	}

	public void ProcessSecondBattle(string apiName, dynamic data)
	{
		SecondBattleData = data.ToString();

		// I'm not sure if support ever happens in the second battle
		// air support for night to day battles maybe?
		AddSupport(SecondBattleData);
	}

	private void AddSupport(string battleData)
	{
		JsonNode? battle = JsonNode.Parse(battleData);
		JsonNode? support = battle?["api_support_info"] ?? battle?["api_n_support_info"];

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

		SubmitData();
		ClearBattleState();

		Cell = response.ApiNo;
		FleetBeforeBattle = MakeFleet(KcDatabase);
	}

	public void ApiPort_Port_ResponseReceived(string apiname, dynamic data)
	{
		if (FleetBeforeBattle is not null && World is int world)
		{
			FleetAfterBattle = MakeFleetAfter(KcDatabase, world);
			SubmitData();
		}

		ClearState();
	}

	private void ClearState()
	{
		EventDifficulty = null;
		SortieFleetId = null;
		FleetType = null;
		CellCount = null;
		World = null;
		Map = null;

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
		if (FleetBeforeBattle is null) return;
		if (FleetAfterBattle is null) return;
		if (FirstBattleData is null) return;
		if (CellCount is not int cellCount) return;
		if (World is not int world) return;
		if (Map is not int map) return;
		if (Cell is not int cell) return;
		if (EventDifficulty is not int eventDifficulty) return;
		if (IsBossNode is not bool isBossNode) return;

		List<string> battleData = [FirstBattleData];

		if (SecondBattleData is not null)
		{
			battleData.Add(SecondBattleData);
		}

		try
		{
			PoiDbBattleSubmission submission = new()
			{
				Body = new()
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
						Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
					}
				}
			};

			Task.Run(async () =>
			{
				using HttpClient client = MakeHttpClient();

				try
				{
					_ = await client.PostAsJsonAsync("/battle", submission);
				}
				catch (Exception e)
				{
					LogError(e);
				}
			});
		}
		catch (Exception e)
		{
			LogError(e);
			ClearState();
		}
	}

	private Fleet? MakeFleet(KCDatabase kcDatabase)
	{
		if (World is not int world) return null;
		if (SortieFleetId is not int sortieFleetId) return null;

		FleetData? fleet = kcDatabase.Fleet.Fleets[sortieFleetId];

		if (fleet is null) return null;

		bool isCombinedFleetSortie = fleet.FleetID is 1 &&
			FleetType is not ElectronicObserverTypes.FleetType.Single;

		FleetData? escortFleet = isCombinedFleetSortie switch
		{
			true => kcDatabase.Fleet.Fleets[2],
			_ => null,
		};

		return new()
		{
			Type = kcDatabase.Fleet.CombinedFlag,
			Main = fleet.MembersWithoutEscaped
				!.OfType<IShipData>()
				.Select(Extensions.MakeShip)
				.ToList(),
			Escort = escortFleet?.MembersWithoutEscaped
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
			FleetType is not ElectronicObserverTypes.FleetType.Single;

		FleetData? escortFleet = isCombinedFleetSortie switch
		{
			true => kcDatabase.Fleet.Fleets[2],
			_ => null,
		};

		return new()
		{
			Main = fleet.MembersWithoutEscaped
				!.OfType<IShipData>()
				.Select(Extensions.MakeShip)
				.ToList(),
			Escort = escortFleet?.MembersWithoutEscaped
				!.OfType<IShipData>()
				.Select(Extensions.MakeShip)
				.ToList(),
			Lbac = kcDatabase.BaseAirCorps.Values
				.Where(a => a.MapAreaID == world)
				.Select(MakeAirBase)
				.ToList(),
		};
	}

	private static ApiAirBase MakeAirBase(BaseAirCorpsData ab) => new()
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

	private static ApiPlaneInfo MakeAirBasePlane(IBaseAirCorpsSquadron s) => new()
	{
		ApiCond = s.Condition,
		ApiCount = s.AircraftCurrent,
		ApiMaxCount = s.AircraftMax,
		ApiSlotid = s.EquipmentID,
		ApiSquadronId = s.SquadronID,
		ApiState = s.State,
	};
}
