using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常艦隊 vs 連合艦隊 昼戦 <br />
/// api_req_combined_battle/ec_battle
/// </summary>
public sealed class BattleEnemyCombinedDay : CombinedDayBattleData
{
	public override string Title => ConstantsRes.Title_EnemyCombinedDay;

	public BattleEnemyCombinedDay(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleEcBattleResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		EmulateBattle();
	}

	public BattleEnemyCombinedDay(PhaseFactory phaseFactory, BattleFleets fleets, OpeningTorpedoRework_ApiReqCombinedBattleEcBattleResponse battle)
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
		yield return ClosingTorpedo;
		yield return Shelling2;
		yield return Shelling3;
	}
}
