using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseJetAirBattle(IKCDatabase kcDatabase, ApiInjectionKouku apiInjectionKouku)
	: PhaseAirBattleBase(kcDatabase, apiInjectionKouku)
{
	public override string Title => BattleRes.BattlePhaseJet;
}
