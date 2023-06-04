using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;
using ElectronicObserver.Properties.Data;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleCombinedNormalNight : BattleNight
{
	public override string Title => BattleRes.CombinedFleetNightBattle;

	public BattleCombinedNormalNight(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqCombinedBattleMidnightBattleResponse battle)
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
		yield return NightInitial;
		yield return FriendlySupportInfo;
		yield return FriendlyShelling;
		yield return NightBattle;
	}
}
