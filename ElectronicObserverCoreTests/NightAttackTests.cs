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
public class NightAttackTests
{
	private DatabaseFixture Db { get; }
	private static int Precision => 3;

	public NightAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void NightAttackTest1()
	{
		ShipStats stats = new ShipStats
		{
			Level = 175,
			Luck = 84,
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.MainGun46Kai(10),
			Equipment.Quint(10),
			Equipment.Quint(10),
		};

		IShipData bismarck = Ship.BismarckDrei(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			bismarck,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			NightAttack.CutinTorpedoTorpedo,
			NightAttack.Shelling,
		};

		List<NightAttack> actual = bismarck.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(293, bismarck.GetNightAttackPower(actual[0]));
		Assert.Equal(195, bismarck.GetNightAttackPower(actual[1]));

		List<double> attackRates = actual.Select(a => bismarck.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.787, totalRates[0], Precision);
		Assert.Equal(0.213, totalRates[1], Precision);
	}

	[Fact]
	public void NightAttackTest2()
	{
		ShipStats stats = new ShipStats
		{
			Level = 122,
			Luck = 25,
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.T97NightAttacker(),
			Equipment.ReppuuKaiNiESkilled(),
			Equipment.T97NightAttacker(),
			Equipment.ReppuuKaiNiE(),
			Equipment.NightScamp(),
		};

		IShipData akagi = Ship.AkagiKaiNi(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			akagi,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			CvnciAttack.CutinAirAttackFighterFighterAttacker,
			CvnciAttack.CutinAirAttackFighterAttacker,
			CvnciAttack.CutinAirAttackFighterOtherOther,
			NightAttack.AirAttack,
		};

		List<NightAttack> actual = akagi.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(371, akagi.GetNightAttackPower(actual[0]));
		Assert.Equal(371, akagi.GetNightAttackPower(actual[1]));
		Assert.Equal(370, akagi.GetNightAttackPower(actual[2]));
		Assert.Equal(366, akagi.GetNightAttackPower(actual[3]));

		List<double> attackRates = actual.Select(a => akagi.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.6, totalRates[0], Precision);
		Assert.Equal(0.219, totalRates[1], Precision);
		Assert.Equal(0.091, totalRates[2], Precision);
		Assert.Equal(0.09, totalRates[3], Precision);
	}

	[Fact]
	public void NightAttackTest3()
	{
		ShipStats stats = new ShipStats
		{
			Level = 125,
			Luck = 17,
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.T97NightAttacker(),
			Equipment.T97NightAttacker(),
			Equipment.ReppuuKaiNiESkilled(),
			Equipment.NightScamp(),
		};

		IShipData taiyou = Ship.TaiyouKaiNi(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			taiyou,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			CvnciAttack.CutinAirAttackFighterAttacker,
			CvnciAttack.CutinAirAttackFighterOtherOther,
			NightAttack.AirAttack,
		};

		List<NightAttack> actual = taiyou.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(251, taiyou.GetNightAttackPower(actual[0]));
		Assert.Equal(247, taiyou.GetNightAttackPower(actual[1]));
		Assert.Equal(209, taiyou.GetNightAttackPower(actual[2]));

		List<double> attackRates = actual.Select(a => taiyou.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.478, totalRates[0], Precision);
		Assert.Equal(0.23, totalRates[1], Precision);
		Assert.Equal(0.292, totalRates[2], Precision);
	}

	[Fact]
	public void ArkRoyal()
	{
		ShipStats stats = new ShipStats
		{
			Level = 130,
			Luck = 16,
		};

		List<IEquipmentData?> equip = new List<IEquipmentData?>
		{
			Equipment.SwordfishMk3Skilled(),
			Equipment.ReppuuKaiNiESkilled(),
			Equipment.OTO(10),
			Equipment.OTO(10),
		};

		IShipData ark = Ship.ArkRoyalKai(stats, equip);

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			ark,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			NightAttack.DoubleShelling,
			NightAttack.Shelling,
		};

		List<NightAttack> actual = ark.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(74, ark.GetNightAttackPower(actual[0]));
		Assert.Equal(62, ark.GetNightAttackPower(actual[1]));

