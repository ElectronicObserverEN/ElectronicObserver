using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class ParameterTests
{
	private DatabaseFixture Db { get; }

	public ParameterTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void ParameterTest1()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			Level = 180,
		};
		
		Assert.Equal(182, kamikaze.MasterShip.ASW.GetNextLevel(kamikaze.Level));
		Assert.Equal(183, kamikaze.MasterShip.Evasion.GetNextLevel(kamikaze.Level));
	}

	[Fact(DisplayName = "Ship without ASW")]
	public void ParameterTest2()
	{
		ShipDataMock bismarck = new(Db.MasterShips[ShipId.BismarckDrei])
		{
			Level = 180,
		};

		Assert.Null(bismarck.MasterShip.ASW.GetNextLevel(bismarck.Level));
		Assert.Equal(181, bismarck.MasterShip.Evasion.GetNextLevel(bismarck.Level));
	}
}
