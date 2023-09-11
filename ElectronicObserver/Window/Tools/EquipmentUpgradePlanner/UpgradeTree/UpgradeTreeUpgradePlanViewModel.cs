using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public class UpgradeTreeUpgradePlanViewModel : UpgradeTreeNodeViewModel
{
	public override string DisplayName => $"{RequiredCount}x {Plan.EquipmentName} (Goal : {Plan.DesiredUpgradeLevel})";
	public override string DisplayIcon => "icon";
	public override EquipmentUpgradePlanCostViewModel? Cost => Plan.Cost.Model is { Fuel: 0, Ammo: 0, Steel: 0, Bauxite: 0 } ? null : Plan.Cost;
	public override UpgradeTreeViewNodeState State => IsPlanned ? UpgradeTreeViewNodeState.Planned : UpgradeTreeViewNodeState.ToCraft;

	private EquipmentUpgradePlanItemViewModel Plan { get; }

	private int RequiredCount { get; }

	private EquipmentUpgradeData EquipmentUpgradeData { get; }

	private EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public bool IsPlanned { get; }

	public UpgradeTreeUpgradePlanViewModel(int requiredCount, EquipmentUpgradePlanItemViewModel plan, bool isPlanned)
	{
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradeData = KCDatabase.Instance.Translation.EquipmentUpgrade;

		Plan = plan;
		RequiredCount = requiredCount;
		IsPlanned = isPlanned;

		Initialize();
	}

	private void Initialize()
	{
		InitializeEquipmentPlan(1, Plan.EquipmentMasterDataId);

		foreach (EquipmentUpgradePlanCostEquipmentViewModel equipment in Plan.Cost.RequiredEquipments)
		{
			Items.Add(InitializeEquipmentPlanChild(equipment.Required, equipment.Equipment.EquipmentId));
		}
	}

	private void InitializeEquipmentPlan(int required, EquipmentId equipment)
	{
		if (Plan.EquipmentId is null)
		{
			EquipmentUpgradeDataModel? upgradePlan = EquipmentUpgradeData.UpgradeList
				.FirstOrDefault(equipmentData => equipmentData.ConvertTo
					.Any(equipmentAfterConvertion =>
						equipmentAfterConvertion.IdEquipmentAfter == (int)equipment));

			if (upgradePlan is not null)
			{
				EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

				// Use a setting to set default level ?
				newPlan.DesiredUpgradeLevel = UpgradeLevel.Conversion;
				newPlan.EquipmentMasterDataId = (EquipmentId)upgradePlan.EquipmentId;

				Items.Add(new UpgradeTreeUpgradePlanViewModel(required, newPlan, false));
			}
			else
			{
				Items.Add(new UpgradeTreeCraftPlanViewModel(required, equipment));
			}
		}
	}

	private UpgradeTreeNodeViewModel InitializeEquipmentPlanChild(int required, EquipmentId equipment)
	{
		// if fodder = upgraded equipment, return a craft plan to avoid infinite loops
		if (equipment == Plan.EquipmentMasterDataId)
		{
			return new UpgradeTreeCraftPlanViewModel(required, equipment);
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

			return new UpgradeTreeUpgradePlanViewModel(required, newPlan, false);
		}

		// if equipment can't be upgraded, return an empty plan
		// TODO : turn this into a "craft plan" if the equipment is craftable
		// TODO : Could also obtain equipment from ship stock equipments => Should we add something to link ship training plan to upgrade plans ?
		return new UpgradeTreeCraftPlanViewModel(required, equipment);
	}
}
