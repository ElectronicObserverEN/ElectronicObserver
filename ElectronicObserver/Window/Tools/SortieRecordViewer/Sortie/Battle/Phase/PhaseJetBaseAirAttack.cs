using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseJetBaseAirAttack : PhaseBase
{
	public override string Title => BattleRes.BattlePhaseLandBasedJet;

	public List<PhaseBaseAirAttackUnit> Units { get; } = new();

	public PhaseJetBaseAirAttack(IKCDatabase kcDatabase, ApiAirBaseInjection apiAirBaseInjection)
	{
		Units.Add(new(kcDatabase, apiAirBaseInjection, 0));
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets, List<int> damages)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		foreach (PhaseBaseAirAttackUnit attackUnit in Units)
		{
			FleetsAfterPhase = attackUnit.EmulateBattle(FleetsAfterPhase, damages);
		}

		return FleetsAfterPhase.Clone();
	}
}
