using System;
using System.Net.Http;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.PoiDbSubmission;

public class PoiDbSubmissionService
{
	private static string Version => $"七四式EN-{SoftwareInformation.VersionEnglish}";
	private PoiDbQuestSubmissionService QuestSubmissionService { get; }

	public PoiDbSubmissionService()
	{
		QuestSubmissionService = new(Version, MakeHttpClient, LogError);

		APIObserver.Instance.ApiReqQuest_ClearItemGet.RequestReceived += QuestSubmissionService.ApiReqQuest_ClearItemGetOnRequestReceived;
		APIObserver.Instance.ApiGetMember_QuestList.ResponseReceived += QuestSubmissionService.ApiGetMember_QuestListOnResponseReceived;
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
