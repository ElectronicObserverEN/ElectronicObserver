using System;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseNightBattleDefender
{
	public BattleIndex Defender { get; set; }
	public HitType CriticalFlag { get; set; }
	public double RawDamage { get; set; }
	public bool GuardsFlagship => RawDamage != Math.Floor(RawDamage);
	public int Damage => (int)RawDamage;
}
