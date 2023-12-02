using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public partial class UpgradeTreeUpgradePlanViewModel : ObservableObject
{
	public EquipmentId EquipmentId => Plan.EquipmentMasterDataId;

	public int Count => Plan switch
	{
		EquipmentUpgradePlanItemViewModel => 1,
		EquipmentConversionPlanItemViewModel conversion => conversion.EquipmentRequiredForUpgradePlan.Count,
		EquipmentCraftPlanItemViewModel craftPlan => craftPlan.RequiredCount,
		EquipmentAssignmentItemViewModel => 1,

		_ => throw new NotImplementedException(),
	};

	public string DisplayName => Plan switch
	{
		EquipmentUpgradePlanItemViewModel plan => $"{Count}x {plan.EquipmentName} ({Translations.Goal}: {plan.DesiredUpgradeLevel.Display()})",
		EquipmentConversionPlanItemViewModel => $"{Count}x {Plan.EquipmentMasterData?.NameEN}",
		EquipmentCraftPlanItemViewModel => $"{Count}x {Plan.EquipmentMasterData?.NameEN}",
		EquipmentAssignmentItemViewModel => $"{Count}x {Plan.EquipmentMasterData?.NameEN} (already owned)",

		_ => throw new NotImplementedException(),
	};

	public EquipmentUpgradePlanCostViewModel? Cost => Plan.Cost.Model switch
	{
		{ Fuel: 0, Ammo: 0, Steel: 0, Bauxite: 0 } => null,
		_ => Plan.Cost,
	};

	public bool CanBePlanned => Plan switch
	{
		EquipmentUpgradePlanItemViewModel plan => !EquipmentUpgradePlanManager.PlannedUpgrades.Contains(plan),
		_ => false,
	};

	public bool AlreadyPlanned => Plan is EquipmentUpgradePlanItemViewModel plan && EquipmentUpgradePlanManager.PlannedUpgrades.Contains(plan);

	public bool CanAssignEquipment => Plan is EquipmentConversionPlanItemViewModel or EquipmentCraftPlanItemViewModel;

	public bool AlreadyAssignedAnEquipment => Plan is EquipmentAssignmentItemViewModel;

	private IEquipmentPlanItemViewModel Plan { get; set; }

	public ObservableCollection<UpgradeTreeUpgradePlanViewModel> Children { get; } = new();

	private EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }
	public EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public UpgradeTreeUpgradePlanViewModel(IEquipmentPlanItemViewModel plan)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();

		Plan = plan;

		Initialize();
	}

	/// <summary>
	/// Unsubscribes the plans that aren't saved in db from the API events
	/// </summary>
	public void CleanupUnusedPlan()
	{
		if (Plan is EquipmentUpgradePlanItemViewModel plan)
		{
			if (!EquipmentUpgradePlanManager.PlannedUpgrades.Contains(plan))
			{
				plan.UnsubscribeFromApis();
			}
		}
		else
		{
			Plan.UnsubscribeFromApis();
		}

		foreach (UpgradeTreeUpgradePlanViewModel child in Children)
		{
			child.CleanupUnusedPlan();
		}
	}
	
	private void Initialize()
	{
		foreach (IEquipmentPlanItemViewModel planChild in Plan.GetUpgradePlanChildren())
		{
			Children.Add(new UpgradeTreeUpgradePlanViewModel(planChild));
		}
	}

	[RelayCommand]
	private void AddEquipmentPlan()
	{
		EquipmentUpgradePlanItemViewModel newPlan = Plan switch
		{
			EquipmentUpgradePlanItemViewModel plan => plan,
			_ => throw new NotImplementedException(),
		};

		if (!newPlan.OpenPlanDialog()) return;

		EquipmentUpgradePlanManager.AddPlan(newPlan);
		EquipmentUpgradePlanManager.Save();

		EquipmentPlanHasChanged();
	}

	[RelayCommand]
	private void EditEquipmentPlan()
	{
		if (Plan is not EquipmentUpgradePlanItemViewModel plan) return;

		if (!plan.OpenPlanDialog()) return;

		EquipmentUpgradePlanManager.Save();

		// Update display
		EquipmentPlanHasChanged();
	}

	[RelayCommand]
	private void RemoveEquipmentPlan()
	{
		if (Plan is not EquipmentUpgradePlanItemViewModel plan) return;

		EquipmentUpgradePlanManager.RemovePlan(plan);
		EquipmentUpgradePlanManager.Save();

		plan = EquipmentUpgradePlanManager.MakePlanViewModel(new());
		plan.DesiredUpgradeLevel = UpgradeLevel.Conversion;
		plan.EquipmentMasterDataId = Plan.EquipmentMasterDataId;

		Plan = plan;

		EquipmentPlanHasChanged();
	}

	private void EquipmentPlanHasChanged()
	{
		Children.Clear();
		OnPropertyChanged(nameof(DisplayName));

		CleanupUnusedPlan();
		Initialize();

		OnPropertyChanged(nameof(CanBePlanned));
		OnPropertyChanged(nameof(AlreadyPlanned));
	}

	[RelayCommand]
	private void UnassignEquipmentToPlan()
	{
		if (Plan is not EquipmentAssignmentItemViewModel plan) return;
		if (plan.AssignedPlan is null) return;

		EquipmentUpgradePlanManager.RemoveAssignment(plan);
		EquipmentUpgradePlanManager.Save();

		EquipmentPlanHasChanged();
	}

	[RelayCommand]
	private void AssignEquipmentToPlan()
	{
		EquipmentUpgradePlanItemViewModel plan = Plan switch
		{
			EquipmentUpgradePlanItemViewModel upgradePlan => upgradePlan,
			EquipmentConversionPlanItemViewModel conversion => conversion.EquipmentToUpgradePlan,
			_ => throw new NotImplementedException(),
		};

		EquipmentAssignmentItemViewModel assignmentViewModel = new(new());
		assignmentViewModel.AssignedPlan = plan;

		assignmentViewModel.EquipmentFilter = Children
			.Where(child => child.Plan switch
			{
				EquipmentCraftPlanItemViewModel => true,
				EquipmentConversionPlanItemViewModel p => !p.EquipmentRequiredForUpgradePlan.TrueForAll(EquipmentUpgradePlanManager.PlannedUpgrades.Contains),
				_ => false,
			})
			.Select(p => p.EquipmentId)
			.Distinct()
			.ToList();

		assignmentViewModel.OpenEquipmentPicker();

		if (assignmentViewModel.SaveChanges())
		{
			EquipmentUpgradePlanManager.AddAssignment(assignmentViewModel);
			EquipmentUpgradePlanManager.Save();
			EquipmentPlanHasChanged();
		}
	}
}
