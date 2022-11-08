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
	private ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; set; } = new();
	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgradesFilteredAndSorted { get; set; } = new();

	public EquipmentUpgradePlannerTranslationViewModel EquipmentUpgradePlanner { get; set; } = new();

	private readonly EquipmentPickerService EquipmentPicker;

	public EquipmentUpgradePlannerViewModel()
	{
		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;
	}

	public override void Loaded()
	{
		base.Loaded();
		PlannedUpgrades = KCDatabase.Instance.EquipmentUpgradePlanManager.PlannedUpgrades;
		PlannedUpgrades.CollectionChanged += (_, _) => Update();
		Update();
	}

	[ICommand]
	public void AddEquipmentPlan()
	{
		IEquipmentData? equipment = EquipmentPicker.OpenEquipmentPicker();

		if (equipment != null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = KCDatabase.Instance.EquipmentUpgradePlanManager.AddPlan();

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.Equipment = equipment;
		}
	}

	[ICommand]
	public void RemovePlan(EquipmentUpgradePlanItemViewModel planToRemove)
	{
		KCDatabase.Instance.EquipmentUpgradePlanManager.RemovePlan(planToRemove);
	}

	private void Update()
	{
		PlannedUpgradesFilteredAndSorted.Clear();

		// TODO : add a way to hide finished
		foreach (EquipmentUpgradePlanItemViewModel plan in PlannedUpgrades.OrderBy(plan => plan.Finished))
		{
			PlannedUpgradesFilteredAndSorted.Add(plan);
		}
	}
}
