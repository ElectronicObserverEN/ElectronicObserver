using System;
using System.Net.Http;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbAirDefenseSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbFriendFleetSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.PoiDbSubmission;

public class PoiDbSubmissionService
{
	private static string Version => $"七四式EN-{SoftwareInformation.VersionEnglish}";

	private KCDatabase KcDatabase { get; }
	private PoiDbQuestSubmissionService? QuestSubmissionService { get; set; }
	private PoiDbBattleSubmissionService? BattleSubmissionService { get; set; }
	private PoiDbFriendFleetSubmissionService? FriendFleetSubmissionService { get; set; }
	private PoiDbAirDefenseSubmissionService? AirDefenseSubmissionService { get; set; }
	private PoiDbRouteSubmissionService? RouteSubmissionService { get; set; }

	public PoiDbSubmissionService(KCDatabase kcDatabase)
	{
		KcDatabase = kcDatabase;

		Configuration.Instance.ConfigurationChanged += OnConfigurationChanged;

		OnConfigurationChanged();
	}

	private void OnConfigurationChanged()
	{
		// todo: config
		bool poiSubmissionEnabled = true;

		if (poiSubmissionEnabled)
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
		QuestSubmissionService ??= MakeQuestSubmissionService(Version, MakeHttpClient, LogError);
		BattleSubmissionService ??= MakeBattleSubmissionService(KcDatabase, Version, MakeHttpClient, LogError);
		FriendFleetSubmissionService ??= MakeFriendFleetSubmissionService(KcDatabase, Version, MakeHttpClient, LogError);
		AirDefenseSubmissionService ??= MakeAirDefenseSubmissionService(KcDatabase, Version, MakeHttpClient, LogError);
		RouteSubmissionService ??= MakeRouteSubmissionService(KcDatabase, Version, MakeHttpClient, LogError);
	}

	private void UnsubscribeFromApis()
	{
		if (QuestSubmissionService is not null)
		{
			UnsubscribeFromQuestApis(QuestSubmissionService);
			QuestSubmissionService = null;
		}

		if (BattleSubmissionService is not null)
		{
			UnsubscribeFromBattleApis(BattleSubmissionService);
			BattleSubmissionService = null;
		}

		if (FriendFleetSubmissionService is not null)
		{
			UnsubscribeFromFriendFleetApis(FriendFleetSubmissionService);
			FriendFleetSubmissionService = null;
		}

		if (AirDefenseSubmissionService is not null)
		{
			UnsubscribeFromAirDefenseApis(AirDefenseSubmissionService);
			AirDefenseSubmissionService = null;
		}

		if (RouteSubmissionService is not null)
		{
			UnsubscribeFromRouteApis(RouteSubmissionService);
			RouteSubmissionService = null;
		}
	}

	private static PoiDbQuestSubmissionService MakeQuestSubmissionService(string version,
		Func<HttpClient> makeHttpClient, Action<Exception> logError)
	{
		PoiDbQuestSubmissionService questSubmissionService = new(version, makeHttpClient, logError);

		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived += questSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiGetMember_QuestList.ResponseReceived += questSubmissionService.ApiGetMember_QuestListOnResponseReceived;

		return questSubmissionService;
	}

	private static void UnsubscribeFromQuestApis(PoiDbQuestSubmissionService questSubmissionService)
	{
		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived -= questSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiGetMember_QuestList.ResponseReceived -= questSubmissionService.ApiGetMember_QuestListOnResponseReceived;
	}

	private static PoiDbBattleSubmissionService MakeBattleSubmissionService(KCDatabase kcDatabase,
		string version, Func<HttpClient> makeHttpClient, Action<Exception> logError)
	{
		PoiDbBattleSubmissionService battleSubmissionService = new(kcDatabase, version, makeHttpClient, logError);

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived += battleSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived += battleSubmissionService.ApiReqMap_NextOnResponseReceived;

		APIObserver.Instance.ApiReqSortie_Battle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqBattleMidnight_SpMidnight.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_AirBattle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdAirBattle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_NightToDay.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdShooting.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_Battle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_SpMidnight.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_AirBattle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_BattleWater.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcBattle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattle.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdShooting.ResponseReceived += battleSubmissionService.ProcessFirstBattle;

		APIObserver.Instance.ApiReqBattleMidnight_Battle.ResponseReceived += battleSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += battleSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += battleSubmissionService.ProcessSecondBattle;

		APIObserver.Instance.ApiPort_Port.ResponseReceived += battleSubmissionService.ApiPort_Port_ResponseReceived;

		return battleSubmissionService;
	}

	private static void UnsubscribeFromBattleApis(PoiDbBattleSubmissionService battleSubmissionService)
	{
		APIObserver.Instance.ApiReqMap_Start.ResponseReceived -= battleSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived -= battleSubmissionService.ApiReqMap_NextOnResponseReceived;

		APIObserver.Instance.ApiReqSortie_Battle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqBattleMidnight_SpMidnight.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_AirBattle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdAirBattle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_NightToDay.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdShooting.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_Battle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_SpMidnight.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_AirBattle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_BattleWater.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdAirBattle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcBattle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcNightToDay.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattle.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattleWater.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdShooting.ResponseReceived -= battleSubmissionService.ProcessFirstBattle;

		APIObserver.Instance.ApiReqBattleMidnight_Battle.ResponseReceived -= battleSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_MidnightBattle.ResponseReceived -= battleSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived -= battleSubmissionService.ProcessSecondBattle;

		APIObserver.Instance.ApiPort_Port.ResponseReceived -= battleSubmissionService.ApiPort_Port_ResponseReceived;
	}

