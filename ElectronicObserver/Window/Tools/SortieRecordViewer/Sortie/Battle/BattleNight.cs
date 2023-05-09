using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class BattleNight : BattleData
{
	protected PhaseNightInitial NightInitial { get; }
	protected PhaseFriendlyShelling? FriendlyShelling { get; }
	protected PhaseNightBattle? NightBattle { get; }

	protected BattleNight(IKCDatabase kcDatabase, BattleFleets fleets, INightBattleApiResponse battle) 
		: base(kcDatabase, fleets, battle)
	{
		NightInitial = new(kcDatabase, fleets, battle);
		FriendlyShelling = GetFriendlyShellingPhase(battle.ApiFriendlyBattle);
		NightBattle = GetNightBattlePhase(battle.ApiHougeki);
	}

	private static PhaseFriendlyShelling? GetFriendlyShellingPhase(ApiFriendlyBattle? a) => a switch
	{
		null => null,
		_ => new(a),
	};

	private static PhaseNightBattle? GetNightBattlePhase(ApiHougeki? a) => a switch
	{
		null => null,
		_ => new(a),
	};
}
