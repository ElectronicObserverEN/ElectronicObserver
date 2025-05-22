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
public class RichelieuSpecialAttackTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	private ShipDataMock Richelieu => new(Db.MasterShips[ShipId.RichelieuKai]);
	private ShipDataMock JeanBart => new(Db.MasterShips[ShipId.JeanBartKai]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Ukuru => new(Db.MasterShips[ShipId.UkuruKai]);
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai]);
	
	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void RichelieuAttackTriggerTest9()
	{
		FleetDataMock fleet = new();

		RichelieuSpecialAttack specialAttack = new(fleet);

		ShipDataMock richelieu = new(Db.MasterShips[ShipId.RichelieuKai])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.RichelieuKai].HPMax * 0.33)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			richelieu,
			JeanBart,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
	public void RichelieuAttackTriggerTest10()
	{
		FleetDataMock fleet = new();

		RichelieuSpecialAttack specialAttack = new(fleet);

		ShipDataMock richelieu = new(Db.MasterShips[ShipId.RichelieuKai])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.RichelieuKai].HPMax * 0.66)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			richelieu,
			JeanBart,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<RichelieuSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper chuuha")]
	public void RichelieuAttackTriggerTest11()
	{
		FleetDataMock fleet = new();

		RichelieuSpecialAttack specialAttack = new(fleet);

		ShipDataMock jeanBart = new(Db.MasterShips[ShipId.JeanBartKai])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.JeanBartKai].HPMax * 0.33)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Richelieu,
			jeanBart,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<RichelieuSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper taiha")]
	public void RichelieuAttackTriggerTest12()
	{
		FleetDataMock fleet = new();

		RichelieuSpecialAttack specialAttack = new(fleet);

		ShipDataMock jeanBart = new(Db.MasterShips[ShipId.JeanBartKai])
		{
			HPCurrent = 1,
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Richelieu,
			jeanBart,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Jean Bart & Richelieu")]
	public void RichelieuAttackTriggerTest13()
	{
		FleetDataMock fleet = new();

		RichelieuSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			JeanBart,
			Richelieu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<RichelieuSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Damage - Richelieu & Jean Bart")]
	public void RichelieuAttackDamage1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.RichelieuKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
					},
				},
				new ShipDataMock(Db.MasterShips[ShipId.JeanBartKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
					},
				},
				Hachijou,
				Kamikaze,
				Ukuru,
				Jervis,
			}),
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2.02, specialAttacksHits[0].PowerModifier, 2);
		Assert.Equal(2.02, specialAttacksHits[1].PowerModifier, 2);
		Assert.Equal(1.93, specialAttacksHits[2].PowerModifier, 2);
	}

	[Fact(DisplayName = "Damage - Jean Bart & Richelieu")]
	public void RichelieuAttackDamage2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.JeanBartKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
					},
				},
				new ShipDataMock(Db.MasterShips[ShipId.RichelieuKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
					},
				},
				Hachijou,
				Kamikaze,
				Ukuru,
				Jervis,
			}),
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(1.93, specialAttacksHits[0].PowerModifier, 2);
		Assert.Equal(1.93, specialAttacksHits[1].PowerModifier, 2);
		Assert.Equal(1.93, specialAttacksHits[2].PowerModifier, 2);
	}
}
