using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeDayViewModel
{
	public string DisplayValue { get; set; }

	public DayOfWeek DayValue { get; set; }

	public SolidColorBrush Background => new(BackgroundColor);
	private Color BackgroundColor { get; set; }

	public List<IShipDataMaster> Helpers { get; set; }

	private TimeChangeService TimeService { get; } = Ioc.Default.GetService<TimeChangeService>()!;

	public EquipmentUpgradeDayViewModel(DayOfWeek day, List<int> helpers)
	{
		KCDatabase db = KCDatabase.Instance;

		DayValue = day;
		Helpers = helpers.Select(id => db.MasterShips[id]).ToList();

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
		BackgroundColor = Helpers.Any() switch
		{
			true => DayValue == TimeService.CurrentDayOfWeekJST ? Color.FromArgb(255, 30, 142, 255) : Color.FromArgb(153, 65, 65, 247),
			_ => Color.FromArgb(0, 0, 0, 0),
		};
	}
}
