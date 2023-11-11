using System.Collections.ObjectModel;
using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public class UpgradeTreeViewModel : WindowViewModelBase
{
	public ObservableCollection<UpgradeTreeUpgradePlanViewModel> Items { get; } = new();

	public UpgradeTreeViewModel(EquipmentUpgradePlanItemViewModel plan)
	{
		Items.Add(new UpgradeTreeUpgradePlanViewModel(plan)
		{
			RequiredCount = 1
		});
	}
}
