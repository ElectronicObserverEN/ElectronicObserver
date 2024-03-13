using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes;
using Xunit;
using System.Collections.ObjectModel;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class SmokeGeneratorTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	private static void CheckSmokeRate(List<SmokeGeneratorTriggerRate> rates, int smokeCount, double expectedRate)
	{
		SmokeGeneratorTriggerRate? rate = rates.Find(rate => rate.SmokeGeneratorCount == smokeCount);

		Assert.NotNull(rate);
		Assert.Equal(expectedRate, rate.ActivationRatePercentage);
	}

	[Fact(DisplayName = "1 smoke generator")]
	public void SmokeGeneratorTest1()
	{
		ShipDataMock hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 180,
			LuckBase = 53,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen]),
			},
		};

		FleetDataMock fleet = new FleetDataMock()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>([hachijou]),
		};

		List<SmokeGeneratorTriggerRate> rates = fleet.GetSmokeTriggerRates();

		Assert.NotEmpty(rates);
		Assert.Single(rates);

		CheckSmokeRate(rates, 1, 40);
	}

	[Fact(DisplayName = "1 smoke generator +9")]
	public void SmokeGeneratorTest2()
	{
		ShipDataMock hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 180,
			LuckBase = 53,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen])
				{
					Level = 9,
				},
			},
		};

		FleetDataMock fleet = new FleetDataMock()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>([hachijou]),
		};

		List<SmokeGeneratorTriggerRate> rates = fleet.GetSmokeTriggerRates();

		Assert.NotEmpty(rates);
		Assert.Single(rates);

		CheckSmokeRate(rates, 1, 80);
	}

	[Fact(DisplayName = "1 smoke generator kai")]
	public void SmokeGeneratorTest3()
	{
		ShipDataMock hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 180,
			LuckBase = 53,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGeneratorKai_SmokeScreen]),
			},
		};

		FleetDataMock fleet = new FleetDataMock()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>([hachijou]),
		};

		List<SmokeGeneratorTriggerRate> rates = fleet.GetSmokeTriggerRates();

		Assert.NotEmpty(rates);
		Assert.Equal(2, rates.Count);

		CheckSmokeRate(rates, 1, 51);
		CheckSmokeRate(rates, 2, 49);
	}
}
