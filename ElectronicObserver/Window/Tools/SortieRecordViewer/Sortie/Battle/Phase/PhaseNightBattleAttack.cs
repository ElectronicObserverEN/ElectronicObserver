using System.Collections.Generic;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseNightBattleAttack
{
	public BattleIndex Attacker { get; set; }
	public bool NightAirAttackFlag { get; set; }
	public NightAttackKind AttackType { get; set; }
	public List<int> EquipmentIDs { get; set; }
	public List<PhaseNightBattleDefender> Defenders { get; set; } = new();
}
