﻿using System;
using System.Globalization;
using System.Windows.Media;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeHelpersDayViewModel
{
	public string DisplayValue { get; set; }

	public SolidColorBrush Background => new(BackgroundColor);
	private Color BackgroundColor { get; set; }

	public EquipmentUpgradeHelpersDayViewModel(System.DayOfWeek day, bool helperDay)
	{
		DisplayValue = CultureInfo.CurrentCulture.Name switch
		{
			"ja-JP" => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day)[..1],
			_ => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day)[..3]
		};

		BackgroundColor = helperDay switch
		{
			true => day == TimeZoneInfo.ConvertTimeBySystemTimeZoneId(System.DateTime.Today, "Tokyo Standard Time").DayOfWeek ? Color.FromArgb(255, 30, 142, 255) : Color.FromArgb(153, 65, 65, 247),
			_ => Color.FromArgb(0, 0, 0, 0),
		};
	}
}
