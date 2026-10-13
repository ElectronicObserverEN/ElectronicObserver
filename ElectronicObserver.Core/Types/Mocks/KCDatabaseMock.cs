using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types.Mocks;

public class KCDatabaseMock : IKCDatabase
{
	public IDDictionary<IShipDataMaster> MasterShips { get; init; } = [];
	public IDDictionary<IEquipmentDataMaster> MasterEquipments { get; init; } = [];
	public IDDictionary<IUseItemMaster> MasterUseItems { get; init; } = [];
	public IDDictionary<IUseItem> UseItems { get; init; } = [];
	public IDDictionary<IMapInfoData> MapInfo { get; init; } = [];
	public IDDictionary<IMapAreaData> MapArea{ get; init; } = [];
	public IDDictionary<IMissionData> Mission { get; init; } = [];
}
