using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 連合艦隊 vs 通常艦隊 開幕夜戦 <br />
/// api_req_combined_battle/sp_midnight
/// </summary>
public sealed class BattleCombinedNightOnly : NightOnlyBattleData
{
	public override string Title => ConstantsRes.Title_CombinedNightOnly;

	private PhaseSupport? Support { get; }
	private PhaseNightBattle? NightBattle { get; }

	public BattleCombinedNightOnly(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqCombinedBattleSpMidnightResponse battle)
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
