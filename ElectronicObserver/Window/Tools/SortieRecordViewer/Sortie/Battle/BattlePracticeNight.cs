using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.MidnightBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 演習 夜戦 <br />
/// api_req_practice/midnight_battle
/// </summary>
public sealed class BattlePracticeNight : BattleData
{
	public override string Title => ConstantsRes.Title_PracticeNight;

	private PhaseNightInitial? NightInitial { get; }
	private PhaseNightBattle? NightBattle { get; }

	public BattlePracticeNight(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqPracticeMidnightBattleResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		NightInitial = PhaseFactory.NightInitial(fleets, battle);
		NightBattle = PhaseFactory.NightBattle(battle.ApiHougeki);
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return NightInitial;
		yield return NightBattle;
	}
}
