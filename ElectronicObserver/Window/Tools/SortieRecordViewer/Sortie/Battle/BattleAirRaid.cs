using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleAirRaid : BattleData
{
	private PhaseJetBaseAirAttack JetBaseAirAttack { get; }
	private PhaseJetAirBattle? JetAirBattle { get; }
	private PhaseBaseAirAttack? BaseAirAttack { get; }
	private PhaseAirBattle? AirBattle { get; }

	public BattleAirRaid(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqSortieLdAirbattleResponse battle)
		: base(kcDatabase, fleets, battle)
	{
		JetBaseAirAttack = new();
		// todo: check if these actually exist
		// JetAirBattle = GetJetAirBattlePhase(battle.ApiInjectionKouku);
		// BaseAirAttack = GetBaseAirAttackPhase(battle.ApiAirBaseAttack);
		AirBattle = GetAirBattlePhase(battle.ApiKouku, AirPhaseType.Raid);

		foreach (PhaseBase phase in Phases)
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return JetBaseAirAttack;
		yield return JetAirBattle;
		yield return BaseAirAttack;
		yield return AirBattle;
	}
}
