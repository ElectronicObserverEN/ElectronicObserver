using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Properties.Window;
using ElectronicObserver.Services;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public partial class EquipmentUpgradePlannerViewModel : WindowViewModelBase
{
	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; set; } = new();

	public EquipmentUpgradePlanner EquipmentUpgradePlanner { get; set; } = new();

	private readonly EquipmentPickerService EquipmentPicker;

	public EquipmentUpgradePlannerViewModel()
	{
		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;
	}

	public override void Loaded()
	{
		base.Loaded();
		PlannedUpgrades = KCDatabase.Instance.EquipmentUpgradePlanManager.PlannedUpgrades;
	}

	[ICommand]
	public void AddEquipmentPlan()
	{
		IEquipmentData? equipment = EquipmentPicker.OpenEquipmentPicker();

		if (equipment != null) PlannedUpgrades.Add(new EquipmentUpgradePlanItemViewModel(equipment)
		{
			// Use a setting to set default level ?
			DesiredUpgradeLevel = UpgradeLevel.Max
		});
	}
}
