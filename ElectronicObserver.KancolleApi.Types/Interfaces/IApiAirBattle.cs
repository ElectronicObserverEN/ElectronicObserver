using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;
using ApiStage1 = ElectronicObserver.KancolleApi.Types.Models.ApiStage1;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IApiAirBattle
{
	List<List<int>?> ApiPlaneFrom { get; set; }
	ApiStage1? ApiStage1 { get; set; }
	ApiStage? ApiStage2 { get; set; }
	ApiStage3? ApiStage3 { get; set; }
	ApiStage3? ApiStage3Combined { get; set; }
}
