using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class RadarBattleData : FirstBattleData, IBaseAirAttack
{
	protected PhaseJetBaseAirAttack? JetBaseAirAttack { get; }
	public PhaseBaseAirAttack? BaseAirAttack { get; }
	protected PhaseRadar? Shelling1 { get; }

	protected RadarBattleData(PhaseFactory phaseFactory, BattleFleets fleets, IRadarBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		JetBaseAirAttack = PhaseFactory.JetBaseAirAttack(battle.ApiAirBaseInjection);
		BaseAirAttack = PhaseFactory.BaseAirAttack(battle.ApiAirBaseAttack);
		Shelling1 = PhaseFactory.Radar(battle.ApiHougeki1);
	}
}
