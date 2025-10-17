using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseJetAirBattle(IKCDatabase kcDatabase, ApiInjectionKouku apiInjectionKouku)
	: PhaseAirBattleBase(kcDatabase, apiInjectionKouku)
{
	public override string Title => BattleRes.BattlePhaseJet;
}
