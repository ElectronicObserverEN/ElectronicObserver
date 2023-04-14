using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.AirDefense;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Attacks.Specials;
using ElectronicObserverTypes.Mocks;
using log4net.Core;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class Yamato12SpecialAttackTests
{
	private DatabaseFixture Db { get; }

	private ShipDataMock Yamato => new(Db.MasterShips[ShipId.YamatoKaiNi]);
	private ShipDataMock Musashi => new(Db.MasterShips[ShipId.MusashiKaiNi]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Ukuru => new(Db.MasterShips[ShipId.UkuruKai]);
	private ShipDataMock Furei => new(Db.MasterShips[ShipId.I201Kai])
	{
		MasterID = 1
	};
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai])
	{
		MasterID = 2
	};

	public Yamato12SpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - 6 ships with one sub")]
	public void YamatoAttack12TriggerTest1()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Furei
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships")]
	public void YamatoAttack12TriggerTest2()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with sub escaped")]
	public void YamatoAttack12TriggerTest3()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Furei
		});

		// Furei escaped
		fleet.EscapedShipList = new ReadOnlyCollection<int>(new List<int>()
		{
			Furei.MasterID
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with surface ship escaped")]
	public void YamatoAttack12TriggerTest4()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		// Jervis escaped
		fleet.EscapedShipList = new ReadOnlyCollection<int>(new List<int>()
		{
			Jervis.MasterID
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 7 ships")]
	public void YamatoAttack12TriggerTest5()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		// Strike force
		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Furei,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 7 ships with sub")]
	public void YamatoAttack12TriggerTest8()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		// Strike force
		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Furei,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 7 ships with sub escaped")]
	public void YamatoAttack12TriggerTest6()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		// Strike force
		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Furei,
			Ukuru,
			Jervis
		});

		// Furei escaped
		fleet.EscapedShipList = new ReadOnlyCollection<int>(new List<int>()
		{
			Furei.MasterID
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 7 ships with surface ship escaped")]
	public void YamatoAttack12TriggerTest7()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		// Strike force
		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Furei,
			Ukuru,
			Jervis
		});

		// Jervis escaped
		fleet.EscapedShipList = new ReadOnlyCollection<int>(new List<int>()
		{
			Jervis.MasterID
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with flagship shouha")]
	public void YamatoAttack12TriggerTest9()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		ShipDataMock yamato = Yamato;
		yamato.HPCurrent = 43;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with flagship chuuha")]
	public void YamatoAttack12TriggerTest10()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		ShipDataMock yamato = Yamato;
		yamato.HPCurrent = 63;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			yamato,
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with helper shouha")]
	public void YamatoAttack12TriggerTest11()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		ShipDataMock musashi = Musashi;
		musashi.HPCurrent = 43;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - 6 ships with helper chuuha")]
	public void YamatoAttack12TriggerTest12()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		ShipDataMock musashi = Musashi;
		musashi.HPCurrent = 63;

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Musashi Yamato")]
	public void YamatoAttack12TriggerTest13()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Musashi,
			Yamato,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Yamato Iowa (Valid)")]
	public void YamatoAttack12TriggerTest14()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Yamato,
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<Yamato12SpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Yamato Iowa (Invalid)")]
	public void YamatoAttack12TriggerTest15()
	{
		FleetDataMock fleet = new();

		Yamato12SpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Yamato,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Musashi,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Musashi,
			new ShipDataMock(Db.MasterShips[ShipId.IowaKai]),
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}
}
