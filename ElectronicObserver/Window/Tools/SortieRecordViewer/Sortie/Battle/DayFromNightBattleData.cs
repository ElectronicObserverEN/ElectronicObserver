using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class DayFromNightBattleData : NightOnlyBattleData, IBaseAirAttack, IAirBattle
{
	public bool NextToDay { get; }

	protected PhaseNightBattle? NightBattle { get; }
	protected PhaseNightBattle? NightBattle2 { get; }
	protected PhaseJetBaseAirAttack? JetBaseAirAttack { get; }
	public PhaseJetAirBattle? JetAirBattle { get; }
	public PhaseBaseAirAttack? BaseAirAttack { get; }
	public PhaseAirBattle? AirBattle { get; }
	public PhaseSupport? Support { get; }
	protected PhaseOpeningAsw? OpeningAsw { get; }
	protected PhaseOpeningTorpedo? OpeningTorpedo { get; }
	protected PhaseShelling? Shelling1 { get; }
	protected PhaseShelling? Shelling2 { get; }
	protected PhaseClosingTorpedo? ClosingTorpedo { get; }

	protected DayFromNightBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IDayFromNightBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		NextToDay = battle.ApiDayFlag != 0;

		NightBattle = PhaseFactory.NightBattle(battle.ApiNHougeki1);
		NightBattle2 = PhaseFactory.NightBattle(battle.ApiNHougeki2);
		JetBaseAirAttack = PhaseFactory.JetBaseAirAttack(battle.ApiAirBaseInjection);
		JetAirBattle = PhaseFactory.JetAirBattle(battle.ApiInjectionKouku);
		BaseAirAttack = PhaseFactory.BaseAirAttack(battle.ApiAirBaseAttack);
		AirBattle = PhaseFactory.AirBattle(battle.ApiKouku, AirPhaseType.Battle);
		Support = PhaseFactory.Support(battle.ApiSupportFlag, battle.ApiSupportInfo, false);
		OpeningAsw = PhaseFactory.OpeningAsw(battle.ApiOpeningTaisen);
		OpeningTorpedo = PhaseFactory.OpeningTorpedo(battle.ApiOpeningAtack);
		Shelling1 = PhaseFactory.Shelling(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = PhaseFactory.Shelling(battle.ApiHougeki2, DayShellingPhase.Second);
		ClosingTorpedo = PhaseFactory.ClosingTorpedo(battle.ApiRaigeki);
	}

	protected DayFromNightBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IOpeningTorpedoRework_DayFromNightBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		NextToDay = battle.ApiDayFlag != 0;

		NightBattle = PhaseFactory.NightBattle(battle.ApiNHougeki1);
		NightBattle2 = PhaseFactory.NightBattle(battle.ApiNHougeki2);
		JetBaseAirAttack = PhaseFactory.JetBaseAirAttack(battle.ApiAirBaseInjection);
		JetAirBattle = PhaseFactory.JetAirBattle(battle.ApiInjectionKouku);
		BaseAirAttack = PhaseFactory.BaseAirAttack(battle.ApiAirBaseAttack);
		AirBattle = PhaseFactory.AirBattle(battle.ApiKouku, AirPhaseType.Battle);
		Support = PhaseFactory.Support(battle.ApiSupportFlag, battle.ApiSupportInfo, false);
		OpeningAsw = PhaseFactory.OpeningAsw(battle.ApiOpeningTaisen);
		OpeningTorpedo = PhaseFactory.OpeningTorpedo(battle.ApiOpeningAtack);
		Shelling1 = PhaseFactory.Shelling(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = PhaseFactory.Shelling(battle.ApiHougeki2, DayShellingPhase.Second);
		ClosingTorpedo = PhaseFactory.ClosingTorpedo(battle.ApiRaigeki);
	}
}
