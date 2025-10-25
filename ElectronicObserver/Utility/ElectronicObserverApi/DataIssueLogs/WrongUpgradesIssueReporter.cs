using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.ElectronicObserverApi.Models;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class WrongUpgradesIssueReporter(ElectronicObserverApiService api)
{
	public void ProcessUpgradeList(string _, dynamic data)
	{
		if (!api.IsServerAvailable) return;

		DayOfWeek day = DateTimeHelper.GetJapanStandardTimeNow().DayOfWeek;

		// if no helper => ignore
		int helperId = KCDatabase.Instance.Fleet.Fleets[1].Members[1];
		if (helperId <= 0) return;
		IShipData helper = KCDatabase.Instance.Ships[helperId];

		List<APIReqKousyouRemodelSlotlistResponse>? parsedResponse = ParseResponse(data);

		List<EquipmentUpgradeDataModel> expectedUpgrades = EquipmentsThatCanBeUpgradedByCurrentHelper(helper.MasterShip, day);

		if (CheckForIssue(parsedResponse, expectedUpgrades))
		{
			EquipmentUpgradeIssueModel report = new()
			{
				DataVersion = SoftwareUpdater.CurrentVersion.EquipmentUpgrades,
				SoftwareVersion = SoftwareInformation.VersionEnglish,
				SoftwareDataSource = Configuration.Config.Control.UpdateRepoURL.ToString(),

				ActualUpgrades = parsedResponse.Select(apiData => apiData.ApiSlotId).ToList(),
				ExpectedUpgrades = expectedUpgrades.Select(upgrade => upgrade.EquipmentId).ToList(),
				Day = day,
				HelperId = (int)helper.MasterShip.ShipId,
			};

#pragma warning disable CS4014
			api.PostJson("EquipmentUpgradeIssues", report);
#pragma warning restore CS4014
		}
	}

	/// <summary>
	/// Checks for issues
	/// </summary>
	/// <returns>true if an issue is detected</returns>
	private bool CheckForIssue(List<APIReqKousyouRemodelSlotlistResponse> actualUpgrades, List<EquipmentUpgradeDataModel> expectedUpgrades)
	{
		// Check data
		if (actualUpgrades.Any(actualUpgrade => expectedUpgrades.All(upgSaved => upgSaved.EquipmentId != actualUpgrade.ApiSlotId) && !IsBaseUpgradeEquipment((EquipmentId)actualUpgrade.ApiSlotId)))
		{
			return true;
		}

		return expectedUpgrades.Any(expectedUpgrade => actualUpgrades.All(upgApi => expectedUpgrade.EquipmentId != upgApi.ApiSlotId));
	}

	private List<APIReqKousyouRemodelSlotlistResponse>? ParseResponse(dynamic data)
	{
		if (!data.IsArray) return null;

		return JsonSerializer.Deserialize<List<APIReqKousyouRemodelSlotlistResponse>>(data.ToString());
	}

	private List<EquipmentUpgradeDataModel> EquipmentsThatCanBeUpgradedByCurrentHelper(IShipDataMaster helper, DayOfWeek day)
	{
		KCDatabase db = KCDatabase.Instance;

		return helper.CanUpgradeEquipments(day,
				db.Translation.EquipmentUpgrade.UpgradeList)
			.ToList();
	}
	
	private bool IsBaseUpgradeEquipment(EquipmentId equipmentId) => equipmentId is
		EquipmentId.MainGunSmall_12_7cmTwinGun or 
		EquipmentId.MainGunMedium_14cmSingleGun or 
		EquipmentId.Torpedo_61cmQuadrupleTorpedo or 
		EquipmentId.DepthCharge_Type94DepthChargeProjector;
}
