using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class QuestTrackerManagerTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact]
	public void ThisRemodelOrHigher()
	{
		ShipConditionModelV2 yamatoCondition = new()
		{
			Id = ShipId.YamatoKaiNi,
			MustBeFlagship = true,
			RemodelComparisonType = RemodelComparisonType.AtLeast,
		};

		ShipTypeConditionModel clCondition = new()
		{
			Count = 1,
			ComparisonType = ComparisonType.GreaterOrEqual,
			Types = new ObservableCollection<ShipTypes>([ShipTypes.LightCruiser]),
		};

		ShipTypeConditionModel ddCondition = new()
		{
			Count = 2,
			ComparisonType = ComparisonType.GreaterOrEqual,
			Types = new ObservableCollection<ShipTypes>([ShipTypes.Destroyer]),
		};

		GroupConditionViewModel group = new(new()
		{
			GroupOperator = Operator.And,
			Conditions = new ObservableCollection<ICondition?>([yamatoCondition, clCondition, ddCondition]),
		}, null!);

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNiJuu]),
				new ShipDataMock(Db.MasterShips[ShipId.GambierBayMkII]),
				new ShipDataMock(Db.MasterShips[ShipId.DeRuyterKai]),
				new ShipDataMock(Db.MasterShips[ShipId.MurakumoKaiNi]),
				new ShipDataMock(Db.MasterShips[ShipId.AkebonoKaiNi]),
				new ShipDataMock(Db.MasterShips[ShipId.UshioKaiNi]),
			]),
		};

		Assert.True(group.ConditionMet(fleet));
	}

	[Fact]
	public void ShipClassCondition()
	{
		PartialShipConditionModelV2 partialShipCondition = new()
		{
			Count = 2,
			Conditions = new ObservableCollection<ShipConditionModelV2>(
			[
				new() { ShipClass = ShipClass.Kamikaze },
			]),
		};

		GroupConditionViewModel group = new(new()
		{
			GroupOperator = Operator.And,
			Conditions = new ObservableCollection<ICondition?>([partialShipCondition]),
		}, null!);

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>((List<IShipData?>)
			[
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.AsakazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.HarukazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.MatsukazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.HatakazeKai]),
			]),
		};

		Assert.True(group.ConditionMet(fleet));
	}

	/// <summary>
	/// Test for quest ID 981 (B185)
	/// 2 of Fletcher-class + John C.Butler-class
	/// </summary>
	[Fact]
	public void Quest981Conditions()
	{
		PartialShipConditionModelV2 partialShipCondition = new()
		{
			Count = 2,
			Conditions = new(
			[
				new() { ShipClass = ShipClass.Fletcher },
				new() { ShipClass = ShipClass.JohnCButler },
			]),
		};

		GroupConditionViewModel group = new(new()
		{
			GroupOperator = Operator.And,
			Conditions = new ObservableCollection<ICondition?>([partialShipCondition]),
		}, null!);

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
				new ShipDataMock(Db.MasterShips[ShipId.FletcherKaiMod2]),
				new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			]),
		};

		Assert.False(group.ConditionMet(fleet));

		fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
				new ShipDataMock(Db.MasterShips[ShipId.FletcherMkII]),
				new ShipDataMock(Db.MasterShips[ShipId.SamuelBRobertsKai]),
			]),
		};

		Assert.True(group.ConditionMet(fleet));

		fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
				new ShipDataMock(Db.MasterShips[ShipId.FletcherMkII]),
				new ShipDataMock(Db.MasterShips[ShipId.Johnston]),
			]),
		};

		Assert.True(group.ConditionMet(fleet));
	}

	[Fact(DisplayName = "ship position condition - min remodel")]
	public void QuestTrackerManagerTest1()
	{
		ShipPositionConditionModel zuihouCondition = new()
		{
			Id = ShipId.ZuihouKaiNi,
			Position = 1,
			RemodelComparisonType = RemodelComparisonType.AtLeast,
		};

		GroupConditionViewModel group = new(new()
		{
			GroupOperator = Operator.And,
			Conditions = new ObservableCollection<ICondition?>([zuihouCondition]),
		}, null!);

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				new ShipDataMock(Db.MasterShips[ShipId.ZuihouKaiNiB]),
			]),
		};

		Assert.True(group.ConditionMet(fleet));
	}

	[Fact(DisplayName = "ship type min level")]
	public void QuestTrackerManagerTest2()
	{
		FleetDataMock failFleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkagiKaiNi]) { Level = 89 },
				new ShipDataMock(Db.MasterShips[ShipId.KagaKaiNi]) { Level = 89 },
			]),
		};

		FleetDataMock successFleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(
			[
				new ShipDataMock(Db.MasterShips[ShipId.AkagiKaiNi]) { Level = 90 },
				new ShipDataMock(Db.MasterShips[ShipId.KagaKaiNi]) { Level = 90 },
			]),
		};

		ShipTypeConditionModel condition = new()
		{
			Types = [ShipTypes.AircraftCarrier],
			ComparisonType = ComparisonType.GreaterOrEqual,
			Count = 2,
			Level = 90,
		};

		ShipTypeConditionViewModel conditionViewModel = new(condition);

		Assert.False(conditionViewModel.ConditionMet(failFleet));
		Assert.True(conditionViewModel.ConditionMet(successFleet));
	}
}
