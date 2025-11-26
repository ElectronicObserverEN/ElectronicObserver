using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.KCReplayDbSubmission.KCReplayDbQuestSubmission;

public class KCReplayDbQuestItemsSubmissionService(
	KCReplayDbHttpClient kcReplayDbHttpClient,
	Action<Exception> logError)
{
	private KCReplayDbHttpClient KCReplayDbHttpClient { get; } = kcReplayDbHttpClient;
	private Action<Exception> LogError { get; } = logError;

	private int? QuestId { get; set; }
	private List<int>? ItemSelection { get; set; }
	private JsonNode? QuestReward { get; set; }

	private void ClearState()
	{
		QuestId = null;
		ItemSelection = null;
		QuestReward = null;
	}

	public void ApiReqQuest_ClearItemGetOnRequestReceived(string apiName, dynamic data)
	{
		ClearState();

		try
		{
			QuestId = int.Parse(data["api_quest_id"]);

			ItemSelection = [];

			if (data.ContainsKey("api_select_no"))
			{
				ItemSelection.Add(int.Parse(data["api_select_no"]));
			}

			if (data.ContainsKey("api_select_no2"))
			{
				ItemSelection.Add(int.Parse(data["api_select_no2"]));
			}
		}
		catch (Exception e)
		{
			LogError(e);
			ClearState();
		}
	}

	public void ApiReqQuest_ClearItemGetOnResponseReceived(string apiName, dynamic data)
	{
		try
		{
			if (data?.ToString() is not string jsonString)
			{
				ClearState();
				return;
			}

			QuestReward = JsonNode.Parse(jsonString);

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
		if (QuestId is not { } questId) return;
		if (ItemSelection is null) return;
		if (QuestReward is null) return;

		try
		{
			KCReplayDbQuestItemsSubmissionData submissionData = new()
			{
				ItemSelection = ItemSelection.Count > 0 ? ItemSelection : null,
				QuestId = questId,
				RewardData = QuestReward,
			};

			Task.Run(async () =>
			{
				try
				{
					await KCReplayDbHttpClient.QuestItems(submissionData);
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
