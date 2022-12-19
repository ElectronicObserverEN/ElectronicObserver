using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

public class EquipmentUpgradePlanCostViewModel
{
	public EquipmentUpgradePlanCostModel Model { get; set; }

	public EquipmentUpgradePlanCostMaterialViewModel Fuel { get; set; }

	public EquipmentUpgradePlanCostMaterialViewModel Ammo { get; set; }

	public EquipmentUpgradePlanCostMaterialViewModel Steel { get; set; }

	public EquipmentUpgradePlanCostMaterialViewModel Bauxite { get; set; }

	/// <summary>
	/// "screws"
	/// </summary>
	public EquipmentUpgradePlanCostMaterialViewModel ImprovementMaterial { get; set; }

	/// <summary>
	/// "devmats"
	/// </summary>
	public EquipmentUpgradePlanCostMaterialViewModel DevelopmentMaterial { get; set; }

	public List<EquipmentUpgradePlanCostEquipmentViewModel> RequiredEquipments { get; set; } = new();

	public List<EquipmentUpgradePlanCostConsumableViewModel> RequiredConsumables { get; set; } = new();

	public EquipmentUpgradePlanCostViewModel(EquipmentUpgradePlanCostModel model)
	{
		Model = model;

		Fuel = new(model.Fuel, UseItemId.Fuel);
		Ammo = new(model.Ammo, UseItemId.Ammo);
		Steel = new(model.Steel, UseItemId.Steel);
		Bauxite = new(model.Bauxite, UseItemId.Bauxite);

		DevelopmentMaterial = new(model.DevelopmentMaterial, UseItemId.DevelopmentMaterial);
		ImprovementMaterial = new(model.ImprovementMaterial, UseItemId.ImproveMaterial);

		RequiredEquipments = model.RequiredEquipments.Select(item => new EquipmentUpgradePlanCostEquipmentViewModel(item)).ToList();
		RequiredConsumables = model.RequiredConsumables.Select(item => new EquipmentUpgradePlanCostConsumableViewModel(item)).ToList();
	}

}
