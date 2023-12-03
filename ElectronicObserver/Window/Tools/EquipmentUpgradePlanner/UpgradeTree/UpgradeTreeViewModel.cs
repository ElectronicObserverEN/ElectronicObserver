using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public class UpgradeTreeViewModel : WindowViewModelBase
{
	public ObservableCollection<UpgradeTreeUpgradePlanViewModel> Items { get; } = new();
	
	public EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public EquipmentUpgradePlanManager UpgradeManager { get; }

	public UpgradeTreeViewModel(EquipmentUpgradePlanItemViewModel plan)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		UpgradeManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();

		Items.Add(new UpgradeTreeUpgradePlanViewModel(plan));
	}

	public override void Closed()
	{
		Task.Run(() =>
		{
			foreach (UpgradeTreeUpgradePlanViewModel item in Items)
			{
				item.CleanupUnusedPlan();
			}
		});

		base.Closed();
	}
}
