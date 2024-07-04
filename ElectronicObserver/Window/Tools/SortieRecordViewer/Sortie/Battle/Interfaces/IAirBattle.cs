using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Interfaces;

public interface IAirBattle
{
	PhaseJetAirBattle? JetAirBattle { get; }
	PhaseAirBattle? AirBattle { get; }
}
