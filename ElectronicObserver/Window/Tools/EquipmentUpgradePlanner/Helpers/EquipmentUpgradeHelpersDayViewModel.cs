using System;
using System.Globalization;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Services;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeHelpersDayViewModel
{
	public string DisplayValue { get; set; }

	public DayOfWeek DayValue { get; set; }

	public SolidColorBrush Background => new(BackgroundColor);
	private Color BackgroundColor { get; set; }

	public bool IsHelperDay { get; set; }

	private TimeChangeService TimeService { get; } = Ioc.Default.GetService<TimeChangeService>()!;

	public EquipmentUpgradeHelpersDayViewModel(DayOfWeek day, bool helperDay)
	{
		DayValue = day;
		IsHelperDay = helperDay;

		DisplayValue = CultureInfo.CurrentCulture.Name switch
		{
			"ja-JP" => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day)[..1],
			_ => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day)[..3]
		};

		Update();

		TimeService.DayChanged += () => Update();
	}

	private void Update()
	{
		BackgroundColor = IsHelperDay switch
		{
			true => DayValue == TimeService.CurrentDayOfWeekJST ? Color.FromArgb(255, 30, 142, 255) : Color.FromArgb(153, 65, 65, 247),
			_ => Color.FromArgb(0, 0, 0, 0),
		};
	}
}
