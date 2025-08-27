using System;
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
public class KongouSpecialAttackTests
{
	private DatabaseFixture Db { get; }

	public KongouSpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void KongouSpecialAttackTest1()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.KongouKaiNiC].HPMax * 0.33)
			},
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
	public void KongouSpecialAttackTest2()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.KongouKaiNiC].HPMax * 0.66)
			},
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper chuuha")]
	public void KongouSpecialAttackTest6()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC]),
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.HieiKaiNiC].HPMax * 0.33)
			},
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper shouha")]
	public void KongouSpecialAttackTest3()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC]),
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.HieiKaiNiC].HPMax * 0.66)
			},
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Hiei Kongou")]
	public void KongouSpecialAttackTest4()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC]),
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Hiei Haruna")]
	public void KongouSpecialAttackTest5()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC]),
			new ShipDataMock(Db.MasterShips[ShipId.HarunaKaiNiB]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC]),
			new ShipDataMock(Db.MasterShips[ShipId.HarunaKaiNiC]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Kongou Haruna")]
	public void KongouSpecialAttackTest7()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC]),
			new ShipDataMock(Db.MasterShips[ShipId.HarunaKaiNiB]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC]),
			new ShipDataMock(Db.MasterShips[ShipId.HarunaKaiNiC]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Haruna Kongou")]
	public void KongouSpecialAttackTest8()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.HarunaKaiNiB]),
			new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Haruna Hiei")]
	public void KongouSpecialAttackTest9()
	{
		FleetDataMock fleet = new();

		KongouSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.HarunaKaiNiB]),
			new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<KongouSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Damage - Kongou Hiei")]
	public void KongouSpecialAttackDamage1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC]),
				new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC]),
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2.4, specialAttacksHits[0].PowerModifier, 1);
		Assert.Equal(2.4, specialAttacksHits[1].PowerModifier, 1);

		Assert.Equal(Math.Floor(143 * 2.4), fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(Math.Floor(145 * 2.4), fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
	}

	[Fact(DisplayName = "Damage - Gun bonus")]
	public void KongouSpecialAttackDamage2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.KongouKaiNiC])
				{
					SlotInstance = [
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_35_6cmTwinGunMountKaiYon]),
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.HieiKaiNiC])
				{
					SlotInstance = [
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_35_6cmTwinGunMountKaiYon]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_35_6cmTwinGunMountKaiSanC]),
					],
				},
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2.66, specialAttacksHits[0].PowerModifier, 2);
		Assert.Equal(2.76, specialAttacksHits[1].PowerModifier, 2);

		// TODO : the damage value is not checked against any other tools yet
	}
}
