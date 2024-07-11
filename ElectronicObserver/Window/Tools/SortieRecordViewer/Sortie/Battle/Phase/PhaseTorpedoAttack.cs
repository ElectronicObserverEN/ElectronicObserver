using System.Collections.Generic;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public record PhaseTorpedoAttack
{
	public required BattleIndex Attacker { get; init; }
	public DayAttackKind AttackType { get; init; }
	public List<PhaseShellingDefender> Defenders { get; init; } = [];
}
