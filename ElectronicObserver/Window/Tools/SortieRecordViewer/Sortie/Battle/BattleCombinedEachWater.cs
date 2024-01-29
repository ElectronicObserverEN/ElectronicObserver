using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 水上部隊 vs 連合艦隊 昼戦 <br />
/// api_req_combined_battle/each_battle_water
/// </summary>
public sealed class BattleCombinedEachWater : CombinedDayBattleData
{
	public override string Title => ConstantsRes.Title_CombinedEachWater;

	public BattleCombinedEachWater(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleEachBattleWaterResponse battle)
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
		yield return Shelling3;
		yield return Torpedo;
	}
}
