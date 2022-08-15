using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Moq;
using Xunit;

namespace ElectronicObserverCoreTests;

public static class NumberExtensions
{
	public static double RoundDown(this double value, int precision = 0)
	{
		double power = Math.Pow(10, precision);
		return Math.Floor(value * power) / power;
	}
}

[Collection(DatabaseCollection.Name)]
public class LoSTests
{
	private DatabaseFixture Db { get; }

	private int AdmiralLevel => 120;

	private IShipData Kamikaze
	{
		get
		{
			Mock<IShipData> mock = new();

			mock.Setup(s => s.LOSTotal).Returns(27);
			mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			mock.Setup(s => s.MasterShip.ShipClass).Returns(66);

			return mock.Object;
		}
	}

	private IShipData Asakaze
	{
		get
		{
			Mock<IShipData> mock = new();

			mock.Setup(s => s.LOSTotal).Returns(28);
			mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			mock.Setup(s => s.MasterShip.ShipClass).Returns(66);

			return mock.Object;
		}
	}

	private IShipData Harukaze
	{
		get
		{
			Mock<IShipData> mock = new();

			mock.Setup(s => s.LOSTotal).Returns(26);
			mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			mock.Setup(s => s.MasterShip.ShipClass).Returns(66);

			return mock.Object;
		}
	}

	private IShipData Matsukaze
	{
		get
		{
			Mock<IShipData> mock = new();

			mock.Setup(s => s.LOSTotal).Returns(28);
			mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			mock.Setup(s => s.MasterShip.ShipClass).Returns(66);

			return mock.Object;
		}
	}

	private IShipData Hatakaze
	{
		get
		{
			Mock<IShipData> mock = new();

			mock.Setup(s => s.LOSTotal).Returns(26);
			mock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
			mock.Setup(s => s.MasterShip.ShipClass).Returns(66);

			return mock.Object;
		}
	}

	private ReadOnlyCollection<IEquipmentData> NoEquip => new(new List<IEquipmentData>());

	public LoSTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void LoSTest1()
	{
		// no equip, non-kai Kamikaze class
		Mock<IFleetData> mockFleet = new();

		ReadOnlyCollection<IShipData> ships = new(new[] { Kamikaze, Asakaze, Harukaze, Matsukaze, Hatakaze });
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected = -20.03;
		double actual = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);

