using System;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class DataAndTranslationIssueReporter
{
	private WrongUpgradesIssueReporter WrongUpgradesIssueReporter { get; }
	private WrongUpgradesCostIssueReporter WrongUpgradesCostIssueReporter { get; }
	private FitBonusIssueReporter FitBonusIssueReporter { get; }
	private SoftwareIssueReporter SoftwareIssueReporter { get; }

	public DataAndTranslationIssueReporter(WrongUpgradesIssueReporter wrongUpgrades, WrongUpgradesCostIssueReporter wrongCosts, FitBonusIssueReporter fitBonuses, SoftwareIssueReporter softwareIssues)
	{
		WrongUpgradesIssueReporter = wrongUpgrades;
		WrongUpgradesCostIssueReporter = wrongCosts;
		FitBonusIssueReporter = fitBonuses;
		SoftwareIssueReporter = softwareIssues;

		SubscribeToApi();
	}

	private void SubscribeToApi()
	{
		APIObserver api = APIObserver.Instance;

		// Upgrade ships / days
		api.ApiReqKousyou_RemodelSlotList.ResponseReceived += WrongUpgradesIssueReporter.ProcessUpgradeList;

		// Upgrade costs
		api.ApiReqKousyou_RemodelSlotList.ResponseReceived += WrongUpgradesCostIssueReporter.ProcessUpgradeList;
		api.ApiReqKousyou_RemodelSlotListDetail.RequestReceived += WrongUpgradesCostIssueReporter.ProcessUpgradeCostRequest;
		api.ApiReqKousyou_RemodelSlotListDetail.ResponseReceived += WrongUpgradesCostIssueReporter.ProcessUpgradeCostResponse;

		// fixme : currently report false positive (and issue on remodel even tho there's code to prevent that)
		//api.ApiGetMember_Ship3.ResponseReceived += FitBonusIssueReporter.ProcessShipDataChanged;

		// App crashes
		AppDomain.CurrentDomain.UnhandledException += SoftwareIssueReporter.ProcessException;
	}
}
