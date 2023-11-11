using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public class UpgradeTreeViewModel : WindowViewModelBase
{
	public ObservableCollection<UpgradeTreeUpgradePlanViewModel> Items { get; } = new();

	public EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public UpgradeTreeViewModel(EquipmentUpgradePlanItemViewModel plan)
	{
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();

		Items.Add(new UpgradeTreeUpgradePlanViewModel(plan)
		{
			RequiredCount = 1
		});
	}
}
