using ElectronicObserverTypes.Data;

namespace ElectronicObserverTypes.Mocks;

public class KCDatabaseMock : IKCDatabase
{
	public IDDictionary<IShipDataMaster> MasterShips { get; } = new();
}
