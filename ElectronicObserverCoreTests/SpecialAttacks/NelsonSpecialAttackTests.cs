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
public class NelsonSpecialAttackTests
{
	private DatabaseFixture Db { get; }

	public NelsonSpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void NelsonSpecialAttackTest1()
	{
		FleetDataMock fleet = new();

		NelsonSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.NelsonKai])
			{
				HPCurrent = 31
			},
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
	public void NelsonSpecialAttackTest2()
	{
		FleetDataMock fleet = new();

		NelsonSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.NelsonKai])
			{
				HPCurrent = 61
			},
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NelsonSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper taiha")]
	public void NelsonSpecialAttackTest3()
	{
		FleetDataMock fleet = new();

		NelsonSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.NelsonKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai])
			{
				HPCurrent = 1,
			},
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai])
			{
				HPCurrent = 1,
			},
			new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NelsonSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Submarine as helper")]
	public void NelsonSpecialAttackTest4()
	{
		FleetDataMock fleet = new();

		NelsonSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.NelsonKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.NelsonKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Carrier as helper")]
	public void NelsonSpecialAttackTest5()
	{
		FleetDataMock fleet = new();

		NelsonSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.NelsonKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ZuihouKaiNiB]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.NelsonKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ZuihouKaiNiB]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Damage - Nelson touch")]
	public void NelsonAttackDamage1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.NelsonKai]),
				new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
				new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
				new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
				new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
				new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(2, specialAttacksHits[0].PowerModifier, 3);
		Assert.Equal(2, specialAttacksHits[1].PowerModifier, 3);
		Assert.Equal(2, specialAttacksHits[2].PowerModifier, 3);

		Assert.Equal(238, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet));
		Assert.Equal(80, fleet.MembersInstance[2]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet));
		Assert.Equal(84, fleet.MembersInstance[4]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet));

		Assert.Equal(228, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(70, fleet.MembersInstance[2]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
		Assert.Equal(74, fleet.MembersInstance[4]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet), 3);
	}
}
