using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcNightToDay;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常/連合艦隊 vs 連合艦隊　夜昼戦 <br />
/// api_req_combined_battle/ec_night_to_day
/// </summary>
public sealed class BattleEnemyCombinedDayFromNight : DayFromNightBattleData
{
	public override string Title => BattleRes.EnemyCombinedFleetNightDayBattle;

	public BattleEnemyCombinedDayFromNight(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleEcNightToDayResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		EmulateBattle();
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return NightInitial;
		yield return FriendlySupportInfo;
		yield return FriendlyShelling;
		yield return NightSupport;
		yield return NightBattle;
		yield return NightBattle2;
		yield return JetBaseAirAttack;
		yield return JetAirBattle;
		yield return BaseAirAttack;
		yield return Support;
		yield return AirBattle;
		yield return OpeningAsw;
		yield return OpeningTorpedo;
		yield return Shelling1;
		yield return Shelling2;
		yield return Torpedo;
	}
}