	private static PoiDbFriendFleetSubmissionService MakeFriendFleetSubmissionService(KCDatabase kcDatabase,
		string version, Func<HttpClient> makeHttpClient, Action<Exception> logError)
	{
		PoiDbFriendFleetSubmissionService friendFleetSubmissionService = new(kcDatabase, version, makeHttpClient, logError);

		APIObserver.Instance.ApiGetMember_MapInfo.ResponseReceived += friendFleetSubmissionService.ApiGetMember_MapInfo_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived += friendFleetSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived += friendFleetSubmissionService.ApiReqMap_NextOnResponseReceived;

		APIObserver.Instance.ApiReqSortie_Battle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqBattleMidnight_SpMidnight.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_AirBattle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdAirBattle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_NightToDay.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdShooting.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_Battle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_SpMidnight.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_AirBattle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_BattleWater.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcBattle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattle.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdShooting.ResponseReceived += friendFleetSubmissionService.ProcessFirstBattle;

		APIObserver.Instance.ApiReqBattleMidnight_Battle.ResponseReceived += friendFleetSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += friendFleetSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += friendFleetSubmissionService.ProcessSecondBattle;

		APIObserver.Instance.ApiPort_Port.ResponseReceived += friendFleetSubmissionService.ApiPort_Port_ResponseReceived;

		return friendFleetSubmissionService;
	}

	private static void UnsubscribeFromFriendFleetApis(PoiDbFriendFleetSubmissionService friendFleetSubmissionService)
	{
		APIObserver.Instance.ApiGetMember_MapInfo.ResponseReceived -= friendFleetSubmissionService.ApiGetMember_MapInfo_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived -= friendFleetSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived -= friendFleetSubmissionService.ApiReqMap_NextOnResponseReceived;

		APIObserver.Instance.ApiReqSortie_Battle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqBattleMidnight_SpMidnight.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_AirBattle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdAirBattle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_NightToDay.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqSortie_LdShooting.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_Battle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_SpMidnight.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_AirBattle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_BattleWater.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdAirBattle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcBattle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcNightToDay.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattle.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EachBattleWater.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;
		APIObserver.Instance.ApiReqCombinedBattle_LdShooting.ResponseReceived -= friendFleetSubmissionService.ProcessFirstBattle;

		APIObserver.Instance.ApiReqBattleMidnight_Battle.ResponseReceived -= friendFleetSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_MidnightBattle.ResponseReceived -= friendFleetSubmissionService.ProcessSecondBattle;
		APIObserver.Instance.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived -= friendFleetSubmissionService.ProcessSecondBattle;

		APIObserver.Instance.ApiPort_Port.ResponseReceived -= friendFleetSubmissionService.ApiPort_Port_ResponseReceived;
	}

	private static PoiDbAirDefenseSubmissionService MakeAirDefenseSubmissionService(KCDatabase kcDatabase,
		string version, Func<HttpClient> makeHttpClient, Action<Exception> logError)
	{
		PoiDbAirDefenseSubmissionService airDefenseSubmissionService = new(kcDatabase, version, makeHttpClient, logError);

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived += airDefenseSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived += airDefenseSubmissionService.ApiReqMap_NextOnResponseReceived;

		return airDefenseSubmissionService;
	}

	private static void UnsubscribeFromAirDefenseApis(PoiDbAirDefenseSubmissionService airDefenseSubmissionService)
	{
		APIObserver.Instance.ApiReqMap_Start.ResponseReceived -= airDefenseSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived -= airDefenseSubmissionService.ApiReqMap_NextOnResponseReceived;
	}

	private static PoiDbRouteSubmissionService MakeRouteSubmissionService(KCDatabase kcDatabase,
		string version, Func<HttpClient> makeHttpClient, Action<Exception> logError)
	{
		PoiDbRouteSubmissionService routeSubmissionService = new(kcDatabase, version, makeHttpClient, logError);

		APIObserver.Instance.ApiGetMember_MapInfo.ResponseReceived += routeSubmissionService.ApiGetMember_MapInfo_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived += routeSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived += routeSubmissionService.ApiReqMap_NextOnResponseReceived;

		APIObserver.Instance.ApiPort_Port.ResponseReceived += routeSubmissionService.ApiPort_Port_ResponseReceived;

		return routeSubmissionService;
	}

	private static void UnsubscribeFromRouteApis(PoiDbRouteSubmissionService routeSubmissionService)
	{
		APIObserver.Instance.ApiGetMember_MapInfo.ResponseReceived -= routeSubmissionService.ApiGetMember_MapInfo_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived -= routeSubmissionService.ApiReqMap_Start_ResponseReceived;

		APIObserver.Instance.ApiReqMap_Next.ResponseReceived -= routeSubmissionService.ApiReqMap_NextOnResponseReceived;

		APIObserver.Instance.ApiPort_Port.ResponseReceived -= routeSubmissionService.ApiPort_Port_ResponseReceived;
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
