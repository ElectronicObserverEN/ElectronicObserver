using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class WrongUpgradesIssueReporter
{
	private ElectronicObserverApiService? Api { get; set; }

	public WrongUpgradesIssueReporter()
	{
	}

	public void ProcessUpgradeList(string _, dynamic data)
	{
		ElectronicObserverApiService api = Ioc.Default.GetRequiredService<ElectronicObserverApiService>();

		// if no helper => ignore
		int helperId = KCDatabase.Instance.Fleet.Fleets[1].Members[2];
		if (helperId <= 0) return;
		IShipData helper = KCDatabase.Instance.Ships[helperId];

		List<APIReqKousyouRemodelSlotlistResponse>? parsedResponse = ParseResponse(data);

		List<EquipmentUpgradeDataModel> expectedUpgrades = EquipmentsThatCanBeUpgradedByCurrentHelper(helper.MasterShip);

		if (CheckForIssue(parsedResponse, expectedUpgrades))
		{
			EquipmentUpgradeIssueModel report = new()
			{
				DataVersion = SoftwareUpdater.CurrentVersion.EquipmentUpgrades,
				ActualUpgrades = parsedResponse.Select(apiData => apiData.ApiSlotId).ToList(),
				ExpectedUpgrades = expectedUpgrades.Select(upgrade => upgrade.EquipmentId).ToList(),
				Day = DateTimeHelper.GetJapanStandardTimeNow().DayOfWeek,
				SoftwareVersion = SoftwareInformation.VersionEnglish,
				HelperId = (int)helper.MasterShip.ShipId
			};

			api.PostJson("EquipmentUpgradeIssues", report);
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

	private List<EquipmentUpgradeDataModel> EquipmentsThatCanBeUpgradedByCurrentHelper(IShipDataMaster helper)
	{
		KCDatabase db = KCDatabase.Instance;

		return helper.CanUpgradeEquipments(DateTimeHelper.GetJapanStandardTimeNow().DayOfWeek,
				db.Translation.EquipmentUpgrade.UpgradeList)
			.ToList();
	}
	
	private bool IsBaseUpgradeEquipment(EquipmentId equipmentId) => equipmentId switch
	{
		EquipmentId.MainGunSmall_12_7cmTwinGun
			or EquipmentId.MainGunMedium_14cmSingleGun
			or EquipmentId.Torpedo_61cmQuadrupleTorpedo
			or EquipmentId.DepthCharge_Type94DepthChargeProjector
			=> true,
		_ => false
	};
}
