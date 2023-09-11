using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public class UpgradeTreeCraftPlanViewModel : UpgradeTreeNodeViewModel
{
	public override string DisplayName => $"{RequiredCount}x {Equipment.NameEN}";
	public override string DisplayIcon => "icon";
	public override UpgradeTreeViewNodeState State { get; }
	public override EquipmentUpgradePlanCostViewModel? Cost => null;

	private IEquipmentDataMaster Equipment { get; }
	private int RequiredCount { get; }

	public UpgradeTreeCraftPlanViewModel(int requiredCount, EquipmentId equipment)
	{
		Equipment = KCDatabase.Instance.MasterEquipments[(int)equipment];
		RequiredCount = requiredCount;

		Initialize();
	}

	private void Initialize()
	{
		// TODO
	}
}
