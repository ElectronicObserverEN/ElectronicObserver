using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Services;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public partial class EquipmentUpgradePlannerViewModel : WindowViewModelBase
{
	private ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; }
	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgradesFilteredAndSorted { get; } = new();

	public EquipmentUpgradePlannerTranslationViewModel EquipmentUpgradePlanner { get; set; } = new();

	private EquipmentPickerService EquipmentPicker { get; }
	private EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }
	public EquipmentUpgradePlanCostViewModel TotalCost { get; set; } = new(new());

	public GridLength PlanListWidth { get; set; } = new GridLength(350, GridUnitType.Pixel);

	public bool DisplayFinished { get; set; } = true;

	public EquipmentUpgradePlannerViewModel()
	{
		EquipmentPicker = Ioc.Default.GetService<EquipmentPickerService>()!;
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		PlannedUpgrades = EquipmentUpgradePlanManager.PlannedUpgrades;
	}

	public override void Loaded()
	{
		base.Loaded();
		PlannedUpgrades.CollectionChanged += (_, _) => Update();
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => Update();
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => UpdateTotalCost();
		EquipmentUpgradePlanManager.PlanCostUpdated += (_, _) => UpdateTotalCost();
		PropertyChanged += EquipmentUpgradePlannerViewModel_PropertyChanged;
		Update();
		UpdateTotalCost();
	}

	public override void Closed()
	{
		base.Closed();
		EquipmentUpgradePlanManager.Save();
	}

	private void EquipmentUpgradePlannerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(DisplayFinished)) return;

		Update();
	}

	[RelayCommand]
	private void AddEquipmentPlan()
	{
		IEquipmentData? equipment = EquipmentPicker.OpenEquipmentPicker();

		if (equipment != null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.AddPlan();

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.EquipmentId = equipment.MasterID;
		}
	}

	[RelayCommand]
	private void AddEquipmentPlanFromMasterData()
	{
		IEquipmentDataMaster? equipment = EquipmentPicker.OpenMasterEquipmentPicker();

		if (equipment != null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.AddPlan();

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.EquipmentMasterDataId = equipment.EquipmentId;
		}
	}

	[RelayCommand]
	private void RemovePlan(EquipmentUpgradePlanItemViewModel planToRemove)
	{
		EquipmentUpgradePlanManager.RemovePlan(planToRemove);
	}


	private void UpdateTotalCost() 
		=> TotalCost = new(PlannedUpgrades
			.Where(plan => !plan.Finished)
			.Aggregate(new EquipmentUpgradePlanCostModel(), (cost, viewModel) => cost + viewModel.Cost.Model, totalCost => totalCost));

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
