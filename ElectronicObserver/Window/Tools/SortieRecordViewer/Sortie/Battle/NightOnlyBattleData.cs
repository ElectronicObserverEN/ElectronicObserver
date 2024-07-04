using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class NightOnlyBattleData : NightBattleData
{
	public PhaseSupport? NightSupport { get; }

	protected NightOnlyBattleData(PhaseFactory phaseFactory, BattleFleets fleets, INightBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		NightSupport = PhaseFactory.Support(battle.ApiNSupportFlag, battle.ApiNSupportInfo, true);
	}
}
