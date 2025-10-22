using System.Collections.Generic;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseSupportAttack
{
	public SupportType AttackType { get; init; }
	public List<PhaseSupportDefender> Defenders { get; init; } = new();
}
