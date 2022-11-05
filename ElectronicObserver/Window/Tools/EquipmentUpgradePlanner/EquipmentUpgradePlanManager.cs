using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public class EquipmentUpgradePlanManager
{
	public bool IsInitialized { get; private set; } = false;

	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; private set; } = new();

	private ElectronicObserverContext DatabaseContext { get; set; } = new();

	public EquipmentUpgradePlanManager()
	{
		SubscribeToApi();
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += (_, __) => Load();
	}

	public void Load()
	{
		if (IsInitialized) return;
		if (!KCDatabase.Instance.MasterEquipments.Any()) return;

		PlannedUpgrades.Clear();

		List<EquipmentUpgradePlanItemModel> models = DatabaseContext.EquipmentUpgradePlanItems.ToList();

		foreach (EquipmentUpgradePlanItemModel model in models)
		{
			PlannedUpgrades.Add(new EquipmentUpgradePlanItemViewModel(model));
		}

		IsInitialized = true;
	}

	public EquipmentUpgradePlanItemViewModel AddPlan()
	{
		EquipmentUpgradePlanItemModel plan = new();
		DatabaseContext.EquipmentUpgradePlanItems.Add(plan);

		EquipmentUpgradePlanItemViewModel planViewModel = new(plan);
		PlannedUpgrades.Add(planViewModel);

		return planViewModel;
	}

	public void DeletePlan(EquipmentUpgradePlanItemViewModel planViewModel)
	{
		DatabaseContext.EquipmentUpgradePlanItems.Remove(planViewModel.Plan);
		PlannedUpgrades.Remove(planViewModel);
	}

	public void Save()
	{
		if (!IsInitialized) return;
		DatabaseContext.SaveChanges();
	}
}
