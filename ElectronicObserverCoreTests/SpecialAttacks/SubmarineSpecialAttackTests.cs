﻿using System;
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
public class SubmarineSpecialAttackTests
{
	private DatabaseFixture Db { get; }

	public SubmarineSpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void SubmarineSpecialAttackTest1()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
				HPCurrent = (int)(Db.MasterShips[ShipId.Taigei].HPMax * 0.33)
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<SubmarineSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship taiha")]
	public void SubmarineSpecialAttackTest2()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
				HPCurrent = 1
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - One sub chuuha")]
	public void SubmarineSpecialAttackTest3()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai])
			{
				HPCurrent =  (int)(Db.MasterShips[ShipId.I13Kai].HPMax * 0.3)
			},
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<SubmarineSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Level 29 AS")]
	public void SubmarineSpecialAttackTest8()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 29,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}


	[Fact(DisplayName = "Can trigger - Two sub chuuha")]
	public void SubmarineSpecialAttackTest4()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai])
			{
				HPCurrent =  (int)(Db.MasterShips[ShipId.I203Kai].HPMax * 0.3)
			},
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai])
			{
				HPCurrent =  (int)(Db.MasterShips[ShipId.I13Kai].HPMax * 0.3)
			},
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Attackers indexes - 2 subs")]
	public void SubmarineSpecialAttackTest5()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
		});

		Assert.Equal(3, specialAttack.GetAttacks().Count);

		List<int> expectedIndexes = new() { 0, 1, 2 };
		List<Action<SpecialAttackHit>> elementInspector = expectedIndexes
			.Select(index => new Action<SpecialAttackHit>((hit) => Assert.Equal(index, hit.ShipIndex)))
			.ToList();

		Assert.Collection(specialAttack.GetAttacks(), elementInspector.ToArray());
	}

	[Fact(DisplayName = "Attackers indexes - 3 subs")]
	public void SubmarineSpecialAttackTest6()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai]),
		});

		Assert.Equal(3, specialAttack.GetAttacks().Count);

		List<int> expectedIndexes = new() { 0, 1, 3 };
		List<Action<SpecialAttackHit>> elementInspector = expectedIndexes
			.Select(index => new Action<SpecialAttackHit>((hit) => Assert.Equal(index, hit.ShipIndex)))
			.ToList();

		Assert.Collection(specialAttack.GetAttacks(), elementInspector.ToArray());
	}

	[Fact(DisplayName = "Attackers indexes - 3 subs, 1 chuuha")]
	public void SubmarineSpecialAttackTest7()
	{
		FleetDataMock fleet = new();

		SubmarineSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai])
			{
				HPCurrent =  (int)(Db.MasterShips[ShipId.I13Kai].HPMax * 0.3)
			},
		});

		Assert.Equal(3, specialAttack.GetAttacks().Count);

		List<int> expectedIndexes = new() { 0, 1, 2 };
		List<Action<SpecialAttackHit>> elementInspector = expectedIndexes
			.Select(index => new Action<SpecialAttackHit>((hit) => Assert.Equal(index, hit.ShipIndex)))
			.ToList();

		Assert.Collection(specialAttack.GetAttacks(), elementInspector.ToArray());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai])
			{
				HPCurrent =  (int)(Db.MasterShips[ShipId.I203Kai].HPMax * 0.3)
			},
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai]),
		});

		Assert.Equal(3, specialAttack.GetAttacks().Count);

		expectedIndexes = new() { 0, 1, 3 };
		elementInspector = expectedIndexes
			.Select(index => new Action<SpecialAttackHit>((hit) => Assert.Equal(index, hit.ShipIndex)))
			.ToList();

		Assert.Collection(specialAttack.GetAttacks(), elementInspector.ToArray());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Taigei])
			{
				Level = 99,
			},
			new ShipDataMock(Db.MasterShips[ShipId.I201Kai])
			{
				HPCurrent =  (int)(Db.MasterShips[ShipId.I201Kai].HPMax * 0.3)
			},
			new ShipDataMock(Db.MasterShips[ShipId.I203Kai]),
			new ShipDataMock(Db.MasterShips[ShipId.I13Kai]),
		});

		Assert.Equal(3, specialAttack.GetAttacks().Count);

		expectedIndexes = new() { 0, 2, 3 };
		elementInspector = expectedIndexes
			.Select(index => new Action<SpecialAttackHit>((hit) => Assert.Equal(index, hit.ShipIndex)))
			.ToList();

		Assert.Collection(specialAttack.GetAttacks(), elementInspector.ToArray());
	}
}
