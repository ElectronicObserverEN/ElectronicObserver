using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class FitBonusTest
{
	private DatabaseFixture Db { get; }

	private static ElectronicObserver.Data.Translation.FitBonusData BonusData { get; } = new ();

	public FitBonusTest(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void FitBonusTest1()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 175,
			AllSlotInstance = new List<IEquipmentData>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE])
				{
					Level = 10
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
			}
		};

		FitBonusValue expectedBonus = new FitBonusValue()
		{
			Firepower = 2,
			Evasion = 3 + 3 + (2 * 2),
			AntiAir = 2 + (2 * 2),
			ASW = 1 * 2,
			Accuracy = 0,
			Armor = 0,
			LOS = 0,
			Range = 0,
			Torpedo = 0,
		};

		FitBonusValue finalBonus = hachijou.GetFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}
}
