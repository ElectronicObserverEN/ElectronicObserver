using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IApiJetAirBattle
{
	List<List<int>?> ApiPlaneFrom { get; set; }
	ApiStage1And2Jet? ApiStage1 { get; set; }
	ApiStage1And2Jet? ApiStage2 { get; set; }
	ApiStage3Jet? ApiStage3 { get; set; }
	ApiStage3JetCombined? ApiStage3Combined { get; set; }
}
