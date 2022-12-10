using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;

public class EquipmentUpgradeDayViewModel
{
	public string DisplayValue { get; set; }

	public DayOfWeek DayValue { get; set; }

	public SolidColorBrush Background => new(BackgroundColor);
	private Color BackgroundColor { get; set; }

	public List<IShipDataMaster> Helpers { get; set; }

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

		BackgroundColor = helpers.Any() switch
		{
			true => day == TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Today, "Tokyo Standard Time").DayOfWeek ? Color.FromArgb(255, 30, 142, 255) : Color.FromArgb(153, 65, 65, 247),
			_ => Color.FromArgb(0, 0, 0, 0),
		};
	}
}
