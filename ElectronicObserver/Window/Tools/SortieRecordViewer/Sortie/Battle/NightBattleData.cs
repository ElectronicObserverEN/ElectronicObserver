using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public abstract class NightBattleData : FirstBattleData
{
	protected PhaseNightInitial? NightInitial { get; }
	protected PhaseFriendlySupportInfo? FriendlySupportInfo { get; }
	protected PhaseFriendlyShelling? FriendlyShelling { get; }

	protected NightBattleData(PhaseFactory phaseFactory, BattleFleets fleets, INightBattleApiResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		NightInitial = PhaseFactory.NightInitial(fleets, battle);
		FriendlySupportInfo = PhaseFactory.FriendlySupportInfo(battle.ApiFriendlyInfo);
		FriendlyShelling = PhaseFactory.FriendlyShelling(battle.ApiFriendlyBattle);
	}
}
