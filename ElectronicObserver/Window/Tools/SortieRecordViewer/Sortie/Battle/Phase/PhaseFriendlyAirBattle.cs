using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseFriendlyAirBattle : PhaseAirBattleBase
{
	public override string Title => BattleRes.BattlePhaseFriendlyShelling;

	public PhaseFriendlyAirBattle(IKCDatabase kcDatabase, ApiKouku apiKouku)
		: base(kcDatabase, apiKouku)
	{
	}
}
