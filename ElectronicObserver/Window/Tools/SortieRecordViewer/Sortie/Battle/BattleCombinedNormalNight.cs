using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 連合艦隊 vs 通常艦隊 夜戦 <br />
/// api_req_combined_battle/midnight_battle
/// </summary>
public sealed class BattleCombinedNormalNight : SecondNightBattleData
{
	public override string Title => BattleRes.CombinedFleetNightBattle;

	public BattleCombinedNormalNight(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleMidnightBattleResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		EmulateBattle();
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
