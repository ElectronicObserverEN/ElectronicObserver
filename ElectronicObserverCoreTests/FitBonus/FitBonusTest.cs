using System.Threading.Tasks;
using ElectronicObserver.Avalonia.Translation.FitBonus;
using ElectronicObserver.Services;

namespace ElectronicObserverCoreTests.FitBonus;

public abstract class FitBonusTest(DatabaseFixture db)
{
	protected DatabaseFixture Db { get; } = db;

	protected static FitBonusDataService BonusData { get; } = new(null!, new SoftwareUpdaterService(), new EoLogger());

	static FitBonusTest()
	{
		BonusData = new(null!, new SoftwareUpdaterService(), new EoLogger());

		while (BonusData.FitBonusList.Count is 0)
		{
			Task.Delay(10).Wait();
		}
	}
}
