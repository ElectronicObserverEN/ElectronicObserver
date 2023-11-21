using ElectronicObserver.Observer;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class DataAndTranslationIssueReporter
{
	private WrongUpgradesIssueReporter WrongUpgradesIssueReporter { get; }
	private FitBonusIssueReporter FitBonusIssueReporter { get; }

	public DataAndTranslationIssueReporter(ElectronicObserverApiService api)
	{
		WrongUpgradesIssueReporter = new(api);
		FitBonusIssueReporter = new(api);

		SubscribeToApi();
	}

	private void SubscribeToApi()
	{
		APIObserver api = APIObserver.Instance;

		api.ApiReqKousyou_RemodelSlotList.ResponseReceived += WrongUpgradesIssueReporter.ProcessUpgradeList;

		api.ApiGetMember_Ship3.ResponseReceived += FitBonusIssueReporter.ProcessShipDataChanged;
	}
}
