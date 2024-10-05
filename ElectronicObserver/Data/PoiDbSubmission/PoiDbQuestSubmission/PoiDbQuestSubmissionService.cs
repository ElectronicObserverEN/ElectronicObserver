using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;

public class PoiDbQuestSubmissionService(
	string version,
	Func<HttpClient> makeHttpClient,
	Action<Exception> logError)
{
	private string Version { get; } = version;
	private Func<HttpClient> MakeHttpClient { get; } = makeHttpClient;
	private Action<Exception> LogError { get; } = logError;

	private dynamic? LastRawQuestList { get; set; }
	private int? CompletedQuestId { get; set; }
	private List<int>? OldQuestIds { get; set; }
	private List<int>? NewQuestIds { get; set; }
	private List<JsonNode>? OldQuestList { get; set; }
	private List<JsonNode>? NewQuestList { get; set; }

	private void ClearState()
	{
		CompletedQuestId = null;
		OldQuestIds = null;
		NewQuestIds = null;
		OldQuestList = null;
		NewQuestList = null;
	}

	public void ApiReqQuest_ClearItemGetOnRequestReceived(string apiName, dynamic data)
	{
		if (LastRawQuestList is null) return;

		ClearState();

		try
		{
			CompletedQuestId = int.Parse(data["api_quest_id"]);

			OldQuestList = QuestsFromRawList((string)LastRawQuestList.ToString());

			OldQuestIds = OldQuestList
				?.Select(n => n["api_no"])
				.Select(n => n?.GetValueKind() switch
				{
					JsonValueKind.Number => n.GetValue<int>(),
					_ => (int?)null,
				})
				.OfType<int>()
				.Where(i => i != CompletedQuestId)
				.ToList();
		}
		catch (Exception e)
		{
			LogError(e);
			ClearState();
		}
	}

	public void ApiGetMember_QuestListOnResponseReceived(string apiName, dynamic data)
	{
		LastRawQuestList = data;

		if (CompletedQuestId is null) return;

		try
		{
			NewQuestList = QuestsFromRawList((string?)LastRawQuestList?.ToString());

			NewQuestIds = NewQuestList
				?.Select(n => n["api_no"])
				.Select(n => n?.GetValueKind() switch
				{
					JsonValueKind.Number => n.GetValue<int>(),
					_ => (int?)null,
				})
				.OfType<int>()
				.ToList();

			SubmitData();
		}
		catch (Exception e)
		{
			LogError(e);
			ClearState();
		}
	}

	private void SubmitData()
	{
		if (OldQuestIds is null) return;
		if (NewQuestIds is null) return;
		if (OldQuestList is null) return;
		if (NewQuestList is null) return;
		if (CompletedQuestId is not int completedQuestId) return;

		try
		{
			List<int> newlyUnlockedQuestIds = NewQuestIds
				.Where(i => !OldQuestIds.Contains(i))
				.ToList();

			if (!newlyUnlockedQuestIds.Any())
			{
				ClearState();
				return;
			}

			List<int> questDataIds = [completedQuestId, .. newlyUnlockedQuestIds];

			List<JsonNode> newQuestData = OldQuestList
				.Concat(NewQuestList)
				.Select(n => (Node: n, ApiNo: n["api_no"]?.GetValueKind() switch
				{
					JsonValueKind.Number => n["api_no"]?.GetValue<int>(),
					_ => null,
				}))
				.DistinctBy(t => t.ApiNo)
				.Where(t => t.ApiNo switch
				{
					int apiNo => questDataIds.Contains(apiNo),
					_ => false,
				})
				.Select(t => t.Node)
				.ToList();

			PoiDbQuestSubmission submission = new()
			{
				Form = new()
				{
					CompletedQuestId = completedQuestId,
					NewQuestIds = newlyUnlockedQuestIds,
					NewQuestData = newQuestData,
					Version = Version,
				}
			};

			Task.Run(async () =>
			{
				using HttpClient client = MakeHttpClient();

				try
				{
					_ = await client.PostAsJsonAsync("/quest", submission);
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

	private List<JsonNode>? QuestsFromRawList(string? rawData)
	{
		if (rawData is null) return null;

		try
		{
			return JsonNode
				.Parse(rawData)?["api_list"]
				?.AsArray()
				.OfType<JsonNode>()
				.ToList();
		}
		catch (Exception e)
		{
			LogError(e);
		}

		return null;
	}
}
