using System;
using System.Net.Http;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.PoiDbSubmission;

public class PoiDbSubmissionService
{
	private static string Version => $"七四式EN-{SoftwareInformation.VersionEnglish}";
	private PoiDbQuestSubmissionService QuestSubmissionService { get; }
	private PoiDbBattleSubmissionService BattleSubmissionService { get; }

	public PoiDbSubmissionService(KCDatabase kcDatabase)
	{
		QuestSubmissionService = new(Version, MakeHttpClient, LogError);
		BattleSubmissionService = new(kcDatabase, Version, MakeHttpClient, LogError);

		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived += QuestSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiGetMember_QuestList.ResponseReceived += QuestSubmissionService.ApiGetMember_QuestListOnResponseReceived;

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived += BattleSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived += BattleSubmissionService.ApiReqMap_NextOnResponseReceived;
		
		APIObserver.Instance.ApiReqSortie_Battle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqBattleMidnight_SpMidnight.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_AirBattle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdAirBattle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_NightToDay.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdShooting.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_Battle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_SpMidnight.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_AirBattle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_BattleWater.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcBattle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattle.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdShooting.ResponseReceived += BattleSubmissionService.ProcessFirstBattle;
		
		APIObserver.Instance.ApiReqBattleMidnight_Battle.ResponseReceived += BattleSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += BattleSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += BattleSubmissionService.ProcessSecondBattle;

		APIObserver.Instance.ApiPort_Port.ResponseReceived += BattleSubmissionService.ApiPort_Port_ResponseReceived;
	}

	private static HttpClient MakeHttpClient() => new()
	{
		BaseAddress = new("http://report2.kcwiki.org:17027/"),
	};

	private static void LogError(Exception e)
	{
		Logger.Add(2, "PoiDB error", e);
	}
}
