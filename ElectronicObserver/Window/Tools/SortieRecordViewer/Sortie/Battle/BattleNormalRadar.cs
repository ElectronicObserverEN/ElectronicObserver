using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdShooting;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常艦隊 vs 通常艦隊 レーダー射撃 <br />
/// api_req_sortie/ld_shooting
/// </summary>
public sealed class BattleNormalRadar : RadarBattleData
{
	public override string Title => ConstantsRes.Title_NormalRadar;

	public BattleNormalRadar(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqSortieLdShootingResponse battle)
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
