using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class DayBattleData : AirBattleData
{
	public PhaseSupport? Support { get; }
	protected PhaseOpeningAsw? OpeningAsw { get; }
	protected PhaseOpeningTorpedo? OpeningTorpedo { get; }
	protected PhaseShelling? Shelling1 { get; }
	protected PhaseShelling? Shelling2 { get; }
	protected PhaseClosingTorpedo? ClosingTorpedo { get; }

	protected DayBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IDayBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		Support = PhaseFactory.Support(battle.ApiSupportFlag, battle.ApiSupportInfo, false);
		OpeningAsw = PhaseFactory.OpeningAsw(battle.ApiOpeningTaisen);
		OpeningTorpedo = PhaseFactory.OpeningTorpedo(battle.ApiOpeningAtack);
		Shelling1 = PhaseFactory.Shelling(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = PhaseFactory.Shelling(battle.ApiHougeki2, DayShellingPhase.Second);
		ClosingTorpedo = PhaseFactory.ClosingTorpedo(battle.ApiRaigeki);
	}

	protected DayBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IOpeningTorpedoRework_DayBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		Support = PhaseFactory.Support(battle.ApiSupportFlag, battle.ApiSupportInfo, false);
		OpeningAsw = PhaseFactory.OpeningAsw(battle.ApiOpeningTaisen);
		OpeningTorpedo = PhaseFactory.OpeningTorpedo(battle.ApiOpeningAtack);
		Shelling1 = PhaseFactory.Shelling(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = PhaseFactory.Shelling(battle.ApiHougeki2, DayShellingPhase.Second);
		ClosingTorpedo = PhaseFactory.ClosingTorpedo(battle.ApiRaigeki);
	}
}
