using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserver.Utility;
using ElectronicObserverTypes;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Services.ApiFileService;

// this is suboptimal but I'm not sure how else to migrate old files to the new format
// migrating all at once is very expensive
public class ApiFileService : ObservableObject
{
	private static int CurrentApiFileVersion => 1;

	// before version 1:
	// hp value is set to hp max instead of hp current
	private static int CurrentSortieRecordVersion => 1;

	private KCDatabase KcDatabase { get; }

	private int? CurrentSortieId { get; set; }

	private static List<string> IgnoredApis { get; } = new()
	{
		"api_start2/getData",
		"api_get_member/questlist",
		"api_get_member/slot_item",
		"api_get_member/unsetslot",
		"api_get_member/ship3",
		"api_get_member/furniture",
		"api_get_member/require_info",
	};

	private Channel<ApiFileData> ApiProcessingChannel { get; } = Channel.CreateUnbounded<ApiFileData>(new UnboundedChannelOptions
	{
		SingleReader = true,
		SingleWriter = true,
		AllowSynchronousContinuations = true,
	});

	private static Queue<int> SortieIdsToProcess { get; } = new();

	public ApiFileService(KCDatabase db)
	{
		KcDatabase = db;

		Task.Run(ProcessApiDataAsync);
	}

	private async Task ProcessApiDataAsync()
	{
		// basically while (true)
		while (await ApiProcessingChannel.Reader.WaitToReadAsync())
		{
			try
			{
				ApiFileData apiFile = await ApiProcessingChannel.Reader.ReadAsync();
				await SaveApiData(apiFile.ApiName, apiFile.RequestBody, apiFile.ResponseBody);
			}
			catch (Exception e)
			{
				Logger.Add(3, $"Api file saving error: {e.GetBaseException().Message} {e.StackTrace}");
			}
		}
	}

	private async Task SaveApiData(string apiName, string requestBody, string responseBody)
	{
		if (IgnoredApis.Contains(apiName)) return;

		await using ElectronicObserverContext db = new();

		requestBody = FormatRequest(requestBody);

		responseBody = TrimSvdata(responseBody);
		responseBody = TrimPort(apiName, responseBody);

		ApiFile requestFile = new()
		{
			ApiFileType = ApiFileType.Request,
			Name = apiName,
			Content = requestBody,
			TimeStamp = DateTime.UtcNow,
			Version = CurrentApiFileVersion,
		};

		ApiFile responseFile = new()
		{
			ApiFileType = ApiFileType.Response,
			Name = apiName,
			Content = responseBody,
			TimeStamp = DateTime.UtcNow,
			Version = CurrentApiFileVersion,
		};

		await db.ApiFiles.AddAsync(requestFile);
		await db.ApiFiles.AddAsync(responseFile);

		await ProcessSortieData(db, requestFile, responseFile);
		await ProcessExpeditionData(db, requestFile, responseFile);

		await db.SaveChangesAsync();
	}

	public async Task Add(string apiName, string requestBody, string responseBody)
	{
		await ApiProcessingChannel.Writer.WriteAsync(new(apiName, requestBody, responseBody));
	}

	/// <summary>
	/// Convert query params to json.
	/// </summary>
	private static string FormatRequest(string requestBody)
	{
		NameValueCollection query = HttpUtility.ParseQueryString(requestBody);

		Dictionary<string, string> dictionary = query.AllKeys
			.ToDictionary(k => k!, k => query[k]!);

		return JsonSerializer.Serialize(dictionary);
	}

	private static string TrimSvdata(string responseBody)
	{
		if (responseBody.StartsWith("s"))
		{
			// trim "svdata="
			responseBody = responseBody[7..];
		}

		return responseBody;
	}

	private static string TrimPort(string apiName, string responseBody)
	{
		if (apiName is not "api_port/port") return responseBody;

		try
		{
			JsonNode? portResponse = JsonNode.Parse(responseBody);

			if (portResponse?["api_data"] is not null)
			{
				portResponse["api_data"]!["api_ship"] = new JsonArray();
				portResponse["api_data"]!["api_deck_port"] = new JsonArray();
				portResponse["api_data"]!["api_log"] = new JsonArray();
				portResponse["api_data"]!["api_ndock"] = new JsonArray();

				responseBody = JsonSerializer.Serialize(portResponse);
			}
			else
			{
				// todo: report failed parsing
			}
		}
		catch
		{
			// todo: report failed parsing
		}

		return responseBody;
	}

