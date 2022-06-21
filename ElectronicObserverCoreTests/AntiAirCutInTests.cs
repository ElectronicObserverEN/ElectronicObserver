using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class AntiAirCutInTests
{
	private DatabaseFixture Db { get; }

	public AntiAirCutInTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Regular destroyer")]
	public void AntiAirCutInTest1()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			AllSlotInstance = new List<IEquipmentData>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_Type13AirRadarKai_LateModel]),
			}
		};

		List<AntiAirCutIn> cutins = AntiAirCutIn.PossibleCutIns(kamikaze);

		Assert.Equal(2, cutins.Count);
		Assert.Equal(5, cutins[0].Id);
		Assert.Equal(0, cutins[1].Id);
	}

	[Fact(DisplayName = "Destroyer with special cut-in")]
	public void AntiAirCutInTest2()
	{
		ShipDataMock isokaze = new(Db.MasterShips[ShipId.IsokazeBKai])
		{
			AllSlotInstance = new List<IEquipmentData>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_Type13AirRadarKai_LateModel]),
			}
		};

		List<AntiAirCutIn> cutins = AntiAirCutIn.PossibleCutIns(isokaze);

		Assert.Equal(3, cutins.Count);
		Assert.Equal(5, cutins[0].Id);
		Assert.Equal(29, cutins[1].Id);
		Assert.Equal(0, cutins[2].Id);
	}

	[Fact(DisplayName = "Atlanta")]
	public void AntiAirCutInTest3()
	{
		ShipDataMock atlanta = new(Db.MasterShips[ShipId.AtlantaKai])
		{
			AllSlotInstance = new List<IEquipmentData>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_GFCSMk_37_5inchTwinDualpurposeGunMount_ConcentratedDeployment]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_GFCSMk_37_5inchTwinDualpurposeGunMount_ConcentratedDeployment]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
			}
		};

		List<AntiAirCutIn> cutins = AntiAirCutIn.PossibleCutIns(atlanta);

		Assert.Equal(5, cutins.Count);
		Assert.Equal(38, cutins[0].Id);
		Assert.Equal(40, cutins[1].Id);
		Assert.Equal(41, cutins[2].Id);
		Assert.Equal(5, cutins[3].Id);
		Assert.Equal(0, cutins[4].Id);
	}
}
