﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Material;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbFriendFleetSubmission;

public class PoiDbFriendFleetSubmissionService(
	KCDatabase kcDatabase,
	string version,
	PoiHttpClient poiHttpClient,
	Action<Exception> logError)
{
	private KCDatabase KcDatabase { get; } = kcDatabase;
	private string Version { get; } = version;
	private PoiHttpClient PoiHttpClient { get; } = poiHttpClient;
	private Action<Exception> LogError { get; } = logError;

	// don't need to clear these
	private int TorchesBefore { get; set; }
	private int TorchesAfter { get; set; }
	private FriendFleetRequestFlag? FriendFleetRequestFlag { get; set; }
	private FriendFleetRequestType? FriendFleetRequestType { get; set; }
	private List<ApiMapInfo>? ApiMapInfo { get; set; }

	private int? EventDifficulty { get; set; }
	private int? World { get; set; }
	private int? Map { get; set; }
	private int? Cell { get; set; }
	private List<JsonNode>? Deck1 { get; set; }
	private List<JsonNode>? Deck2 { get; set; }
	private List<int>? EscapeList { get; set; }
	private JsonNode? ApiFriendlyInfo { get; set; }
	private JsonNode? ApiFriendlyBattle { get; set; }
	private int? PlayerFormation { get; set; }
	private Dictionary<string, JsonNode?>? Enemy { get; set; }

	public void ApiGetMember_MapInfo_ResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiGetMemberMapinfoResponse response = JsonSerializer.Deserialize<ApiGetMemberMapinfoResponse>(json)!;

		ApiMapInfo = response.ApiMapInfo;
	}

	public void ApiReqMap_Start_ResponseReceived(string apiname, dynamic data)
	{
		ApiReqMapStartResponse response = JsonSerializer.Deserialize<ApiReqMapStartResponse>(data.ToString());

		World = response.ApiMapareaId;
		Map = response.ApiMapinfoNo;
		Cell = response.ApiNo;
		EventDifficulty = KcDatabase.Battle.Compass.MapInfo.EventDifficulty;
	}

	public void ProcessFirstBattle(string apiName, dynamic data)
	{
		string json = data.ToString();

		Enemy = JsonNode.Parse(json)
			?.AsObject()
			.Where(kvp => RelevantKey(kvp.Key))
			.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		ProcessFriendFleet(json);

		static bool RelevantKey(string key) => key is
			"api_ship_ke" or
			"api_ship_ke_combined" or
			"api_e_nowhps" or
			"api_e_nowhps_combined" or
			"api_xal01";
	}

	public void ProcessSecondBattle(string apiName, dynamic data)
	{
		string json = data.ToString();

		ProcessFriendFleet(json);
	}

	private void ProcessFriendFleet(string battleData)
	{
		JsonNode? battle = JsonNode.Parse(battleData);

		if (battle is null) return;
		if (battle["api_friendly_info"] is not JsonNode friendlyInfo) return;
		if (battle["api_friendly_battle"] is not JsonNode friendlyBattle) return;
		if (battle["api_formation"] is not JsonArray formation) return;
		if (formation.GetValues<int?>().FirstOrDefault() is not int playerFormation) return;

		ApiFriendlyInfo = friendlyInfo;
		ApiFriendlyBattle = friendlyBattle;
		PlayerFormation = playerFormation;

		FleetData? fleet = KcDatabase.Fleet.Fleets.Values.FirstOrDefault(f => f.IsInSortie);

		if (fleet is null) return;

		Deck1 = fleet.MembersInstance!
			.OfType<ShipData>()
			.Select(Extensions.MakeShip)
			.ToList();

		EscapeList = fleet.EscapedShipList.ToList();

		bool isCombinedFleetSortie = fleet.FleetID is 1 &&
			KcDatabase.Fleet.CombinedFlag is not FleetType.Single;

		if (isCombinedFleetSortie)
		{
			FleetData escortFleet = KcDatabase.Fleet.Fleets[2];

			Deck2 = escortFleet.MembersInstance!
				.OfType<ShipData>()
				.Select(Extensions.MakeShip)
				.ToList();

			EscapeList.AddRange(escortFleet.EscapedShipList);
		}
	}

	public void ApiReqMap_NextOnResponseReceived(string apiname, dynamic data)
	{
		ApiReqMapNextResponse response = JsonSerializer.Deserialize<ApiReqMapNextResponse>(data.ToString());

		Cell = response.ApiNo;
	}

	public void ApiPort_Port_ResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiPortPortResponse response = JsonSerializer.Deserialize<ApiPortPortResponse>(json)!;

		TorchesBefore = TorchesAfter;
		TorchesAfter = response.ApiMaterial
			.FirstOrDefault(m => m.ApiId is ApiGetMemberMaterialId.InstantConstruction)
			?.ApiValue ?? 0;
		FriendFleetRequestFlag = response.ApiFriendlySetting?.ApiRequestFlag;
		FriendFleetRequestType = response.ApiFriendlySetting?.ApiRequestType;

		if (ApiFriendlyBattle is not null)
		{
			SubmitData();
		}

		ClearState();
	}

	private void ClearState()
	{
		EventDifficulty = null;
		PlayerFormation = null;
		World = null;
		Map = null;
		Cell = null;
		Deck1 = null;
		Deck2 = null;
		EscapeList = null;
		ApiFriendlyInfo = null;
		ApiFriendlyBattle = null;
		Enemy = null;
	}

	[SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
	private void SubmitData()
	{
		if (EscapeList is null) return;
		if (Deck1 is null) return;
		if (ApiFriendlyInfo is null) return;
		if (ApiFriendlyBattle is null) return;
		if (Enemy is null) return;
		if (World is not int world) return;
		if (Map is not int map) return;
		if (Cell is not int cell) return;
		if (EventDifficulty is not int eventDifficulty) return;
		if (PlayerFormation is not int playerFormation) return;
		if (FriendFleetRequestType is not { } friendFleetRequestType) return;
		if (FriendFleetRequestFlag is not { } friendFleetRequestFlag) return;
		if (ApiMapInfo?.FirstOrDefault(m => m.ApiId == 10 * world + map) is not { } mapInfo) return;

		try
		{
			PoiDbFriendFleetSubmissionData submissionData = new()
			{
				Body = new()
				{
					World = world,
					Map = map,
					Cell = cell,
					MapLevel = eventDifficulty,
					FriendlyStatus = new()
					{
						FirenumBefore = TorchesBefore,
						Firenum = TorchesAfter,
						Flag = friendFleetRequestFlag,
						Type = friendFleetRequestType,
						NowMaphp = mapInfo.ApiEventmap?.ApiNowMaphp ?? 0,
						MaxMaphp = mapInfo.ApiEventmap?.ApiNowMaphp ?? 0,
						Version = Version,
					},
					ApiFriendlyBattle = ApiFriendlyBattle,
					EscapeList = EscapeList,
					Formation = playerFormation,
					Enemy = Enemy,
					Deck1 = Deck1,
					Deck2 = Deck2,
					Version = Version,
				},
			};

			Dictionary<string, Dictionary<string, JsonNode?>> dictionarySubmission = JsonSerializer
				.Deserialize<Dictionary<string, Dictionary<string, JsonNode?>>>(JsonSerializer.Serialize(submissionData))!;

			foreach ((string key, JsonNode? value) in ApiFriendlyInfo.AsObject())
			{
				dictionarySubmission["body"].Add(key, value);
			}

			Task.Run(async () =>
			{
				try
				{
					await PoiHttpClient.FriendFleet(dictionarySubmission);
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
}
