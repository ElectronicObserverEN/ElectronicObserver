using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class ShellingSupportTests
{
	private DatabaseFixture Db { get; }
	private int Precision => 3;

	public ShellingSupportTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void ShellingSupportTest1()
	{
		ShipDataMock ise = new ShipDataMock(Db.MasterShips[ShipId.IseKaiNi])
		{
			Level = 99,
			Condition = 85,
			FirepowerFit = 6,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_41cmTripleGunKaiNi])
				{
					Level = 6,

				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_41cmTripleGunKaiNi])
				{
					Level = 6,

				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
			},
		};

		Assert.Equal(153, ise.GetShellingSupportDamage());
		Assert.Equal(159.6, ise.GetShellingSupportAccuracy(), Precision);
	}

	[Fact]
	public void ShellingSupportTest2()
	{
		ShipDataMock taihou = new ShipDataMock(Db.MasterShips[ShipId.TaihouKai])
		{
			Level = 99,
			Condition = 85,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_Type32SurfaceRADAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_Type32SurfaceRADAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_Type32SurfaceRADAR]),
			},
		};

		Assert.Equal(166, taihou.GetShellingSupportDamage());
		Assert.Equal(136.8, taihou.GetShellingSupportAccuracy(), Precision);
	}
}
