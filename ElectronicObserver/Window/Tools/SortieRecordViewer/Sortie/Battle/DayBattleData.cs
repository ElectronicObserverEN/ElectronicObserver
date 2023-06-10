using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class DayBattleData : AirBattleData
{
	protected PhaseSupport? Support { get; }
	protected PhaseOpeningAsw? OpeningAsw { get; }
	protected PhaseTorpedo? OpeningTorpedo { get; }
	protected PhaseShelling? Shelling1 { get; }
	protected PhaseShelling? Shelling2 { get; }
	protected PhaseTorpedo? Torpedo { get; }

	protected DayBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IDayBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		Support = PhaseFactory.Support(battle.ApiSupportFlag, battle.ApiSupportInfo, false);
		OpeningAsw = PhaseFactory.OpeningAsw(battle.ApiOpeningTaisen);
		OpeningTorpedo = PhaseFactory.Torpedo(battle.ApiOpeningAtack, TorpedoPhase.Opening);
		Shelling1 = PhaseFactory.Shelling(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = PhaseFactory.Shelling(battle.ApiHougeki2, DayShellingPhase.Second);
		Torpedo = PhaseFactory.Torpedo(battle.ApiRaigeki, TorpedoPhase.Closing);
	}
}
