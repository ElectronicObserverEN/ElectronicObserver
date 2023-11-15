using System.Linq;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;
using ElectronicObserverTypes;
using Xunit;

namespace ElectronicObserverCoreTests;

public class UpgradeTreeTests
{
	private static UpgradeTreeViewModel MakeUpgradeTree(EquipmentId equipmentId, UpgradeLevel upgradeLevel)
	{
		EquipmentUpgradePlanItemModel upgradePlan = new()
		{
			EquipmentId = equipmentId,
			DesiredUpgradeLevel = upgradeLevel,
		};

		EquipmentUpgradePlanItemViewModel upgradePlanViewModel = new(upgradePlan);
		upgradePlanViewModel.UpdateCosts();

		return new(upgradePlanViewModel);
	}

	[Fact(DisplayName = "S-51J kai max")]
	public void UpgradeTreeTest1()
	{
		UpgradeTreeViewModel tree = MakeUpgradeTree(EquipmentId.Autogyro_S51JKai, UpgradeLevel.Max);

		UpgradeTreeUpgradePlanViewModel root = tree.Items[0];

		Assert.Equal(EquipmentId.Autogyro_S51JKai, root.EquipmentId);

		UpgradeTreeUpgradePlanViewModel? s51jKaiPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_S51JKai);

