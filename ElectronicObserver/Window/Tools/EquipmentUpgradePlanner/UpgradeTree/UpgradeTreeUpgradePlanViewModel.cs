using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public partial class UpgradeTreeUpgradePlanViewModel : ObservableObject
{
	public EquipmentId EquipmentId => Plan.EquipmentMasterDataId;

	public int Count => Plan switch
	{
		EquipmentUpgradePlanItemViewModel => RequiredCount,
		EquipmentConversionPlanItemViewModel conversion => conversion.EquipmentRequiredForUpgradePlan.Count,
		EquipmentCraftPlanItemViewModel => RequiredCount,

		_ => throw new NotImplementedException(),
	};

	public string DisplayName => Plan switch
	{
		EquipmentUpgradePlanItemViewModel plan => $"{Count}x {plan.EquipmentName} ({Translations.Goal}: {plan.DesiredUpgradeLevel.Display()})",
		EquipmentConversionPlanItemViewModel => $"{Count}x {Plan.EquipmentMasterData?.NameEN}",
		EquipmentCraftPlanItemViewModel => $"{Count}x {Plan.EquipmentMasterData?.NameEN}",

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

	private IEquipmentPlanItemViewModel Plan { get; set; }

	private int RequiredCount { get; }

	private EquipmentUpgradeData EquipmentUpgradeData { get; }

	private EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public ObservableCollection<UpgradeTreeUpgradePlanViewModel> Children { get; } = new();

	public EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public UpgradeTreeUpgradePlanViewModel(EquipmentUpgradePlanItemViewModel plan, int required)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

		RequiredCount = required;
		Plan = plan;

		Initialize(plan);
	}

	public UpgradeTreeUpgradePlanViewModel(EquipmentConversionPlanItemViewModel plan)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

		Plan = plan;
		RequiredCount = 1;

		plan.EquipmentRequiredForUpgradePlan.ForEach(InitializeFromConversion);
	}

	public UpgradeTreeUpgradePlanViewModel(EquipmentCraftPlanItemViewModel plan, int required)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

		RequiredCount = required;
		Plan = plan;
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

	private void InitializeFromConversion(EquipmentUpgradePlanItemViewModel plan)
	{
		Children.Add(new UpgradeTreeUpgradePlanViewModel(plan, 1));
	}

	private void Initialize(EquipmentUpgradePlanItemViewModel plan)
	{
		AddUpgradedEquipmentNodeIfNeeded(plan);

		foreach (EquipmentUpgradePlanCostEquipmentViewModel equipment in plan.Cost.RequiredEquipments)
		{
			Children.Add(InitializeEquipmentPlanChild(plan, equipment.Required, equipment.Equipment.EquipmentId));
		}
	}

	/// <summary>
	/// Adds a node to specify the equipment needs to be crafted / upgraded from something else
	/// </summary>
	/// <param name="plan"></param>
	private void AddUpgradedEquipmentNodeIfNeeded(EquipmentUpgradePlanItemViewModel plan)
	{
		if (plan.EquipmentId is not null) return;

		EquipmentUpgradeDataModel? upgradePlan = GetPlanToMakeEquipmentFromUpgrade(plan.EquipmentMasterDataId);

		if (upgradePlan is not null)
		{
			List<EquipmentUpgradePlanItemViewModel> children =
				EquipmentUpgradePlanManager.PlannedUpgrades
					.Where(p => p.Plan.Parent == plan.Plan)
					.ToList();

			while (children.Count < RequiredCount)
			{
				EquipmentUpgradePlanItemViewModel newChild = EquipmentUpgradePlanManager.MakePlanViewModel(new());
				newChild.EquipmentMasterDataId = (EquipmentId)upgradePlan.EquipmentId;
				newChild.DesiredUpgradeLevel = UpgradeLevel.Conversion;
				newChild.Parent = plan.Plan;
				newChild.ShouldBeConvertedInto = plan.EquipmentMasterDataId;
				children.Add(newChild);
			}

			foreach (EquipmentUpgradePlanItemViewModel child in children)
			{
				Children.Add(new UpgradeTreeUpgradePlanViewModel(new EquipmentConversionPlanItemViewModel(plan, new List<EquipmentUpgradePlanItemViewModel> { child })));
			}
		}
		else
		{
			Children.Add(new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(plan.EquipmentMasterDataId), RequiredCount));
		}
	}

	private UpgradeTreeUpgradePlanViewModel InitializeEquipmentPlanChild(EquipmentUpgradePlanItemViewModel plan, int required, EquipmentId equipment)
	{
		// if fodder = upgraded equipment, return a craft plan to avoid infinite loops
		if (equipment == Plan.EquipmentMasterDataId && plan.ShouldBeConvertedInto == equipment)
		{
			return new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(equipment), required);
		}

		EquipmentUpgradeDataModel? planModel = GetPlanToMakeEquipmentFromUpgrade(equipment);

		// if no plan found : return a new plan if the equipment can be made from conversion
		if (planModel is not null)
		{
			List<EquipmentUpgradePlanItemViewModel> children =
				EquipmentUpgradePlanManager.PlannedUpgrades
					.Where(p => p.Plan.Parent == plan.Plan)
					.ToList();

			while (children.Count < required)
			{
				EquipmentUpgradePlanItemViewModel newChild = EquipmentUpgradePlanManager.MakePlanViewModel(new());
				newChild.EquipmentMasterDataId = (EquipmentId)planModel.EquipmentId;
				newChild.DesiredUpgradeLevel = UpgradeLevel.Conversion;
				newChild.Parent = plan.Plan;
				newChild.ShouldBeConvertedInto = equipment;
				children.Add(newChild);
			}

			return new UpgradeTreeUpgradePlanViewModel(new EquipmentConversionPlanItemViewModel(plan, children));
		}

		// if equipment can't be upgraded, return an empty plan
		// TODO : turn this into a "craft plan" if the equipment is craftable
		// TODO : Could also obtain equipment from ship stock equipments => Should we add something to link ship training plan to upgrade plans ?
		return new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(equipment), required);
	}

	private EquipmentUpgradeDataModel? GetPlanToMakeEquipmentFromUpgrade(EquipmentId equipment)
	{
		EquipmentUpgradeDataModel? upgradePlan = EquipmentUpgradeData.UpgradeList
			.FirstOrDefault(equipmentData => equipmentData.ConvertTo
				.Any(equipmentAfterConvertion =>
					equipmentAfterConvertion.IdEquipmentAfter == (int)equipment));

		return upgradePlan;
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

		EquipmentPlanHasChanged(newPlan);
	}

	[RelayCommand]
	private void EditEquipmentPlan()
	{
		if (Plan is not EquipmentUpgradePlanItemViewModel plan) return;

		if (!plan.OpenPlanDialog()) return;

		EquipmentUpgradePlanManager.Save();

		// Update display
		EquipmentPlanHasChanged(plan);
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

		EquipmentPlanHasChanged(plan);
	}

	private void EquipmentPlanHasChanged(EquipmentUpgradePlanItemViewModel plan)
	{
		Children.Clear();
		OnPropertyChanged(nameof(DisplayName));

		CleanupUnusedPlan();
		Initialize(plan);

		OnPropertyChanged(nameof(CanBePlanned));
		OnPropertyChanged(nameof(AlreadyPlanned));
	}
}
