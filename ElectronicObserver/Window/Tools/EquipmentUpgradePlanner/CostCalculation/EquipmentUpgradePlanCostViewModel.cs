using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

public class EquipmentUpgradePlanCostViewModel
{
	public EquipmentUpgradePlanCostModel Model { get; set; }

	public int Fuel { get; set; }

	public int Ammo { get; set; }

	public int Steel { get; set; }

	public int Bauxite { get; set; }

	/// <summary>
	/// "screws"
	/// </summary>
	public int ImprovementMaterial { get; set; }

	/// <summary>
	/// "devmats"
	/// </summary>
	public int DevelopmentMaterial { get; set; }

	public List<EquipmentUpgradePlanCostEquipmentViewModel> RequiredEquipments { get; set; } = new();

	public List<EquipmentUpgradePlanCostConsumableViewModel> RequiredConsumables { get; set; } = new();

	public EquipmentUpgradePlanCostViewModel(EquipmentUpgradePlanCostModel model)
	{
		Model = model;

		Fuel = model.Fuel;
		Ammo = model.Ammo;
		Steel = model.Steel;
		Bauxite = model.Bauxite;

		DevelopmentMaterial = model.DevelopmentMaterial;
		ImprovementMaterial = model.ImprovementMaterial;

		RequiredEquipments = model.RequiredEquipments.Select(item => new EquipmentUpgradePlanCostEquipmentViewModel(item)).ToList();
		RequiredConsumables = model.RequiredConsumables.Select(item => new EquipmentUpgradePlanCostConsumableViewModel(item)).ToList();
	}

}
