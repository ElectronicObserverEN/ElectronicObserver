using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class BattleNight : BattleData
{
	public PhaseNightInitial NightInitial { get; }
	public PhaseFriendlyShelling FriendlyShelling { get; }
	public PhaseNightBattle NightBattle { get; }

	protected BattleNight(IKCDatabase kcDatabase, BattleFleets fleets, INightBattleApiResponse battle) : base(kcDatabase, fleets, battle)
	{
		NightInitial = new(kcDatabase, fleets, battle);
		FriendlyShelling = new();
		NightBattle = new(battle.ApiHougeki);
	}
}
