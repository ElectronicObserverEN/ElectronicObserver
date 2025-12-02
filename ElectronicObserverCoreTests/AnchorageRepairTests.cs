using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class AnchorageRepairTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = "Akashi no gear")]
	public void AnchorageRepairTest1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai]),
			]),
		};

		Assert.Equal(2, fleet.GetAnchorageRepairCount());
	}

	[Fact(DisplayName = "Asahi no gear")]
	public void AnchorageRepairTest2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AsahiKai]),
			]),
		};

		Assert.Equal(0, fleet.GetAnchorageRepairCount());
	}

	[Fact(DisplayName = "Akashi + Asahi no gear")]
	public void AnchorageRepairTest3()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai]),
				new ShipDataMock(Db.MasterShips[ShipId.AsahiKai]),
			]),
		};

		Assert.Equal(3, fleet.GetAnchorageRepairCount());
	}

	[Fact(DisplayName = "Akashi with 4 cranes")]
	public void AnchorageRepairTest4()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
					],
				},
			]),
		};

		Assert.Equal(6, fleet.GetAnchorageRepairCount());
	}

	[Fact(DisplayName = "Akashi + Asahi with 4 cranes")]
	public void AnchorageRepairTest5()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.AsahiKai]),
			]),
		};

		Assert.Equal(7, fleet.GetAnchorageRepairCount());
	}

	[Fact(DisplayName = "Repair speedup")]
	public void AnchorageRepairTest6()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai]),
				new ShipDataMock(Db.MasterShips[ShipId.AsahiKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
					],
				},
			]),
		};

		Assert.Equal(0.85, fleet.GetAnchorageRepairTimeModifier(), 2);
	}

	[Fact(DisplayName = "Second repair ship must have a crane for repair speedup")]
	public void AnchorageRepairTest7()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RepairFacility_ShipRepairFacility]),
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.AsahiKai]),
			]),
		};

		Assert.Equal(1, fleet.GetAnchorageRepairTimeModifier(), 2);
	}
}
