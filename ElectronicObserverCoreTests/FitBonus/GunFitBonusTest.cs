using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;
using Xunit;

namespace ElectronicObserverCoreTests.FitBonus;

[Collection(DatabaseCollection.Name)]
public class GunFitBonusTest(DatabaseFixture db) : FitBonusTest(db)
{
	[Fact(DisplayName = "14cm Kai Ni on an AV")]
	public void GunFitBonusTest1()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData teste = new ShipDataMock(Db.MasterShips[ShipId.CommandantTesteKai])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_14cmTwinGunKaiNi])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 1,
			Torpedo = 1,
			Evasion = 1
		};

		FitBonusValue finalBonus = teste.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "14cm Kai Ni on Yuubari K2 Toku")]
	public void GunFitBonusTest2()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData yuubari = new ShipDataMock(Db.MasterShips[ShipId.YuubariKaiNiToku])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_14cmTwinGunKaiNi])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 6,
			AntiAir = 2,
			ASW = 2,
			Evasion = 3
		};

		FitBonusValue finalBonus = yuubari.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "14cm Kai +7 on Yuubari K2 Toku")]
	public void GunFitBonusTest3()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData yuubari = new ShipDataMock(Db.MasterShips[ShipId.YuubariKaiNiToku])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_14cmTwinGunKai])
				{
					Level = 7
				}
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 5,
			Torpedo = 1,
			AntiAir = 1,
			ASW = 1,
			Evasion = 2
		};

		FitBonusValue finalBonus = yuubari.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "14cm Kai +10 and 14cm Kai Ni on an AV")]
	public void GunFitBonusTest4()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData teste = new ShipDataMock(Db.MasterShips[ShipId.CommandantTeste])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_14cmTwinGunKai])
				{
					Level = 10
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_14cmTwinGunKaiNi])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 0 + 1,
			Torpedo = 0 + 1,
			Evasion = 0 + 1
		};

		FitBonusValue finalBonus = teste.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "14cm Kai +10 and 14cm Kai Ni on Nishsin")]
	public void GunFitBonusTest5()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData teste = new ShipDataMock(Db.MasterShips[ShipId.NisshinA])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_14cmTwinGunKai])
				{
					Level = 10
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_14cmTwinGunKaiNi])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 4 + 4,
			Torpedo = 3 + 3,
			AntiAir = 1 + 1,
			Evasion = 1 + 2
		};

		FitBonusValue finalBonus = teste.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}
}