	private async Task ProcessSortieData(ElectronicObserverContext db, ApiFile requestFile, ApiFile responseFile)
	{
		if (requestFile.Name is "api_req_map/start")
		{
			await CreateNewSortie(db, requestFile, responseFile);
			return;
		}

		if (requestFile.Name is "api_port/port")
		{
			if (CurrentSortieId is int sortieId)
			{
				SortieIdsToProcess.Enqueue(sortieId);
			}

			CurrentSortieId = null;
			return;
		}

		SortieRecord? sortieRecord = CurrentSortieId switch
		{
			int id => await db.Sorties.FirstOrDefaultAsync(s => s.Id == id),
			_ => null,
		};

		if (sortieRecord is null)
		{
			// this should be all apis that are not related to a sortie
			// apis related to sortie are all api calls that happen between
			// "api_req_map/start" and "api_port/port"
			// can also happen if there's an error when parsing "api_req_map/start"
			return;
		}

		sortieRecord.ApiFiles.Add(requestFile);
		sortieRecord.ApiFiles.Add(responseFile);

		await db.SaveChangesAsync();
	}

	private async Task CreateNewSortie(ElectronicObserverContext db, ApiFile requestFile, ApiFile responseFile)
	{
		if (CurrentSortieId is not null)
		{
			// todo: log bug - CurrentSortieId should always be null before a sortie starts
		}

		ApiReqMapStartRequest? request = null;
		ApiResponse<ApiReqMapStartResponse>? response = null;

		try
		{
			request = JsonSerializer.Deserialize<ApiReqMapStartRequest>(requestFile.Content);
			response = JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartResponse>>(responseFile.Content);
		}
		catch
		{
			// todo: report failed parsing
		}

		if (request is null) return;
		if (response is null) return;
		if (!int.TryParse(request.ApiDeckId, out int fleetId)) return;

		MapInfoData? map = KcDatabase.MapInfo.Values
			.Where(m => m.MapAreaID == response.ApiData.ApiMapareaId)
			.FirstOrDefault(m => m.MapInfoID == response.ApiData.ApiMapinfoNo);

		if (map is null) return;

		SortieMapData mapData = new()
		{
			RequiredDefeatedCount = map.RequiredDefeatedCount,
			MapHPMax = map.MapHPMax,
			MapHPCurrent = map.MapHPCurrent,
		};

		int nodeSupportFleetId = KcDatabase.Fleet.NodeSupportFleetId(map.MapAreaID) ?? 0;
		int bossSupportFleetId = KcDatabase.Fleet.BossSupportFleetId(map.MapAreaID) ?? 0;

		SortieFleetData fleetData = MakeSortieFleet(KcDatabase.Fleet.Fleets.Values,
			KcDatabase.BaseAirCorps.Values, KcDatabase.Fleet.CombinedFlag, fleetId,
			nodeSupportFleetId, bossSupportFleetId, response.ApiData.ApiMapareaId);

		SortieRecord sortieRecord = new()
		{
			Version = CurrentSortieRecordVersion,
			World = response.ApiData.ApiMapareaId,
			Map = response.ApiData.ApiMapinfoNo,
			FleetData = fleetData,
			MapData = mapData,
		};

		sortieRecord.ApiFiles.Add(requestFile);
		sortieRecord.ApiFiles.Add(responseFile);

		await db.Sorties.AddAsync(sortieRecord);
		await db.SaveChangesAsync();

		CurrentSortieId = sortieRecord.Id;
	}

	private async Task ProcessExpeditionData(ElectronicObserverContext db, ApiFile requestFile, ApiFile responseFile)
	{
		if (requestFile.Name is not "api_req_mission/result") return;

		ApiReqMissionResultRequest? request = null;
		ApiResponse<ApiReqMissionResultResponse>? response = null;

		try
		{
			request = JsonSerializer.Deserialize<ApiReqMissionResultRequest>(requestFile.Content);
			response = JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(responseFile.Content);
		}
		catch
		{
			// todo: report failed parsing
		}

		if (request is null) return;
		if (response is null) return;
		if (!int.TryParse(request.ApiDeckId, out int fleetId)) return;

		IFleetData f = KCDatabase.Instance.Fleet[fleetId];

		SortieFleet fleet = MakeSortieFleet(f);

		ExpeditionRecord expedition = new()
		{
			Expedition = f.ExpeditionDestination,
			Fleet = fleet,
		};

		expedition.ApiFiles.Add(requestFile);
		expedition.ApiFiles.Add(responseFile);

		await db.Expeditions.AddAsync(expedition);
		await db.SaveChangesAsync();
	}

	private static bool ShouldIncludeFleet(IFleetData fleet, FleetType combinedFlag, int fleetId,
		int nodeSupportFleetId, int bossSupportFleetId) =>
		fleet.ID == fleetId ||
		fleet.ID == 2 && combinedFlag is not FleetType.Single ||
		fleet.ID == nodeSupportFleetId ||
		fleet.ID == bossSupportFleetId;

