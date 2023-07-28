using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class SurfaceShipCountTests
{
	private DatabaseFixture Db { get; }

	private ShipDataMock Yamato => new(Db.MasterShips[ShipId.YamatoKaiNi]);
	private ShipDataMock Ise => new(Db.MasterShips[ShipId.IseKaiNi]);
	private ShipDataMock Hyuuga => new(Db.MasterShips[ShipId.HyuugaKaiNi]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Ukuru => new(Db.MasterShips[ShipId.UkuruKai]);
	private ShipDataMock Furei => new(Db.MasterShips[ShipId.I201Kai]);
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai]);

	public SurfaceShipCountTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "6 ships with one sub")]
	public void SurfaceShipCountTest1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Hachijou,
				Furei,
			}),
		};

		Assert.Equal(5, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}

	[Fact(DisplayName = "6 ships")]
	public void SurfaceShipCountTest2()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Hachijou,
				Jervis,
			}),
		};

		Assert.Equal(6, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}

	[Fact(DisplayName = "6 ships with sub escaped")]
	public void SurfaceShipCountTest3()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Hachijou,
				Furei,
			}),

			// Furei escaped
			EscapedShipList = new ReadOnlyCollection<int>(new List<int>
			{
				5,
			}),
		};

		Assert.Equal(5, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}

	[Fact(DisplayName = "6 ships with surface ship escaped")]
	public void SurfaceShipCountTest4()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Hachijou,
				Jervis,
			}),

			// Jervis escaped
			EscapedShipList = new ReadOnlyCollection<int>(new List<int>
			{
				5,
			}),
		};

		Assert.Equal(5, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}

	[Fact(DisplayName = "7 ships")]
	public void SurfaceShipCountTest5()
	{
		FleetDataMock fleet = new()
		{
			// Strike force
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Ukuru,
				Hachijou,
				Jervis,
			}),
		};

		Assert.Equal(7, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}

	[Fact(DisplayName = "7 ships with sub")]
	public void SurfaceShipCountTest8()
	{
		FleetDataMock fleet = new()
		{
			// Strike force
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Furei,
				Hachijou,
				Jervis,
			}),
		};

		Assert.Equal(6, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}

	[Fact(DisplayName = "7 ships with sub escaped")]
	public void SurfaceShipCountTest6()
	{
		FleetDataMock fleet = new()
		{
			// Strike force
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Furei,
				Hachijou,
				Jervis,
			}),

			// Furei escaped
			EscapedShipList = new ReadOnlyCollection<int>(new List<int>
			{
				4,
			}),
		};

		Assert.Equal(6, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}

	[Fact(DisplayName = "7 ships with surface ship escaped")]
	public void SurfaceShipCountTest7()
	{
		FleetDataMock fleet = new()
		{
			// Strike force
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				Yamato,
				Ise,
				Hyuuga,
				Kamikaze,
				Furei,
				Hachijou,
				Jervis,
			}),

			// Jervis escaped
			EscapedShipList = new ReadOnlyCollection<int>(new List<int>
			{
				6,
			}),
		};

		Assert.Equal(5, fleet.NumberOfSurfaceShipNotRetreatedNotSunk());
	}
}
