using System.Collections.Generic;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class AirBattleAttack
{
	public AirAttack AttackType { get; init; }
	public List<AirBattleDefender> Defenders { get; init; } = new();
}