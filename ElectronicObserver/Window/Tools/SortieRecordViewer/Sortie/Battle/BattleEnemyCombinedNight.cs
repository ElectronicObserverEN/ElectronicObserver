using System;
using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常/連合艦隊 vs 連合艦隊 夜戦 <br />
/// api_req_combined_battle/ec_midnight_battle
/// </summary>
public sealed class BattleEnemyCombinedNight : SecondNightBattleData
{
	public override string Title => ConstantsRes.Title_EnemyCombinedNight;

	public BattleEnemyCombinedNight(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleEcMidnightBattleResponse battle) 
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
