using System;
using System.Net;
using System.Net.Http;
using ElectronicObserver.Observer;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.KanlogSubmission;

public class KanlogSubmissionService
{
	private const string BaseUrl = "https://api.kanlog.info";
	private static string Version =>
#if DEBUG
		$"七四式EN-DEBUG-{SoftwareInformation.VersionEnglish}";
#else
		$"七四式EN-{SoftwareInformation.VersionEnglish}";
#endif

	private KCDatabase KcDatabase { get; }
	private ServerManager ServerManager { get; }
	private KanlogHttpClient KanlogClient { get; }

	private KanlogDropSubmissionService? DropSubmissionService { get; set; }

	public KanlogSubmissionService(KCDatabase kcDatabase, ServerManager serverManager)
	{
		KcDatabase = kcDatabase;
		ServerManager = serverManager;
		KanlogClient = new KanlogHttpClient(BaseUrl, new());

		Configuration.Instance.ConfigurationChanged += OnConfigurationChanged;
		ServerManager.CurrentServerChanged += (_, _) => OnConfigurationChanged();

		OnConfigurationChanged();
	}

	private void OnConfigurationChanged()
	{
		if (Configuration.Config.DataSubmission.SendDataToKanlog && ServerManager.CurrentServer is not null)
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
		if (DropSubmissionService is null)
		{
			DropSubmissionService = new(KcDatabase, Version, KanlogClient, LogError);

			APIObserver.Instance.ApiReqMap_Start.ResponseReceived += DropSubmissionService.ApiReqMap_Start_ResponseReceived;

			APIObserver.Instance.ApiReqMap_Next.ResponseReceived += DropSubmissionService.ApiReqMap_NextOnResponseReceived;

			APIObserver.Instance.ApiReqSortie_BattleResult.ResponseReceived += DropSubmissionService.BattleResult_ResponseReceived;
			APIObserver.Instance.ApiReqCombinedBattle_BattleResult.ResponseReceived += DropSubmissionService.BattleResult_ResponseReceived;
		}
	}

	private void UnsubscribeFromApis()
	{
		if (DropSubmissionService is not null)
		{
			APIObserver.Instance.ApiReqMap_Start.ResponseReceived -= DropSubmissionService.ApiReqMap_Start_ResponseReceived;

			APIObserver.Instance.ApiReqMap_Next.ResponseReceived -= DropSubmissionService.ApiReqMap_NextOnResponseReceived;

			APIObserver.Instance.ApiReqSortie_BattleResult.ResponseReceived -= DropSubmissionService.BattleResult_ResponseReceived;
			APIObserver.Instance.ApiReqCombinedBattle_BattleResult.ResponseReceived -= DropSubmissionService.BattleResult_ResponseReceived;
			
			DropSubmissionService = null;
		}
	}

	private void LogError(Exception e)
	{
		if (e is HttpRequestException { StatusCode: >= HttpStatusCode.InternalServerError })
		{
			Logger.Add(1, KanlogSubmissionResources.Error, e);
		}
		else
		{
			Logger.Add(2, KanlogSubmissionResources.Error, e);
		}
	}
}
