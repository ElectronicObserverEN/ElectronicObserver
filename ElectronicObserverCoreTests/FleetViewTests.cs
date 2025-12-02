using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Wpf.Fleet;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class FleetViewTests
{
	private DatabaseFixture Db { get; }

	private SerializableColor Color { get; } = new();

	public FleetViewTests(DatabaseFixture db)
	{
		Db = db;

		Configuration.Config.UI.BarColorSchemes =
		[
			[ Color, Color, Color, Color, Color, Color, Color, Color, Color, Color, Color, Color],
		];

		Configuration.Config.UI.SetBarColorScheme();
	}

	[Fact(DisplayName = "Basic anchorage repair test")]
	public void FleetViewTest1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai]),
				new ShipDataMock(Db.MasterShips[ShipId.BismarckDrei])
				{
					Level = 185,
					HPCurrent = 98,
				},
			]),
		};

		FleetViewModel fleetViewModel = new(1);

		fleetViewModel.Updated(fleet);

		Assert.Equal(2, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(98, fleetViewModel.ControlMember[1].HP.HPBar.Value);

		DateTime anchorageRepairTimer = DateTime.Now.AddMinutes(-21);
		fleetViewModel.UpdateTimerTick(fleet, anchorageRepairTimer);

		Assert.Equal(2, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(99, fleetViewModel.ControlMember[1].HP.AkashiRepairBar.Value);
	}

	[Fact(DisplayName = "Advanced anchorage repair test")]
	public void FleetViewTest2()
	{
		// Bisko repair time should be 28 minutes
		FleetDataMock fleet = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkashiKai]),
				new ShipDataMock(Db.MasterShips[ShipId.BismarckDrei])
				{
					Level = 185,
					HPCurrent = 97,
				},
			]),
		};

		FleetViewModel fleetViewModel = new(1);

		fleetViewModel.Updated(fleet);

		Assert.Equal(2, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(97, fleetViewModel.ControlMember[1].HP.HPBar.Value);

		DateTime anchorageRepairTimer = DateTime.Now.AddMinutes(-21);
		fleetViewModel.UpdateTimerTick(fleet, anchorageRepairTimer);

		Assert.Equal(2, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(98, fleetViewModel.ControlMember[1].HP.AkashiRepairBar.Value);

		anchorageRepairTimer = DateTime.Now.AddMinutes(-55);
		fleetViewModel.UpdateTimerTick(fleet, anchorageRepairTimer);

		Assert.Equal(2, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(98, fleetViewModel.ControlMember[1].HP.AkashiRepairBar.Value);

		anchorageRepairTimer = DateTime.Now.AddMinutes(-57);
		fleetViewModel.UpdateTimerTick(fleet, anchorageRepairTimer);

		Assert.Equal(2, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(99, fleetViewModel.ControlMember[1].HP.AkashiRepairBar.Value);
	}

	[Fact(DisplayName = "Advanced anchorage repair test with repair speedup")]
	public void FleetViewTest3()
	{
		// Bisko repair time should be 28 minutes, 23:54 with speedup
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
						null,
						null,
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.BismarckDrei])
				{
					Level = 185,
					HPCurrent = 97,
				},
			]),
		};

		FleetViewModel fleetViewModel = new(1);

		fleetViewModel.Updated(fleet);

		Assert.Equal(3, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(97, fleetViewModel.ControlMember[2].HP.HPBar.Value);

		DateTime anchorageRepairTimer = DateTime.Now.AddMinutes(-21);
		fleetViewModel.UpdateTimerTick(fleet, anchorageRepairTimer);

		Assert.Equal(3, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(98, fleetViewModel.ControlMember[2].HP.AkashiRepairBar.Value);

		anchorageRepairTimer = DateTime.Now.AddMinutes(-45);
		fleetViewModel.UpdateTimerTick(fleet, anchorageRepairTimer);

		Assert.Equal(3, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(98, fleetViewModel.ControlMember[2].HP.AkashiRepairBar.Value);

		anchorageRepairTimer = DateTime.Now.AddMinutes(-50);
		fleetViewModel.UpdateTimerTick(fleet, anchorageRepairTimer);

		Assert.Equal(3, fleetViewModel.ControlMember.Count(m => m.HP.HPBar.Value > 0));
		Assert.Equal(99, fleetViewModel.ControlMember[2].HP.AkashiRepairBar.Value);
	}
}
