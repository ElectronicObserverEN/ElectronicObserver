using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

	/// <summary>
	/// Property to store the equipment Data before the upgrade
	/// </summary>
	private EquipmentData? CurrentUpgradeEquipment { get; set; }

	public event EventHandler? PlanFinished;

	public EquipmentUpgradePlanManager()
	{
		SubscribeToApi();
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += (_, __) => Load();
		o.ApiReqKousyou_RemodelSlot.ResponseReceived += HandleEquipmentUpgradeResponse;
		o.ApiReqKousyou_RemodelSlot.RequestReceived += HandleEquipmentUpgradeRequest;
	}

	public void Load()
	{
		if (IsInitialized) return;
		if (!KCDatabase.Instance.MasterEquipments.Any()) return;

		PlannedUpgrades.Clear();

		List<EquipmentUpgradePlanItemModel> models = DatabaseContext.EquipmentUpgradePlanItems.ToList();

		foreach (EquipmentUpgradePlanItemModel model in models)
		{
			EquipmentUpgradePlanItemViewModel plan = new(model);
			plan.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(plan.Finished)) return;

				PlanFinished?.Invoke(this, EventArgs.Empty);
			};
			PlannedUpgrades.Add(plan);
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

	public void RemovePlan(EquipmentUpgradePlanItemViewModel planViewModel)
	{
		DatabaseContext.EquipmentUpgradePlanItems.Remove(planViewModel.Plan);
		PlannedUpgrades.Remove(planViewModel);
	}

	public void Save()
	{
		if (!IsInitialized) return;
		DatabaseContext.SaveChanges();
	}

	private void HandleEquipmentUpgradeRequest(string apiname, dynamic data)
	{
		int idEquipment = int.Parse(data["api_slot_id"]);

		if (KCDatabase.Instance.Equipments.ContainsKey(idEquipment))
			CurrentUpgradeEquipment = KCDatabase.Instance.Equipments[idEquipment];
	}

	private void HandleEquipmentUpgradeResponse(string apiname, dynamic data)
	{
		// Shouldn't happen ...
		if (CurrentUpgradeEquipment is null) return;

		// In case of failure => do nothing
		if (!data.api_after_slot()) return;

		int idEquipment = (int)data.api_after_slot.api_id;

		// Equipment not found => shouldn't happen
		if (!KCDatabase.Instance.Equipments.ContainsKey(idEquipment)) return;

		// Find the upgrade plan
		EquipmentUpgradePlanItemViewModel? plan = FindEquipmentPlanFromEquipmentData(CurrentUpgradeEquipment);

		// No plan found => do nothing (again)
		if (plan is null) return;

		// if api_remodel_id is set and the two values in it are different, the equipment has been converted !
		if (data.api_remodel_id() && data.api_remodel_id[0] != data.api_remodel_id[1])
			UpdatePlanAfterEquipmentConversion(plan, (int)data.api_remodel_id[0]);
		else
			UpdatePlanAfterEquipmentImprovment(plan);


		plan.Update();
	}

	private void UpdatePlanAfterEquipmentConversion(EquipmentUpgradePlanItemViewModel plan, int oldEquipmentMasterId)
	{
		// Equipment conversion = plan finished anyway since we can't go higher 
		plan.Finished = true;
		// Also unset the equipment id (drop id) since it changed masterId
		// This should be enough ?
		plan.EquipmentMasterDataId = (EquipmentId)oldEquipmentMasterId;
		plan.EquipmentId = null;
	}

	private void UpdatePlanAfterEquipmentImprovment(EquipmentUpgradePlanItemViewModel plan)
	{
		if (plan.DesiredUpgradeLevel == UpgradeLevel.Conversion) return;

		if (plan.Equipment.UpgradeLevel >= plan.DesiredUpgradeLevel)
		{
			// Normal upgrade : do the normal stuff
			// base api code updated the equipment data
			plan.Finished = true;
		}
	}

	/// <summary>
	/// Look for an equipment plan from an equipment data
	/// If equipment plan found, returns it
	/// If no equipment plan found, looks for a "matching" plan
	/// If we found a "matching" plan, assign it to the equipment and returns it
	/// </summary>
	/// <param name="equipmentData"></param>
	/// <returns>Null if not found</returns>
	private EquipmentUpgradePlanItemViewModel? FindEquipmentPlanFromEquipmentData(IEquipmentData equipmentData)
	{
		EquipmentUpgradePlanItemViewModel? foundPlan = PlannedUpgrades.FirstOrDefault(plan => plan.Equipment?.MasterID == equipmentData.MasterID);

		// Plan found, return it
		if (foundPlan != null) return foundPlan;

		// Not found => look for a matching plan
		foundPlan = PlannedUpgrades
			.Where(plan => plan.Equipment?.MasterID == 0)
			.Where(plan => !plan.Finished)
			.Where(plan => plan.DesiredUpgradeLevel < equipmentData.UpgradeLevel)
			.Where(plan => plan.Equipment?.EquipmentID == equipmentData.EquipmentID)
			.FirstOrDefault();

		// Assign the plan to this equipment
		if (foundPlan != null)
		{
			foundPlan.EquipmentId = equipmentData.MasterID;
		}

		return foundPlan;
	}
}
