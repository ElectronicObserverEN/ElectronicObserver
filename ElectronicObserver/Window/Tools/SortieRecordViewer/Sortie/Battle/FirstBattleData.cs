using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class FirstBattleData : BattleData
{
	protected PhaseSearching Searching { get; }

	protected FirstBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IFirstBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		Searching = PhaseFactory.Searching(battle);
	}
}
