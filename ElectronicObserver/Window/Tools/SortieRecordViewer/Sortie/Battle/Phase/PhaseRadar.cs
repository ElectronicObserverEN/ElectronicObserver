using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseRadar : PhaseShelling
{
	public PhaseRadar(IKCDatabase kcDatabase, ApiHougeki1 shellingData)
		: base(kcDatabase, shellingData, DayShellingPhase.First)
	{
	}
}
