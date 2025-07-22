using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Common;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Control.Paging;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public partial class EquipmentUpgradePlannerViewModel : WindowViewModelBase
{
	private ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; }

	public PagingControlViewModel PlannedUpgradesPager { get; }

	public EquipmentUpgradePlannerTranslationViewModel EquipmentUpgradePlanner { get; }

	public EquipmentUpgradePlanManager EquipmentUpgradePlanManager { get; }

	public EquipmentUpgradePlanCostViewModel TotalCost { get; private set; } = new(new());

	public GridLength PlanListWidth { get; set; } = new(350, GridUnitType.Pixel);

	public EquipmentUpgradeFilterViewModel Filters { get; set; } = new();

	private TransliterationService TransliterationService { get; }

	public EquipmentUpgradePlannerViewModel()
	{
		PlannedUpgradesPager = new();
		EquipmentUpgradePlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentUpgradePlanner = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();
		TransliterationService = Ioc.Default.GetRequiredService<TransliterationService>();
		PlannedUpgrades = EquipmentUpgradePlanManager.PlannedUpgrades;
	}

	public override void Loaded()
	{
		base.Loaded();
		PlannedUpgrades.CollectionChanged += (_, _) => Update();
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => Update();
		EquipmentUpgradePlanManager.PlanFinished += (_, _) => UpdateTotalCost();
		EquipmentUpgradePlanManager.PlanCostUpdated += (_, _) => UpdateTotalCost();
		EquipmentUpgradePlanManager.PlanEquipmentMasterUpdated += (_, _) => Update();
		Filters.PropertyChanged += (_, _) => Update();
		Update();
		UpdateTotalCost();
	}

	[RelayCommand]
	private void AddEquipmentPlan()
	{
		EquipmentSelectorViewModel equipmentSelector = new(TransliterationService, [.. KCDatabase.Instance.Equipments.Values]);

		equipmentSelector.ShowDialog();

		if (equipmentSelector.DialogResult is true)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.EquipmentId = equipmentSelector.SelectedEquipment.MasterID;

			if (newPlan.OpenPlanDialog())
			{
				EquipmentUpgradePlanManager.AddPlan(newPlan);
				EquipmentUpgradePlanManager.Save();
			}
		}
	}

	[RelayCommand]
	private void AddEquipmentPlanFromMasterData()
	{
		List<IEquipmentData> equipment = KCDatabase.Instance.MasterEquipments.Values
			.Where(e => !e.IsAbyssalEquipment)
			.Select(e => new EquipmentDataMock(e))
			.OfType<IEquipmentData>()
			.ToList();

		EquipmentSelectorViewModel equipmentSelector = new(TransliterationService, equipment);

		equipmentSelector.ShowDialog();

		if (equipmentSelector.DialogResult is true)
		{
			EquipmentUpgradePlanItemViewModel newPlan = EquipmentUpgradePlanManager.MakePlanViewModel(new());

			// Use a setting to set default level ?
			newPlan.DesiredUpgradeLevel = UpgradeLevel.Max;
			newPlan.EquipmentMasterDataId = equipmentSelector.SelectedEquipment.EquipmentId;

			if (newPlan.OpenPlanDialog())
			{
				EquipmentUpgradePlanManager.AddPlan(newPlan);
				EquipmentUpgradePlanManager.Save();
			}
		}
	}

	[RelayCommand]
	private void RemovePlan(EquipmentUpgradePlanItemViewModel planToRemove)
	{
		EquipmentUpgradePlanManager.RemovePlan(planToRemove);
		EquipmentUpgradePlanManager.Save();
	}

	[RelayCommand]
	private void OpenEditDialog(EquipmentUpgradePlanItemViewModel plan)
	{
		if (plan.OpenPlanDialog())
		{
			EquipmentUpgradePlanManager.Save();
		}
	}

	[RelayCommand]
	private void OpenTreeDialog(EquipmentUpgradePlanItemViewModel plan)
	{
		UpgradeTreeView view = new(new(plan));
		view.ShowDialog();
	}

	private void UpdateTotalCost()
	{
		TotalCost.UnsubscribeFromApis();
		TotalCost = new(PlannedUpgrades
			.Where(plan => !plan.Finished)
			.Aggregate(new EquipmentUpgradePlanCostModel(), (cost, viewModel) => cost + viewModel.Cost.Model, totalCost => totalCost));
	}

	private void Update()
	{
		PlannedUpgradesPager.Items = PlannedUpgrades
			.Where(Filters.MeetsFilterCondition)
			.OrderBy(plan => plan.Finished)
			.Cast<object>()
			.ToList();
	}
}
