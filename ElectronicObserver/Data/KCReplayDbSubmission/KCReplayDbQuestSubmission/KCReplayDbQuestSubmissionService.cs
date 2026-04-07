using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.KCReplayDbSubmission.KCReplayDbQuestSubmission;

public class KCReplayDbQuestSubmissionService(
	KCReplayDbHttpClient kcReplayDbHttpClient,
	Action<Exception> logError)
{
	private KCReplayDbHttpClient KCReplayDbHttpClient { get; } = kcReplayDbHttpClient;
	private Action<Exception> LogError { get; } = logError;

	private dynamic? LastRawQuestList { get; set; }
	private List<int> OldQuestIds { get; set; } = [];
	private List<int>? NewQuestIds { get; set; }
	private List<JsonNode> OldQuestList { get; set; } = [];
	private List<JsonNode>? NewQuestList { get; set; }

	public void ApiGetMember_QuestListOnResponseReceived(string apiName, dynamic data)
	{
		LastRawQuestList = data;

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
		}
	}

	private void SubmitData()
	{
		if (NewQuestIds is null) return;
		if (NewQuestList is null) return;

		try
		{
			List<int> newlyUnlockedQuestIds = NewQuestIds
				.Where(i => !OldQuestIds.Contains(i))
				.ToList();

			if (newlyUnlockedQuestIds.Count is 0)
			{
				return;
			}

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
					int apiNo => newlyUnlockedQuestIds.Contains(apiNo),
					_ => false,
				})
				.Select(t => t.Node)
				.ToList();

			OldQuestIds.AddRange(NewQuestIds);
			OldQuestList.AddRange(NewQuestList);

			KCReplayDbQuestSubmissionData submissionData = new()
			{
				QuestsData = newQuestData,
			};

			Task.Run(async () =>
			{
				try
				{
					await KCReplayDbHttpClient.Quest(submissionData);
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
