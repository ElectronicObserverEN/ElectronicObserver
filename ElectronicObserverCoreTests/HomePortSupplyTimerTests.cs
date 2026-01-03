using System;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class HomePortSupplyTimerTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]) { ID = 1 };
	private ShipDataMock Asakaze => new(Db.MasterShips[ShipId.AsakazeKai]) { ID = 2 };
	private ShipDataMock Harukaze => new(Db.MasterShips[ShipId.HarukazeKai]) { ID = 3 };
	private ShipDataMock Matsukaze => new(Db.MasterShips[ShipId.MatsukazeKai]) { ID = 4 };
	private ShipDataMock Hatakaze => new(Db.MasterShips[ShipId.HatakazeKai]) { ID = 5 };
	private ShipDataMock Nosaki => new(Db.MasterShips[ShipId.NosakiKai]) { ID = 6 };

	[Fact(DisplayName = "Going to port after 14 minutes doesn't reset the timer")]
	public void HomePortSupplyTimerTest1()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-14);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		service.EnterPort();

		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Going to port after 15 minutes resets the timer")]
	public void HomePortSupplyTimerTest2()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-15);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		service.EnterPort();

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Moving ships in a non-home port supply fleet doesn't reset the timer")]
	public void HomePortSupplyTimerTest3()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 5, Nosaki.ID, Nosaki, null);
		service.FleetUpdated(fleet, 1, 4, Nosaki.ID, Nosaki, null);
		service.FleetUpdated(fleet, 1, 3, Hatakaze.ID, Hatakaze, null);

		Assert.False(HomePortSupplyService.IsHomePortSupplyFleet(fleet));
		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Adding Nosaki to the fleet as second ship resets the timer")]
	public void HomePortSupplyTimerTest4()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 1, Nosaki.ID, Nosaki, null);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Removing all ships from a home port supply fleet doesn't reset the timer")]
	public void HomePortSupplyTimerTest5()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 0, -2, null, null);

		Assert.True(HomePortSupplyService.IsHomePortSupplyFleet(fleet));
		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Removing Nosaki from a home port supply fleet doesn't reset the timer")]
	public void HomePortSupplyTimerTest6()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 1, -1, null, null);

		Assert.True(HomePortSupplyService.IsHomePortSupplyFleet(fleet));
		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Moving Nosaki to positions 3, 4, 5, 6 in a home port supply fleet doesn't reset the timer")]
	public void HomePortSupplyTimerTest7()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 2, Nosaki.ID, Nosaki, null);
		service.FleetUpdated(fleet, 1, 3, Nosaki.ID, Nosaki, null);
		service.FleetUpdated(fleet, 1, 4, Nosaki.ID, Nosaki, null);
		service.FleetUpdated(fleet, 1, 5, Nosaki.ID, Nosaki, null);

		Assert.True(HomePortSupplyService.IsHomePortSupplyFleet(fleet));
		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Moving Nosaki to position 1 resets the timer")]
	public void HomePortSupplyTimerTest8()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
				Nosaki,
			]),
		};

		service.FleetUpdated(fleet, 1, 0, Nosaki.ID, Nosaki, null);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Moving Nosaki to position 2 resets the timer")]
	public void HomePortSupplyTimerTest9()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
				Nosaki,
			]),
		};

		service.FleetUpdated(fleet, 1, 1, Nosaki.ID, Nosaki, null);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Adding a ship to a home port supply fleet resets the timer")]
	public void HomePortSupplyTimerTest10()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 5, Hatakaze.ID, Hatakaze, null);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Moving ships in a home port supply fleet resets the timer")]
	public void HomePortSupplyTimerTest11()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 4, Hatakaze.ID, Hatakaze, null);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Removing a ship from a home port supply fleet resets the timer")]
	public void HomePortSupplyTimerTest12()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet, 1, 5, -1, null, null);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Moving Nosaki from a home port supply fleet to a different fleet without creating a new home port supply fleet doesn't reset the timer")]
	public void HomePortSupplyTimerTest13()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet1 = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
			]),
		};

		FleetDataMock fleet2 = new()
		{
			FleetID = 2,
			MembersInstance = new(
			[
				Matsukaze,
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet1, 2, 2, Nosaki.ID, Nosaki, null);
		service.FleetUpdated(fleet2, 2, 2, Nosaki.ID, Nosaki, null);

		Assert.True(HomePortSupplyService.IsHomePortSupplyFleet(fleet1));
		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Moving Nosaki from a home port supply fleet to a different fleet creating a new home port supply fleet resets the timer")]
	public void HomePortSupplyTimerTest14()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet1 = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
			]),
		};

		FleetDataMock fleet2 = new()
		{
			FleetID = 2,
			MembersInstance = new(
			[
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet1, 2, 1, Nosaki.ID, Nosaki, null);

		Assert.True(HomePortSupplyService.IsHomePortSupplyFleet(fleet1));
		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);

		service.FleetUpdated(fleet2, 2, 1, Nosaki.ID, Nosaki, null);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Replacing Nosaki with a ship that's position 2 in a different fleet resets the timer")]
	public void HomePortSupplyTimerTest15()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet1 = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Asakaze,
				Harukaze,
			]),
		};

		FleetDataMock fleet2 = new()
		{
			FleetID = 2,
			MembersInstance = new(
			[
				Matsukaze,
				Hatakaze,
				Nosaki,
			]),
		};

		service.FleetUpdated(fleet2, 2, 2, Asakaze.ID, Asakaze, Nosaki);

		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);

		service.FleetUpdated(fleet1, 2, 2, Asakaze.ID, Asakaze, Nosaki);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Replacing a ship with a ship from a home port supply fleet resets the timer")]
	public void HomePortSupplyTimerTest16()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet1 = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
			]),
		};

		FleetDataMock fleet2 = new()
		{
			FleetID = 2,
			MembersInstance = new(
			[
				Hatakaze,
			]),
		};

		service.FleetUpdated(fleet2, 2, 0, Matsukaze.ID, Matsukaze, Hatakaze);

		Assert.True(HomePortSupplyService.IsHomePortSupplyFleet(fleet1));
		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);

		service.FleetUpdated(fleet1, 2, 0, Matsukaze.ID, Matsukaze, Hatakaze);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Replacing Nosaki in a home port supply fleet with a ship that's not in a fleet doesn't reset the timer")]
	public void HomePortSupplyTimerTest17()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet1 = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
			]),
		};

		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);

		service.FleetUpdated(fleet1, 1, 1, Hatakaze.ID, Hatakaze, Nosaki);

		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Replacing Nosaki in a home port supply fleet with a ship that's not in position 1 or 2 doesn't reset the timer")]
	public void HomePortSupplyTimerTest18()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet1 = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);

		service.FleetUpdated(fleet1, 1, 1, Hatakaze.ID, Hatakaze, Nosaki);

		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);
	}

	[Fact(DisplayName = "Replacing Nosaki in a home port supply fleet with a ship that's in position 1 or 2 resets the timer")]
	public void HomePortSupplyTimerTest19()
	{
		DateTime homePortSupplyTimer = DateTime.Now.AddMinutes(-10);

		HomePortSupplyService service = new();
		service.SetTimer(homePortSupplyTimer);

		FleetDataMock fleet1 = new()
		{
			FleetID = 1,
			MembersInstance = new(
			[
				Kamikaze,
				Nosaki,
				Asakaze,
				Harukaze,
				Matsukaze,
				Hatakaze,
			]),
		};

		Assert.Equal(homePortSupplyTimer, service.HomePortSupplyTimer);

		service.FleetUpdated(fleet1, 1, 1, Kamikaze.ID, Kamikaze, Nosaki);

		Assert.NotEqual(homePortSupplyTimer, service.HomePortSupplyTimer);
	}
}
