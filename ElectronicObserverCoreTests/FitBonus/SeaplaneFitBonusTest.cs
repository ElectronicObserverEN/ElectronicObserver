using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Serialization.FitBonus;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests.FitBonus;

[Collection(DatabaseCollection.Name)]
public class SeaplaneFitBonusTest(DatabaseFixture db) : FitBonusTest(db)
{
	[Fact(DisplayName = "Walrus on Kuma (No bonus)")]
	public void SeaplaneFitBonusTest1()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData kuma = new ShipDataMock(Db.MasterShips[ShipId.KumaKai])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Walrus])
			}
		};

		FitBonusValue expectedBonus = new();

		FitBonusValue finalBonus = kuma.GetTheoreticalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "Walrus on a british ship")]
	public void SeaplaneFitBonusTest2()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData sheffield = new ShipDataMock(Db.MasterShips[ShipId.SheffieldKai])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Walrus])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 2,
			ASW = 3,
			LOS = 2,
			Evasion = 2
		};

		FitBonusValue finalBonus = sheffield.GetTheoreticalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact(DisplayName = "Walrus on Nelson-Class")]
	public void SeaplaneFitBonusTest3()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData rodney = new ShipDataMock(Db.MasterShips[ShipId.RodneyKai])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Walrus])
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 6,
			ASW = 3,
			Accuracy = 2,
			LOS = 5,
			Evasion = 4
		};

		FitBonusValue finalBonus = rodney.GetTheoreticalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}
}
