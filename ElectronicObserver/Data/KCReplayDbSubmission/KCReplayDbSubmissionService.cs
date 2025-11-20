using System;
using ElectronicObserver.Data.KCReplayDbSubmission.KCReplayDbQuestSubmission;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.KCReplayDbSubmission;

public class KCReplayDbSubmissionService
{
	private KCReplayDbHttpClient KcReplayDbHttpClient { get; } = new();
	private KCReplayDbQuestSubmissionService? QuestSubmissionService { get; set; }
	private KCReplayDbQuestItemsSubmissionService? QuestItemsSubmissionService { get; set; }

	public KCReplayDbSubmissionService()
	{
		Configuration.Instance.ConfigurationChanged += OnConfigurationChanged;

		OnConfigurationChanged();
	}

	private void OnConfigurationChanged()
	{
		if (Configuration.Config.DataSubmission.SendDataToKancolleReplayDb)
		{
			SubscribeToApis();
		}
		else
		{
			UnsubscribeFromApis();
		}
	}

	private void SubscribeToApis()
	{
		QuestSubmissionService ??= MakeQuestSubmissionService(KcReplayDbHttpClient, LogError);
		QuestItemsSubmissionService ??= MakeQuestItemsSubmissionService(KcReplayDbHttpClient, LogError);
	}

	private void UnsubscribeFromApis()
	{
		if (QuestSubmissionService is not null)
		{
			UnsubscribeFromQuestApis(QuestSubmissionService);
			QuestSubmissionService = null;
		}

		if (QuestItemsSubmissionService is not null)
		{
			UnsubscribeFromQuestItemsApis(QuestItemsSubmissionService);
			QuestItemsSubmissionService = null;
		}
	}

	private static KCReplayDbQuestSubmissionService MakeQuestSubmissionService(KCReplayDbHttpClient httpClient, Action<Exception> logError)
	{
		KCReplayDbQuestSubmissionService questSubmissionService = new(httpClient, logError);

		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived += questSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiGetMember_QuestList.ResponseReceived += questSubmissionService.ApiGetMember_QuestListOnResponseReceived;

		return questSubmissionService;
	}

	private static void UnsubscribeFromQuestApis(KCReplayDbQuestSubmissionService questSubmissionService)
	{
		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived -= questSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiGetMember_QuestList.ResponseReceived -= questSubmissionService.ApiGetMember_QuestListOnResponseReceived;
	}

	private static KCReplayDbQuestItemsSubmissionService MakeQuestItemsSubmissionService(KCReplayDbHttpClient httpClient, Action<Exception> logError)
	{
		KCReplayDbQuestItemsSubmissionService questSubmissionService = new(httpClient, logError);

		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived += questSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived += questSubmissionService.ApiReqQuest_ClearItemGetOnResponseReceived;

		return questSubmissionService;
	}

	private static void UnsubscribeFromQuestItemsApis(KCReplayDbQuestItemsSubmissionService questSubmissionService)
	{
		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived -= questSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiReqQuest_ClearItemGet.ResponseReceived -= questSubmissionService.ApiReqQuest_ClearItemGetOnResponseReceived;
	}

	private static void LogError(Exception e)
	{
		Logger.Add(2, KCReplayDbSubmissionResources.Error, e);
	}
}
