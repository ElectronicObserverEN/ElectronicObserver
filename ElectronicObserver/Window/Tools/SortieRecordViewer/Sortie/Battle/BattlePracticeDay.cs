using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 演習 昼戦 <br />
/// api_req_practice/battle
/// </summary>
public sealed class BattlePracticeDay : FirstBattleData
{
	public override string Title => ConstantsRes.Title_PracticeDay;

	private PhaseJetAirBattle? JetAirBattle { get; }
	private PhaseAirBattle? AirBattle { get; }
	private PhaseOpeningAsw? OpeningAsw { get; }
	private PhaseTorpedo? OpeningTorpedo { get; }
	private PhaseShelling? Shelling1 { get; }
	private PhaseShelling? Shelling2 { get; }
	private PhaseTorpedo? Torpedo { get; }

	public BattlePracticeDay(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqPracticeBattleResponse battle)
		: base(phaseFactory, fleets, battle)
	{
		JetAirBattle = PhaseFactory.JetAirBattle(battle.ApiInjectionKouku);
		AirBattle = PhaseFactory.AirBattle(battle.ApiKouku, AirPhaseType.Battle);
		OpeningAsw = PhaseFactory.OpeningAsw(battle.ApiOpeningTaisen);
		OpeningTorpedo = PhaseFactory.Torpedo(battle.ApiOpeningAtack, TorpedoPhase.Opening);
		Shelling1 = PhaseFactory.Shelling(battle.ApiHougeki1, DayShellingPhase.First);
		Shelling2 = PhaseFactory.Shelling(battle.ApiHougeki2, DayShellingPhase.Second);
		Torpedo = PhaseFactory.Torpedo(battle.ApiRaigeki, TorpedoPhase.Closing);

		EmulateBattle();
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return JetAirBattle;
		yield return AirBattle;
		yield return OpeningAsw;
		yield return OpeningTorpedo;
		yield return Shelling1;
		yield return Shelling2;
		yield return Torpedo;
	}
}
