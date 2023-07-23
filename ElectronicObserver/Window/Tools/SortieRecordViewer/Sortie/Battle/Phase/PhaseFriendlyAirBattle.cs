using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseFriendlyAirBattle : PhaseAirBattleBase
{
	public override string Title => BattleRes.BattlePhaseFriendlyShelling;

	public PhaseFriendlyAirBattle(ApiKouku apiKouku) : base(apiKouku)
	{
		
	}
}
