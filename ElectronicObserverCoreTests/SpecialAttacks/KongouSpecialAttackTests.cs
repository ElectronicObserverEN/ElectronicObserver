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

		Assert.Equal(2.2, specialAttacksHits[0].PowerModifier, 1);
		Assert.Equal(2.2, specialAttacksHits[1].PowerModifier, 1);

		Assert.Equal(314, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(319, fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
	}
}
