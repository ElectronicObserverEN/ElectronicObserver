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
public class ColoradoSpecialAttackTests
{
	private DatabaseFixture Db { get; }

	public ColoradoSpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
	public void ColoradoSpecialAttackTest1()
	{
		FleetDataMock fleet = new();

		ColoradoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.ColoradoKai].HPMax * 0.33)
			},
			new ShipDataMock(Db.MasterShips[ShipId.ContediCavournuovo]),
			new ShipDataMock(Db.MasterShips[ShipId.MarylandKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void ColoradoSpecialAttackTest2()
	{
		FleetDataMock fleet = new();

		ColoradoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.ColoradoKai].HPMax * 0.66)
			},
			new ShipDataMock(Db.MasterShips[ShipId.ContediCavournuovo]),
			new ShipDataMock(Db.MasterShips[ShipId.MarylandKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<ColoradoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper chuuha")]
	public void ColoradoSpecialAttackTest6()
	{
		FleetDataMock fleet = new();

		ColoradoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.MarylandKai])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.MarylandKai].HPMax * 0.33),
			},
			new ShipDataMock(Db.MasterShips[ShipId.ContediCavournuovo])
			{
				HPCurrent = (int)(Db.MasterShips[ShipId.MarylandKai].HPMax * 0.33),
			},
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<ColoradoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper taiha")]
	public void ColoradoSpecialAttackTest3()
	{
		FleetDataMock fleet = new();

		ColoradoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.MarylandKai])
			{
				HPCurrent = 1,
			},
			new ShipDataMock(Db.MasterShips[ShipId.ContediCavournuovo])
			{
				HPCurrent = 1,
			},
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HibikiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Aviation Battleship as helper")]
	public void ColoradoSpecialAttackTest4()
	{
		FleetDataMock fleet = new();

		ColoradoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IseKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.HyuugaKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<ColoradoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Light Cruiser as helper")]
	public void ColoradoSpecialAttackTest5()
	{
		FleetDataMock fleet = new();

		ColoradoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.MarylandKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KumaKaiNiD]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.KumaKaiNiD]),
			new ShipDataMock(Db.MasterShips[ShipId.MarylandKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}
}
