using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 機動部隊 vs 連合艦隊 昼戦 <br />
/// api_req_combined_battle/each_battle
/// </summary>
public sealed class BattleCombinedEachDay : CombinedDayBattleData
{
	public override string Title => ConstantsRes.Title_CombinedEachDay;

	public BattleCombinedEachDay(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleEachBattleResponse battle)
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
		yield return FriendlySupportInfo;
		yield return FriendlyAirBattle;
		yield return AirBattle;
		yield return Support;
		yield return OpeningAsw;
		yield return OpeningTorpedo;
		yield return Shelling1;
		yield return Shelling2;
		yield return Torpedo;
		yield return Shelling3;
	}
}
