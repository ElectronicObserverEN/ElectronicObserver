using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public class UpgradeTreePlanViewModel : UpgradeTreeNodeViewModel
{
	public override string DisplayName => $"{RequiredCount}x {Plan.EquipmentName}";
	public override string DisplayIcon => "icon";

	public override EquipmentUpgradePlanCostViewModel? Cost => Plan.Cost.Model is { Fuel: 0, Ammo: 0, Steel: 0, Bauxite: 0 } ? null : Plan.Cost;

	private EquipmentUpgradePlanItemViewModel Plan { get; }
	private int RequiredCount { get; }

	public UpgradeTreePlanViewModel(int requiredCount, EquipmentUpgradePlanItemViewModel plan)
	{
		Plan = plan;
		RequiredCount = requiredCount;

		Initialize();
	}

	private void Initialize()
	{
		foreach (EquipmentUpgradePlanCostEquipmentViewModel equipment in Plan.Cost.RequiredEquipments)
		{
			// Default
			Items.Add(new UpgradeTreePlanViewModel(equipment.Required, new(new()
			{
				EquipmentId = equipment.Equipment.EquipmentId,
			})));
		}
	}
}
