using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;

public class EquipmentAssignmentItemViewModel : IEquipmentPlanItemViewModel
{
	public EquipmentAssignmentItemModel Model { get; }

	public IEquipmentData? AssignedEquipment { get; set; }

	public EquipmentUpgradePlanItemViewModel? AssignedPlan { get; set; }

	public EquipmentId EquipmentMasterDataId { get; private set; }

	public EquipmentUpgradePlanCostViewModel Cost => new(new());

	private EquipmentUpgradePlanManager PlanManager { get; }
	private EquipmentPickerService EquipmentPicker { get; }

	public EquipmentAssignmentItemViewModel(EquipmentAssignmentItemModel model)
	{
		PlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentPicker = Ioc.Default.GetRequiredService<EquipmentPickerService>();

		Model = model;

		LoadModel();
	}

	public void LoadModel()
	{
		AssignedPlan = PlanManager.PlannedUpgrades.FirstOrDefault(plan => plan.Plan == Model.Plan);

		AssignedEquipment = KCDatabase.Instance.Equipments.ContainsKey(Model.EquipmentId) switch
		{
			true => KCDatabase.Instance.Equipments[Model.EquipmentId]!,
			_ => null
		};

		EquipmentMasterDataId = Model.EquipmentMasterDataId;
	}

	/// <summary>
	/// Saves changes to the model
	/// Returns false if changes couldn't have been saved
	/// TODO : maybe there's a better way to do data validation
	/// </summary>
	/// <returns></returns>
	public bool SaveChanges()
	{
		if (AssignedPlan is null) return false;
		if (AssignedEquipment is null) return false;

		Model.Plan = AssignedPlan.Plan;
		Model.EquipmentId = AssignedEquipment.ID;

		return true;
	}

	public void UnsubscribeFromApis()
	{
		Cost.UnsubscribeFromApis();
	}

	public List<IEquipmentPlanItemViewModel> GetPlanChildren()
	{
		return new();
	}

	public void OpenEquipmentPicker()
	{
		EquipmentAssignmentPickerViewModel viewModel = new(PlanManager, EquipmentMasterDataId);

		IEquipmentData? equipment = EquipmentPicker.OpenEquipmentPicker(viewModel);

		if (equipment is null) return;

		AssignedEquipment = equipment;
	}
}
