using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常艦隊 vs 通常艦隊 長距離空襲戦 <br />
/// api_req_sortie/ld_airbattle
/// 5-2-C
/// </summary>
public sealed class BattleAirRaid : AirBattleData
{
	public override string Title => ConstantsRes.Title_NormalFleetAirRaid;

	public BattleAirRaid(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqSortieLdAirbattleResponse battle)
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
