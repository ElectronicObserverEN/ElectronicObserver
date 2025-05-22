using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseOpeningAsw : PhaseShelling
{
	public override string Title => BattleRes.BattlePhaseOpeningAsw;

	public PhaseOpeningAsw(IKCDatabase kcDatabase, ApiHougeki1 shellingData)
		: base(kcDatabase, shellingData, DayShellingPhase.Other)
	{
	}
}
