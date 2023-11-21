using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class DataAndTranslationIssueReporter
{
	private WrongUpgradesIssueReporter WrongUpgradesIssueReporter { get; }
	private FitBonusIssueReporter FitBonusIssueReporter { get; }

	public DataAndTranslationIssueReporter()
	{
		ElectronicObserverApiService api = Ioc.Default.GetRequiredService<ElectronicObserverApiService>();
		WrongUpgradesIssueReporter = new(api);
		FitBonusIssueReporter = new(api);

		SubscribeToAPI();
	}

	private void SubscribeToAPI()
	{
		APIObserver api = APIObserver.Instance;

		api.ApiReqKousyou_RemodelSlotList.ResponseReceived += WrongUpgradesIssueReporter.ProcessUpgradeList;
	}
}
