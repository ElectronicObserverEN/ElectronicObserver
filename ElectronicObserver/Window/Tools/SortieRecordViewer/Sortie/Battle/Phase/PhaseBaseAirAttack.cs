using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseBaseAirAttack : PhaseBase
{
	public override string Title => BattleRes.BattlePhaseLandBasedAir;

	public List<PhaseBaseAirAttackUnit> Units { get; } = new();

	public PhaseBaseAirAttack(IKCDatabase kcDatabase, List<ApiAirBaseAttack> apiAirBaseAttack)
	{
		foreach (PhaseBaseAirAttackUnit attackUnit in apiAirBaseAttack
			.Select((ab, i) => new PhaseBaseAirAttackUnit(kcDatabase, ab, i)))
		{
			Units.Add(attackUnit);
		}
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
