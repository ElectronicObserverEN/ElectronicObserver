using System;
using System.Globalization;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public static class Extensions
{
	public static string ToDisplay(this DayOfWeek day) => CultureInfo.DefaultThreadCurrentCulture?.Name switch
	{
		"ja-JP" => CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.GetDayName(day)[..1],
		string => CultureInfo.DefaultThreadCurrentCulture.DateTimeFormat.GetDayName(day)[..3],
		_ => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day)[..3],
	};
}
