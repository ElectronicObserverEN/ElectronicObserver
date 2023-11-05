using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 水上部隊 vs 通常艦隊 昼戦 <br />
/// api_req_combined_battle/battle_water
/// </summary>
public sealed class BattleCombinedWater : CombinedDayBattleData
{
	public override string Title => BattleRes.SuijouButaiDayBattle;

	public BattleCombinedWater(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleBattleWaterResponse battle)
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
