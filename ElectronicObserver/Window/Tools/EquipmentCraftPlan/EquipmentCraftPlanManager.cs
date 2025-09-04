using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Database;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Window.Tools.EquipmentCraftPlan;

public class EquipmentCraftPlanManager
{
	public ObservableCollection<EquipmentCraftPlanItemViewModel> Plans { get; } = new();

	private ElectronicObserverContext DatabaseContext { get; } = new();

	public event EventHandler? PlanFinished;
	public event EventHandler? PlanCostUpdated;
	public event EventHandler? PlanEquipmentMasterUpdated;

	public EquipmentCraftPlanManager()
	{
		SubscribeToApi();
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += Initialize;
	}

	private void Initialize(string apiname, object data)
	{
		Load();
		APIObserver.Instance.ApiPort_Port.ResponseReceived -= Initialize;
	}

	private void Load()
	{
		Plans.Clear();

		List<EquipmentCraftPlanItemModel> models = DatabaseContext.EquipmentCraftPlan.ToList();

		foreach (EquipmentCraftPlanItemModel model in models)
		{
			EquipmentCraftPlanItemViewModel plan = MakePlanViewModel(model);
			Plans.Add(plan);
		}
	}

	public void AddPlan(EquipmentCraftPlanItemViewModel plan)
	{
		DatabaseContext.EquipmentCraftPlan.Add(plan.Model);
		Plans.Add(plan);
	}

	public EquipmentCraftPlanItemViewModel MakePlanViewModel(EquipmentCraftPlanItemModel model)
	{
		EquipmentCraftPlanItemViewModel plan = new(model);

		plan.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(plan.Finished)) return;

			PlanFinished?.Invoke(this, EventArgs.Empty);
		};


		plan.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(plan.Cost)) return;

			PlanCostUpdated?.Invoke(this, EventArgs.Empty);
		};

		plan.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(plan.EquipmentMasterDataId)) return;

			PlanEquipmentMasterUpdated?.Invoke(this, EventArgs.Empty);
		};

		return plan;
	}

	public void RemovePlan(EquipmentCraftPlanItemViewModel planViewModel)
	{
		DatabaseContext.EquipmentCraftPlan.Remove(planViewModel.Model);
		Plans.Remove(planViewModel);
	}

	public void Save()
	{
		DatabaseContext.SaveChanges();
	}
}
