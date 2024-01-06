using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

/// <summary>
/// 通常艦隊 vs 通常艦隊 航空戦 <br />
/// api_req_sortie/airbattle
/// 1-6-F
/// </summary>
public sealed class BattleAirBattle : AirBattleData
{
	public override string Title => ConstantsRes.Title_NormalFleetAirBattle;

	private PhaseSupport? Support { get; }
	private PhaseAirBattle? AirBattle2 { get; }

	public BattleAirBattle(PhaseFactory phaseFactory, BattleFleets fleets, ApiReqSortieAirbattleResponse battle)
		: base(phaseFactory, fleets, battle, true)
	{
		Support = PhaseFactory.Support(battle.ApiSupportFlag, battle.ApiSupportInfo, false);
		AirBattle2 = PhaseFactory.AirBattle(battle.ApiKouku2, AirPhaseType.Second);

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
		yield return AirBattle2;
	}
}
