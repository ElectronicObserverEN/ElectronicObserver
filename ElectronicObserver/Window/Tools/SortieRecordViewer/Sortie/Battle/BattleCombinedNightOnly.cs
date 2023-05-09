using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleCombinedNightOnly : BattleNight
{
	public BattleCombinedNightOnly(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqCombinedBattleSpMidnightResponse battle)
		: base(kcDatabase, fleets, battle)
	{
		foreach (PhaseBase phase in Phases)
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return NightInitial;
		yield return FriendlySupportInfo;
		yield return FriendlyShelling;
		yield return Support;
		yield return NightBattle;
	}
}
