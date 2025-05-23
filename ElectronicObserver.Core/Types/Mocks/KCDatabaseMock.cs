using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types.Mocks;

public class KCDatabaseMock : IKCDatabase
{
	public IDDictionary<IShipDataMaster> MasterShips { get; } = new();
	public IDDictionary<IEquipmentDataMaster> MasterEquipments { get; set; } = new();
	public IDDictionary<IUseItemMaster> MasterUseItems { get; set; } = new();
	public IDDictionary<IUseItem> UseItems { get; set; } = new();
	public IDDictionary<IMapInfoData> MapInfo { get; set; } = new();
}
