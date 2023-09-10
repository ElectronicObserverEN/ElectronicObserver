using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Common;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public abstract class UpgradeTreeNodeViewModel
{
	public abstract string DisplayName { get; }
	public abstract string DisplayIcon { get; }
	
	public abstract EquipmentUpgradePlanCostViewModel? Cost { get; }

	public ObservableCollection<UpgradeTreeNodeViewModel> Items { get; } = new();
}
