using System;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class HomePortSupplyTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = "Basic test")]
	public void HomePortSupplyTest1()
	{
		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.NosakiKai]),
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply while on expedition")]
	public void HomePortSupplyTest2()
	{
		ShipDataMock nosaki = new(Db.MasterShips[ShipId.NosakiKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				nosaki,
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		fleet.ExpeditionState = ExpeditionState.OnExpedition;

		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply if supply ship doesn't have full fuel")]
	public void HomePortSupplyTest3()
	{
		ShipDataMock nosaki = new(Db.MasterShips[ShipId.NosakiKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				nosaki,
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		nosaki.Fuel -= 1;

		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply if supply ship doesn't have full ammo")]
	public void HomePortSupplyTest4()
	{
		ShipDataMock nosaki = new(Db.MasterShips[ShipId.NosakiKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				nosaki,
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		nosaki.Ammo -= 1;

		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply if supply ship doesn't have at least 30 morale")]
	public void HomePortSupplyTest5()
	{
		ShipDataMock nosaki = new(Db.MasterShips[ShipId.NosakiKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				nosaki,
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		nosaki.Condition = 29;

		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));

		nosaki.Condition = 30;

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply if supply ship is shouha or worse")]
	public void HomePortSupplyTest6()
	{
		ShipDataMock nosaki = new(Db.MasterShips[ShipId.NosakiKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				nosaki,
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		nosaki.HPCurrent = (int)(nosaki.HPMax * 0.75);

		Assert.Equal(DamageState.Light, nosaki.DamageState);
		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply if supply ship is in docks")]
	public void HomePortSupplyTest7()
	{
		ShipDataMock nosaki = new(Db.MasterShips[ShipId.NosakiKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				nosaki,
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		nosaki.RepairingDockID = 0;

		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply docked ships")]
	public void HomePortSupplyTest8()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				kamikaze,
				new ShipDataMock(Db.MasterShips[ShipId.NosakiKai]),
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		kamikaze.RepairingDockID = 0;

		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));
	}

	[Fact(DisplayName = "Can't home port supply if all ships are already at max condition")]
	public void HomePortSupplyTest9()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);

		FleetDataMock fleet = new()
		{
			ExpeditionState = ExpeditionState.NotDeployed,
			MembersInstance = new(
			[
				kamikaze,
				new ShipDataMock(Db.MasterShips[ShipId.NosakiKai]),
			]),
		};

		Assert.True(HomePortSupplyService.CanHomePortSupply(fleet));

		kamikaze.Condition = HomePortSupplyService.MaxSupplyCondition;

		Assert.False(HomePortSupplyService.CanHomePortSupply(fleet));
	}
}
