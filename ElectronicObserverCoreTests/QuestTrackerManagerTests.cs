using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class QuestTrackerManagerTests
{
	private DatabaseFixture Db { get; }

	public QuestTrackerManagerTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void ThisRemodelOrHigher()
	{
		ShipConditionModel yamatoCondition = new()
		{
			Id = ShipId.YamatoKaiNi,
			MustBeFlagship = true,
			RemodelComparisonType = RemodelComparisonType.AtLeast,
		};

		ShipTypeConditionModel clCondition = new()
		{
			Count = 1,
			ComparisonType = ComparisonType.GreaterOrEqual,
			Types = new ObservableCollection<ShipTypes>(new()
			{
				ShipTypes.LightCruiser,
			})
		};

		ShipTypeConditionModel ddCondition = new()
		{
			Count = 2,
			ComparisonType = ComparisonType.GreaterOrEqual,
			Types = new ObservableCollection<ShipTypes>(new()
			{
				ShipTypes.Destroyer,
			})
		};

		GroupConditionViewModel group = new(new()
		{
			GroupOperator = Operator.And,
			Conditions = new ObservableCollection<ICondition?>(new()
			{
				yamatoCondition,
				clCondition,
				ddCondition,
			})
		});

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData>(new List<IShipData>
			{
				new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNiJuu]),
				new ShipDataMock(Db.MasterShips[ShipId.GambierBayMkII]),
				new ShipDataMock(Db.MasterShips[ShipId.DeRuyterKai]),
				new ShipDataMock(Db.MasterShips[ShipId.MurakumoKaiNi]),
				new ShipDataMock(Db.MasterShips[ShipId.AkebonoKaiNi]),
				new ShipDataMock(Db.MasterShips[ShipId.UshioKaiNi]),
			})
		};

		Assert.True(group.ConditionMet(fleet));
	}
}
