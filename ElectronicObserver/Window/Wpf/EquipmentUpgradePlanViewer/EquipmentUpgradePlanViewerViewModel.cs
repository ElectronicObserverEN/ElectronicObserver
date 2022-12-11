using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Behaviors.PersistentColumns;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using Jot;

namespace ElectronicObserver.Window.Wpf.EquipmentUpgradePlanViewer;

public class EquipmentUpgradePlanViewerViewModel : AnchorableViewModel
{
	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgradesFiltered { get; set; } = new();

	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public EquipmentUpgradePlanViewerTranslationViewModel Translation { get; }

	private EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public EquipmentUpgradeFilterViewModel Filters { get; set; } = new();

	private Tracker Tracker { get; }


	public EquipmentUpgradePlanViewerViewModel() : base("EquipmentUpgradePlanViewer", "EquipmentUpgradePlanViewer", ImageSourceIcons.GetIcon(IconContent.ItemModdingMaterial))
	{
		Translation = Ioc.Default.GetService<EquipmentUpgradePlanViewerTranslationViewModel>()!;
		Tracker = Ioc.Default.GetService<Tracker>()!;

		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();

		Title = Translation.Title;
		Translation.PropertyChanged += (_, _) => Title = Translation.Title;
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => Update();
		EquipmentUpgradePlanManager.PlanEquipmentMasterUpdated += (_, _) => Update();
		Filters.PropertyChanged += (_, _) => Update(); 

		EquipmentUpgradePlanManager.PlannedUpgrades.CollectionChanged += (_, _) => Update();

		StartJotTracking();
	}

	private void Update()
	{
		PlannedUpgradesFiltered.Clear();

		foreach (EquipmentUpgradePlanItemViewModel plan in EquipmentUpgradePlanManager.PlannedUpgrades.Where(Filters.MeetsFilterCondition))
		{
			PlannedUpgradesFiltered.Add(plan);
		}
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
