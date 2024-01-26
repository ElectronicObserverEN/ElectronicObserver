using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks.Specials;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests.SpecialAttacks;

[Collection(DatabaseCollection.Name)]
public class Yamato12SpecialAttackTests
{
	private DatabaseFixture Db { get; }

	private ShipDataMock Yamato => new(Db.MasterShips[ShipId.YamatoKaiNi]);
	private ShipDataMock Musashi => new(Db.MasterShips[ShipId.MusashiKaiNi]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Ukuru => new(Db.MasterShips[ShipId.UkuruKai]);
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai]);

	public Yamato12SpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - 6 ships with flagship chuuha")]
	public void YamatoAttack12TriggerTest9()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		var yamato = Yamato;
		yamato.HPCurrent = 43;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with flagship shouha")]
	public void YamatoAttack12TriggerTest10()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		var yamato = Yamato;
		yamato.HPCurrent = 63;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with helper chuuha")]
	public void YamatoAttack12TriggerTest11()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		var musashi = Musashi;
		musashi.HPCurrent = 43;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with helper shouha")]
	public void YamatoAttack12TriggerTest12()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		var musashi = Musashi;
		musashi.HPCurrent = 63;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Musashi Yamato")]
	public void YamatoAttack12TriggerTest13()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Musashi,
			Yamato,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Yamato Iowa (Valid)")]
	public void YamatoAttack12TriggerTest14()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Yamato Iowa (Invalid)")]
	public void YamatoAttack12TriggerTest15()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Yamato,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Musashi,
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Damage - Yamato Musashi")]
	public void YamatoAttack12AttackDamage1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Musashi,
				Hachijou,
				Kamikaze,
				Ukuru,
				Jervis
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		static void FlagshipPowerMod(SpecialAttackHit hit) => Assert.Equal(1.54, hit.PowerModifier, 3);
		static void HelperPowerMod(SpecialAttackHit hit) => Assert.Equal(1.86, hit.PowerModifier, 3);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Collection(specialAttacksHits, FlagshipPowerMod, FlagshipPowerMod, HelperPowerMod);
	}

	[Fact(DisplayName = "Damage - Yamato Musashi with radar")]
	public void YamatoAttack12AttackDamage2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
					}
				},
				new ShipDataMock(Db.MasterShips[ShipId.MusashiKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
					}
				},
				Hachijou,
				Kamikaze,
				Ukuru,
				Jervis
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		static void FlagshipPowerMod(SpecialAttackHit hit) => Assert.Equal(1.948, hit.PowerModifier, 3);
		static void HelperPowerMod(SpecialAttackHit hit) => Assert.Equal(2.353, hit.PowerModifier, 3);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Collection(specialAttacksHits, FlagshipPowerMod, FlagshipPowerMod, HelperPowerMod);
	}

	[Fact(DisplayName = "Damage - Yamato Musashi with AP shell")]
	public void YamatoAttack12AttackDamage4()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
					},
					FirepowerFit = 2
				},
				new ShipDataMock(Db.MasterShips[ShipId.MusashiKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
					},
					FirepowerFit = 2
				},
				Hachijou,
				Kamikaze,
				Ukuru,
				Jervis
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		static void FlagshipPowerMod(SpecialAttackHit hit) => Assert.Equal(2.079, hit.PowerModifier, 3);
		static void HelperPowerMod(SpecialAttackHit hit) => Assert.Equal(2.511, hit.PowerModifier, 3);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Collection(specialAttacksHits, FlagshipPowerMod, FlagshipPowerMod, HelperPowerMod);

		Assert.Equal(336, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet));
		Assert.Equal(336, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet));
		Assert.Equal(409, fleet.MembersInstance[1]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet));

		Assert.Equal(326, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(326, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
		Assert.Equal(366, fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet), 3);
	}

	[Fact(DisplayName = "Damage - Yamato Musashi with radar and AP shell")]
	public void YamatoAttack12AttackDamage3()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShell]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
					}
				},
				new ShipDataMock(Db.MasterShips[ShipId.MusashiKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShell]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
					}
				},
				Hachijou,
				Kamikaze,
				Ukuru,
				Jervis
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		static void FlagshipPowerMod(SpecialAttackHit hit) => Assert.Equal(2.63, hit.PowerModifier, 3);
		static void HelperPowerMod(SpecialAttackHit hit) => Assert.Equal(3.176, hit.PowerModifier, 3);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Collection(specialAttacksHits, FlagshipPowerMod, FlagshipPowerMod, HelperPowerMod);
	}
}
