using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserverTypes;

namespace ElectronicObserver.Services.ApiFileService;

// this is suboptimal but I'm not sure how else to migrate old files to the new format
// migrating all at once is very expensive
public class ApiFileService : ObservableObject
{
	private static int CurrentApiFileVersion => 1;

	private ElectronicObserverContext Db { get; } = new();
	private KCDatabase KCDatabase { get; }

	private BlockingCollection<ApiFileData> ApiFileQueue { get; } = new();

	private SortieRecord? SortieRecord { get; set; }

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

	public ApiFileService(KCDatabase db)
	{
		KCDatabase = db;

		// https://michaelscodingspot.com/c-job-queues/
		Thread thread = new(ProcessQueue)
		{
			IsBackground = true,
		};
		thread.Start();
	}

	private async void ProcessQueue()
	{
		foreach (ApiFileData apiFile in ApiFileQueue.GetConsumingEnumerable())
		{
			await SaveApiData(apiFile.ApiName, apiFile.RequestBody, apiFile.ResponseBody);
		}
	}

	private async Task SaveApiData(string apiName, string requestBody, string responseBody)
	{
		if (IgnoredApis.Contains(apiName)) return;

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

		await Db.ApiFiles.AddAsync(requestFile);
		await Db.ApiFiles.AddAsync(responseFile);

		await ProcessSortieData(requestFile, responseFile);
		await ProcessExpeditionData(requestFile, responseFile);

		await Db.SaveChangesAsync();
	}

	public Task Add(string apiName, string requestBody, string responseBody)
	{
		ApiFileQueue.Add(new(apiName, requestBody, responseBody));

		return Task.CompletedTask;
	}

	public void SaveChanges()
	{
		Db.SaveChanges();
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
			ApiResponse<ApiPortPortResponse>? response = JsonSerializer
				.Deserialize<ApiResponse<ApiPortPortResponse>>(responseBody);

			if (response?.ApiData is not null)
			{
				response.ApiData.ApiShip = new();
				response.ApiData.ApiDeckPort = new();
				response.ApiData.ApiLog = new();
				response.ApiData.ApiNdock = new();

				responseBody = JsonSerializer.Serialize(response);
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

	private async Task ProcessSortieData(ApiFile requestFile, ApiFile responseFile)
	{
		if (requestFile.Name is "api_port/port")
		{
			SortieRecord = null;
			return;
		}

		if (requestFile.Name is "api_req_map/start")
		{
			if (SortieRecord is not null)
			{
				// todo: log bug - SortieRecord should always be null before a sortie starts
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

			MapInfoData? map = KCDatabase.MapInfo.Values
				.Where(m => m.MapAreaID == response.ApiData.ApiMapareaId)
				.FirstOrDefault(m => m.MapInfoID == response.ApiData.ApiMapinfoNo);

			if (map is null) return;

			SortieMapData mapData = new()
			{
				RequiredDefeatedCount = map.RequiredDefeatedCount,
				MapHPMax = map.MapHPMax,
				MapHPCurrent = map.MapHPCurrent,
			};

			int nodeSupportFleetId = KCDatabase.Fleet.NodeSupportFleetId(map.MapAreaID) ?? 0;
			int bossSupportFleetId = KCDatabase.Fleet.BossSupportFleetId(map.MapAreaID) ?? 0;

			bool ShouldIncludeFleet(IFleetData fleet) =>
				fleet.ID == fleetId ||
				fleet.ID == 2 && KCDatabase.Fleet.CombinedFlag != 0 ||
				fleet.ID == nodeSupportFleetId ||
				fleet.ID == bossSupportFleetId;

			SortieFleetData fleetData = new()
			{
				FleetId = fleetId,
				NodeSupportFleetId = nodeSupportFleetId,
				BossSupportFleetId = bossSupportFleetId,
				CombinedFlag = KCDatabase.Fleet.CombinedFlag,
				Fleets = KCDatabase.Fleet.Fleets.Values
					.Select(f => ShouldIncludeFleet(f) switch
					{
						true => new SortieFleet
						{
							Name = f.Name,
							Ships = f.MembersInstance
								.Where(s => s is not null)
								.Select(MakeSortieShip)
								.ToList(),
						},

						_ => null,
					}).ToList(),
				AirBases = KCDatabase.BaseAirCorps.Values
					.Where(a => a.MapAreaID == map.MapAreaID)
					.Select(a => new SortieAirBase
					{
						Name = a.Name,
						ActionKind = a.ActionKind,
						AirCorpsId = a.AirCorpsID,
						BaseDistance = a.Base_Distance,
						BonusDistance = a.Bonus_Distance,
						MapAreaId = a.MapAreaID,
						Squadrons = a.Squadrons.Values
							.Select(s => new SortieAirBaseSquadron
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
							}).ToList(),
					}).ToList(),
			};

			SortieRecord = new()
			{
				World = response.ApiData.ApiMapareaId,
				Map = response.ApiData.ApiMapinfoNo,
				FleetData = fleetData,
				MapData = mapData,
			};

			await Db.Sorties.AddAsync(SortieRecord);
		}

		if (SortieRecord is null)
		{
			// this should be all apis that are not related to a sortie
			// apis related to sortie are all api calls that happen between
			// "api_req_map/start" and "api_port/port"
			// can also happen if there's an error when parsing "api_req_map/start"
			return;
		}

		SortieRecord.ApiFiles.Add(requestFile);
		SortieRecord.ApiFiles.Add(responseFile);
	}

	private async Task ProcessExpeditionData(ApiFile requestFile, ApiFile responseFile)
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

		SortieFleet fleet = new()
		{
			Name = f.Name,
			Ships = f.MembersInstance
				.Where(s => s is not null)
				.Select(MakeSortieShip!)
				.ToList(),
		};

		ExpeditionRecord expedition = new()
		{
			Expedition = f.ExpeditionDestination,
			Fleet = fleet,
		};

		await Db.Expeditions.AddAsync(expedition);

		expedition.ApiFiles.Add(requestFile);
		expedition.ApiFiles.Add(responseFile);
	}

	private static SortieShip MakeSortieShip(IShipData s) => new()
	{
		Id = s.MasterShip.ShipId,
		DropId = s.MasterID,
		Level = s.Level,
		Condition = s.Condition,
		Kyouka = s.Kyouka.ToList(),
		Fuel = s.Fuel,
		Ammo = s.Ammo,
		Range = s.Range,
		Speed = s.Speed,
		EquipmentSlots = s.SlotInstance
			.Zip(s.Aircraft, (Equipment, AircraftCurrent) => (Equipment, AircraftCurrent))
			.Zip(s.MasterShip.Aircraft, (s, AircraftMax) => new SortieEquipmentSlot
			{
				Equipment = s.Equipment switch
				{
					{ } => new()
					{
						Id = s.Equipment.EquipmentId,
						Level = s.Equipment.Level,
						AircraftLevel = s.Equipment.AircraftLevel,
					},
					_ => null,
				},
				AircraftCurrent = s.AircraftCurrent,
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
	};
}
