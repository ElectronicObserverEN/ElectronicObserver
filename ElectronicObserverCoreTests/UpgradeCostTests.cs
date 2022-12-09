﻿using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

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
			SliderLevel = SliderUpgradeLevel.FromLevel6,
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
			ImprovementMaterial = (4 * 4) + (4 * 6),

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

		Assert.Equal(expectedCost, cost);
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
			SliderLevel = SliderUpgradeLevel.FromLevel7,
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

			ImprovementMaterial =
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

		Assert.Equal(expectedCost, cost);
	}


	[Fact]
	public void UpgradeCostTest3()
	{
		Assert.NotEmpty(UpgradeData.UpgradeList);

		EquipmentUpgradePlanItemModel plan = new()
		{
			// lvl 8 -> Conversion
			DesiredUpgradeLevel = UpgradeLevel.Conversion,
			EquipmentId = EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai,
			SliderLevel = SliderUpgradeLevel.FromLevel7,
			SelectedHelper = ShipId.FletcherMkII
		};

		EquipmentDataMock equipment = new EquipmentDataMock(Db.MasterEquipment[plan.EquipmentId])
		{
			UpgradeLevel = UpgradeLevel.Eight
		};
		IShipDataMaster helper = Db.MasterShips[plan.SelectedHelper];

		EquipmentUpgradePlanCostModel cost = equipment.CalculateUpgradeCost(UpgradeData.UpgradeList, helper, plan.DesiredUpgradeLevel, plan.SliderLevel);


		EquipmentUpgradePlanCostModel expectedCost = new EquipmentUpgradePlanCostModel()
		{
			Fuel = 3 * 30,
			Ammo = 3 * 90,
			Steel = 3 * 190,
			Bauxite = 3 * 180,

			DevelopmentMaterial =
			// 8 -> Max (Slider)
			(2 * 22) +
			// Conversion (Slider)
			(1 * 39),

			ImprovementMaterial =
			// 7 -> Max (Slider)
			(2 * 11) +
			// Conversion (Slider)
			(1 * 18),

			RequiredConsumables = new()
			{
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)UseItemId.NewModelArmamentMaterials,
					Required = 2
				},
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)UseItemId.Medals,
					Required = 2
				},
			},
			RequiredEquipments = new()
			{
				// 8 -> Max
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.MainGunSmall_5inchSingleGunMk_30,
					Required = 2 * 1
				},
				// Conversion
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.RadarSmall_GFCSMk_37,
					Required = 1
				}
			}
		};

		Assert.Equal(expectedCost, cost);
	}

	[Fact]
	public void UpgradeCostTest4()
	{
		Assert.NotEmpty(UpgradeData.UpgradeList);

		EquipmentUpgradePlanItemModel plan = new()
		{
			// lvl 0 -> Conversion
			DesiredUpgradeLevel = UpgradeLevel.Conversion,
			EquipmentId = EquipmentId.CarrierBasedBomber_Type99DiveBomber_EgusaSquadron,
			SliderLevel = SliderUpgradeLevel.ConversionOnly,
			SelectedHelper = ShipId.SouryuuKaiNi
		};

		EquipmentDataMock equipment = new EquipmentDataMock(Db.MasterEquipment[plan.EquipmentId])
		{
			UpgradeLevel = UpgradeLevel.Zero
		};
		IShipDataMaster helper = Db.MasterShips[plan.SelectedHelper];

		EquipmentUpgradePlanCostModel cost = equipment.CalculateUpgradeCost(UpgradeData.UpgradeList, helper, plan.DesiredUpgradeLevel, plan.SliderLevel);


		EquipmentUpgradePlanCostModel expectedCost = new EquipmentUpgradePlanCostModel()
		{
			Fuel = 11 * 300,
			Ammo = 11 * 300,
			Steel = 11 * 0,
			Bauxite = 11 * 390,

			DevelopmentMaterial =
			// 0 -> 6
			(6 * 6) +
			// 6 -> Max
			(4 * 7) +
			// Conversion (Slider)
			(1 * 28),

			ImprovementMaterial =
			// 0 -> 6
			(6 * 4) +
			// 6 -> Max
			(4 * 5) +
			// Conversion (Slider)
			(1 * 20),

			RequiredConsumables = new()
			{
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)UseItemId.NewModelAviationArmamentMaterials,
					Required = 3
				},
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)UseItemId.SkilledCrewMember,
					Required = 2
				},
			},
			RequiredEquipments = new()
			{
				// 0 -> 6
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.CarrierBasedBomber_Type99DiveBomber,
					Required = 6 * 2
				},
				// 6 -> Max
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.CarrierBasedBomber_Type99DiveBomberModel22,
					Required = 4 * 1
				},
				// Conversion (Slider)
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.CarrierBasedBomber_Suisei,
					Required = 1 * 6
				},
			}
		};

		Assert.Equal(expectedCost, cost);


	}


	[Fact]
	public void UpgradeCostTest5()
	{
		Assert.NotEmpty(UpgradeData.UpgradeList);

		EquipmentUpgradePlanItemModel plan = new()
		{
			// lvl 0 -> Conversion
			DesiredUpgradeLevel = UpgradeLevel.Conversion,
			EquipmentId = EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai,
			SliderLevel = SliderUpgradeLevel.FromLevel6,
			SelectedHelper = ShipId.FletcherMkII
		};

		EquipmentDataMock equipment = new EquipmentDataMock(Db.MasterEquipment[plan.EquipmentId])
		{
			UpgradeLevel = UpgradeLevel.Zero
		};
		IShipDataMaster helper = Db.MasterShips[plan.SelectedHelper];

		EquipmentUpgradePlanCostModel cost = equipment.CalculateUpgradeCost(UpgradeData.UpgradeList, helper, plan.DesiredUpgradeLevel, plan.SliderLevel);


		EquipmentUpgradePlanCostModel expectedCost = new EquipmentUpgradePlanCostModel()
		{
			Fuel = 11 * 30,
			Ammo = 11 * 90,
			Steel = 11 * 190,
			Bauxite = 11 * 180,

			DevelopmentMaterial =
			// 0 -> 6
			(6 * 12) +
			// 6 -> Max (Slider)
			(4 * 22) +
			// Conversion (Slider)
			(1 * 39),

			ImprovementMaterial =
			// 0 -> 6
			(6 * 7) +
			// 6 -> Max (Slider)
			(4 * 11) +
			// Conversion (Slider)
			(1 * 18),

			RequiredConsumables = new()
			{
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)UseItemId.NewModelArmamentMaterials,
					Required = 2
				},
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)UseItemId.Medals,
					Required = 2
				},
			},
			RequiredEquipments = new()
			{
				// 0 -> 6
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE,
					Required = 6 * 2
				},
				// 6 -> Max
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.MainGunSmall_5inchSingleGunMk_30,
					Required = 4 * 1
				},
				// Conversion
				new EquipmentUpgradePlanCostItemModel()
				{
					Id = (int)EquipmentId.RadarSmall_GFCSMk_37,
					Required = 1
				}
			}
		};

		Assert.Equal(expectedCost, cost);
	}

}
