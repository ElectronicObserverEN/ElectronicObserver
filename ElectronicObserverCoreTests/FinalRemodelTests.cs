using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using Xunit;

namespace ElectronicObserverCoreTests;


[Collection(DatabaseCollection.Name)]
public class FinalRemodelTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = "Kamikaze")]
	public void ShipDataExtensionsTest1()
	{
		Assert.False(Db.MasterShips[ShipId.Kamikaze].IsFinalRemodel());
		Assert.True(Db.MasterShips[ShipId.KamikazeKai].IsFinalRemodel());
	}

	[Fact(DisplayName = "Souya")]
	public void ShipDataExtensionsTest2()
	{
		Assert.True(Db.MasterShips[ShipId.Souya645].IsFinalRemodel());
		Assert.True(Db.MasterShips[ShipId.Souya650].IsFinalRemodel());
		Assert.True(Db.MasterShips[ShipId.Souya699].IsFinalRemodel());
	}

	[Fact(DisplayName = "Kaga")]
	public void ShipDataExtensionsTest3()
	{
		Assert.False(Db.MasterShips[ShipId.Kaga].IsFinalRemodel());
		Assert.False(Db.MasterShips[ShipId.KagaKai].IsFinalRemodel());
		Assert.True(Db.MasterShips[ShipId.KagaKaiNi].IsFinalRemodel());
		Assert.True(Db.MasterShips[ShipId.KagaKaiNiE].IsFinalRemodel());
		Assert.True(Db.MasterShips[ShipId.KagaKaiNiGo].IsFinalRemodel());
	}
}
