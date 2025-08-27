using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class DayAttackTests
{
	private DatabaseFixture Db { get; }
	private int Precision => 3;

	private ShipDataMock BismarckMock => new(Db.MasterShips[ShipId.BismarckDrei])
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
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShell])
			{
				Level = 8,
			},
		},
		Aircraft = new List<int> { 4, 4, 4, 4 },
	};

	private ShipDataMock Isek2Mock => new(Db.MasterShips[ShipId.IseKaiNi])
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
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShell])
			{
				Level = 8,
			},
		},
		Aircraft = new List<int> { 2, 2, 22, 22, 9 },
	};

	private ShipDataMock TaihouMock => new(Db.MasterShips[ShipId.TaihouKai])
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

	public DayAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void DayAttackTest1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}),
		};

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling,
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
	public void DayAttackTest2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}),
		};

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.ZuiunMultiAngle,
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling,
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
	public void DayAttackTest3()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}),
		};

		List<Enum> expected = new List<Enum>
		{
			DayAirAttackCutinKind.FighterBomberAttacker,
			DayAirAttackCutinKind.BomberBomberAttacker,
			DayAirAttackCutinKind.BomberAttacker,
			DayAttackKind.AirAttack,
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
	public void DayAttackTest4()
	{
		ShipDataMock NagatoMock = new(Db.MasterShips[ShipId.NagatoKai])
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

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				NagatoMock,
			}),
		};

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling,
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

	[Fact(DisplayName = "STF")]
	public void DayAttackTest5()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				BismarckMock,
				Isek2Mock,
				TaihouMock,
			}),
			FleetType = FleetType.Surface,
		};

		List<Enum> expected = new List<Enum>
		{
			DayAttackKind.CutinMainMain,
			DayAttackKind.DoubleShelling,
			DayAttackKind.Shelling,
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
	public void DayAttackTest6()
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
			DayAttackKind.Shelling,
		};

		List<Enum> actual = yamashioMaru.GetDayAttacks().ToList();

		Assert.Equal(expected, actual);

		Assert.Equal(137, yamashioMaru.GetDayAttackPower(actual[0], fleet));
	}
}
