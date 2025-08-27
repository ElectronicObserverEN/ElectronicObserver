using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class AccuracyTests
{
	private DatabaseFixture Db { get; }

	public AccuracyTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void AccuracyTest1()
	{
		IShipData bismarck = new ShipDataMock(Db.MasterShips[ShipId.BismarckDrei])
		{
			Level = 175,
			Condition = 85,
			LuckBase = 84,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai])
				{
					Level = 7
				},
			},
		};

		IFleetData fleet = new FleetDataMock
		{
			FleetType = FleetType.Single,
			MembersInstance = new(new List<IShipData?>
			{
				bismarck
			}),
		};

		double expected = 160;

		Assert.Equal(expected, bismarck.GetDayAttackAccuracy(DayAttackKind.NormalAttack, fleet));
	}

	[Fact]
	public void AccuracyTest2()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			Level = 180,
			LuckBase = 99,
		};

		Assert.Equal(184, kamikaze.NextAccuracyLevel());
	}
}
