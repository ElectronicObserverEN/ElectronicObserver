using System.Collections.Generic;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Serialization.FitBonus;
using ElectronicObserver.Core.Types;

namespace ElectronicObserverCoreTests.FitBonus;

[Collection(DatabaseCollection.Name)]
public class AviationPersonnelFitBonusTest(DatabaseFixture db) : FitBonusTest(db)
{
	[Fact(DisplayName = "Skilled Deck Personnel + Aviation Maintenance Personnel")]
	public void FitBonusTest()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData zuiho = new ShipDataMock(Db.MasterShips[ShipId.Zuihou])
		{
			Level = 180,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.AviationPersonnel_SkilledDeckPersonnel_AviationMaintenancePersonnel])
				{
					Level = 10
				},
			}
		};

		FitBonusValue expectedBonus = new()
		{
			Firepower = 3,
			Evasion = 2,
			AntiAir = 1,
			Accuracy = 2,
			Torpedo = 1,
			Bombing = 1,
		};

		FitBonusValue finalBonus = zuiho.GetTheoreticalFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}
}
