using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;
using Xunit;

namespace ElectronicObserverCoreTests.FitBonus;

[Collection(DatabaseCollection.Name)]
public class RadarFitBonusTest(DatabaseFixture db) : FitBonusTest(db)
{
	[Fact(DisplayName = "Passive Radiolocator E27 + Type 22 Surface Radar Kai 4 Calibrated Late Model")]
	public void RadarFitBonusTest1()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData kazagumo = new ShipDataMock(Db.MasterShips[ShipId.KazagumoKaiNi])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_PassiveRadiolocator_E27_Type22SurfaceRadarKai4_CalibratedLateModel])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 1,
			Accuracy = 2,
			Evasion = 1,
			LOS = 1
		};

		FitBonusValue finalBonus = kazagumo.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "Passive Radiolocator E27 + Type 22 Surface Radar Kai 4 Calibrated Late Model with 12.7cm D Kai Ni")]
	public void RadarFitBonusTest2()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData kazagumo = new ShipDataMock(Db.MasterShips[ShipId.KazagumoKaiNi])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_PassiveRadiolocator_E27_Type22SurfaceRadarKai4_CalibratedLateModel]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12_7cmTwinGunModelDKaiNi]),
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 1 + 6,
			Torpedo = 0 + 6,
			Accuracy = 2 + 0,
			Evasion = 1 + 4,
			LOS = 1 + 0
		};

		FitBonusValue finalBonus = kazagumo.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "Passive Radiolocator E27 + Type 22 Surface Radar Kai 4 Calibrated Late Model with 12.7cm D Kai Ni +3")]
	public void RadarFitBonusTest3()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData kazagumo = new ShipDataMock(Db.MasterShips[ShipId.KazagumoKaiNi])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_PassiveRadiolocator_E27_Type22SurfaceRadarKai4_CalibratedLateModel]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12_7cmTwinGunModelDKaiNi]) { UpgradeLevel = UpgradeLevel.Three }
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 1 + 8,
			Torpedo = 0 + 6,
			Accuracy = 2 + 2,
			Evasion = 1 + 4,
			LOS = 1 + 0
		};

		FitBonusValue finalBonus = kazagumo.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "Passive Radiolocator E27 + Type 22 Surface Radar Kai 4 Calibrated Late Model with 12.7cm D Kai Ni +3 and 12.7cm D Kai San +4")]
	public void RadarFitBonusTest4()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData kazagumo = new ShipDataMock(Db.MasterShips[ShipId.KazagumoKaiNi])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_PassiveRadiolocator_E27_Type22SurfaceRadarKai4_CalibratedLateModel]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12_7cmTwinGunModelDKaiNi]) { UpgradeLevel = UpgradeLevel.Three },
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12_7cmTwinGunModelDKaiSan]) { UpgradeLevel = UpgradeLevel.Four }
		}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 1 + 8 + 6,
			Torpedo = 0 + 6 + 4,
			Accuracy = 2 + 2 + 3,
			Evasion = 1 + 4 + 3,
			LOS = 1 + 0 + 0,
			AntiAir = 0 + 0 + 3
		};

		FitBonusValue finalBonus = kazagumo.GetTheoricalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}
}
