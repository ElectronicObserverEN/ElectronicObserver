using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class FitBonusTest
{
	private DatabaseFixture Db { get; }

	public FitBonusTest(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void FitBonusTest1()
	{
		IShipData hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 175,
			AllSlotInstance = new List<IEquipmentData>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE])
				{
					Level = 10
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
			}
		};

		List<FitBonusPerEquipment> bonuses = new List<FitBonusPerEquipment>()
		{
			new FitBonusPerEquipment()
			{
				EquipmentIds = new List<EquipmentId>()
				{
					EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE
				},
				Bonuses = new List<FitBonusData>()
				{
					new FitBonusData()
					{
						ShipTypes = new List<ShipTypes>() { ShipTypes.Escort },
						Bonuses = new FitBonusValue() { AntiAir = 2, ASW = 1, Evasion = 2 },
						BonusesIfLOSRadar = new FitBonusValue() { Firepower = 2, Evasion = 3 },
						BonusesIfAirRadar = new FitBonusValue() { AntiAir = 2, Evasion = 3 },
					},

					new FitBonusData()
					{
						ShipClasses = new List<ShipClass>() { (ShipClass)22, (ShipClass)66, (ShipClass)101 },
						Bonuses = new FitBonusValue() { AntiAir = 2, Evasion = 1 },
						BonusesIfLOSRadar = new FitBonusValue() { Firepower = 1, Evasion = 2 },
						BonusesIfAirRadar = new FitBonusValue() { AntiAir = 2, Evasion = 2 },
					},

					new FitBonusData()
					{
						ShipIds = new List<ShipId>() { ShipId.Yura, ShipId.Naka, ShipId.Kinu },
						Bonuses = new FitBonusValue() { AntiAir = 1 },
					},

					new FitBonusData()
					{
						ShipMasterIds = new List<ShipId>() { ShipId.YuraKai, ShipId.NakaKai, ShipId.KinuKai },
						Bonuses = new FitBonusValue() { Evasion = 1 },
					},

					new FitBonusData()
					{
						ShipMasterIds = new List<ShipId>() { ShipId.YuraKaiNi, ShipId.NakaKaiNi, ShipId.KinuKaiNi },
						Bonuses = new FitBonusValue() { Evasion = 1 },
						BonusesIfLOSRadar = new FitBonusValue() { Firepower = 1, Evasion = 1 },
						BonusesIfAirRadar = new FitBonusValue() { AntiAir = 2, Evasion = 2 },
					},

					new FitBonusData()
					{
						ShipMasterIds = new List<ShipId>() { ShipId.YukikazeKaiNi },
						Bonuses = new FitBonusValue() { AntiAir = 3, Evasion = 2 },
						BonusesIfLOSRadar = new FitBonusValue() { Firepower = 2, Evasion = 2 },
						BonusesIfAirRadar = new FitBonusValue() { AntiAir = 3, Evasion = 2 },
					},
				}
			}
		};

		FitBonusValue expectedBonus = new FitBonusValue()
		{
			Firepower = 2,
			Evasion = 3 + 3 + (2 * 2),
			AntiAir = 2 + (2 * 2),
			ASW = 1 * 2,
			Accuracy = 0,
			Armor = 0,
			LOS = 0,
			Range = 0,
			Torpedo = 0,
		};

		FitBonusValue finalBonus = hachijou.GetFitBonus(bonuses);

		Assert.Equal(expectedBonus.ASW, finalBonus.ASW);
		Assert.Equal(expectedBonus.Accuracy, finalBonus.Accuracy);
		Assert.Equal(expectedBonus.AntiAir, finalBonus.AntiAir);
		Assert.Equal(expectedBonus.Armor, finalBonus.Armor);
		Assert.Equal(expectedBonus.Evasion, finalBonus.Evasion);
		Assert.Equal(expectedBonus.Firepower, finalBonus.Firepower);
		Assert.Equal(expectedBonus.LOS, finalBonus.LOS);
		Assert.Equal(expectedBonus.Range, finalBonus.Range);
		Assert.Equal(expectedBonus.Torpedo, finalBonus.Torpedo);
	}
}
