using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class NightAttack
{
	public IShipData Attacker { get; init; }
	public IShipData Defender { get; init; }
	public NightAttackKind AttackKind { get; set; }
	public int Damage { get; set; }
	public bool GuardsFlagship { get; set; }
	public HitType CriticalFlag { get; set; }
}
