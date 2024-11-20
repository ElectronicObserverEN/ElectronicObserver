using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class ExerciseExpTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = "https://twitter.com/yukicacoon/status/1858776234035212674")]
	public void ExerciseExpTest1()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KashimaKai]) { Level = 163 },
			new ShipDataMock(Db.MasterShips[ShipId.KongouKai]) { Level = 42 },
			new ShipDataMock(Db.MasterShips[ShipId.KitakamiKai]) { Level = 46 },
			new ShipDataMock(Db.MasterShips[ShipId.Maruyu]) { Level = 18 },
			new ShipDataMock(Db.MasterShips[ShipId.Fubuki]) { Level = 51 },
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 33 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 153, 148);

		Assert.Equal(750, exp.BaseA);
		Assert.Equal(900, exp.BaseS);
		Assert.Equal(945, exp.TrainingCruiserSurfaceA);
		Assert.Equal(1134, exp.TrainingCruiserSurfaceS);
	}
}
