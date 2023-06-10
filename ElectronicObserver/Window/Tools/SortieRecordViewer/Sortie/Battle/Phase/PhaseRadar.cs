using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseRadar : PhaseShelling
{
	public PhaseRadar(ApiHougeki1? shellingData) : base(shellingData, DayShellingPhase.First)
	{
	}
}
