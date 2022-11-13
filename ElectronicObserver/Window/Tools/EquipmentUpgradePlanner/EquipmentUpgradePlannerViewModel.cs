using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public partial class EquipmentUpgradePlannerViewModel : WindowViewModelBase
{
	private ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; set; } = new();
	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgradesFilteredAndSorted { get; set; } = new();

	public EquipmentUpgradePlannerTranslationViewModel EquipmentUpgradePlanner { get; set; } = new();

	private EquipmentPickerService EquipmentPicker { get; }

	public bool DisplayFinished { get; set; } = true;

	public EquipmentUpgradePlannerViewModel()
	{
		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;
	}

	public override void Loaded()
	{
		base.Loaded();
		PlannedUpgrades = KCDatabase.Instance.EquipmentUpgradePlanManager.PlannedUpgrades;
		PlannedUpgrades.CollectionChanged += (_, _) => Update();
		KCDatabase.Instance.EquipmentUpgradePlanManager.PlanFinished += (_, _) => Update();
		PropertyChanged += EquipmentUpgradePlannerViewModel_PropertyChanged;
		Update();
	}

	private void EquipmentUpgradePlannerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(DisplayFinished)) return;

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
			newPlan.EquipmentId = equipment.MasterID;
		}
	}

	[ICommand]
	public void AddEquipmentPlanFromMasterData()
	{
		IEquipmentDataMaster? equipment = EquipmentPicker.OpenMasterEquipmentPicker();

		if (equipment != null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = KCDatabase.Instance.EquipmentUpgradePlanManager.AddPlan();

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.EquipmentMasterDataId = equipment.EquipmentId;
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

		List<EquipmentUpgradePlanItemViewModel> plans = PlannedUpgrades
			.Where(plan => DisplayFinished || !plan.Finished)
			.OrderBy(plan => plan.Finished)
			.ToList();

		foreach (EquipmentUpgradePlanItemViewModel plan in plans)
		{
			PlannedUpgradesFilteredAndSorted.Add(plan);
		}
	}
}
