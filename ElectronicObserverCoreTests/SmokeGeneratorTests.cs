using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class SmokeGeneratorTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;
	private int Precision => 3;

	private void CheckSmokeRate(List<SmokeGeneratorTriggerRate> rates, int smokeCount, double expectedRate)
	{
		SmokeGeneratorTriggerRate? rate = rates.Find(rate => rate.SmokeGeneratorCount == smokeCount);

		Assert.NotNull(rate);
		Assert.Equal(expectedRate, rate.ActivationRatePercentage, Precision);
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

	[Fact(DisplayName = "1 smoke generator kai+2 + 1 smoke generator+2")]
	public void SmokeGeneratorTest4()
	{
		ShipDataMock hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 180,
			LuckBase = 53,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGeneratorKai_SmokeScreen])
				{
					Level = 2,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen])
				{
					Level = 2,
				},
			},
		};

		FleetDataMock fleet = new FleetDataMock()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>([hachijou]),
		};

		List<SmokeGeneratorTriggerRate> rates = fleet.GetSmokeTriggerRates();

		Assert.NotEmpty(rates);
		Assert.Equal(3, rates.Count);

		CheckSmokeRate(rates, 1, 30);
		CheckSmokeRate(rates, 2, 30);
		CheckSmokeRate(rates, 3, 40);
	}

	[Fact(DisplayName = "1 smoke generator + 1 smoke generator on two ships")]
	public void SmokeGeneratorTest5()
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

		ShipDataMock hibiki = new ShipDataMock(Db.MasterShips[ShipId.HibikiKai])
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
			MembersInstance = new ReadOnlyCollection<IShipData?>([hachijou, hibiki]),
		};

		List<SmokeGeneratorTriggerRate> rates = fleet.GetSmokeTriggerRates();

		Assert.NotEmpty(rates);
		Assert.Equal(2, rates.Count);

		CheckSmokeRate(rates, 1, 51);
		CheckSmokeRate(rates, 2, 49);
	}

	[Fact(DisplayName = "6 smoke generator on two ships")]
	public void SmokeGeneratorTest6()
	{
		ShipDataMock hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 180,
			LuckBase = 53,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen]),
			},
		};

		ShipDataMock hibiki = new ShipDataMock(Db.MasterShips[ShipId.HibikiKai])
		{
			Level = 180,
			LuckBase = 53,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen]),
			},
		};

		FleetDataMock fleet = new FleetDataMock()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>([hachijou, hibiki]),
		};

		List<SmokeGeneratorTriggerRate> rates = fleet.GetSmokeTriggerRates();

		Assert.NotEmpty(rates);
		Assert.Equal(3, rates.Count);

		CheckSmokeRate(rates, 1, 0);
		CheckSmokeRate(rates, 2, 21);
		CheckSmokeRate(rates, 3, 79);
	}

	[Fact(DisplayName = "0 smoke generator")]
	public void SmokeGeneratorTest7()
	{
		ShipDataMock hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 180,
			LuckBase = 53,
		};

		FleetDataMock fleet = new FleetDataMock()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>([hachijou]),
		};

		List<SmokeGeneratorTriggerRate> rates = fleet.GetSmokeTriggerRates();

		Assert.Empty(rates);
	}
}
