using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattle : PhaseAirBattleBase
{
	private AirPhaseType Type { get; }
	public string Title => Type switch
	{
		AirPhaseType.First => BattleRes.BattlePhaseAirAttackFirst,
		AirPhaseType.Second => BattleRes.BattlePhaseAirAttackSecond,
		AirPhaseType.Raid => BattleRes.BattlePhaseAirRaid,
		_ => BattleRes.BattlePhaseAirBattle,
	};

	public PhaseAirBattle(ApiKouku airBattleData, AirPhaseType type) : base(airBattleData)
	{
		Type = type;
	}
}
