using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.NightToDay;
using ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常/連合艦隊 vs 通常艦隊　夜昼戦 <br />
/// api_req_sortie/night_to_day
/// </summary>
public sealed class BattleNormalDayFromNight : DayFromNightBattleData
{
	public override string Title => ConstantsRes.Title_NormalDayFromNight;

	public BattleNormalDayFromNight(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqSortieNightToDayResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		EmulateBattle();
	}

	public BattleNormalDayFromNight(PhaseFactory phaseFactory, BattleFleets fleets, OpeningTorpedoRework_ApiReqSortieNightToDayResponse battle)
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
		yield return ClosingTorpedo;
	}
}
