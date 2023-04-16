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
public class NagatoSpecialAttackTests
{
	private DatabaseFixture Db { get; }

	private ShipDataMock Nagato => new(Db.MasterShips[ShipId.NagatoKaiNi]);
	private ShipDataMock Mutsu => new(Db.MasterShips[ShipId.MutsuKaiNi]);
	private ShipDataMock Kamikaze => new(Db.MasterShips[ShipId.KamikazeKai]);
	private ShipDataMock Hachijou => new(Db.MasterShips[ShipId.HachijouKai]);
	private ShipDataMock Ukuru => new(Db.MasterShips[ShipId.UkuruKai]);
	private ShipDataMock Jervis => new(Db.MasterShips[ShipId.JervisKai]);

	public NagatoSpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
	public void NagatoAttackTriggerTest9()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock nagato = new(Db.MasterShips[ShipId.NagatoKaiNi])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.NagatoKaiNi].HPMax * 0.33)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			nagato,
			Mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
	public void NagatoAttackTriggerTest10()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock nagato = new(Db.MasterShips[ShipId.NagatoKaiNi])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.NagatoKaiNi].HPMax * 0.66)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			nagato,
			Mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NagatoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper chuuha")]
	public void NagatoAttackTriggerTest11()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock mutsu = new(Db.MasterShips[ShipId.MutsuKaiNi])
		{
			HPCurrent = (int)(Db.MasterShips[ShipId.MutsuKaiNi].HPMax * 0.33)
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Nagato,
			mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NagatoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Helper taiha")]
	public void NagatoAttackTriggerTest12()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		ShipDataMock mutsu = new(Db.MasterShips[ShipId.MutsuKaiNi])
		{
			HPCurrent = 1
		};

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Nagato,
			mutsu,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Empty(fleet.GetSpecialAttacks());
		Assert.False(specialAttack.CanTrigger());
	}

	[Fact(DisplayName = "Can trigger - Mutsu Nagato")]
	public void NagatoAttackTriggerTest13()
	{
		FleetDataMock fleet = new();

		NagatoSpecialAttack specialAttack = new(fleet);

		fleet.MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
		{
			Mutsu,
			Nagato,
			Hachijou,
			Kamikaze,
			Ukuru,
			Jervis
		});

		Assert.Single(fleet.GetSpecialAttacks());
		Assert.IsType<NagatoSpecialAttack>(fleet.GetSpecialAttacks().First());
		Assert.True(specialAttack.CanTrigger());
	}
}
