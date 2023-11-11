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
	public string DisplayName => Plan switch
	{
		EquipmentCraftPlanItemViewModel => $"{RequiredCount}x {Plan.EquipmentMasterData?.NameEN}",
		EquipmentUpgradePlanItemViewModel plan =>
			$"{RequiredCount}x {plan.EquipmentName} ({Translations.Goal}: {plan.DesiredUpgradeLevel.Display()})",
		_ => ""
	};

	public EquipmentUpgradePlanCostViewModel? Cost =>
		Plan?.Cost.Model is null or { Fuel: 0, Ammo: 0, Steel: 0, Bauxite: 0 } ? null : Plan.Cost;

	public UpgradeTreeViewNodeState State => Plan switch
	{
		not null => UpgradeTreeViewNodeState.Planned,
		_ => UpgradeTreeViewNodeState.ToCraft
	};

	public bool CanBePlanned => Plan switch
	{
		EquipmentUpgradePlanItemViewModel plan => !EquipmentUpgradePlanManager.PlannedUpgrades.Contains(plan),
		not null => GetPlanToMakeEquipmentFromUpgrade(Plan.EquipmentMasterDataId) is not null,
		_ => false
	};

	public bool AlreadyPlanned => Plan is EquipmentUpgradePlanItemViewModel plan && EquipmentUpgradePlanManager.PlannedUpgrades.Contains(plan);

	private IEquipmentPlanItemViewModel? Plan { get; set; }

	public int RequiredCount { get; }
	public EquipmentId? NeedsToBeConvertedInto { get; set; }

	private EquipmentUpgradeData EquipmentUpgradeData { get; }

	private EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public ObservableCollection<UpgradeTreeUpgradePlanViewModel> Children { get; } = new();

	public EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public UpgradeTreeUpgradePlanViewModel(EquipmentUpgradePlanItemViewModel plan, int required, EquipmentId? needsToBeConvertedInto)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

		RequiredCount = required;
		NeedsToBeConvertedInto = needsToBeConvertedInto;
		Plan = plan;

		Initialize(plan);
	}

	public UpgradeTreeUpgradePlanViewModel(EquipmentCraftPlanItemViewModel plan, int required)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

		RequiredCount = required;
		Plan = plan;
	}

	private void Initialize(EquipmentUpgradePlanItemViewModel plan)
	{
		AddUpgradedEquipmentNodeIfNeeded(plan);

		foreach (EquipmentUpgradePlanCostEquipmentViewModel equipment in plan.Cost.RequiredEquipments)
		{
			Children.Add(InitializeEquipmentPlanChild(equipment.Required, equipment.Equipment.EquipmentId));
		}
	}

	/// <summary>
	/// Adds a node to specify the equipment ness to be crafted / upgraded from something else
	/// </summary>
	/// <param name="plan"></param>
	private void AddUpgradedEquipmentNodeIfNeeded(EquipmentUpgradePlanItemViewModel plan)
	{
		if (plan.EquipmentId is not null) return;

		EquipmentUpgradeDataModel? upgradePlan = GetPlanToMakeEquipmentFromUpgrade(plan.EquipmentMasterDataId);

		if (upgradePlan is not null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			newPlan.DesiredUpgradeLevel = UpgradeLevel.Conversion;
			newPlan.EquipmentMasterDataId = (EquipmentId)upgradePlan.EquipmentId;

			Enumerable.Repeat(
				new UpgradeTreeUpgradePlanViewModel(newPlan, 1, plan.EquipmentMasterDataId), RequiredCount)
				.ToList()
				.ForEach(Children.Add);
		}
		else
		{
			Children.Add(new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(plan.EquipmentMasterDataId), RequiredCount));
		}
	}

	private UpgradeTreeUpgradePlanViewModel InitializeEquipmentPlanChild(int required, EquipmentId equipment)
	{
		// if fodder = upgraded equipment, return a craft plan to avoid infinite loops
		if (equipment == Plan?.EquipmentMasterDataId && NeedsToBeConvertedInto == equipment)
		{
			return new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(equipment), required);
		}

		// TODO : look for assigned plan

		// if no plan found : return a new plan if the equipment can be made from conversion
		if (GetPlanToMakeEquipmentFromUpgrade(equipment) is not null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			newPlan.EquipmentMasterDataId = equipment;

			return new UpgradeTreeUpgradePlanViewModel(newPlan, required, Plan?.EquipmentMasterDataId);
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
		if (Plan is null) return;

		EquipmentUpgradePlanItemViewModel newPlan = Plan switch
		{
			EquipmentUpgradePlanItemViewModel plan => plan,
			_ => EquipmentUpgradePlanManager.MakePlanViewModel(new())
		};

		EquipmentUpgradePlanItemViewModel editVm = new(newPlan.Plan)
		{
			DesiredUpgradeLevel = UpgradeLevel.Conversion,
			EquipmentMasterDataId = Plan.EquipmentMasterDataId,
			AllowToChangeDesiredUpgradeLevel = false,
			AllowToChangeEquipment = false,
			EquipmentAfterConversionFilter = NeedsToBeConvertedInto
		};

		if (!newPlan.OpenPlanDialog(editVm)) return;

		EquipmentUpgradePlanManager.AddPlan(newPlan);
		EquipmentUpgradePlanManager.Save();

		Plan = newPlan;
		EquipmentPlanHasChanged(newPlan);
	}

	[RelayCommand]
	private void EditEquipmentPlan()
	{
		if (Plan is not EquipmentUpgradePlanItemViewModel plan) return;

		EquipmentUpgradePlanItemViewModel editVm = new(plan.Plan)
		{
			AllowToChangeDesiredUpgradeLevel = false,
			AllowToChangeEquipment = false,
			EquipmentAfterConversionFilter = NeedsToBeConvertedInto
		};

		if (!plan.OpenPlanDialog(editVm)) return;

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
		Initialize(plan);

		OnPropertyChanged(nameof(CanBePlanned));
		OnPropertyChanged(nameof(AlreadyPlanned));
	}
}
