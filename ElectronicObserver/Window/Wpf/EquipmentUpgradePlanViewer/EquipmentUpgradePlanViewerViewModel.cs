using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Behaviors.PersistentColumns;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace ElectronicObserver.Window.Wpf.EquipmentUpgradePlanViewer;

public class EquipmentUpgradePlanViewerViewModel : AnchorableViewModel
{
	private ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades = new();
	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgradesFiltered { get; set; } = new();

	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public EquipmentUpgradePlanViewerTranslationViewModel Translation { get; }

	public bool DisplayFinished { get; set; } = false;


	public EquipmentUpgradePlanViewerViewModel() : base("EquipmentUpgradePlanViewer", "EquipmentUpgradePlanViewer", ImageSourceIcons.GetIcon(IconContent.ItemModdingMaterial))
	{
		Translation = Ioc.Default.GetService<EquipmentUpgradePlanViewerTranslationViewModel>()!;

		Title = Translation.Title;
		Translation.PropertyChanged += (_, _) => Title = Translation.Title;
		KCDatabase.Instance.EquipmentUpgradePlanManager.OnPlanFinished += (_, _) => Update();
		PropertyChanged += EquipmentUpgradePlanViewerViewModel_PropertyChanged; 
	}

	private void EquipmentUpgradePlanViewerViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(DisplayFinished)) return;
		Update();
	}

	private void Update()
	{
		PlannedUpgradesFiltered.Clear();

		foreach (EquipmentUpgradePlanItemViewModel plan in PlannedUpgrades.Where(plan => DisplayFinished || !plan.Finished))
		{
			PlannedUpgradesFiltered.Add(plan);
		}
	}

	public override void Loaded()
	{
		base.Loaded();
		PlannedUpgrades = KCDatabase.Instance.EquipmentUpgradePlanManager.PlannedUpgrades;
		PlannedUpgrades.CollectionChanged += (_, _) => Update();
	}
}
