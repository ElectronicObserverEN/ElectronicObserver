using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IApiAirBattle
{
	List<List<int>?> ApiPlaneFrom { get; set; }
	ApiStage1? ApiStage1 { get; set; }
	ApiStage2? ApiStage2 { get; set; }
	ApiStage3? ApiStage3 { get; set; }
	ApiStage3Combined? ApiStage3Combined { get; set; }
}
