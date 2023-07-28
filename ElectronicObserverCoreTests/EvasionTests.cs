using System.Collections.Generic;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class EvasionTests
{
	private DatabaseFixture Db { get; }

	public EvasionTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void EvasionTest1()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.Kamikaze])
		{
			Level = 1,
			LuckBase = 30,
			EvasionFit = 3,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_Type4PassiveSONAR])
				{
					Level = 10,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts]),
			},
		};

		Assert.Equal(51, kamikaze.ShellingEvasion());
		Assert.Equal(55, kamikaze.TorpedoEvasion());
		Assert.Equal(61, kamikaze.NightEvasion());
	}
}
