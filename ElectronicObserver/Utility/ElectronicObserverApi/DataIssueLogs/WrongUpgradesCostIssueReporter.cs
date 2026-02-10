using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Core;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.ElectronicObserverApi.Models.UpgradeCosts;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class WrongUpgradesCostIssueReporter(ElectronicObserverApiService api)
{
	private IShipDataMaster? Ship { get; set; }
	private IEquipmentData? Equipment { get; set; }

	private Dictionary<EquipmentId, APIReqKousyouRemodelSlotlistResponse> RessourcePerEquipment { get; } = [];

	private List<(ShipId, EquipmentId, UpgradeLevel)> AlreadyReportedIssues { get; } = [];

	public void ProcessUpgradeList(string _, dynamic data)
	{
		if (!api.IsServerAvailable) return;

		RessourcePerEquipment.Clear();

		List<APIReqKousyouRemodelSlotlistResponse>? response = JsonSerializer.Deserialize<List<APIReqKousyouRemodelSlotlistResponse>>(data.ToString());

		if (response is null) return;

		foreach (APIReqKousyouRemodelSlotlistResponse upgrade in response)
		{
			RessourcePerEquipment.Add((EquipmentId)upgrade.ApiSlotId, upgrade);
		}
	}

	public void ProcessUpgradeCostRequest(string _, dynamic data)
	{
		if (!api.IsServerAvailable) return;

		Ship = null;
		Equipment = null;

		int slotId = int.Parse(data["api_slot_id"].ToString());

		// if no helper => ignore
		int helperId = KCDatabase.Instance.Fleet.Fleets[1].Members[1];
		if (helperId <= 0) return;
		IShipData helper = KCDatabase.Instance.Ships[helperId];

		IEquipmentData? equipment = KCDatabase.Instance.Equipments[slotId];

		if (equipment is null) return;

		Ship = helper.MasterShip;
		Equipment = equipment;
	}

	public void ProcessUpgradeCostResponse(string _, dynamic data)
	{
		if (!api.IsServerAvailable) return;
		DayOfWeek day = DateTimeHelper.GetJapanStandardTimeNow().DayOfWeek;

		if (Ship is null) return;
		if (Equipment is null) return;

		if (AlreadyReportedIssues.Contains((Ship.ShipId, Equipment.MasterEquipment.EquipmentId, Equipment.UpgradeLevel))) return;

		ApiReqKousyouRemodelSlotlistDetailResponse? response = JsonSerializer.Deserialize<ApiReqKousyouRemodelSlotlistDetailResponse>(data.ToString());

		if (!RessourcePerEquipment.TryGetValue(Equipment.EquipmentId, out APIReqKousyouRemodelSlotlistResponse? baseCostResponse)) return;

		EquipmentUpgradePlanCostModel expectedCostSlider = Equipment.CalculateNextUpgradeCost(KCDatabase.Instance.Translation.EquipmentUpgrade.UpgradeList, Ship, SliderUpgradeLevel.Always, day);
		EquipmentUpgradePlanCostModel expectedCostNoSlider = Equipment.CalculateNextUpgradeCost(KCDatabase.Instance.Translation.EquipmentUpgrade.UpgradeList, Ship, SliderUpgradeLevel.Never, day);
		
		// cost not found -> only report cost if the equipment has no upgrade known
		// this is to avoid reporting cost when the issue is just that the ship can upgrade something, but the upgrade data hasn't been updated yet
		if (expectedCostNoSlider.Ammo is 0 && expectedCostNoSlider.Fuel is 0 && expectedCostNoSlider.Bauxite is 0 && expectedCostNoSlider.Steel is 0)
		{
			if (KCDatabase.Instance.Translation.EquipmentUpgrade.UpgradeList.Any(upgrade => upgrade.EquipmentId == Equipment.EquipmentID && upgrade.Improvement.Count > 0)) return;
		}

		if (HasIssue(expectedCostSlider, expectedCostNoSlider, response, baseCostResponse))
		{
#pragma warning disable CS4014
			api.PostJson("EquipmentUpgradeCostIssues/v2", BuildIssueModel(expectedCostSlider, expectedCostNoSlider, response, baseCostResponse, Equipment.UpgradeLevel));
#pragma warning restore CS4014

			AlreadyReportedIssues.Add((Ship.ShipId, Equipment.MasterEquipment.EquipmentId, Equipment.UpgradeLevel));
		}
	}

	private EquipmentUpgradeCostIssueModel? BuildIssueModel(EquipmentUpgradePlanCostModel expectedCostSlider, EquipmentUpgradePlanCostModel expectedCostNoSlider, ApiReqKousyouRemodelSlotlistDetailResponse actualCost, APIReqKousyouRemodelSlotlistResponse baseCostResponse, UpgradeLevel level)
	{
		if (Ship is null) return null;
		if (Equipment is null) return null;

		return new()
		{
			SoftwareVersion = SoftwareInformation.VersionEnglish,
			DataVersion = SoftwareUpdater.CurrentDataVersion.EquipmentUpgrades,
			SoftwareDataSource = Configuration.Config.Control.UpdateRepoURL.ToString(),

			EquipmentId = Equipment.EquipmentId,
			HelperId = Ship.ShipId,
			UpgradeLevel = level,

			Expected = BuildExpectedCostModel(expectedCostSlider, expectedCostNoSlider),

			Actual = BuildActualCostModel(actualCost, baseCostResponse),
		};
	}

	private EquipmentUpgradeCostDetailIssueModel BuildActualCostModel(ApiReqKousyouRemodelSlotlistDetailResponse actualCost, APIReqKousyouRemodelSlotlistResponse baseCostResponse)
	{
		return new()
		{
			Fuel = baseCostResponse.ApiReqFuel,
			Ammo = baseCostResponse.ApiReqBull,
			Steel = baseCostResponse.ApiReqSteel,
			Bauxite = baseCostResponse.ApiReqBauxite,

			DevmatCost = actualCost.ApiReqBuildkit,
			SliderDevmatCost = actualCost.ApiCertainBuildkit,

			ImproveMatCost = actualCost.ApiReqRemodelkit,
			SliderImproveMatCost = actualCost.ApiCertainRemodelkit,

			EquipmentDetail = actualCost.GetRequiredEquipments(),
			ConsumableDetail = actualCost.GetRequiredItems(),
		};
	}

	private EquipmentUpgradeCostDetailIssueModel BuildExpectedCostModel(EquipmentUpgradePlanCostModel expectedCostSlider, EquipmentUpgradePlanCostModel expectedCostNoSlider)
	{
		return new()
		{
			Fuel = expectedCostNoSlider.Fuel,
			Ammo = expectedCostNoSlider.Ammo,
			Steel = expectedCostNoSlider.Steel,
			Bauxite = expectedCostNoSlider.Bauxite,

			DevmatCost = expectedCostNoSlider.DevelopmentMaterial,
			SliderDevmatCost = expectedCostSlider.DevelopmentMaterial,

			ImproveMatCost = expectedCostNoSlider.ImprovementMaterial,
			SliderImproveMatCost = expectedCostSlider.ImprovementMaterial,

			EquipmentDetail = expectedCostNoSlider.RequiredEquipments.Select(eq => new EquipmentUpgradeImprovementCostItemDetail()
			{
				Id = eq.Id,
				Count = eq.Required,
			}).ToList(),

			ConsumableDetail = expectedCostNoSlider.RequiredConsumables.Select(item => new EquipmentUpgradeImprovementCostItemDetail()
			{
				Id = item.Id,
				Count = item.Required,
			}).ToList(),
		};
	}

	private bool HasIssue(EquipmentUpgradePlanCostModel expectedCostSlider, EquipmentUpgradePlanCostModel expectedCostNoSlider, ApiReqKousyouRemodelSlotlistDetailResponse actualCost, APIReqKousyouRemodelSlotlistResponse baseCostResponse)
	{
		if (expectedCostNoSlider.Fuel != baseCostResponse.ApiReqFuel) return true;
		if (expectedCostNoSlider.Ammo != baseCostResponse.ApiReqBull) return true;
		if (expectedCostNoSlider.Steel != baseCostResponse.ApiReqSteel) return true;
		if (expectedCostNoSlider.Bauxite != baseCostResponse.ApiReqBauxite) return true;

		if (expectedCostNoSlider.DevelopmentMaterial != actualCost.ApiReqBuildkit) return true;
		if (expectedCostNoSlider.ImprovementMaterial != actualCost.ApiReqRemodelkit) return true;

		if (expectedCostSlider.DevelopmentMaterial != actualCost.ApiCertainBuildkit) return true;
		if (expectedCostSlider.ImprovementMaterial != actualCost.ApiCertainRemodelkit) return true;

		if (AreRequirementsDifferent(expectedCostNoSlider.RequiredEquipments, actualCost.GetRequiredEquipments())) return true;
		if (AreRequirementsDifferent(expectedCostNoSlider.RequiredConsumables, actualCost.GetRequiredItems())) return true;

		return false;
	}

	public bool AreRequirementsDifferent(List<EquipmentUpgradePlanCostItemModel> expected, List<EquipmentUpgradeImprovementCostItemDetail> actual)
	{
		if (expected.Count != actual.Count) return true;

		foreach (EquipmentUpgradePlanCostItemModel item in expected)
		{
			if (!actual.Any(actualItem => actualItem.Id == item.Id && actualItem.Count == item.Required)) return true;
		}

		return false;
	}
}
