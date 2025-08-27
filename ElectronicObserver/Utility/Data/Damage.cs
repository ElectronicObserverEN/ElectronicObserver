using System;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Utility.Data;

public static class Damage
{
	public static int DayAttackCap => 220;
	public static int NightAttackCap => 360;
	public static int AswAttackCap => 170;
	public static int SupportAttackCap => 170;

	public static double Cap(double damage, double cap)
	{
		if (damage < cap) return damage;

		return cap + Math.Sqrt(damage - cap);
	}

	public static double EngagementDayAttackMod(EngagementType form) => form switch
	{
		EngagementType.Parallel => 1.0,
		EngagementType.HeadOn => 0.8,
		EngagementType.TAdvantage => 1.2,
		EngagementType.TDisadvantage => 0.6,
		_ => 1.0
	};
}
