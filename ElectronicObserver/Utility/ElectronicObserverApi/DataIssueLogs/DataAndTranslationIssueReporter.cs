using ElectronicObserver.Observer;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class DataAndTranslationIssueReporter
{
	private WrongUpgradesIssueReporter WrongUpgradesIssueReporter { get; }

	public DataAndTranslationIssueReporter()
	{
		WrongUpgradesIssueReporter = new();

		SubscribeToAPI();
	}

	private void SubscribeToAPI()
	{
		APIObserver api = APIObserver.Instance;

		api.ApiReqKousyou_RemodelSlotList.ResponseReceived += WrongUpgradesIssueReporter.ProcessUpgradeList;
	}
}
