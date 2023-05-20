using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseOpeningAsw : PhaseShelling
{
	public override string Title => BattleRes.BattlePhaseOpeningAsw;

	public PhaseOpeningAsw(ApiHougeki1? shellingData) : base(shellingData, DayShellingPhase.First)
	{
	}
}
