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
		EquipmentAssignmentItemViewModel => $"{Count}x {Plan.EquipmentMasterData?.NameEN} ({Translations.AlreadyOwned})",

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

	public bool CanAssignEquipment => Plan is EquipmentConversionPlanItemViewModel or EquipmentCraftPlanItemViewModel && UpgradePlanHasBeenSavedInDatabase;
	public bool CanAssignEquipmentToUpgrade => CanAssignEquipment && UpgradePlan?.EquipmentMasterDataId == Plan.EquipmentMasterDataId && !EquipmentToUpgradeHasBeenSet;
	public bool CanAssignEquipmentToConsume => CanAssignEquipment && UpgradePlan?.Cost.RequiredEquipments.Any(eq => eq.Equipment.EquipmentId == Plan.EquipmentMasterDataId) is true;

	public bool CanAddPlanAndAssignEquipment => Plan is EquipmentConversionPlanItemViewModel or EquipmentCraftPlanItemViewModel && !UpgradePlanHasBeenSavedInDatabase;
	public bool CanAddPlanAndAssignEquipmentToUpgrade => CanAddPlanAndAssignEquipment && UpgradePlan?.EquipmentMasterDataId == Plan.EquipmentMasterDataId && !EquipmentToUpgradeHasBeenSet;
	public bool CanAddPlanAndAssignEquipmentToConsume => CanAddPlanAndAssignEquipment && UpgradePlan?.Cost.RequiredEquipments.Any(eq => eq.Equipment.EquipmentId == Plan.EquipmentMasterDataId) is true;

	private bool EquipmentToUpgradeHasBeenSet => UpgradePlan switch
	{
		not null => EquipmentUpgradePlanManager.GetAssignments(UpgradePlan.Plan).Any(plan => plan.WillBeUsedForConversion),
		_ => false
	};

	public bool UpgradePlanHasBeenSavedInDatabase => UpgradePlan is not null && EquipmentUpgradePlanManager.PlannedUpgrades.Contains(UpgradePlan);

	public EquipmentUpgradePlanItemViewModel? UpgradePlan => Plan switch
	{
		EquipmentConversionPlanItemViewModel conversionPlan => conversionPlan.EquipmentToUpgradePlan,
		EquipmentCraftPlanItemViewModel craftPlan => craftPlan.PlannedToBeUsedBy,
		EquipmentUpgradePlanItemViewModel upgradePlan => upgradePlan,
		_ => null
	};

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
		foreach (IEquipmentPlanItemViewModel planChild in Plan.GetPlanChildren())
		{
			UpgradeTreeUpgradePlanViewModel child = new(planChild);
			Children.Add(child);

			if (planChild is EquipmentAssignmentItemViewModel or EquipmentConversionPlanItemViewModel or EquipmentCraftPlanItemViewModel)
			{
				child.PropertyChanged += (_, args) =>
				{
					if (args.PropertyName is not nameof(Children)) return;
					EquipmentPlanHasChanged();
				};
			}
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
		CleanupUnusedPlan();
		Children.Clear();
		OnPropertyChanged(nameof(DisplayName));

		Initialize();

		OnPropertyChanged(nameof(CanBePlanned));
		OnPropertyChanged(nameof(AlreadyPlanned));
	}

	[RelayCommand]
	private void UnAssignEquipmentToPlan()
	{
		if (Plan is not EquipmentAssignmentItemViewModel plan) return;
		if (plan.AssignedPlan is null) return;

		EquipmentUpgradePlanManager.RemoveAssignment(plan);
		EquipmentUpgradePlanManager.Save();

		OnPropertyChanged(nameof(Children));
	}

	[RelayCommand]
	private void AssignEquipmentToUpgradeToPlan() => AssignEquipmentToPlan(true);

	[RelayCommand]
	private void AssignEquipmentToPlan() => AssignEquipmentToPlan(false);

	private void AssignEquipmentToPlan(bool willBeUsedForConversion)
	{
		if (UpgradePlan is null) return;

		EquipmentAssignmentItemViewModel assignmentViewModel = new(new()
		{
			EquipmentMasterDataId = Plan.EquipmentMasterDataId,
			Plan = UpgradePlan.Plan,
			WillBeUsedForConversion = willBeUsedForConversion
		});

		assignmentViewModel.AssignedPlan = UpgradePlan;

		assignmentViewModel.OpenEquipmentPicker();

		if (assignmentViewModel.AssignedEquipment is not null && assignmentViewModel.TrySaveChanges())
		{
			EquipmentUpgradePlanManager.AddAssignment(assignmentViewModel);
			EquipmentUpgradePlanManager.Save();
			OnPropertyChanged(nameof(Children));
		}
	}

	[RelayCommand]
	private void CreatePlanAndAssignEquipmentToUpgrade() => CreatePlanAndAssignEquipment(true);

	[RelayCommand]
	private void CreatePlanAndAssignEquipment() => CreatePlanAndAssignEquipment(false);

	private void CreatePlanAndAssignEquipment(bool willBeUsedForConversion)
	{
		if (UpgradePlan is null) return;

		EquipmentAssignmentItemViewModel assignmentViewModel = new(new()
		{
			EquipmentMasterDataId = Plan.EquipmentMasterDataId,
			Plan = UpgradePlan.Plan,
			WillBeUsedForConversion = willBeUsedForConversion
		});

		assignmentViewModel.AssignedPlan = UpgradePlan;

		assignmentViewModel.OpenEquipmentPicker();

		if (assignmentViewModel.AssignedEquipment is not null && assignmentViewModel.TrySaveChanges())
		{
			if (!UpgradePlan.OpenPlanDialog()) return;

			EquipmentUpgradePlanManager.AddAssignment(assignmentViewModel);
			EquipmentUpgradePlanManager.AddPlan(UpgradePlan);
			EquipmentUpgradePlanManager.Save();

			OnPropertyChanged(nameof(Children));
		}
	}
}
