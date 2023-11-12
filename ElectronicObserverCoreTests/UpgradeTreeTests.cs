using System;
using System.Linq;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;
using ElectronicObserverTypes;
using Xunit;

namespace ElectronicObserverCoreTests;

public class UpgradeTreeTests
{
	private static int GetCount(UpgradeTreeUpgradePlanViewModel plan) => plan.Plan switch
	{
		EquipmentUpgradePlanItemViewModel => plan.RequiredCount,
		EquipmentConversionPlanItemViewModel conversion => conversion.EquipmentRequiredForUpgradePlan.Count,
		EquipmentCraftPlanItemViewModel => plan.RequiredCount,

		_ => throw new NotImplementedException(),
	};

	[Fact(DisplayName = "S-51J kai example")]
	public void UpgradeTreeTest1()
	{
		EquipmentUpgradePlanItemModel upgradePlan = new()
		{
			EquipmentId = EquipmentId.Autogyro_S51JKai,
			DesiredUpgradeLevel = UpgradeLevel.Max,
		};

		EquipmentUpgradePlanItemViewModel upgradePlanViewModel = new(upgradePlan);
		upgradePlanViewModel.UpdateCosts();

		UpgradeTreeViewModel tree = new(upgradePlanViewModel);

		UpgradeTreeUpgradePlanViewModel root = tree.Items[0];

		Assert.NotNull(root.Plan);
		Assert.Equal(EquipmentId.Autogyro_S51JKai, root.Plan.EquipmentMasterDataId);

		UpgradeTreeUpgradePlanViewModel? s51jKaiPlan = root.Children
			.FirstOrDefault(p => p.Plan?.EquipmentMasterDataId is EquipmentId.Autogyro_S51JKai);

		UpgradeTreeUpgradePlanViewModel? zuiunPlan = root.Children
			.FirstOrDefault(p => p.Plan?.EquipmentMasterDataId is EquipmentId.SeaplaneBomber_Zuiun);

		UpgradeTreeUpgradePlanViewModel? t3dcpPlan = root.Children
			.FirstOrDefault(p => p.Plan?.EquipmentMasterDataId is EquipmentId.DepthCharge_Type3DepthChargeProjector);

		Assert.NotNull(s51jKaiPlan);
		Assert.NotNull(zuiunPlan);
		Assert.NotNull(t3dcpPlan);

		Assert.Equal(1, s51jKaiPlan.RequiredCount);
		Assert.Equal(12, GetCount(zuiunPlan));
		Assert.Equal(8, GetCount(t3dcpPlan));
	}
}
