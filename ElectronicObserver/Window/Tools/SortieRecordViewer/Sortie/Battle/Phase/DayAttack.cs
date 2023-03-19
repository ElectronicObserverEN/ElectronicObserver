using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class DayAttack
{
	public IShipData Attacker { get; init; }
	public IShipData Defender { get; init; }
	public DayAttackKind AttackKind { get; set; }
	public int Damage { get; set; }
	public bool GuardsFlagship { get; set; }
	public HitType CriticalFlag { get; set; }
}