	public static SortieFleetData MakeSortieFleet(IEnumerable<IFleetData?> fleets,
		IEnumerable<IBaseAirCorpsData> airBases, FleetType combinedFlag, int fleetId, int nodeSupportFleetId,
		int bossSupportFleetId, int world) => new()
	{
		FleetId = fleetId,
		NodeSupportFleetId = nodeSupportFleetId,
		BossSupportFleetId = bossSupportFleetId,
		CombinedFlag = combinedFlag,
		Fleets = fleets
			.Select(f => f switch
			{
				null => null,
				_ => ShouldIncludeFleet(f, combinedFlag, fleetId, nodeSupportFleetId, bossSupportFleetId) switch
				{
					true => MakeSortieFleet(f),
					_ => null,
				},
			}).ToList(),
		AirBases = airBases
			.Where(a => a.MapAreaID == world)
			.Select(MakeSortieAirBase)
			.ToList(),
	};

	private static SortieFleet MakeSortieFleet(IFleetData f) => new()
	{
		Name = f.Name,
		Ships = f.MembersInstance
			.Where(s => s is not null)
			.Cast<IShipData>()
			.Select(MakeSortieShip)
			.ToList(),
	};

	private static SortieShip MakeSortieShip(IShipData s) => new()
	{
		Id = s.MasterShip.ShipId,
		DropId = s.MasterID,
		Level = s.Level,
		Condition = s.Condition,
		Kyouka = [.. s.Kyouka],
		Fuel = s.Fuel,
		Ammo = s.Ammo,
		Hp = s.HPCurrent,
		Armor = s.ArmorTotal,
		Evasion = s.EvasionTotal,
		Aircraft = [.. s.Aircraft],
		Range = s.Range,
		Speed = s.Speed,
		Firepower = s.FirepowerTotal,
		Torpedo = s.TorpedoTotal,
		Aa = s.AATotal,
		Asw = s.ASWTotal,
		Search = s.LOSTotal,
		Luck = s.LuckTotal,
		EquipmentSlots = s.SlotInstance
			.Zip(s.Aircraft, (Equipment, AircraftCurrent) => (Equipment, AircraftCurrent))
			.Zip(s.MasterShip.Aircraft, (slot, AircraftMax) => new SortieEquipmentSlot
			{
				Equipment = slot.Equipment switch
				{
					{ } => new()
					{
						Id = slot.Equipment.EquipmentId,
						Level = slot.Equipment.Level,
						AircraftLevel = slot.Equipment.AircraftLevel,
					},
					_ => null,
				},
				AircraftCurrent = slot.AircraftCurrent,
				AircraftMax = AircraftMax,
			}).ToList(),
		ExpansionSlot = s.IsExpansionSlotAvailable switch
		{
			true => new()
			{
				Equipment = s.ExpansionSlotInstance switch
				{
					{ } eq => new()
					{
						Id = eq.EquipmentId,
						Level = eq.Level,
						AircraftLevel = eq.AircraftLevel,
					},
					_ => null,
				},
				AircraftCurrent = 0,
				AircraftMax = 0,
			},
			_ => null,
		},
		SpecialEffectItems = s.SpecialEffectItems,
	};

	private static SortieAirBase MakeSortieAirBase(IBaseAirCorpsData a) => new()
	{
		Name = a.Name,
		ActionKind = a.ActionKind,
		AirCorpsId = a.AirCorpsID,
		BaseDistance = a.BaseDistance,
		BonusDistance = a.BonusDistance,
		MapAreaId = a.MapAreaID,
		Squadrons = a.Squadrons.Values
			.Select(MakeSortieAirBaseSquadron)
			.ToList(),
	};

	private static SortieAirBaseSquadron MakeSortieAirBaseSquadron(IBaseAirCorpsSquadron s) => new()
	{
		AircraftCurrent = s.AircraftCurrent,
		State = s.State,
		Condition = s.Condition,
		EquipmentSlot = new()
		{
			AircraftCurrent = s.AircraftCurrent,
			AircraftMax = s.AircraftMax,
			Equipment = s.EquipmentInstance switch
			{
				{ } => new SortieEquipment
				{
					Id = s.EquipmentInstance.EquipmentId,
					Level = s.EquipmentInstance.Level,
					AircraftLevel = s.EquipmentInstance.AircraftLevel,
				},
				_ => null,
			},
		},
	};

	public async Task ProcessedApi(string apiName)
	{
		if (apiName is not "api_port/port") return;
		if (!SortieIdsToProcess.TryDequeue(out int sortieId)) return;

		await using ElectronicObserverContext db = new();

		SortieRecord? sortie = await db.Sorties.FirstOrDefaultAsync(s => s.Id == sortieId);

		if (sortie is null) return;

		int fleetId = sortie.FleetData.FleetId;
		int nodeSupportFleetId = sortie.FleetData.NodeSupportFleetId;
		int bossSupportFleetId = sortie.FleetData.BossSupportFleetId;

		sortie.FleetAfterSortieData = MakeSortieFleet(KcDatabase.Fleet.Fleets.Values,
			KcDatabase.BaseAirCorps.Values, KcDatabase.Fleet.CombinedFlag, fleetId,
			nodeSupportFleetId, bossSupportFleetId, sortie.World);

		await db.SaveChangesAsync();
	}
}
