using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Mocks;
using Moq;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class DayAttackTests
{
	private DatabaseFixture Db { get; }
	private int Precision => 3;

	private IShipData BismarckMock => new ShipDataMock(Db.MasterShips[ShipId.BismarckDrei])
	{
		Level = 175,
		Fleet = 1,
		LuckBase = 84,
		SlotInstance = new List<IEquipmentData?>
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai])
			{
				Level = 10,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai])
			{
				Level = 10,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type0ReconSeaplaneModel11B_Skilled]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1APShell])
			{
				Level = 8,
			},
		},
		Aircraft = new List<int> { 4, 4, 4, 4 },
	};

	private IShipData Isek2Mock => new ShipDataMock(Db.MasterShips[ShipId.IseKaiNi])
	{
		Level = 130,
		Fleet = 1,
		LuckBase = 46,
		FirepowerFit = 3 + 3 + 5 + 3,
		SlotInstance = new List<IEquipmentData?>
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_41cmTripleGunKaiNi])
			{
				Level = 6,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_41cmTripleGunKaiNi])
			{
				Level = 6,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_ZuiunKaiNi_634AirGroup])
			{
				Level = 10,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_Zuiun_634AirGroup])
			{
				Level = 10,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1APShell])
			{
				Level = 8,
			},
		},
		Aircraft = new List<int> { 2, 2, 22, 22, 9 },
	};

	private IShipData TaihouMock => new ShipDataMock(Db.MasterShips[ShipId.TaihouKai])
	{
		Level = 149,
		Fleet = 1,
		LuckBase = 7,
		SlotInstance = new List<IEquipmentData?>
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_Type97TorpedoBomber_TomonagaSquadron]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type99DiveBomber_EgusaSquadron]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type99DiveBomber_EgusaSquadron]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ZeroFighterModel52C_wIwaiFlight]),
		},
		Aircraft = new List<int> { 30, 24, 24, 8 },
	};

	private IShipData Bismarck
	{
		get
		{
			ShipStats stats = new ShipStats
			{
				Luck = 84,
				LoS = 87
			};

			List<IEquipmentData?> equip = new List<IEquipmentData?>
			{
				Equipment.MainGun46Kai(10),
				Equipment.MainGun46Kai(10),
				Equipment.YuraReconSkilled(),
				Equipment.T1ApShell(8),
			};

			OtherData other = new OtherData
			{
				Fleet = 1
			};

			IShipData bismarck = Ship.BismarckDrei(stats, equip, other: other);

			return bismarck;
		}
	}

	private IShipData Isek2
	{
		get
		{
			ShipStats stats = new ShipStats
			{
				Level = 130,
				LoS = 85,
				Luck = 46
			};

			List<IEquipmentData?> equip = new List<IEquipmentData?>
			{
				Equipment.MainGun41K2Triple(6),
				Equipment.MainGun41K2Triple(6),
				Equipment.Zuiunk2(10),
				Equipment.Zuiun634(10),
				Equipment.T1ApShell(8)
			};

			VisibleFits fits = new VisibleFits
			{
				Firepower = 3 + 3 + 5 + 3
			};

			IShipData ise = Ship.IseKaiNi(stats, equip, fits);

			return ise;
		}
	}

	private IShipData Taihou
	{
		get
		{
			ShipStats stats = new ShipStats
			{
				Level = 149,
				LoS = 90,
				Luck = 7
			};

			List<IEquipmentData?> equip = new List<IEquipmentData?>
			{
				Equipment.T97Tomonaga(),
				Equipment.T99Egusa(),
				Equipment.T99Egusa(),
				Equipment.T52CIwai()
			};

			IShipData taihou = Ship.TaihouKai(stats, equip);

			return taihou;
		}
	}

	public DayAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void DayAttackTest1()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				Bismarck,
				Isek2,
				Taihou
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = Bismarck.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(271, Bismarck.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(217, Bismarck.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(181, Bismarck.GetDayAttackPower(actual[2], fleet));

		List<double> asAttackRates = actual.Select(a => Bismarck.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.587, totalRates[0], Precision);
		Assert.Equal(0.28, totalRates[1], Precision);
		Assert.Equal(0.133, totalRates[2], Precision);
	}

	[Fact]
	public void DayAttackTest2()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				Bismarck,
				Isek2,
				Taihou
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.ZuiunMultiAngle,
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = Isek2.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(234, Isek2.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(261, Isek2.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(208, Isek2.GetDayAttackPower(actual[2], fleet));
		Assert.Equal(174, Isek2.GetDayAttackPower(actual[3], fleet));

		List<double> asAttackRates = actual.Select(a => Isek2.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.625, totalRates[0], Precision);
		Assert.Equal(0.188, totalRates[1], Precision);
		Assert.Equal(0.108, totalRates[2], Precision);
		Assert.Equal(0.079, totalRates[3], Precision);
	}

	[Fact]
	public void DayAttackTest3()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				Bismarck,
				Isek2,
				Taihou
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAirAttackCutinKind.FighterBomberAttacker,
			DayAirAttackCutinKind.BomberBomberAttacker,
			DayAirAttackCutinKind.BomberAttacker,
			DayAttackKind.AirAttack
		};

		List<Enum> actual = Taihou.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);
		Assert.Equal(248, Taihou.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(238, Taihou.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(228, Taihou.GetDayAttackPower(actual[2], fleet));
		Assert.Equal(199, Taihou.GetDayAttackPower(actual[3], fleet));

		List<double> asAttackRates = actual.Select(a => Taihou.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.552, totalRates[0], Precision);
		Assert.Equal(0.229, totalRates[1], Precision);
		Assert.Equal(0.101, totalRates[2], Precision);
		Assert.Equal(0.118, totalRates[3], Precision);
	}

	[Fact]
	public void DayAttackTest4()
	{
		var mockEq1 = new Mock<IEquipmentData>();

		mockEq1.Setup(s => s.Level).Returns(10);
		mockEq1.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.MainGunLarge);
		mockEq1.Setup(s => s.MasterEquipment.IsMainGun).Returns(true);

		IEquipmentData mg356 = mockEq1.Object;

		var mockEq2 = new Mock<IEquipmentData>();

		mockEq2.Setup(s => s.MasterEquipment.LOS).Returns(6);
		mockEq2.Setup(s => s.MasterEquipment.CategoryType).Returns(EquipmentTypes.SeaplaneRecon);

		IEquipmentData seaplane = mockEq2.Object;

		var mock = new Mock<IShipData>();

		mock.Setup(s => s.HPRate).Returns(1);
		mock.Setup(s => s.FirepowerTotal).Returns(129);
		mock.Setup(s => s.LOSBase).Returns(39);
		mock.Setup(s => s.LuckTotal).Returns(32);
		mock.Setup(s => s.AllSlotInstance)
			.Returns(new ReadOnlyCollection<IEquipmentData>(new List<IEquipmentData>
			{
				mg356,
				mg356,
				seaplane
			}));
		mock.Setup(s => s.Aircraft).Returns(new ReadOnlyCollection<int>(new List<int> { 4, 4, 4, 4 }));
		mock.Setup(s => s.MasterShip.ShipId).Returns(ShipId.IseKaiNi);
		mock.Setup(s => s.MasterShip.ShipType).Returns(ShipTypes.AviationBattleship);

		IShipData Nagato = mock.Object;

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				Nagato
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = Nagato.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);
		Assert.Equal(171, Nagato.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(143, Nagato.GetDayAttackPower(actual[1], fleet));

		List<double> asAttackRates = actual.Select(a => Nagato.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.415, totalRates[0], Precision);
		Assert.Equal(0.585, totalRates[1], Precision);
	}

	[Fact]
	public void DayAttackTest5()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				Bismarck,
				Isek2,
				Taihou
			}));
		fleetMock.Setup(f => f.FleetType).Returns(FleetType.Surface);

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = Bismarck.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(286, Bismarck.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(229, Bismarck.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(191, Bismarck.GetDayAttackPower(actual[2], fleet));

		List<double> asAttackRates = actual.Select(a => Bismarck.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.587, totalRates[0], Precision);
		Assert.Equal(0.28, totalRates[1], Precision);
		Assert.Equal(0.133, totalRates[2], Precision);
	}

	[Fact]
	public void DayAttackTest1TestData()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = BismarckMock.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(271, BismarckMock.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(217, BismarckMock.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(181, BismarckMock.GetDayAttackPower(actual[2], fleet));

		List<double> asAttackRates = actual.Select(a => BismarckMock.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.587, totalRates[0], Precision);
		Assert.Equal(0.28, totalRates[1], Precision);
		Assert.Equal(0.133, totalRates[2], Precision);
	}

	[Fact]
	public void DayAttackTest2TestData()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.ZuiunMultiAngle,
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = Isek2Mock.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(234, Isek2Mock.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(261, Isek2Mock.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(208, Isek2Mock.GetDayAttackPower(actual[2], fleet));
		Assert.Equal(174, Isek2Mock.GetDayAttackPower(actual[3], fleet));

		List<double> asAttackRates = actual.Select(a => Isek2Mock.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.625, totalRates[0], Precision);
		Assert.Equal(0.188, totalRates[1], Precision);
		Assert.Equal(0.108, totalRates[2], Precision);
		Assert.Equal(0.079, totalRates[3], Precision);
	}

	[Fact]
	public void DayAttackTest3TestData()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAirAttackCutinKind.FighterBomberAttacker,
			DayAirAttackCutinKind.BomberBomberAttacker,
			DayAirAttackCutinKind.BomberAttacker,
			DayAttackKind.AirAttack
		};

		List<Enum> actual = TaihouMock.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);
		Assert.Equal(248, TaihouMock.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(238, TaihouMock.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(228, TaihouMock.GetDayAttackPower(actual[2], fleet));
		Assert.Equal(199, TaihouMock.GetDayAttackPower(actual[3], fleet));

		List<double> asAttackRates = actual.Select(a => TaihouMock.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.552, totalRates[0], Precision);
		Assert.Equal(0.229, totalRates[1], Precision);
		Assert.Equal(0.101, totalRates[2], Precision);
		Assert.Equal(0.118, totalRates[3], Precision);
	}

	[Fact]
	public void DayAttackTest4TestData()
	{
		IShipData NagatoMock = new ShipDataMock(Db.MasterShips[ShipId.NagatoKai])
		{
			Level = 72,
			Fleet = 1,
			LuckBase = 32,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_35_6cmTwinGun])
				{
					Level = 10,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_35_6cmTwinGun])
				{
					Level = 10,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type0ObservationSeaplane]),
			},
			Aircraft = new List<int> { 4, 4, 4, 4 },
		};

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				NagatoMock
			}));

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = NagatoMock.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);
		Assert.Equal(171, NagatoMock.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(143, NagatoMock.GetDayAttackPower(actual[1], fleet));

		List<double> asAttackRates = actual.Select(a => NagatoMock.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.415, totalRates[0], Precision);
		Assert.Equal(0.585, totalRates[1], Precision);
	}

	[Fact]
	public void DayAttackTest5TestData()
	{
		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(
			new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}));
		fleetMock.Setup(f => f.FleetType).Returns(FleetType.Surface);

		IFleetData fleet = fleetMock.Object;

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling
		};

		List<Enum> actual = BismarckMock.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(286, BismarckMock.GetDayAttackPower(actual[0], fleet));
		Assert.Equal(229, BismarckMock.GetDayAttackPower(actual[1], fleet));
		Assert.Equal(191, BismarckMock.GetDayAttackPower(actual[2], fleet));

		List<double> asAttackRates = actual.Select(a => BismarckMock.GetDayAttackRate(a, fleet)).ToList();
		List<double> totalRates = asAttackRates.ToList().TotalRates();

		Assert.Equal(0.587, totalRates[0], Precision);
		Assert.Equal(0.28, totalRates[1], Precision);
		Assert.Equal(0.133, totalRates[2], Precision);
	}

	[Fact(DisplayName = "Yamashio Maru uses the carrier shelling formula when equipped with a bomber")]
	public void DayAttackTest6TestData()
	{
		ShipDataMock yamashioMaru = new(Db.MasterShips[ShipId.YamashioMaruKai])
		{
			Level = 99,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_OTO152mmTripleRapidfireGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_OTO152mmTripleRapidfireGun]),
			},
		};

		FleetDataMock fleet = new()
		{
			FleetType = FleetType.Single,
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				yamashioMaru,
			}),
		};
		
		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.Shelling
		};

		List<Enum> actual = yamashioMaru.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(137, yamashioMaru.GetDayAttackPower(actual[0], fleet));
	}
}
