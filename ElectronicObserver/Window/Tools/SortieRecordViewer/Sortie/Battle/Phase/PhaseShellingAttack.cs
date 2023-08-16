using System.Collections.Generic;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public record PhaseShellingAttack
{
	public BattleIndex Attacker { get; init; }
	public DayAttackKind AttackType { get; init; }

	/// <summary>
	/// Equipments that get displayed on the screen when the attack happens.
	/// </summary>
	public List<int> EquipmentIDs { get; init; }
	public List<PhaseShellingDefender> Defenders { get; init; } = new();
}
