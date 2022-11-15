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

	public bool DisplayFinished { get; set; } = false;
	private Tracker Tracker { get; }


	public EquipmentUpgradePlanViewerViewModel() : base("EquipmentUpgradePlanViewer", "EquipmentUpgradePlanViewer", ImageSourceIcons.GetIcon(IconContent.ItemModdingMaterial))
	{
		Translation = Ioc.Default.GetService<EquipmentUpgradePlanViewerTranslationViewModel>()!;
		Tracker = Ioc.Default.GetService<Tracker>()!;

		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();

		Title = Translation.Title;
		Translation.PropertyChanged += (_, _) => Title = Translation.Title;
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => Update();
		PropertyChanged += EquipmentUpgradePlanViewerViewModel_PropertyChanged;

		EquipmentUpgradePlanManager.PlannedUpgrades.CollectionChanged += (_, _) => Update();

		StartJotTracking();
	}

	private void EquipmentUpgradePlanViewerViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(DisplayFinished)) return;
		Update();
	}

	private void Update()
	{
		PlannedUpgradesFiltered.Clear();

		foreach (EquipmentUpgradePlanItemViewModel plan in EquipmentUpgradePlanManager.PlannedUpgrades.Where(plan => DisplayFinished || !plan.Finished))
		{
			PlannedUpgradesFiltered.Add(plan);
		}
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
