using System;

namespace ElectronicObserver.Core.Types;

public static class NumberExtensions
{
	public static double RoundDown(this double value, int precision = 0)
	{
		double power = Math.Pow(10, precision);
		return Math.Floor(value * power) / power;
	}
}
