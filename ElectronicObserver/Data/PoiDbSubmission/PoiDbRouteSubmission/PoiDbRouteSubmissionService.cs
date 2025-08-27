using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class PoiDbRouteSubmissionService(
	KCDatabase kcDatabase,
	string version,
	PoiHttpClient poiHttpClient,
	Action<Exception> logError)
{
	private KCDatabase KcDatabase { get; } = kcDatabase;
	private string Version { get; } = version;
	private PoiHttpClient PoiHttpClient { get; } = poiHttpClient;
	private Action<Exception> LogError { get; } = logError;

	private List<ApiMapInfo>? ApiMapInfo { get; set; }
	private ApiMapInfo? CurrentMap { get; set; }
	private int? CellCount { get; set; }
	private List<int>? CellIds { get; set; }
	private ApiHappening? ApiHappening { get; set; }
	private List<ApiItemget>? ApiItemget { get; set; }
	private Dictionary<int, string>? MapLevels { get; set; }
	private int? World { get; set; }
	private int? Map { get; set; }
	private FleetType? CombinedFlag { get; set; }
	private FleetData? Fleet1 { get; set; }
	private FleetData? Fleet2 { get; set; }
	private List<int>? EscapeList { get; set; }

	public void ApiGetMember_MapInfo_ResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiGetMemberMapinfoResponse response = JsonSerializer.Deserialize<ApiGetMemberMapinfoResponse>(json)!;

		ApiMapInfo = response.ApiMapInfo;
		MapLevels = ApiMapInfo
			.ToDictionary(i => i.ApiId, i => i.ApiEventmap?.ApiSelectedRank.ToString() ?? "0");
	}

	public void ApiReqMap_Start_ResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiReqMapStartResponse response = JsonSerializer.Deserialize<ApiReqMapStartResponse>(json)!;

		CellCount = response.ApiCellData.Count;
		World = response.ApiMapareaId;
		Map = response.ApiMapinfoNo;
		CellIds = [response.ApiNo];
		ApiHappening = response.ApiHappening;

		CurrentMap = ApiMapInfo?.FirstOrDefault(m => m.ApiId == 10 * World + Map);

		FleetData? fleet = KcDatabase.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (fleet is null) return;

		Fleet1 = fleet;
		EscapeList = [];
		CombinedFlag = KcDatabase.Fleet.CombinedFlag;

		bool isCombinedFleetSortie = fleet.FleetID is 1 && CombinedFlag is not FleetType.Single;

		if (isCombinedFleetSortie)
		{
			FleetData escortFleet = KcDatabase.Fleet.Fleets[2];

			Fleet2 = escortFleet;
		}

		SubmitData();
	}

	public void ApiReqMap_NextOnResponseReceived(string apiname, dynamic data)
	{
		if (CellIds is null) return;

		string json = data.ToString();
		ApiReqMapNextResponse response = JsonSerializer.Deserialize<ApiReqMapNextResponse>(json)!;

		EscapeList = [];

		if (Fleet1 is not null)
		{
			foreach (int escapedShipDropId in Fleet1.EscapedShipList)
			{
				EscapeList.Add(Fleet1.Members.IndexOf(escapedShipDropId) + 1);
			}
		}

		if (Fleet2 is not null)
		{
			foreach (int escapedShipDropId in Fleet2.EscapedShipList)
			{
				EscapeList.Add(Fleet2.Members.IndexOf(escapedShipDropId) + 6 + 1);
			}
		}

		CellIds.Add(response.ApiNo);

		ApiHappening = response.ApiHappening;
		ApiItemget = response.ApiItemget switch
		{
			JsonElement { ValueKind: JsonValueKind.Array } i
				=> i.Deserialize<List<ApiItemget>>(),

			JsonElement i => [i.Deserialize<ApiItemget>()!],

			_ => null,
		};

		SubmitData();
	}

	public void ApiPort_Port_ResponseReceived(string apiname, dynamic data)
	{
		if (CellIds is not null)
		{
			SubmitData();
		}

		ClearState();
	}

	private void ClearState()
	{
		ApiMapInfo = null;
		CurrentMap = null;
		CellCount = null;
		CellIds = null;
		ApiHappening = null;
		ApiItemget = null;
		MapLevels = null;
		World = null;
		Map = null;
		CombinedFlag = null;
		Fleet1 = null;
		Fleet2 = null;
		EscapeList = null;
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
	private void SubmitData()
	{
		if (CurrentMap is null) return;
		if (Fleet1 is null) return;
		if (EscapeList is null) return;
		if (CellIds is null) return;
		if (MapLevels is null) return;
		if (CombinedFlag is not FleetType fleetType) return;
		if (CellCount is not int cellCount) return;
		if (World is not int world) return;
		if (Map is not int map) return;

		List<PoiDbRouteShip> deck1 = Fleet1.MembersInstance!
			.OfType<ShipData>()
			.Select(MakeShip)
			.ToList();

		List<List<object>> slot1 = Fleet1.MembersInstance!
			.OfType<ShipData>()
			.Select(s => s.SlotInstance
				.Select(e => e.MakePoiEquipment() ?? (object)"-1")
				.ToList())
			.ToList();

		List<PoiDbRouteShip>? deck2 = Fleet2?.MembersInstance!
			.OfType<ShipData>()
			.Select(MakeShip)
			.ToList();

		List<List<object>>? slot2 = Fleet2?.MembersInstance!
			.OfType<ShipData>()
			.Select(s => s.SlotInstance
				.Select(e => e.MakePoiEquipment() ?? (object)-1)
				.ToList())
			.ToList();

		try
		{
			PoiDbRouteSubmissionData submissionData = new()
			{
				FleetType = fleetType,
				Deck1 = deck1,
				Deck2 = deck2,
				EscapeList = EscapeList,
				Slot1 = slot1,
				Slot2 = slot2,
				CellIds = CellIds,
				MapLevels = [MapLevels],
				NextInfo = new()
				{
					World = world.ToString(),
					Map = map.ToString(),
					ItemGet = ApiItemget
						?.Select(a => new PoiApiItemget
						{
							ApiGetcount = a.ApiGetcount.ToString(),
							ApiIconId = a.ApiIconId.ToString(),
							ApiId = a.ApiId.ToString(),
							ApiName = a.ApiName,
							ApiUsemst = a.ApiUsemst.ToString(),
						})
						.ToList(),
					Happening = ApiHappening switch
					{
						null => null,
						_ => new()
						{
							ApiCount = ApiHappening.ApiCount.ToString(),
							ApiDentan = ApiHappening.ApiDentan.ToString(),
							ApiIconId = ApiHappening.ApiIconId.ToString(),
							ApiMstId = ((int)ApiHappening.ApiMstId).ToString(),
							ApiType = ApiHappening.ApiType.ToString(),
							ApiUsemst = ApiHappening.ApiUsemst.ToString(),
						},
					},
					RequiredDefeatCount = CurrentMap.ApiRequiredDefeatCount?.ToString(),
					DefeatCount = CurrentMap.ApiDefeatCount?.ToString(),
					ApiMaxMaphp = CurrentMap.ApiEventmap?.ApiMaxMaphp?.ToString(),
					ApiNowMaphp = CurrentMap.ApiEventmap?.ApiNowMaphp?.ToString(),
				},
				AdmiralLevel = KcDatabase.Admiral.Level,
				LosValues = new()
				{
					SakuOne25 = null,
					SakuOne25a = null,
					SakuOne33x1 = GetSearch(Fleet1, 1),
					SakuOne33x2 = GetSearch(Fleet1, 2),
					SakuOne33x3 = GetSearch(Fleet1, 3),
					SakuOne33x4 = GetSearch(Fleet1, 4),
					SakuTwo25 = null,
					SakuTwo25a = null,
					SakuTwo33x1 = GetSearch(Fleet2, 1),
					SakuTwo33x2 = GetSearch(Fleet2, 2),
					SakuTwo33x3 = GetSearch(Fleet2, 3),
					SakuTwo33x4 = GetSearch(Fleet2, 4),
				},
				ApiCellData = cellCount,
				Version = Version,
			};

			Task.Run(async () =>
			{
				try
				{
					await PoiHttpClient.Route(submissionData);
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

	private static PoiDbRouteShip MakeShip(IShipData ship) => new()
	{
		ApiShipId = ship.ShipID.ToString(),
		ApiLv = ship.Level.ToString(),
		ApiSallyArea = ship.SallyArea switch
		{
			> 0 => ship.SallyArea.ToString(),
			_ => null,
		},
		ApiSoku = ship.Speed.ToString(),
		ApiSlotitemEx = ship.ExpansionSlotInstance?.EquipmentID.ToString() ?? "-1",
		ApiSlotitemLevel = ship.ExpansionSlotInstance?.Level.ToString() ?? "-1",
	};

	[return: NotNullIfNotNull(nameof(fleet))]
	private static string? GetSearch(IFleetData? fleet, int weight) => fleet switch
	{
		null => null,
		_ => Math.Round(Calculator.GetSearchingAbility_New33(fleet, weight), 2, MidpointRounding.ToNegativeInfinity)
			.ToString("F2")
	};
}
