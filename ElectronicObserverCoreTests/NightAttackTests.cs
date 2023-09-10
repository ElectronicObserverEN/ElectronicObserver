using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Mocks;
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
		ShipDataMock bismarck = new(Db.MasterShips[ShipId.BismarckDrei])
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

		ShipDataMock gotland = new(Db.MasterShips[ShipId.Gotlandandra])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.StarShell_StarShell]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				bismarck,
				gotland,
			})
		};

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

		Assert.Equal(226, bismarck.GetNightAttackAccuracy(actual[0], fleet).RoundDown());
		Assert.Equal(137, bismarck.GetNightAttackAccuracy(actual[1], fleet).RoundDown());
	}

	[Fact]
	public void NightAttackTest2()
	{
		ShipDataMock akagi = new(Db.MasterShips[ShipId.AkagiKaiNi])
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

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				akagi,
			}),
		};

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
		ShipDataMock taiyou = new(Db.MasterShips[ShipId.TaiyouKaiNi])
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

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				taiyou,
			}),
		};

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

	[Fact(DisplayName = "Ark Royal")]
	public void NightAttackTest4()
	{
		ShipDataMock ark = new(Db.MasterShips[ShipId.ArkRoyalKai])
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

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				ark,
			}),
		};

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
	public void NightAttackTest5()
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

	[Fact(DisplayName = "Destroyer lookouts - only works for DD, CL and CLT")]
	public void NightAttackTest6()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>()),
		};

		List<IEquipmentData?> torpedoes = new()
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
		};

		List<IEquipmentData?> torpedoesWithDestroyerLookouts = new()
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts]),
		};

		ShipDataMock mogami = new(Db.MasterShips[ShipId.MogamiKaiNiToku])
		{
			SlotInstance = torpedoes,
		};

		double noLookoutsRate = mogami.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		mogami.SlotInstance = torpedoesWithDestroyerLookouts;

		double lookoutsRate = mogami.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		Assert.Equal(noLookoutsRate, lookoutsRate);

		ShipDataMock bismarck = new(Db.MasterShips[ShipId.BismarckDrei])
		{
			SlotInstance = torpedoes,
		};

		noLookoutsRate = bismarck.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		bismarck.SlotInstance = torpedoesWithDestroyerLookouts;

		lookoutsRate = bismarck.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		Assert.Equal(noLookoutsRate, lookoutsRate);

		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			SlotInstance = torpedoes,
		};

		noLookoutsRate = kamikaze.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		kamikaze.SlotInstance = torpedoesWithDestroyerLookouts;

		lookoutsRate = kamikaze.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		Assert.True(noLookoutsRate < lookoutsRate);

		ShipDataMock yahagi = new(Db.MasterShips[ShipId.YahagiKaiNiB])
		{
			SlotInstance = torpedoes,
		};

		noLookoutsRate = yahagi.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		yahagi.SlotInstance = torpedoesWithDestroyerLookouts;

		lookoutsRate = yahagi.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		Assert.True(noLookoutsRate < lookoutsRate);
	}

	[Fact(DisplayName = "Night Zuiun cut-in with (destroyer) lookouts - normal lookouts boost the rate")]
	public void NightAttackTest7()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>()),
		};

		List<IEquipmentData?> zuiunCutIn = new()
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
		};

		List<IEquipmentData?> zuiunCutInWithLookouts = new()
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
		};

		List<IEquipmentData?> zuiunCutInWithDestroyerLookouts = new()
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts]),
		};

		ShipDataMock yamato = new(Db.MasterShips[ShipId.YamatoKaiNiJuu])
		{
			SlotInstance = zuiunCutIn,
		};

		double normalRate = yamato.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		yamato.SlotInstance = zuiunCutInWithLookouts;

		double lookoutsRate = yamato.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		yamato.SlotInstance = zuiunCutInWithDestroyerLookouts;

		double destroyerLookoutsRate = yamato.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		Assert.Equal(normalRate, destroyerLookoutsRate);
		Assert.True(normalRate < lookoutsRate);
	}

	[Fact(DisplayName = "Night Zuiun cut-in rate bonus - reduced for Zuiun cut-in, increased for others")]
	public void NightAttackTest8()
	{
		List<IEquipmentData?> zuiunCutIn = new()
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
		};

		ShipDataMock yamato = new(Db.MasterShips[ShipId.YamatoKaiNiJuu]);

		ShipDataMock mogami = new(Db.MasterShips[ShipId.MogamiKaiNiToku])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_20_3cm_No_3TwinGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunMedium_20_3cm_No_3TwinGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				yamato,
				mogami,
			}),
		};

		double noCutInZuiunCutInRate = mogami.GetNightAttackRate(NightZuiunCutinAttack.NightZuiunCutinZuiun, fleet);
		double noCutInMainTorpedoCutInRate = mogami.GetNightAttackRate(NightAttack.CutinMainTorpedo, fleet);

		yamato.SlotInstance = zuiunCutIn;

		double cutInZuiunCutInRate = mogami.GetNightAttackRate(NightZuiunCutinAttack.NightZuiunCutinZuiun, fleet);
		double cutInMainTorpedoCutInRate = mogami.GetNightAttackRate(NightAttack.CutinMainTorpedo, fleet);

		Assert.True(noCutInZuiunCutInRate > cutInZuiunCutInRate);
		Assert.True(cutInMainTorpedoCutInRate > noCutInMainTorpedoCutInRate);
	}

	[Fact(DisplayName = "Night Zuiun cut-in with searchlight - rate is reduced")]
	public void NightAttackTest9()
	{
		ShipDataMock yamato = new(Db.MasterShips[ShipId.YamatoKaiNiJuu])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
			},
		};

		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				yamato,
				kamikaze,
			}),
		};

		double normalRate = yamato.GetNightAttackRate(NightZuiunCutinAttack.NightZuiunCutinZuiun, fleet);

		kamikaze.SlotInstance = new List<IEquipmentData?>
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Searchlight_Searchlight]),
		};

		double searchlightRate = yamato.GetNightAttackRate(NightZuiunCutinAttack.NightZuiunCutinZuiun, fleet);

		Assert.True(normalRate > searchlightRate);
	}

	[Fact(DisplayName = "Night Zuiun cut-in with flares - rate is reduced")]
	public void NightAttackTest10()
	{
		ShipDataMock yamato = new(Db.MasterShips[ShipId.YamatoKaiNiJuu])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
			},
		};

		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai]);

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				yamato,
				kamikaze,
			}),
		};

		double normalRate = yamato.GetNightAttackRate(NightZuiunCutinAttack.NightZuiunCutinZuiun, fleet);

		kamikaze.SlotInstance = new List<IEquipmentData?>
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.StarShell_StarShell]),
		};

		double searchlightRate = yamato.GetNightAttackRate(NightZuiunCutinAttack.NightZuiunCutinZuiun, fleet);

		Assert.True(normalRate > searchlightRate);
	}

	[Fact(DisplayName = "Night Zuiun cut-in bonus and flare bonus don't stack")]
	public void NightAttackTest11()
	{
		List<IEquipmentData?> zuiunCutIn = new()
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_46cmTripleGunKai]),
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
		};

		ShipDataMock yamato = new(Db.MasterShips[ShipId.YamatoKaiNiJuu]);

		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				yamato,
				kamikaze,
			}),
		};

		double normalRate = kamikaze.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		yamato.SlotInstance = zuiunCutIn;

		double zuiunCutInRate = kamikaze.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		kamikaze.SlotInstance.Add(new EquipmentDataMock(Db.MasterEquipment[EquipmentId.StarShell_StarShell]));

		double zuiunCutInFlareRate = kamikaze.GetNightAttackRate(NightAttack.CutinTorpedoTorpedo, fleet);

		Assert.Equal(zuiunCutInRate, zuiunCutInFlareRate);
		Assert.True(normalRate < zuiunCutInRate);
	}

	[Fact(DisplayName = "Attack rate shouldn't be negative or over 100%")]
	public void NightAttackTest12()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			Level = 180,
			LuckBase = 99,
			HPCurrent = 10,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts]),
			},
		};

		ShipDataMock perth = new(Db.MasterShips[ShipId.PerthKai])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.StarShell_StarShell]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Searchlight_Searchlight]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				kamikaze,
				perth,
			}),
		};

		List<NightAttack> attacks = kamikaze.GetNightAttacks().ToList();

		List<double> attackRates = attacks.Select(a => kamikaze.GetNightAttackRate(a, fleet)).ToList();
		List<double> totalRates = attackRates.ToList().TotalRates();

		Assert.True(kamikaze.HPRate <= 0.5);
		Assert.All(totalRates, rate => Assert.True(rate is >= 0 and <= 1));
	}

	[Fact(DisplayName = "Souya 645 and 650 can't attack at night")]
	public void NightAttackTest13()
	{
		ShipDataMock souya645 = new(Db.MasterShips[ShipId.Souya645]);
		ShipDataMock souya650 = new(Db.MasterShips[ShipId.Souya650]);

		List<NightAttack> expected = new();

		Assert.Equal(expected, souya645.GetNightAttacks().ToList());
		Assert.Equal(expected, souya650.GetNightAttacks().ToList());
	}

	[Fact(DisplayName = "Luigi Kai and Cappellini Kai can't attack at night")]
	public void NightAttackTest14()
	{
		ShipDataMock luigi = new(Db.MasterShips[ShipId.LuigiTorelli]);
		ShipDataMock luigiKai = new(Db.MasterShips[ShipId.LuigiTorelliKai]);
		ShipDataMock capellinni = new(Db.MasterShips[ShipId.CCappellini]);
		ShipDataMock capellinniKai = new(Db.MasterShips[ShipId.CCappelliniKai]);

		List<NightAttack> expected = new();

		Assert.NotEmpty(luigi.GetNightAttacks());
		Assert.NotEmpty(capellinni.GetNightAttacks());
		
		Assert.Equal(expected, luigiKai.GetNightAttacks().ToList());
		Assert.Equal(expected, capellinniKai.GetNightAttacks().ToList());
	}
}