		List<double> attackRates = actual.Select(a => ark.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.99, totalRates[0], Precision);
		Assert.Equal(0.01, totalRates[1], Precision);
	}

	[Fact]
	public void NightAttackTest1TestData()
	{
		IShipData bismarck = new ShipDataMock(Db.MasterShips[ShipId.BismarckDrei])
		{
			Level = 175,
			LuckBase = 84,
			Condition = 49,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai])
				{
					Level = 10,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_61cmQuintuple_OxygenTorpedo])
				{
					Level = 10,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_61cmQuintuple_OxygenTorpedo])
				{
					Level = 10,
				},
			},
		};

		IShipData gotland = new ShipDataMock(Db.MasterShips[ShipId.Gotlandandra])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.StarShell_StarShell]),
			},
		};

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			bismarck,
			gotland,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			NightAttack.CutinTorpedoTorpedo,
			NightAttack.Shelling,
		};

		List<NightAttack> actual = bismarck.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(293, bismarck.GetNightAttackPower(actual[0]));
		Assert.Equal(195, bismarck.GetNightAttackPower(actual[1]));

		List<double> attackRates = actual.Select(a => bismarck.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.82, totalRates[0], Precision);
		Assert.Equal(0.18, totalRates[1], Precision);

		Assert.Equal(206, bismarck.GetNightAttackAccuracy(actual[0], fleet).RoundDown());
		Assert.Equal(125, bismarck.GetNightAttackAccuracy(actual[1], fleet).RoundDown());
	}

	[Fact]
	public void NightAttackTest2TestData()
	{
		IShipData akagi = new ShipDataMock(Db.MasterShips[ShipId.AkagiKaiNi])
		{
			Level = 122,
			LuckBase = 25,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_PrototypeType97TorpedoBomberKaiNo_3ModelE_wType6AirborneRadarKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_PrototypeType97TorpedoBomberKaiNo_3ModelE_wType6AirborneRadarKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.AviationPersonnel_NightOperationAviationPersonnel]),
			},
		};

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			akagi,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			CvnciAttack.CutinAirAttackFighterFighterAttacker,
			CvnciAttack.CutinAirAttackFighterAttacker,
			CvnciAttack.CutinAirAttackFighterOtherOther,
			NightAttack.AirAttack,
		};

		List<NightAttack> actual = akagi.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(371, akagi.GetNightAttackPower(actual[0]));
		Assert.Equal(371, akagi.GetNightAttackPower(actual[1]));
		Assert.Equal(370, akagi.GetNightAttackPower(actual[2]));
		Assert.Equal(366, akagi.GetNightAttackPower(actual[3]));

		List<double> attackRates = actual.Select(a => akagi.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.6, totalRates[0], Precision);
		Assert.Equal(0.219, totalRates[1], Precision);
		Assert.Equal(0.091, totalRates[2], Precision);
		Assert.Equal(0.09, totalRates[3], Precision);
	}

	[Fact]
	public void NightAttackTest3TestData()
	{
		IShipData taiyou = new ShipDataMock(Db.MasterShips[ShipId.TaiyouKaiNi])
		{
			Level = 125,
			LuckBase = 17,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_PrototypeType97TorpedoBomberKaiNo_3ModelE_wType6AirborneRadarKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_PrototypeType97TorpedoBomberKaiNo_3ModelE_wType6AirborneRadarKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.AviationPersonnel_NightOperationAviationPersonnel]),

			},
		};

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			taiyou,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			CvnciAttack.CutinAirAttackFighterAttacker,
			CvnciAttack.CutinAirAttackFighterOtherOther,
			NightAttack.AirAttack,
		};

		List<NightAttack> actual = taiyou.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(251, taiyou.GetNightAttackPower(actual[0]));
		Assert.Equal(247, taiyou.GetNightAttackPower(actual[1]));
		Assert.Equal(209, taiyou.GetNightAttackPower(actual[2]));

		List<double> attackRates = actual.Select(a => taiyou.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.478, totalRates[0], Precision);
		Assert.Equal(0.23, totalRates[1], Precision);
		Assert.Equal(0.292, totalRates[2], Precision);
	}

	[Fact]
	public void ArkRoyalTestData()
	{
		IShipData ark = new ShipDataMock(Db.MasterShips[ShipId.ArkRoyalKai])
		{
			Level = 130,
			LuckBase = 16,
			FirepowerFit = 4,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_SwordfishMk_III_Skilled]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_OTO152mmTripleRapidfireGun])
				{
					Level = 10,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_OTO152mmTripleRapidfireGun])
				{
					Level = 10,
				},
			},
		};

		var fleetMock = new Mock<IFleetData>();

		fleetMock.Setup(f => f.MembersWithoutEscaped).Returns(new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			ark,
		}));

		IFleetData fleet = fleetMock.Object;

		List<NightAttack> expected = new()
		{
			NightAttack.DoubleShelling,
			NightAttack.Shelling,
		};

		List<NightAttack> actual = ark.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(75, ark.GetNightAttackPower(actual[0]));
		Assert.Equal(63, ark.GetNightAttackPower(actual[1]));

		List<double> attackRates = actual.Select(a => ark.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.Equal(0.99, totalRates[0], Precision);
		Assert.Equal(0.01, totalRates[1], Precision);
	}

	[Fact]
	public void NightAttackTest4TestData()
	{
		IShipData fuumii = new ShipDataMock(Db.MasterShips[ShipId.I203Kai])
		{
			Level = 175,
			Condition = 49,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SubmarineTorpedo_SkilledSonarPersonnel_LateModelBowTorpedoMount_4tubes]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Engine_NewHighPressureTemperatureSteamBoiler]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SubmarineEquipment_LateModelRadar_PassiveRadiolocator_SnorkelEquipment])
				{
					Level = 2,
				},
			},
		};

		List<NightAttack> expected = new()
		{
			SubmarineTorpedoCutinAttack.CutinTorpedoTorpedoLateModelTorpedoSubmarineEquipment,
			NightAttack.Torpedo,
		};

		List<NightAttack> actual = fuumii.GetNightAttacks().ToList();

		Assert.Equal(expected, actual);
	}
}
