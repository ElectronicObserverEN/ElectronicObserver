namespace ElectronicObserverCoreTests.FitBonus;

public abstract class FitBonusTest(DatabaseFixture db)
{
	protected DatabaseFixture Db { get; } = db;

	protected ElectronicObserver.Data.Translation.FitBonusData BonusData { get; } = new();
}
