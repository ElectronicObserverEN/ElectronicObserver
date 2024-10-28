using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbAirDefenseSubmission;

public class PoiDbAirDefenseSubmissionService(
	KCDatabase kcDatabase,
	string version,
	Func<HttpClient> makeHttpClient,
	Action<Exception> logError)
{
	private KCDatabase KcDatabase { get; } = kcDatabase;
	private string Version { get; } = version;
	private Func<HttpClient> MakeHttpClient { get; } = makeHttpClient;
	private Action<Exception> LogError { get; } = logError;

	private int? EventDifficulty { get; set; }
	private int? World { get; set; }
	private int? Map { get; set; }
	private int? Cell { get; set; }

	public void ApiReqMap_Start_ResponseReceived(string apiname, dynamic data)
	{
		ApiReqMapStartResponse response = JsonSerializer.Deserialize<ApiReqMapStartResponse>(data.ToString());

		World = response.ApiMapareaId;
		Map = response.ApiMapinfoNo;
		Cell = response.ApiNo;
		EventDifficulty = KcDatabase.Battle.Compass.MapInfo.EventDifficulty;
	}

	public void ApiReqMap_NextOnResponseReceived(string apiname, dynamic data)
	{
		string json = data.ToString();
		ApiReqMapNextResponse response = JsonSerializer.Deserialize<ApiReqMapNextResponse>(json)!;

		Cell = response.ApiNo;

		ProcessAirDefense(json);
	}

	private void ProcessAirDefense(string json)
	{
		JsonNode? apiDestructionBattle = JsonNode.Parse(json)
			?["api_destruction_battle"];

		if (apiDestructionBattle is not null)
		{
			SubmitData(apiDestructionBattle);
		}
	}

	private void ClearState()
	{
		EventDifficulty = null;
		World = null;
		Map = null;
		Cell = null;
	}

	private void SubmitData(JsonNode apiDestructionBattle)
	{
		if (World is not int world) return;
		if (Map is not int map) return;
		if (Cell is not int cell) return;
		if (EventDifficulty is not int eventDifficulty) return;

		try
		{
			PoiDbAirDefenseSubmission submission = new()
			{
				World = world,
				Map = map,
				Cell = cell,
				MapLevel = eventDifficulty,
				Version = Version,
			};

			Dictionary<string, JsonNode?> dictionarySubmission = JsonSerializer
				.Deserialize<Dictionary<string, JsonNode?>>(JsonSerializer.Serialize(submission))!;

			foreach ((string key, JsonNode? value) in apiDestructionBattle.AsObject())
			{
				dictionarySubmission.Add(key, value);
			}

			Task.Run(async () =>
			{
				using HttpClient client = MakeHttpClient();

				try
				{
					_ = await client.PostAsJsonAsync("/air_base_attack", dictionarySubmission);
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
