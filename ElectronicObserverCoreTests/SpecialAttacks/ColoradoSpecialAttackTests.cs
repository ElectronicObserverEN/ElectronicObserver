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
public class ColoradoSpecialAttackTests
{
	private DatabaseFixture Db { get; }

	public ColoradoSpecialAttackTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "Can trigger - Flagship chuuha")]
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

	[Fact(DisplayName = "Can trigger - Flagship shouha")]
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

	[Fact(DisplayName = "Damage - Colorado Maryland Conte")]
	public void ColoradoSpecialAttackDamage1()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.ColoradoKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
					},
					FirepowerFit = 1
				},
				new ShipDataMock(Db.MasterShips[ShipId.MarylandKai])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_SGRadar_LateModel]),
					},
					FirepowerFit = 3
				},
				new ShipDataMock(Db.MasterShips[ShipId.ContediCavournuovo])
				{
					SlotInstance = new List<IEquipmentData?>()
					{
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai]),
					},
				},
				new ShipDataMock(Db.MasterShips[ShipId.HachijouKai]),
				new ShipDataMock(Db.MasterShips[ShipId.ShimushuKai]),
				new ShipDataMock(Db.MasterShips[ShipId.IshigakiKai]),
			})
		};

		List<SpecialAttack> specialAttacks = fleet.GetSpecialAttacks();

		Assert.NotEmpty(specialAttacks);
		Assert.Single(specialAttacks);

		List<SpecialAttackHit> specialAttacksHits = specialAttacks.First().GetAttacks();

		Assert.Equal(1.725, specialAttacksHits[0].PowerModifier, 3);
		Assert.Equal(1.977, specialAttacksHits[1].PowerModifier, 3);
		Assert.Equal(1.755, specialAttacksHits[2].PowerModifier, 3);

		Assert.Equal(194, fleet.MembersInstance[0]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet));
		Assert.Equal(229, fleet.MembersInstance[1]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet));
		Assert.Equal(194, fleet.MembersInstance[2]!.GetDayAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet));

		Assert.Equal(186, fleet.MembersInstance[0]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[0], fleet), 3);
		Assert.Equal(219, fleet.MembersInstance[1]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[1], fleet), 3);
		Assert.Equal(238, fleet.MembersInstance[2]!.GetNightAttackPower(specialAttacks.First(), specialAttacksHits[2], fleet), 3);
	}
}
