﻿using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Serialization.FitBonus;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests.FitBonus;

[Collection(DatabaseCollection.Name)]
public class TorpedoFitBonusTest(DatabaseFixture db) : FitBonusTest(db)
{
	[Fact(DisplayName = "One 21inch 4-tube Bow Torpedo Launcher (Initial Model) on Scamp")]
	public void TorpedoFitBonusTest1()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData scamp = new ShipDataMock(Db.MasterShips[ShipId.ScampKai])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SubmarineTorpedo_21inch4tubeBowTorpedoLauncher_InitialModel])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Torpedo = 1,
			Evasion = 2
		};

		FitBonusValue finalBonus = scamp.GetTheoreticalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "Two 21inch 4-tube Bow Torpedo Launcher (Initial Model) on Scamp (Bonus don't stack)")]
	public void TorpedoFitBonusTest2()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData scamp = new ShipDataMock(Db.MasterShips[ShipId.ScampKai])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SubmarineTorpedo_21inch4tubeBowTorpedoLauncher_InitialModel]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SubmarineTorpedo_21inch4tubeBowTorpedoLauncher_InitialModel])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Torpedo = 1,
			Evasion = 2
		};

		FitBonusValue finalBonus = scamp.GetTheoreticalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "21inch 4-tube Bow Torpedo Launcher (Initial Model) & (Late Model) on Scamp (Bonus don't stack)")]
	public void TorpedoFitBonusTest3()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData scamp = new ShipDataMock(Db.MasterShips[ShipId.ScampKai])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SubmarineTorpedo_21inch4tubeBowTorpedoLauncher_InitialModel]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SubmarineTorpedo_21inch4tubeBowTorpedoLauncher_LateModel])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Torpedo = 1,
			Evasion = 2
		};

		FitBonusValue finalBonus = scamp.GetTheoreticalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}
}