		UpgradeTreeUpgradePlanViewModel? zuiunPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.SeaplaneBomber_Zuiun);

		UpgradeTreeUpgradePlanViewModel? t3dcpPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.DepthCharge_Type3DepthChargeProjector);

		Assert.NotNull(s51jKaiPlan);
		Assert.NotNull(zuiunPlan);
		Assert.NotNull(t3dcpPlan);

		Assert.Equal(1, s51jKaiPlan.Count);
		Assert.Equal(12, zuiunPlan.Count);
		Assert.Equal(8, t3dcpPlan.Count);

		UpgradeTreeUpgradePlanViewModel? s51jPlan = s51jKaiPlan.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_S51J);

		Assert.NotNull(s51jPlan);

		UpgradeTreeTest2(s51jPlan);
	}

	[Theory(DisplayName = "S-51J conversion")]
	[InlineData(null)]
	public void UpgradeTreeTest2(UpgradeTreeUpgradePlanViewModel? s51jRoot)
	{
		UpgradeTreeUpgradePlanViewModel root = s51jRoot ??
			MakeUpgradeTree(EquipmentId.Autogyro_S51J, UpgradeLevel.Conversion).Items[0];

		Assert.Equal(EquipmentId.Autogyro_S51J, root.EquipmentId);

		UpgradeTreeUpgradePlanViewModel? s51jPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_S51J);

		UpgradeTreeUpgradePlanViewModel? zuiunPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.SeaplaneBomber_Zuiun);

		UpgradeTreeUpgradePlanViewModel? saiunPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.CarrierBasedRecon_Saiun);

		UpgradeTreeUpgradePlanViewModel? depthChargePlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.DepthCharge_Type95DepthCharge);

		Assert.NotNull(s51jPlan);
		Assert.NotNull(zuiunPlan);
		Assert.NotNull(saiunPlan);
		Assert.NotNull(depthChargePlan);

		Assert.Equal(1, s51jPlan.Count);
		Assert.Equal(6, zuiunPlan.Count);
		Assert.Equal(4, saiunPlan.Count);
		Assert.Equal(2, depthChargePlan.Count);

		UpgradeTreeUpgradePlanViewModel? oGyroKaiNiPlan = s51jPlan.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi);

		Assert.NotNull(oGyroKaiNiPlan);

		UpgradeTreeTest3(oGyroKaiNiPlan);
	}

	[Theory(DisplayName = "O gyro kai ni conversion")]
	[InlineData(null)]
	public void UpgradeTreeTest3(UpgradeTreeUpgradePlanViewModel? oGyroKaiNiRoot)
	{
		UpgradeTreeUpgradePlanViewModel root = oGyroKaiNiRoot ??
			MakeUpgradeTree(EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi, UpgradeLevel.Conversion).Items[0];

		Assert.Equal(EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi, root.EquipmentId);

		UpgradeTreeUpgradePlanViewModel? oGyroKaiNiPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi);

		UpgradeTreeUpgradePlanViewModel? reconPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.SeaplaneRecon_Type0ReconSeaplane);

		UpgradeTreeUpgradePlanViewModel? bomberPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.CarrierBasedBomber_Type99DiveBomber);

		// fodder used to upgrade oGyroKaiNiPlan
		UpgradeTreeUpgradePlanViewModel? gyroPlan = root.Children
			.Where(p => p != oGyroKaiNiPlan)
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi);

		Assert.NotNull(oGyroKaiNiPlan);
		Assert.NotNull(reconPlan);
		Assert.NotNull(bomberPlan);
		Assert.NotNull(gyroPlan);

		Assert.Equal(1, oGyroKaiNiPlan.Count);
		Assert.Equal(6, reconPlan.Count);
		Assert.Equal(8, bomberPlan.Count);
		Assert.Equal(1, gyroPlan.Count);

		UpgradeTreeUpgradePlanViewModel? oGyroKaiPlan = oGyroKaiNiPlan.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_OTypeObservationAutogyroKai);

		Assert.NotNull(oGyroKaiPlan);

		UpgradeTreeTest4(oGyroKaiPlan);
	}

	[Theory(DisplayName = "O gyro kai conversion")]
	[InlineData(null)]
	public void UpgradeTreeTest4(UpgradeTreeUpgradePlanViewModel? oGyroKaiRoot)
	{
		UpgradeTreeUpgradePlanViewModel root = oGyroKaiRoot ??
			MakeUpgradeTree(EquipmentId.Autogyro_OTypeObservationAutogyroKai, UpgradeLevel.Conversion).Items[0];

		Assert.Equal(EquipmentId.Autogyro_OTypeObservationAutogyroKai, root.EquipmentId);

		UpgradeTreeUpgradePlanViewModel? oGyroKaiPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_OTypeObservationAutogyroKai);

		UpgradeTreeUpgradePlanViewModel? fighterPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.CarrierBasedFighter_Type96Fighter);

		UpgradeTreeUpgradePlanViewModel? gyroPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_KaTypeObservationAutogyro);

		Assert.NotNull(oGyroKaiPlan);
		Assert.NotNull(fighterPlan);
		Assert.NotNull(gyroPlan);

		Assert.Equal(1, oGyroKaiPlan.Count);
		Assert.Equal(4, fighterPlan.Count);
		Assert.Equal(1, gyroPlan.Count);

		UpgradeTreeUpgradePlanViewModel? kaGyroPlan = oGyroKaiPlan.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_KaTypeObservationAutogyro);

		Assert.NotNull(kaGyroPlan);

		UpgradeTreeTest5(kaGyroPlan);
	}

	[Theory(DisplayName = "Ka gyro conversion")]
	[InlineData(null)]
	public void UpgradeTreeTest5(UpgradeTreeUpgradePlanViewModel? kaGyroRoot)
	{
		UpgradeTreeUpgradePlanViewModel root = kaGyroRoot ?? 
			MakeUpgradeTree(EquipmentId.Autogyro_KaTypeObservationAutogyro, UpgradeLevel.Conversion).Items[0];

		Assert.Equal(EquipmentId.Autogyro_KaTypeObservationAutogyro, root.EquipmentId);

		UpgradeTreeUpgradePlanViewModel? kaGyroPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_KaTypeObservationAutogyro);

		UpgradeTreeUpgradePlanViewModel? reconPlan = root.Children
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.SeaplaneRecon_Type0ReconSeaplane);

		// fodder used to upgrade kaGyroPlan
		UpgradeTreeUpgradePlanViewModel? gyroPlan = root.Children
			.Where(p => p != kaGyroPlan)
			.FirstOrDefault(p => p.EquipmentId is EquipmentId.Autogyro_KaTypeObservationAutogyro);

		Assert.NotNull(kaGyroPlan);
		Assert.NotNull(reconPlan);
		Assert.NotNull(gyroPlan);

		Assert.Equal(1, kaGyroPlan.Count);
		Assert.Equal(4, reconPlan.Count);
		Assert.Equal(1, gyroPlan.Count);
	}
}
