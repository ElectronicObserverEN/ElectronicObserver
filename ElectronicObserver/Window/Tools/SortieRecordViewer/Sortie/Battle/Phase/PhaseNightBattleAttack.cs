using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public record PhaseNightBattleAttack
{
	public BattleIndex Attacker { get; init; }
	public bool NightAirAttackFlag { get; set; }
	public NightAttackKind AttackType { get; init; }
	public List<IEquipmentDataMaster> DisplayEquipments { get; set; }
	public List<PhaseNightBattleDefender> Defenders { get; init; } = new();
}
