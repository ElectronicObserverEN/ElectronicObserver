using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class CanSinkTests
{
	private DatabaseFixture Db { get; }

	public CanSinkTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Basic case")]
	public void CanSinkTest1()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);
		ShipDataMock asakaze = new(Db.MasterShips[ShipId.AsakazeKai])
		{
			HPCurrent = 1,
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				kamikaze,
				asakaze,
			}),
		};

		Assert.False(kamikaze.CanSink(fleet));
		Assert.True(asakaze.CanSink(fleet));
	}

	[Fact(DisplayName = "Taiha flagship")]
	public void CanSinkTest2()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);
		ShipDataMock asakaze = new(Db.MasterShips[ShipId.AsakazeKai])
		{
			HPCurrent = 1,
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				asakaze,
				kamikaze,
			}),
		};

		Assert.False(kamikaze.CanSink(fleet));
		Assert.False(asakaze.CanSink(fleet));
	}

	[Fact(DisplayName = "With damecon")]
	public void CanSinkTest3()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);
		ShipDataMock asakaze = new(Db.MasterShips[ShipId.AsakazeKai])
		{
			HPCurrent = 1,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.DamageControl_EmergencyRepairPersonnel]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				kamikaze,
				asakaze,
			}),
		};

		Assert.False(kamikaze.CanSink(fleet));
		Assert.False(asakaze.CanSink(fleet));
	}

	[Fact(DisplayName = "Escaped")]
	public void CanSinkTest4()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);
		ShipDataMock asakaze = new(Db.MasterShips[ShipId.AsakazeKai])
		{
			HPCurrent = 1,
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				kamikaze,
				asakaze,
			}),
		};

		fleet.Escape(fleet.MembersInstance.IndexOf(asakaze) + 1);

		Assert.False(kamikaze.CanSink(fleet));
		Assert.False(asakaze.CanSink(fleet));
	}

	[Fact(DisplayName = "Docked")]
	public void CanSinkTest5()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);
		ShipDataMock asakaze = new(Db.MasterShips[ShipId.AsakazeKai])
		{
			HPCurrent = 1,
			RepairingDockID = 0,
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				kamikaze,
				asakaze,
			}),
		};

		Assert.False(kamikaze.CanSink(fleet));
		Assert.False(asakaze.CanSink(fleet));
	}
}
