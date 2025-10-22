using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class EventLockPlannerTests
{
	private DatabaseFixture Db { get; }
	private LockTranslationData LockTranslator { get; } = new();
	private List<IShipData> AllShips { get; }
	private List<IShipData> AllShipsCopy => AllShips
		.Cast<ShipDataMock>()
		.Select(s => s.Clone())
		.Cast<IShipData>()
		.ToList();

	public EventLockPlannerTests(DatabaseFixture db)
	{
		Db = db;

		AllShips = new List<IShipData>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Kamikaze]) {ID = 1},
			new ShipDataMock(Db.MasterShips[ShipId.Asakaze]) {ID = 2},
			new ShipDataMock(Db.MasterShips[ShipId.Harukaze]) {ID = 3},
			new ShipDataMock(Db.MasterShips[ShipId.Matsukaze]) {ID = 4},
			new ShipDataMock(Db.MasterShips[ShipId.Hatakaze]) {ID = 5},
			new ShipDataMock(Db.MasterShips[ShipId.Bismarck]) {ID = 6},
			new ShipDataMock(Db.MasterShips[ShipId.Warspite]) {ID = 7},
			new ShipDataMock(Db.MasterShips[ShipId.Perth]) {ID = 8},
		};
	}

	[Fact(DisplayName = "Planner initialization")]
	public void EventLockPlannerTest1()
	{
		EventLockPlannerViewModel planner = new(AllShips, LockTranslator);
		planner.Loaded();

		Assert.Equal(AllShips.Count, planner.NoLockGroup.Ships.Count);
		Assert.Empty(planner.LockGroups);
	}

	[Fact(DisplayName = "Add lock")]
	public void EventLockPlannerTest2()
	{
		EventLockPlannerViewModel planner = new(AllShips, LockTranslator);
		planner.Loaded();

		planner.AddLockCommand.Execute(null);

		Assert.Single(planner.LockGroups);
	}

	[Fact(DisplayName = "Lock ship")]
	public void EventLockPlannerTest3()
	{
		EventLockPlannerViewModel planner = new(AllShips, LockTranslator);
		planner.Loaded();

		planner.AddLockCommand.Execute(null);

		ShipLockViewModel kamikaze = planner.NoLockGroup.Ships
			.First(s => s.Ship.MasterShip.ShipId is ShipId.Kamikaze);

		planner.LockGroups.First().Move(kamikaze, planner.NoLockGroup);

		Assert.Equal(AllShips.Count - 1, planner.NoLockGroup.Ships.Count);
		Assert.Single(planner.LockGroups);
		Assert.Single(planner.LockGroups.First().Ships);
	}

	[Fact(DisplayName = "Real lock initialization")]
	public void EventLockPlannerTest4()
	{
		List<IShipData> allShips = AllShipsCopy;
		ShipDataMock kamikaze = (allShips.First(s => s.MasterShip.ShipId is ShipId.Kamikaze) as ShipDataMock)!;
		kamikaze.SallyArea = 2;

		EventLockPlannerViewModel planner = new(allShips, LockTranslator);
		planner.Loaded();

		Assert.Equal(allShips.Count - 1, planner.NoLockGroup.Ships.Count);
		Assert.Equal(2, planner.LockGroups.Count);
		Assert.Single(planner.LockGroups[1].Ships);
	}

	[Fact(DisplayName = "Can't move locked ship to a different lock plan")]
	public void EventLockPlannerTest5()
	{
		List<IShipData> allShips = AllShipsCopy;
		ShipDataMock kamikaze = (allShips.First(s => s.MasterShip.ShipId is ShipId.Kamikaze) as ShipDataMock)!;
		kamikaze.SallyArea = 2;

		EventLockPlannerViewModel planner = new(allShips, LockTranslator);
		planner.Loaded();

		planner.LockGroups[0].Move(planner.LockGroups[1].Ships.First(), planner.LockGroups[1]);

		Assert.Equal(allShips.Count - 1, planner.NoLockGroup.Ships.Count);
		Assert.Equal(2, planner.LockGroups.Count);
		Assert.Empty(planner.LockGroups[0].Ships);
		Assert.Single(planner.LockGroups[1].Ships);
	}

	[Fact(DisplayName = "Type filter")]
	public void EventLockPlannerTest6()
	{
		EventLockPlannerViewModel planner = new(AllShips, LockTranslator);
		planner.Loaded();

		planner.NoLockGroup.Filter.TypeFilters
			.First(f => f.Value is ShipTypeGroup.Destroyers).IsChecked = false;

		int expected = AllShips.Count(s => s.MasterShip.ShipType is not ShipTypes.Destroyer);

		Assert.Equal(expected, planner.NoLockGroup.Ships.Count);
	}
}
