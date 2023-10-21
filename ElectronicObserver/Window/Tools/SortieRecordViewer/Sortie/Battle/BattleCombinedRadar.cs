using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 連合艦隊 vs 通常艦隊 レーダー射撃 <br />
/// api_req_combined_battle/ld_shooting
/// </summary>
public sealed class BattleCombinedRadar : RadarBattleData
{
	public override string Title => BattleRes.CombinedFleetRadarAmbush;

	public BattleCombinedRadar(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleLdShootingResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		EmulateBattle();
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return JetBaseAirAttack;
		yield return BaseAirAttack;
		yield return Shelling1;
	}
}
