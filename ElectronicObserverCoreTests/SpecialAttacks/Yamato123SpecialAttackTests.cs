using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks.Specials;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Core.Types.Attacks.Specials;
using Xunit;

namespace ElectronicObserverCoreTests.SpecialAttacks;

[Collection(DatabaseCollection.Name)]
public class Yamato123SpecialAttackTests
{
	private DatabaseFixture Db { get; }

	private ShipDataMock Yamato => new(Db.MasterShips[ShipId.YamatoKaiNi]);
	private ShipDataMock Ise => new(Db.MasterShips[ShipId.IseKaiNi]);
	private ShipDataMock Hyuuga => new(Db.MasterShips[ShipId.HyuugaKaiNi]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai]);

	public Yamato123SpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - 6 ships with flagship chuuha")]
	public void YamatoAttack123TriggerTest9()
	{
		FleetDataMock fleet = new();

		Yamato123SpecialAttack specialAttack = new(fleet);

		var yamato = Yamato;
		yamato.HPCurrent = 43;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			yamato,
			Ise,
			Hyuuga,
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with flagship shouha")]
	public void YamatoAttack123TriggerTest10()
	{
		FleetDataMock fleet = new();

		Yamato123SpecialAttack specialAttack = new(fleet);

		var yamato = Yamato;
		yamato.HPCurrent = 63;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			yamato,
			Ise,
			Hyuuga,
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato123SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with helper chuuha")]
	public void YamatoAttack123TriggerTest11()
	{
		FleetDataMock fleet = new();

		Yamato123SpecialAttack specialAttack = new(fleet);

		var ise = Ise;
		ise.HPCurrent = 26;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			ise,
			Hyuuga,
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with helper shouha")]
	public void YamatoAttack123TriggerTest12()
	{
		FleetDataMock fleet = new();

		Yamato123SpecialAttack specialAttack = new(fleet);

		var ise = Ise;
		ise.HPCurrent = 51;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			ise,
			Hyuuga,
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato123SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Yamato Musashi Nagato")]
	public void YamatoAttack123TriggerTest13()
	{
		FleetDataMock fleet = new();

		Yamato123SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			new ShipDataMock(Db.MasterShips[ShipId.MusashiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.NagatoKaiNi]),
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Equal(2, fleet.GetSpecialAttacks().Count);
		Assert.Contains(new Yamato123SpecialAttack(fleet), fleet.GetSpecialAttacks());
		Assert.Contains(new Yamato12SpecialAttack(fleet), fleet.GetSpecialAttacks());
		Assert.True(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			new ShipDataMock(Db.MasterShips[ShipId.NagatoKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.MusashiKaiNi]),
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Yamato Ise Hyuuga (Valid)")]
	public void YamatoAttack123TriggerTest14()
	{
		FleetDataMock fleet = new();

		Yamato123SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Ise,
			Hyuuga,
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato123SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Hyuuga,
			Ise,
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato123SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Yamato Ise Hyuuga (Invalid)")]
	public void YamatoAttack123TriggerTest15()
	{
		FleetDataMock fleet = new();

		Yamato123SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Ise,
			Yamato,
			Hyuuga,
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Ise,
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai]),
			Kamikaze,
			Hachijou,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Damage - Yamato Nagato Mutsu")]
	public void YamatoAttack123AttackDamage1()
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
				new ShipDataMock(Db.MasterShips[ShipId.NagatoKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
					},
					FirepowerFit = 2
				},
				new ShipDataMock(Db.MasterShips[ShipId.MutsuKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
					},
					FirepowerFit = 2
				},
				Hachijou,
				Kamikaze,
				Jervis
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2.228, specialAttacksHits[0].PowerModifier, 3);
		Assert.Equal(2.228, specialAttacksHits[1].PowerModifier, 3);
		Assert.Equal(2.228, specialAttacksHits[2].PowerModifier, 3);

		Assert.Equal(360, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet));
		Assert.Equal(302, fleet.MembersInstance[1]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet));
		Assert.Equal(302, fleet.MembersInstance[2]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet));

		Assert.Equal(349, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(291, fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
		Assert.Equal(291, fleet.MembersInstance[2]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet), 3);
	}

	[Fact(DisplayName = "Damage - Yamato Colorado Maryland")]
	public void YamatoAttack123AttackDamage2()
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
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
					},
					FirepowerFit = 2
				},
				new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
					},
					FirepowerFit = 1
				},
				new ShipDataMock(Db.MasterShips[ShipId.MarylandKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
					},
					FirepowerFit = 1
				},
				Hachijou,
				Kamikaze,
				Jervis
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2.562, specialAttacksHits[0].PowerModifier, 3);
		Assert.Equal(1.725, specialAttacksHits[1].PowerModifier, 3);
		Assert.Equal(1.897, specialAttacksHits[2].PowerModifier, 3);

		Assert.Equal(425, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet));
		Assert.Equal(194, fleet.MembersInstance[1]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet));
		Assert.Equal(216, fleet.MembersInstance[2]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet));

		Assert.Equal(367, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(186, fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
		Assert.Equal(206, fleet.MembersInstance[2]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet), 3);
	}
}
