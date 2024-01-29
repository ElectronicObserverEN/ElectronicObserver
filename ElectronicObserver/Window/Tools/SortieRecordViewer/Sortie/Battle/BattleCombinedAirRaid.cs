using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 連合艦隊 vs 通常艦隊 長距離空襲戦 <br />
/// api_req_combined_battle/ld_airbattle
/// </summary>
public sealed class BattleCombinedAirRaid : AirBattleData
{
	public override string Title => ConstantsRes.Title_CombinedFleetAirRaid;

	public BattleCombinedAirRaid(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleLdAirbattleResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		EmulateBattle();
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return JetBaseAirAttack;
		yield return JetAirBattle;
		yield return BaseAirAttack;
		yield return AirBattle;
	}
}
