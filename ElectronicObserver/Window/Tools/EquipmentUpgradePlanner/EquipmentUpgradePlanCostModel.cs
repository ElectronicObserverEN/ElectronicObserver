using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradePlanCostModel
{
	public int Fuel { get; set; }

	public int Ammo { get; set; }

	public int Steel { get; set; }

	public int Bauxite { get; set; }

	/// <summary>
	/// "screws"
	/// </summary>
	public int ImprovmentMaterial { get; set; }

	/// <summary>
	/// "devmats"
	/// </summary>
	public int DevelopmentMaterial { get; set; }

	public List<EquipmentUpgradePlanCostItemModel> RequiredEquipments { get; set; } = new();

	public List<EquipmentUpgradePlanCostItemModel> RequiredConsumables { get; set; } = new();

	public static EquipmentUpgradePlanCostModel operator *(EquipmentUpgradePlanCostModel a, int b) => new EquipmentUpgradePlanCostModel()
	{
		Fuel = a.Fuel * b,
		Ammo = a.Ammo * b,
		Steel = a.Steel * b,
		Bauxite = a.Bauxite * b,

		ImprovmentMaterial = a.ImprovmentMaterial * b,
		DevelopmentMaterial = a.DevelopmentMaterial * b,

		RequiredConsumables = a.RequiredConsumables.Select(cons => new EquipmentUpgradePlanCostItemModel() { Required = cons.Required * b }).ToList(),
		RequiredEquipments = a.RequiredEquipments.Select(equ => new EquipmentUpgradePlanCostItemModel() { Required = equ.Required * b }).ToList(),
	};

	public static EquipmentUpgradePlanCostModel operator +(EquipmentUpgradePlanCostModel a, EquipmentUpgradePlanCostModel b)
	{
		EquipmentUpgradePlanCostModel newModel = new()
		{
			Fuel = a.Fuel + b.Fuel,
			Ammo = a.Ammo + b.Ammo,
			Steel = a.Steel + b.Steel,
			Bauxite = a.Bauxite + b.Bauxite,

			ImprovmentMaterial = a.ImprovmentMaterial + b.ImprovmentMaterial,
			DevelopmentMaterial = a.DevelopmentMaterial + b.DevelopmentMaterial,
		};

		newModel.RequiredConsumables = a.RequiredConsumables
			.Concat(b.RequiredConsumables)
			.GroupBy(cost => cost.Id)
			.Select(costGroup => new EquipmentUpgradePlanCostItemModel
			{
				Id = costGroup.Key,
				Required = costGroup.Sum(cost => cost.Required)
			})
			.ToList();

		newModel.RequiredEquipments.AddRange(a.RequiredEquipments);
		newModel.RequiredEquipments.AddRange(b.RequiredEquipments);

		newModel.RequiredEquipments = newModel.RequiredEquipments
			.GroupBy(cost => cost.Id)
			.Select(costGroup => new EquipmentUpgradePlanCostItemModel
			{
				Id = costGroup.Key,
				Required = costGroup.Sum(cost => cost.Required)
			})
			.ToList();

		return newModel;
	}

	public override bool Equals(object? other)
	{
		if (other is not EquipmentUpgradePlanCostModel) return false;
		EquipmentUpgradePlanCostModel otherCost = (EquipmentUpgradePlanCostModel)other;

		if (Fuel != otherCost.Fuel) return false;
		if (Ammo != otherCost.Ammo) return false;
		if (Steel != otherCost.Steel) return false;
		if (Bauxite != otherCost.Bauxite) return false;

		if (RequiredEquipments.Count != otherCost.RequiredEquipments.Count) return false;

		foreach((EquipmentUpgradePlanCostItemModel v, EquipmentUpgradePlanCostItemModel ov) in RequiredEquipments.Zip(otherCost.RequiredEquipments, (v, ov) => (v, ov)))
		{
			if (!v.Equals(ov)) return false;
		}
		
		if (RequiredConsumables.Count != otherCost.RequiredConsumables.Count) return false;

		foreach ((EquipmentUpgradePlanCostItemModel v, EquipmentUpgradePlanCostItemModel ov) in RequiredConsumables.Zip(otherCost.RequiredConsumables, (v, ov) => (v, ov)))
		{
			if (!v.Equals(ov)) return false;
		}

		return true;
	}

	public override int GetHashCode() => base.GetHashCode();
}
