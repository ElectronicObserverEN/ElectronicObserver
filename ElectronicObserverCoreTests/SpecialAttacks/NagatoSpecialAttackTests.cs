using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks.Specials;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverCoreTests.SpecialAttacks;

[Collection(DatabaseCollection.Name)]
public class NagatoSpecialAttackTests
{
	private DatabaseFixture Db { get; }

	private ShipDataMock Nagato => new(Db.MasterShips[ShipId.NagatoKaiNi]);
	private ShipDataMock Mutsu => new(Db.MasterShips[ShipId.MutsuKaiNi]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Ukuru => new(Db.MasterShips[ShipId.UkuruKai]);
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai]);

	public NagatoSpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void NagatoAttackTriggerTest9()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock nagato = new(Db.MasterShips[ShipId.NagatoKaiNi])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.NagatoKaiNi].HPMax * 0.33)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			nagato,
			Mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
	public void NagatoAttackTriggerTest10()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock nagato = new(Db.MasterShips[ShipId.NagatoKaiNi])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.NagatoKaiNi].HPMax * 0.66)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			nagato,
			Mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NagatoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper chuuha")]
	public void NagatoAttackTriggerTest11()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock mutsu = new(Db.MasterShips[ShipId.MutsuKaiNi])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.MutsuKaiNi].HPMax * 0.33)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Nagato,
			mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NagatoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper taiha")]
	public void NagatoAttackTriggerTest12()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock mutsu = new(Db.MasterShips[ShipId.MutsuKaiNi])
		{
			HPCurrent = 1
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Nagato,
			mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Mutsu Nagato")]
	public void NagatoAttackTriggerTest13()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Mutsu,
			Nagato,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NagatoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Damage - Nagato Mutsu")]
	public void NagatoAttackDamage1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.NagatoKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
					},
					FirepowerFit = 2
				},
				new ShipDataMock(Db.MasterShips[ShipId.MutsuKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
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

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2.608, specialAttacksHits[0].PowerModifier, 3);
		Assert.Equal(2.608, specialAttacksHits[1].PowerModifier, 3);
		Assert.Equal(2.608, specialAttacksHits[2].PowerModifier, 3);

		Assert.Equal(365, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet));
		Assert.Equal(365, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet));
		Assert.Equal(365, fleet.MembersInstance[1]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet));

		Assert.Equal(352, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(352, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
		Assert.Equal(352, fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet), 3);
	}

	[Fact(DisplayName = "Damage - Nagato Conte")]
	public void NagatoAttackDamage2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.NagatoKaiNi])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
					},
					FirepowerFit = 2
				},
				new ShipDataMock(Db.MasterShips[ShipId.ContediCavournuovo])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_16inchTripleGunMountMk_6_GFCS]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_16inchTripleGunMountMk_6_GFCS]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinderKai_Type21RadarKaiNi_SkilledFDC]),
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

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2.173, specialAttacksHits[0].PowerModifier, 3);
		Assert.Equal(2.173, specialAttacksHits[1].PowerModifier, 3);
		Assert.Equal(1.863, specialAttacksHits[2].PowerModifier, 3);

		Assert.Equal(304, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet));
		Assert.Equal(304, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet));
		Assert.Equal(303, fleet.MembersInstance[1]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet));

		Assert.Equal(293, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(293, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
		Assert.Equal(350, fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet), 3);
	}
}
