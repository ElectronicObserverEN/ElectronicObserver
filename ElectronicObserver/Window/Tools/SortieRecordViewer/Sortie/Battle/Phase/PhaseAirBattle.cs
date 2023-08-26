using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattle : PhaseAirBattleBase
{
	public override string Title => Type switch
	{
		AirPhaseType.First => BattleRes.BattlePhaseAirAttackFirst,
		AirPhaseType.Second => BattleRes.BattlePhaseAirAttackSecond,
		AirPhaseType.Raid => BattleRes.BattlePhaseAirRaid,
		_ => BattleRes.BattlePhaseAirBattle,
	};

	private AirPhaseType Type { get; }

	public PhaseAirBattle(IKCDatabase kcDatabase, ApiKouku airBattleData, AirPhaseType type)
		: base(kcDatabase, airBattleData)
	{
		Type = type;
	}
}
