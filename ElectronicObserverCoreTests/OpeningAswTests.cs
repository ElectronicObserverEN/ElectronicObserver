using System.Collections.Generic;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class OpeningAswTests
{
	private DatabaseFixture Db { get; }

	private static FitBonusData BonusData { get; } = new();

	public OpeningAswTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Fit bonus counts for the opening ASW border")]
	public void OpeningAswTest1()
	{
		ShipDataMock kamikaze = new(Db.MasterShips[ShipId.KamikazeKai])
		{
			Level = 123,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_Type3ActiveSONAR]),
			},
		};

		Assert.False(kamikaze.CanDoOpeningAsw());

		kamikaze.AswFit = kamikaze.GetFitBonus(BonusData.FitBonusList).ASW;

		Assert.True(kamikaze.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "CVL 50 ASW condition")]
	public void OpeningAswTest2()
	{
		ShipDataMock ryuujou = new(Db.MasterShips[ShipId.RyuujouKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
			},
		};

		Assert.False(ryuujou.CanDoOpeningAsw());

		ryuujou = new(Db.MasterShips[ShipId.RyuujouKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
			},
		};

		Assert.True(ryuujou.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "Suzuya and Kumano don't count for the CVL 50 ASW condition")]
	public void OpeningAswTest3()
	{
		ShipDataMock kumano = new(Db.MasterShips[ShipId.KumanoCVLKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
			},
		};

		ShipDataMock suzuya = new(Db.MasterShips[ShipId.SuzuyaCVLKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
			},
		};

		Assert.False(kumano.CanDoOpeningAsw());
		Assert.False(suzuya.CanDoOpeningAsw());

		kumano = new(Db.MasterShips[ShipId.KumanoCVLKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
			},
		};

		suzuya = new(Db.MasterShips[ShipId.KumanoCVLKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
			},
		};

		Assert.False(kumano.CanDoOpeningAsw());
		Assert.False(suzuya.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "CVL 65 ASW condition")]
	public void OpeningAswTest4()
	{
		ShipDataMock zuihou = new(Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12_TomonagaSquadron]),
			},
		};

		Assert.False(zuihou.CanDoOpeningAsw());

		zuihou = new(Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TBM3W_3S]),
			},
		};

		Assert.True(zuihou.CanDoOpeningAsw());

		zuihou = new(Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
			},
		};

		Assert.True(zuihou.CanDoOpeningAsw());

		zuihou = new(Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW]),
			},
		};

		Assert.True(zuihou.CanDoOpeningAsw());

		zuihou = new(Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_SuiseiModel22_634AirGroup]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12_MurataSquadron]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Ju87CKaiNi_KMXSkilled]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron]),
			},
		};

		Assert.False(zuihou.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "CVL 100 ASW condition")]
	public void OpeningAswTest5()
	{
		ShipDataMock zuihou = new(Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			ASWModernized = 9,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
			},
		};

		Assert.False(zuihou.CanDoOpeningAsw());

		zuihou = new(Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			ASWModernized = 9,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_FM2Wildcat]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SonarLarge_Type0PassiveSONAR]),
			},
		};

		Assert.True(zuihou.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "Hyuuga condition")]
	public void OpeningAswTest6()
	{
		ShipDataMock hyuuga = new(Db.MasterShips[ShipId.HyuugaKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi]),
			},
		};

		Assert.False(hyuuga.CanDoOpeningAsw());

		hyuuga = new(Db.MasterShips[ShipId.HyuugaKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_OTypeObservationAutogyroKaiNi]),
			},
		};

		Assert.True(hyuuga.CanDoOpeningAsw());

		hyuuga = new(Db.MasterShips[ShipId.HyuugaKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51J]),
			},
		};

		Assert.True(hyuuga.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "Oilers")]
	public void OpeningAswTest7()
	{
		ShipDataMock souya = new(Db.MasterShips[ShipId.Souya699])
		{
			Level = 175,
			ASWModernized = 50,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_Type3ActiveSONAR]),
			},
		};

		Assert.True(souya.CanDoOpeningAsw());

		ShipDataMock hayasui = new(Db.MasterShips[ShipId.HayasuiKai])
		{
			Level = 175,
			ASWModernized = 50,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_Type3ActiveSONAR]),
			},
		};

		Assert.True(hayasui.CanDoOpeningAsw());

		ShipDataMock yamashioMaru = new(Db.MasterShips[ShipId.YamashioMaruKai])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_Type3ActiveSONAR]),
			},
		};

		Assert.True(yamashioMaru.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "CVL with 100+ ASW with 65ASW Condition")]
	public void OpeningASWTest8()
	{
		ShipDataMock gambierbay = new(Db.MasterShips[ShipId.GambierBayMkII])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TBM3W_3S]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.AviationPersonnel_SkilledDeckPersonnel_AviationMaintenancePersonnel]),
			},
		};

		Assert.True(gambierbay.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "Housho k2s with 0 slot no OASW")]
	public void OpeningASWTtest9()
	{
		ShipDataMock houshok2s = new(Db.MasterShips[ShipId.HoushouKaiNiSen])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TBM3W_3S]),
			}
		};
		Assert.False(houshok2s.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "Housho k2s with 0 slot And 1 aircraft OASW")]
	public void OpeningASWTest10()
	{
		ShipDataMock houshok2s = new(Db.MasterShips[ShipId.HoushouKaiNiSen])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_SuiseiModel12_634AirGroupwType3ClusterBombs]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
			}
		};
		Assert.True(houshok2s.CanDoOpeningAsw());
	}

	[Fact(DisplayName = "Emptied carrier can't attack subs")]
	public void OpeningASWTest11()
	{
		ShipDataMock zuihok2b = new (Db.MasterShips[ShipId.ZuihouKaiNiB])
		{
			Level = 175,
			Aircraft = new List<int> { 0, 0, 0, 0, 0, },
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TBM3W_3S]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Sonar_Type3ActiveSONAR])

			}
		};
		Assert.False(zuihok2b.CanAttackSubmarine());
		Assert.False(zuihok2b.CanDoOpeningAsw());
	}
}
