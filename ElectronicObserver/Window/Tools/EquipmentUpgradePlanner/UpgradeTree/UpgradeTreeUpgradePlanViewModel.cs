using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public partial class UpgradeTreeUpgradePlanViewModel
{
	public string DisplayName => Plan switch
	{
		EquipmentCraftPlanItemViewModel => $"{RequiredCount}x {Plan.EquipmentMasterData?.NameEN}",
		EquipmentUpgradePlanItemViewModel plan => $"{RequiredCount}x {plan.EquipmentName} ({Translations.Goal}: {plan.DesiredUpgradeLevel})",
		_ => ""
	};
	
	public EquipmentUpgradePlanCostViewModel? Cost => Plan?.Cost.Model is null or { Fuel: 0, Ammo: 0, Steel: 0, Bauxite: 0 } ? null : Plan.Cost;

	public UpgradeTreeViewNodeState State => Plan switch
	{
		not null => UpgradeTreeViewNodeState.Planned,
		_ => UpgradeTreeViewNodeState.ToCraft
	};

	public bool CanBePlanned => Plan is EquipmentUpgradePlanItemViewModel plan && !EquipmentUpgradePlanManager.PlannedUpgrades.Contains(plan);
	public bool AlreadyPlanned => Plan is EquipmentUpgradePlanItemViewModel plan && EquipmentUpgradePlanManager.PlannedUpgrades.Contains(plan);

	private IEquipmentPlanItemViewModel? Plan { get; }

	public int RequiredCount { get; init; }

	private EquipmentUpgradeData EquipmentUpgradeData { get; }

	private EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public ObservableCollection<UpgradeTreeUpgradePlanViewModel> Children { get; } = new();

	public EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public UpgradeTreeUpgradePlanViewModel(EquipmentUpgradePlanItemViewModel plan)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

		Plan = plan;
		Initialize(plan);
	}

	public UpgradeTreeUpgradePlanViewModel(IEquipmentPlanItemViewModel plan)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

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

		EquipmentUpgradeDataModel? upgradePlan = EquipmentUpgradeData.UpgradeList
			.FirstOrDefault(equipmentData => equipmentData.ConvertTo
				.Any(equipmentAfterConvertion =>
					equipmentAfterConvertion.IdEquipmentAfter == (int)plan.EquipmentMasterDataId));

		if (upgradePlan is not null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			newPlan.DesiredUpgradeLevel = UpgradeLevel.Conversion;
			newPlan.EquipmentMasterDataId = (EquipmentId)upgradePlan.EquipmentId;

			Children.Add(new UpgradeTreeUpgradePlanViewModel(newPlan)
			{
				RequiredCount = 1
			});
		}
		else
		{
			Children.Add(new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(plan.EquipmentMasterDataId))
			{
				RequiredCount = 1
			});
		}
	}

	private UpgradeTreeUpgradePlanViewModel InitializeEquipmentPlanChild(int required, EquipmentId equipment)
	{
		// if fodder = upgraded equipment, return a craft plan to avoid infinite loops
		if (equipment == Plan?.EquipmentMasterDataId)
		{
			return new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(equipment))
			{
				RequiredCount = required
			};
		}

		// TODO : look for assigned plan

		// if no plan found : return a new plan if the equipment can be made from conversion
		EquipmentUpgradeDataModel? upgradePlan = EquipmentUpgradeData.UpgradeList
			.FirstOrDefault(equipmentData => equipmentData.ConvertTo
				.Any(equipmentAfterConvertion =>
					equipmentAfterConvertion.IdEquipmentAfter == (int)equipment));

		if (upgradePlan is not null)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Conversion;
			newPlan.EquipmentMasterDataId = equipment;

			return new UpgradeTreeUpgradePlanViewModel(newPlan)
			{
				RequiredCount = required
			};
		}

		// if equipment can't be upgraded, return an empty plan
		// TODO : turn this into a "craft plan" if the equipment is craftable
		// TODO : Could also obtain equipment from ship stock equipments => Should we add something to link ship training plan to upgrade plans ?
		return new UpgradeTreeUpgradePlanViewModel(new EquipmentCraftPlanItemViewModel(equipment))
		{
			RequiredCount = required
		};
	}

	[RelayCommand]
	private void AddEquipmentPlan()
	{
		if (Plan is not EquipmentUpgradePlanItemViewModel plan) return;
		if (!plan.OpenPlanDialog()) return;

		EquipmentUpgradePlanManager.AddPlan(plan);
		EquipmentUpgradePlanManager.Save();
	}

	[RelayCommand]
	private void EditEquipmentPlan()
	{
		if (Plan is not EquipmentUpgradePlanItemViewModel plan) return;

		if (plan.OpenPlanDialog())
		{
			EquipmentUpgradePlanManager.Save();
		}
	}

	[RelayCommand]
	private void RemoveEquipmentPlan()
	{
		if (Plan is not EquipmentUpgradePlanItemViewModel plan) return;

		EquipmentUpgradePlanManager.RemovePlan(plan);
		EquipmentUpgradePlanManager.Save();
	}
}
