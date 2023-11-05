using System;
using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常/連合艦隊 vs 通常艦隊 開幕夜戦 <br />
/// api_req_battle_midnight/sp_midnight
/// </summary>
public sealed class BattleNightOnly : NightOnlyBattleData
{
	public override string Title => ConstantsRes.Title_NightOnly;

	private PhaseSupport? Support { get; }
	private PhaseNightBattle? NightBattle { get; }

	public BattleNightOnly(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqBattleMidnightSpMidnightResponse battle) 
		: base(phaseFactory, fleets, battle)
	{
		Support = PhaseFactory.Support(battle.ApiNSupportFlag, battle.ApiNSupportInfo, true);
		NightBattle = PhaseFactory.NightBattle(battle.ApiHougeki);

		EmulateBattle();
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return NightInitial;
		yield return FriendlySupportInfo;
		yield return FriendlyShelling;
		yield return Support;
		yield return NightBattle;
	}
}
