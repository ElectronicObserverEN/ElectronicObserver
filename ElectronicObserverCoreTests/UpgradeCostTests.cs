using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;
using ElectronicObserverTypes;
using Xunit;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Utility.Data;
using Microsoft.Extensions.Hosting;

namespace ElectronicObserverCoreTests;


[Collection(DatabaseCollection.Name)]
public class UpgradeCostTests
{
	private DatabaseFixture Db { get; }

	private static ElectronicObserver.Data.Translation.EquipmentUpgradeData UpgradeData { get; } = new();

	public UpgradeCostTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void UpgradeCostTest1()
	{
		Assert.NotEmpty(UpgradeData.UpgradeList);

		EquipmentUpgradePlanItemModel plan = new()
		{
			// lvl 2 -> 10
			DesiredUpgradeLevel = UpgradeLevel.Max,
			EquipmentId = EquipmentId.LandBasedAttacker_Type1AttackBomberModel22A,
			SliderLevel = SliderUpgradeLevel.Seven,
			SelectedHelper = ShipId.KirishimaKaiNi
		};

		EquipmentDataMock equipment = new EquipmentDataMock(Db.MasterEquipment[plan.EquipmentId])
		{
			UpgradeLevel = UpgradeLevel.Two
		};
		IShipDataMaster helper = Db.MasterShips[plan.SelectedHelper];

		EquipmentUpgradePlanCostModel cost = equipment.CalculateUpgradeCost(UpgradeData.UpgradeList, helper, plan.DesiredUpgradeLevel, plan.SliderLevel);


		EquipmentUpgradePlanCostModel expectedCost = new EquipmentUpgradePlanCostModel()
		{
			Fuel = 8 * 430,
			Ammo = 8 * 420,
			Steel = 8 * 0,
			Bauxite = 8 * 540,

			DevelopmentMaterial = (4 * 6) + (4 * 9),
			ImprovmentMaterial = (4 * 4) + (4 * 6),

			RequiredConsumables = new(),
			RequiredEquipments = new()
			{
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.CarrierBasedFighter_ZeroFighterModel21,
					Required = 4 * 2
				},
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.CarrierBasedTorpedo_TenzanModel12A,
					Required = 4 * 2
				}
			}
		};

		AssertCostEquals(expectedCost, cost);
	}

	[Fact]
	public void UpgradeCostTest2()
	{
		Assert.NotEmpty(UpgradeData.UpgradeList);

		EquipmentUpgradePlanItemModel plan = new()
		{
			// lvl 0 -> Conversion
			DesiredUpgradeLevel = UpgradeLevel.Conversion,
			EquipmentId = EquipmentId.MainGunLarge_16inchTripleGunMk_7,
			SliderLevel = SliderUpgradeLevel.Eight,
			SelectedHelper = ShipId.Iowa
		};

		EquipmentDataMock equipment = new EquipmentDataMock(Db.MasterEquipment[plan.EquipmentId])
		{
			UpgradeLevel = UpgradeLevel.Zero
		};
		IShipDataMaster helper = Db.MasterShips[plan.SelectedHelper];

		EquipmentUpgradePlanCostModel cost = equipment.CalculateUpgradeCost(UpgradeData.UpgradeList, helper, plan.DesiredUpgradeLevel, plan.SliderLevel);


		EquipmentUpgradePlanCostModel expectedCost = new EquipmentUpgradePlanCostModel()
		{
			Fuel = 11 * 45,
			Ammo = 11 * 450,
			Steel = 11 * 750,
			Bauxite = 11 * 100,

			DevelopmentMaterial = 
			// 0 -> 6
			(6 * 10) + 
			// 6 -> 7
			(1 * 16) +
			// 7 -> Max (Slider)
			(3 * 24) +
			// Conversion (Slider)
			(1 * 28),

			ImprovmentMaterial =
			// 0 -> 6
			(6 * 6) +
			// 6 -> 7
			(1 * 8) +
			// 7 -> Max (Slider)
			(3 * 12) +
			// Conversion (Slider)
			(1 * 20),

			RequiredConsumables = new(),
			RequiredEquipments = new()
			{
				// 0 -> 6
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.MainGunLarge_41cmTwinGun,
					Required = 6 * 3
				},
				// 7 -> Max
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.MainGunLarge_46cmTripleGun,
					Required = 4 * 3
				},
				// Conversion
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.RadarLarge_Type32SurfaceRADAR,
					Required = 2
				}
			}
		};

		AssertCostEquals(expectedCost, cost);
	}

	private void AssertCostEquals(EquipmentUpgradePlanCostModel expected, EquipmentUpgradePlanCostModel actual)
	{
		Assert.Equal(expected.Fuel, actual.Fuel);
		Assert.Equal(expected.Ammo, actual.Ammo);
		Assert.Equal(expected.Steel, actual.Steel);
		Assert.Equal(expected.Bauxite, actual.Bauxite);

		Assert.Equal(expected.RequiredEquipments.Count, actual.RequiredEquipments.Count);

		foreach (EquipmentUpgradePlanCostItemModel expectedDetail in expected.RequiredEquipments)
		{
			EquipmentUpgradePlanCostItemModel? actualDetail = actual.RequiredEquipments.FirstOrDefault(detail => detail.Id == expectedDetail.Id);

			Assert.NotNull(actualDetail);

			Assert.Equal(expectedDetail.Required, actualDetail.Required);
		}

		Assert.Equal(expected.RequiredConsumables.Count, actual.RequiredConsumables.Count);

		foreach (EquipmentUpgradePlanCostItemModel expectedDetail in expected.RequiredConsumables)
		{
			EquipmentUpgradePlanCostItemModel? actualDetail = actual.RequiredConsumables.FirstOrDefault(detail => detail.Id == expectedDetail.Id);

			Assert.NotNull(actualDetail);

			Assert.Equal(expectedDetail.Required, actualDetail.Required);
		}
	}
}
