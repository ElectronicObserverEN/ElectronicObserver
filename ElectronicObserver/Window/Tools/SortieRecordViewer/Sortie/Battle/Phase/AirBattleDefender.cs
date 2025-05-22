using System;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class AirBattleDefender
{
	public BattleIndex Defender { get; init; }
	public HitType CriticalFlag { get; init; }
	public double RawDamage { get; init; }
	public bool GuardsFlagship => RawDamage > Math.Floor(RawDamage);
	public int Damage => (int)RawDamage;
}
