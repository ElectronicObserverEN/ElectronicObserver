using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattle : PhaseAirBattleBase
{
	public string Title => BattleRes.BattlePhaseAirBattle;

	public PhaseAirBattle(ApiKouku airBattleData) : base(airBattleData)
	{

	}
}
