using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class AswAttackTest
{
	private DatabaseFixture Db { get; }

	public AswAttackTest(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void DayAttackTest1TestData()
	{
		FleetDataMock fleet = new();

		IShipData akebono = new ShipDataMock(Db.MasterShips[ShipId.AkebonoKai])
		{
			Level = 98,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_HFDF_Type144147ASDIC]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.DepthCharge_LightweightASWTorpedo_InitialTestModel]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.DepthCharge_Type2DepthCharge]),
			},
		};

		Assert.Equal(104, akebono.GetAswAttackPower(DayAttackKind.DepthCharge, fleet));
	}

	[Fact]
	public void AswFitBonusTest()
	{
		FleetDataMock fleet = new();

		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			Level = 175,
			ASWModernized = 9,
			LuckBase = 99,
			AswFit = 3,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_Type3ActiveSONAR]),
			},
		};

		Assert.Equal(54, kamikaze.GetAswAttackPower(DayAttackKind.DepthCharge, fleet));
	}
}
