using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseFriendlyAirBattle : PhaseAirBattleBase
{
	public override string Title => BattleRes.BattlePhaseFriendlyShelling;

	public PhaseFriendlyAirBattle(IKCDatabase kcDatabase, ApiKouku apiKouku)
		: base(kcDatabase, apiKouku)
	{
	}
}
