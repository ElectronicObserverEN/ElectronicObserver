using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class AirBattleData : FirstBattleData, IBaseAirAttack, IAirBattle
{
	protected PhaseJetBaseAirAttack? JetBaseAirAttack { get; }
	public PhaseJetAirBattle? JetAirBattle { get; }
	public PhaseBaseAirAttack? BaseAirAttack { get; }
	protected PhaseFriendlySupportInfo? FriendlySupportInfo { get; }
	protected PhaseFriendlyAirBattle? FriendlyAirBattle { get; }
	public PhaseAirBattle? AirBattle { get; }

	protected AirBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IAirBattleApiResponse battle, bool isMultiAirBattle = false)
		: base(phaseFactory, fleets, battle)
	{
		JetBaseAirAttack = PhaseFactory.JetBaseAirAttack(battle.ApiAirBaseInjection);
		JetAirBattle = PhaseFactory.JetAirBattle(battle.ApiInjectionKouku);
		BaseAirAttack = PhaseFactory.BaseAirAttack(battle.ApiAirBaseAttack);
		FriendlySupportInfo = PhaseFactory.FriendlySupportInfo(battle.ApiFriendlyInfo);
		FriendlyAirBattle = PhaseFactory.FriendlyAirBattle(battle.ApiFriendlyKouku);
		AirBattle = PhaseFactory.AirBattle(battle.ApiKouku, isMultiAirBattle switch
		{
			true => AirPhaseType.First,
			_ => AirPhaseType.Battle,
		});
	}
}
