using System;
using System.Globalization;
using ElectronicObserver.Common;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradeFilterDayViewModel : CheckBoxEnumViewModel
{
	public EquipmentUpgradeFilterDayViewModel(Enum value) : base(value)
	{
		Configuration.Instance.ConfigurationChanged += () => OnPropertyChanged("");
	}

	public string DisplayValue => CultureInfo.CurrentCulture.Name switch
	{
		"ja-JP" => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)Value)[..1],
		_ => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)Value)[..3]
	};
}
