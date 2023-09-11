using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public class UpgradeTreeViewModel : WindowViewModelBase
{
	public ObservableCollection<UpgradeTreeNodeViewModel> Items { get; } = new();

	public UpgradeTreeViewModel(EquipmentUpgradePlanItemViewModel plan)
	{
		Items.Add(new UpgradeTreeUpgradePlanViewModel(1, plan, true));
	}
}
