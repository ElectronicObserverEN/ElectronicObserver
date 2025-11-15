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
public class QueenElizabethSpecialAttackTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	private ShipDataMock Warspite => new(Db.MasterShips[ShipId.WarspiteKai]);
	private ShipDataMock Valiant => new(Db.MasterShips[ShipId.ValiantKai]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Ukuru => new(Db.MasterShips[ShipId.UkuruKai]);
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai]);
	
	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void QueenElizabethAttackTriggerTest9()
	{
		FleetDataMock fleet = new();

		QueenElizabethSpecialAttack specialAttack = new(fleet);

		ShipDataMock warspite = new(Db.MasterShips[ShipId.WarspiteKai])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.WarspiteKai].HPMax * 0.33),
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			warspite,
			Valiant,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
	public void QueenElizabethAttackTriggerTest10()
	{
		FleetDataMock fleet = new();

		QueenElizabethSpecialAttack specialAttack = new(fleet);

		ShipDataMock warspite = new(Db.MasterShips[ShipId.WarspiteKai])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.WarspiteKai].HPMax * 0.66),
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			warspite,
			Valiant,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<QueenElizabethSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper chuuha")]
	public void QueenElizabethAttackTriggerTest11()
	{
		FleetDataMock fleet = new();

		QueenElizabethSpecialAttack specialAttack = new(fleet);

		ShipDataMock valiant = new(Db.MasterShips[ShipId.ValiantKai])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.ValiantKai].HPMax * 0.33),
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Warspite,
			valiant,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<QueenElizabethSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper taiha")]
	public void QueenElizabethAttackTriggerTest12()
	{
		FleetDataMock fleet = new();

		QueenElizabethSpecialAttack specialAttack = new(fleet);

		ShipDataMock valiant = new(Db.MasterShips[ShipId.ValiantKai])
		{
			HPCurrent = 1,
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Warspite,
			valiant,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Valiant & Warspite")]
	public void QueenElizabethAttackTriggerTest13()
	{
		FleetDataMock fleet = new();

		QueenElizabethSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Valiant,
			Warspite,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis,
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<QueenElizabethSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Damage - Warspite & Valiant")]
	public void QueenElizabethAttackDamage1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.WarspiteKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
					},
				},
				new ShipDataMock(Db.MasterShips[ShipId.ValiantKai])
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

		Assert.Equal(1.86, specialAttacksHits[0].PowerModifier, 2);
		Assert.Equal(1.86, specialAttacksHits[1].PowerModifier, 2);
		Assert.Equal(1.86, specialAttacksHits[2].PowerModifier, 2);
	}

	[Fact(DisplayName = "Damage - Warspite has no radar & Valiant has one")]
	public void QueenElizabethAttackDamage2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.WarspiteKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
					},
				},
				new ShipDataMock(Db.MasterShips[ShipId.ValiantKai])
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

		Assert.Equal(1.62, specialAttacksHits[0].PowerModifier, 2);
		Assert.Equal(1.62, specialAttacksHits[1].PowerModifier, 2);
		Assert.Equal(1.86, specialAttacksHits[2].PowerModifier, 2);
	}
}
