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
}
