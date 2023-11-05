using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class DayFromNightBattleData : NightOnlyBattleData
{
	protected PhaseNightBattle? NightBattle { get; }
	protected PhaseNightBattle? NightBattle2 { get; }
	protected PhaseJetBaseAirAttack? JetBaseAirAttack { get; }
	protected PhaseJetAirBattle? JetAirBattle { get; }
	protected PhaseBaseAirAttack? BaseAirAttack { get; }
	protected PhaseAirBattle? AirBattle { get; }
	protected PhaseSupport? Support { get; }
	protected PhaseOpeningAsw? OpeningAsw { get; }
	protected PhaseTorpedo? OpeningTorpedo { get; }
	protected PhaseShelling? Shelling1 { get; }
	protected PhaseShelling? Shelling2 { get; }
	protected PhaseTorpedo? Torpedo { get; }

	protected DayFromNightBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IDayFromNightBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		NightBattle = PhaseFactory.NightBattle(battle.ApiNHougeki1);
		NightBattle2 = PhaseFactory.NightBattle(battle.ApiNHougeki2);
		JetBaseAirAttack = PhaseFactory.JetBaseAirAttack(battle.ApiAirBaseInjection);
		JetAirBattle = PhaseFactory.JetAirBattle(battle.ApiInjectionKouku);
		BaseAirAttack = PhaseFactory.BaseAirAttack(battle.ApiAirBaseAttack);
		AirBattle = PhaseFactory.AirBattle(battle.ApiKouku, AirPhaseType.Battle);
		Support = PhaseFactory.Support(battle.ApiSupportFlag, battle.ApiSupportInfo, false);
		OpeningAsw = PhaseFactory.OpeningAsw(battle.ApiOpeningTaisen);
		OpeningTorpedo = PhaseFactory.Torpedo(battle.ApiOpeningAtack, TorpedoPhase.Opening);
		Shelling1 = PhaseFactory.Shelling(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = PhaseFactory.Shelling(battle.ApiHougeki2, DayShellingPhase.Second);
		Torpedo = PhaseFactory.Torpedo(battle.ApiRaigeki, TorpedoPhase.Closing);
	}
}
