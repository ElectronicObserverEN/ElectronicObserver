using System.Collections.Generic;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public record PhaseTorpedoAttack
{
	public BattleIndex Attacker { get; init; }
	public DayAttackKind AttackType { get; init; }
	public List<PhaseShellingDefender> Defenders { get; init; } = new();
}
