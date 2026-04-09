using System.Collections.ObjectModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class CanSinkTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

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
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				kamikaze,
				asakaze,
			]),
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
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				asakaze,
				kamikaze,
			]),
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
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.DamageControl_EmergencyRepairPersonnel]),
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				kamikaze,
				asakaze,
			]),
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
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				kamikaze,
				asakaze,
			]),
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
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				kamikaze,
				asakaze,
			]),
		};

		Assert.False(kamikaze.CanSink(fleet));
		Assert.False(asakaze.CanSink(fleet));
	}

	[Fact(DisplayName = "Used damecon")]
	public void CanSinkTest6()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);
		ShipDataMock asakaze = new(Db.MasterShips[ShipId.AsakazeKai])
		{
			HPCurrent = 1,
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.DamageControl_EmergencyRepairPersonnel]),
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				kamikaze,
				asakaze,
			]),
		};

		Assert.False(kamikaze.CanSink(fleet, kamikaze.HPCurrent, false));
		Assert.True(asakaze.CanSink(fleet, asakaze.HPCurrent, true));
	}

	[Fact(DisplayName = "Used damecon but has extra")]
	public void CanSinkTest7()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);
		ShipDataMock asakaze = new(Db.MasterShips[ShipId.AsakazeKai])
		{
			HPCurrent = 1,
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.DamageControl_EmergencyRepairPersonnel]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.DamageControl_EmergencyRepairPersonnel]),
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				kamikaze,
				asakaze,
			]),
		};

		Assert.False(kamikaze.CanSink(fleet, kamikaze.HPCurrent, false));
		Assert.False(asakaze.CanSink(fleet, asakaze.HPCurrent, true));
	}
}
