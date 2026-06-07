using System;
using ElectronicObserver.Common;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradeFilterDayViewModel : CheckBoxEnumViewModel
{
	public EquipmentUpgradeFilterDayViewModel(Enum value) : base(value)
	{
		Configuration.Instance.ConfigurationChanged += () => OnPropertyChanged("");
	}

	public string DisplayValue => ((DayOfWeek)Value).ToDisplay();
}
