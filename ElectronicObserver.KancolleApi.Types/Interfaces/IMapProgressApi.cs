using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IMapProgressApi
{
	int ApiNo { get; }
	CellType ApiColorNo { get; set; }
	int ApiEventId { get; set; }
	int ApiEventKind { get; set; }
	ApiHappening? ApiHappening { get; set; }
}
