using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleAirBattle : BattleData
{
	public override string Title => ConstantsRes.Title_NormalFleetAirBattle;

	private PhaseJetBaseAirAttack? JetBaseAirAttack { get; }
	private PhaseJetAirBattle? JetAirBattle { get; }
	private PhaseBaseAirAttack? BaseAirAttack { get; }
	private PhaseFriendlyAirBattle FriendlyAirBattle { get; }
	private PhaseAirBattle? AirBattle { get; }
	private PhaseAirBattle? AirBattle2 { get; }

	public BattleAirBattle(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqSortieAirbattleResponse battle)
		: base(kcDatabase, fleets, battle)
	{
		JetBaseAirAttack = null;
		// todo: check if these actually exist
		// JetAirBattle = GetJetAirBattlePhase(battle.ApiInjectionKouku);
		// BaseAirAttack = GetBaseAirAttackPhase(battle.ApiAirBaseAttack);
		FriendlyAirBattle = new();
		AirBattle = GetAirBattlePhase(battle.ApiKouku, AirPhaseType.First);
		AirBattle2 = GetAirBattlePhase(battle.ApiKouku2, AirPhaseType.Second);

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
		yield return FriendlySupportInfo;
		yield return FriendlyAirBattle;
		yield return AirBattle;
		yield return Support;
		yield return AirBattle2;
	}
}