		Assert.Equal(expected, actual.RoundDown(2));
	}

	[Fact]
	public void LoSTest2()
	{
		// Perth with maxed night recon
		Mock<IFleetData> mockFleet = new();

		Mock<IEquipmentData> nightReconMock = new();
		nightReconMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);
		nightReconMock.Setup(e => e.MasterEquipment.LOS).Returns(3);
		nightReconMock.Setup(e => e.Level).Returns(10);
		IEquipmentData nightRecon = nightReconMock.Object;

		ReadOnlyCollection<IEquipmentData> perthGear = new(new List<IEquipmentData>
		{
			nightRecon
		});

		Mock<IShipData> perthMock = new();
		perthMock.Setup(s => s.LOSTotal).Returns(80);
		perthMock.Setup(s => s.AllSlotInstance).Returns(perthGear);
		perthMock.Setup(s => s.MasterShip.ShipClass).Returns(96);
		IShipData perth = perthMock.Object;

		ReadOnlyCollection<IShipData> ships = new(new[] { perth });
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected1 = -21.08;
		double expected2 = -12.92;
		double expected3 = -4.77;
		double expected4 = 3.38;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	[Fact]
	public void LoSTest3()
	{
		Mock<IFleetData> mockFleet = new();

		Mock<IEquipmentData> nightReconMock = new();
		nightReconMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);
		nightReconMock.Setup(e => e.MasterEquipment.LOS).Returns(3);
		nightReconMock.Setup(e => e.Level).Returns(0);
		IEquipmentData nightRecon = nightReconMock.Object;

		ReadOnlyCollection<IEquipmentData> nisshinGear = new(new List<IEquipmentData>
		{
			nightRecon,
			nightRecon,
		});

		Mock<IShipData> nisshinMock = new();
		nisshinMock.Setup(s => s.LOSTotal).Returns(127);
		nisshinMock.Setup(s => s.AllSlotInstance).Returns(nisshinGear);
		nisshinMock.Setup(s => s.MasterShip.ShipClass).Returns(90);
		IShipData nisshin = nisshinMock.Object;

		Mock<IShipData> yuubariMock = new();
		yuubariMock.Setup(s => s.LOSTotal).Returns(74);
		yuubariMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
		yuubariMock.Setup(s => s.MasterShip.ShipClass).Returns(34);
		IShipData yuubari = yuubariMock.Object;

		Mock<IShipData> kuroshioMock = new();
		kuroshioMock.Setup(s => s.LOSTotal).Returns(60);
		kuroshioMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
		kuroshioMock.Setup(s => s.MasterShip.ShipClass).Returns(30);
		IShipData kuroshio = kuroshioMock.Object;

		Mock<IShipData> kagerouMock = new();
		kagerouMock.Setup(s => s.LOSTotal).Returns(62);
		kagerouMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
		kagerouMock.Setup(s => s.MasterShip.ShipClass).Returns(30);
		IShipData kagerou = kagerouMock.Object;

		Mock<IShipData> shiranuiMock = new();
		shiranuiMock.Setup(s => s.LOSTotal).Returns(63);
		shiranuiMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
		shiranuiMock.Setup(s => s.MasterShip.ShipClass).Returns(30);
		IShipData shiranui = shiranuiMock.Object;

		Mock<IShipData> naganamiMock = new();
		naganamiMock.Setup(s => s.LOSTotal).Returns(66);
		naganamiMock.Setup(s => s.AllSlotInstance).Returns(NoEquip);
		naganamiMock.Setup(s => s.MasterShip.ShipClass).Returns(38);
		IShipData naganami = naganamiMock.Object;

		ReadOnlyCollection<IShipData> ships = new(new[] { nisshin, yuubari, kuroshio, kagerou, shiranui, naganami });
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected1 = 10.48;
		double expected2 = 17.68;
		double expected3 = 24.88;
		double expected4 = 32.08;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	[Fact]
	public void LoSTest4()
	{
		Mock<IFleetData> mockFleet = new();

		Mock<IEquipmentData> nightReconMock = new();
		nightReconMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);
		nightReconMock.Setup(e => e.MasterEquipment.LOS).Returns(3);
		nightReconMock.Setup(e => e.Level).Returns(0);
		IEquipmentData nightRecon = nightReconMock.Object;

		Mock<IEquipmentData> skilledLookoutsMock = new();
		skilledLookoutsMock.Setup(e => e.MasterEquipment.CategoryType).Returns(EquipmentTypes.SurfaceShipPersonnel);
		skilledLookoutsMock.Setup(e => e.MasterEquipment.LOS).Returns(2);
		skilledLookoutsMock.Setup(e => e.Level).Returns(0);
		IEquipmentData skilledLookouts = skilledLookoutsMock.Object;

		ReadOnlyCollection<IEquipmentData> nisshinGear = new(new List<IEquipmentData>
		{
			nightRecon,
			nightRecon,
			skilledLookouts
		});

		Mock<IShipData> nisshinMock = new();
		nisshinMock.Setup(s => s.LOSTotal).Returns(129);
		nisshinMock.Setup(s => s.AllSlotInstance).Returns(nisshinGear);
		nisshinMock.Setup(s => s.MasterShip.ShipClass).Returns(90);
		IShipData nisshin = nisshinMock.Object;

		ReadOnlyCollection<IEquipmentData> skilledLookout = new(new List<IEquipmentData>
		{
			skilledLookouts
		});

		Mock<IShipData> yuubariMock = new();
		yuubariMock.Setup(s => s.LOSTotal).Returns(76);
		yuubariMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
		yuubariMock.Setup(s => s.MasterShip.ShipClass).Returns(34);
		IShipData yuubari = yuubariMock.Object;

		Mock<IShipData> kuroshioMock = new();
		kuroshioMock.Setup(s => s.LOSTotal).Returns(62);
		kuroshioMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
		kuroshioMock.Setup(s => s.MasterShip.ShipClass).Returns(30);
		IShipData kuroshio = kuroshioMock.Object;

		Mock<IShipData> kagerouMock = new();
		kagerouMock.Setup(s => s.LOSTotal).Returns(64);
		kagerouMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
		kagerouMock.Setup(s => s.MasterShip.ShipClass).Returns(30);
		IShipData kagerou = kagerouMock.Object;

		Mock<IShipData> shiranuiMock = new();
		shiranuiMock.Setup(s => s.LOSTotal).Returns(65);
		shiranuiMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
		shiranuiMock.Setup(s => s.MasterShip.ShipClass).Returns(30);
		IShipData shiranui = shiranuiMock.Object;

		Mock<IShipData> naganamiMock = new();
		naganamiMock.Setup(s => s.LOSTotal).Returns(68);
		naganamiMock.Setup(s => s.AllSlotInstance).Returns(skilledLookout);
		naganamiMock.Setup(s => s.MasterShip.ShipClass).Returns(38);
		IShipData naganami = naganamiMock.Object;

		ReadOnlyCollection<IShipData> ships = new(new[] { nisshin, yuubari, kuroshio, kagerou, shiranui, naganami });
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected1 = 17.68;
		double expected2 = 32.08;
		double expected3 = 46.48;
		double expected4 = 60.88;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	[Fact]
	public void LoSTest1TestData()
	{
		// no equip, non-kai Kamikaze class
		Mock<IFleetData> mockFleet = new();

		ReadOnlyCollection<IShipData> ships = new(new[]
		{
			new ShipDataMock(Db.MasterShips[ShipId.Kamikaze]) {Level = 175},
			new ShipDataMock(Db.MasterShips[ShipId.Asakaze]) {Level = 175},
			new ShipDataMock(Db.MasterShips[ShipId.Harukaze]) {Level = 175},
			new ShipDataMock(Db.MasterShips[ShipId.Matsukaze]) {Level = 175},
			new ShipDataMock(Db.MasterShips[ShipId.Hatakaze]) {Level = 175},
		});
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected = -20.03;
		double actual = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);

		Assert.Equal(expected, actual.RoundDown(2));
	}

	[Fact]
	public void LoSTest2TestData()
	{
		// Perth with maxed night recon
		Mock<IFleetData> mockFleet = new();

		IShipData perth = new ShipDataMock(Db.MasterShips[ShipId.PerthKai])
		{
			Level = 168,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon])
				{
					Level = 10,
				},
			},
		};

		ReadOnlyCollection<IShipData> ships = new(new[] { perth });
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected1 = -21.08;
		double expected2 = -12.92;
		double expected3 = -4.77;
		double expected4 = 3.38;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	[Fact]
	public void LoSTest3TestData()
	{
		Mock<IFleetData> mockFleet = new();

		IShipData nisshin = new ShipDataMock(Db.MasterShips[ShipId.NisshinA])
		{
			Level = 151,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
			},
		};

		IShipData yuubari = new ShipDataMock(Db.MasterShips[ShipId.YuubariKaiNiToku])
		{
			Level = 159,
		};

		IShipData kuroshio = new ShipDataMock(Db.MasterShips[ShipId.KuroshioKaiNi])
		{
			Level = 162,
		};

		IShipData kagerou = new ShipDataMock(Db.MasterShips[ShipId.KagerouKaiNi])
		{
			Level = 162,
		};

		IShipData shiranui = new ShipDataMock(Db.MasterShips[ShipId.ShiranuiKaiNi])
		{
			Level = 161,
		};

		IShipData naganami = new ShipDataMock(Db.MasterShips[ShipId.NaganamiKaiNi])
		{
			Level = 164,
		};

		ReadOnlyCollection<IShipData> ships = new(new[] { nisshin, yuubari, kuroshio, kagerou, shiranui, naganami });
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected1 = 10.48;
		double expected2 = 17.68;
		double expected3 = 24.88;
		double expected4 = 32.08;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	[Fact]
	public void LoSTest4TestData()
	{
		Mock<IFleetData> mockFleet = new();

		IShipData nisshin = new ShipDataMock(Db.MasterShips[ShipId.NisshinA])
		{
			Level = 151,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		IShipData yuubari = new ShipDataMock(Db.MasterShips[ShipId.YuubariKaiNiToku])
		{
			Level = 159,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		IShipData kuroshio = new ShipDataMock(Db.MasterShips[ShipId.KuroshioKaiNi])
		{
			Level = 162,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		IShipData kagerou = new ShipDataMock(Db.MasterShips[ShipId.KagerouKaiNi])
		{
			Level = 162,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		IShipData shiranui = new ShipDataMock(Db.MasterShips[ShipId.ShiranuiKaiNi])
		{
			Level = 161,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		IShipData naganami = new ShipDataMock(Db.MasterShips[ShipId.NaganamiKaiNi])
		{
			Level = 164,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		ReadOnlyCollection<IShipData> ships = new(new[] { nisshin, yuubari, kuroshio, kagerou, shiranui, naganami });
		mockFleet.Setup(f => f.MembersWithoutEscaped).Returns(ships);

		IFleetData fleet = mockFleet.Object;

		double expected1 = 17.68;
		double expected2 = 32.08;
		double expected3 = 46.48;
		double expected4 = 60.88;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	[Fact]
	public void SgInitialFit()
	{
		IShipData fletcher = new ShipDataMock(Db.MasterShips[ShipId.FletcherMkII])
		{
			Level = 110,
			LosFit = 4,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_SGRadar_InitialModel]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				fletcher
			})
		};

		double expected1 = -24.60;
		double expected2 = -19.80;
		double expected3 = -15.00;
		double expected4 = -10.20;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}
}
