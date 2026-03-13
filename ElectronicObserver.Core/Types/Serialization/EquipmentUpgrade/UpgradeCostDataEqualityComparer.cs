using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

/// <summary>
/// This equality comparer is used to group upgrade data based on the cost of levels 0 -> 5 and 6 -> max
/// Conversion & extra costs are ignored
/// </summary>
public class UpgradeCostDataEqualityComparer : IEqualityComparer<EquipmentUpgradeImprovementModel>
{
	public bool Equals(EquipmentUpgradeImprovementModel? x, EquipmentUpgradeImprovementModel? y)
	{
		if (ReferenceEquals(x, y))
			return true;

		if (x is null || y is null)
			return false;

		return Equals(x.Costs, y.Costs);
	}

	private bool Equals(EquipmentUpgradeImprovementCost? x, EquipmentUpgradeImprovementCost? y)
	{
		if (ReferenceEquals(x, y))
			return true;

		if (x is null || y is null)
			return false;

		if (x.Fuel != y.Fuel) return false;
		if (x.Ammo != y.Ammo) return false;
		if (x.Steel != y.Steel) return false;
		if (x.Bauxite != y.Bauxite) return false;

		if (!Equals(x.Cost0To5, y.Cost0To5)) return false;
		if (!Equals(x.Cost6To9, y.Cost6To9)) return false;

		return true;
	}

	private bool Equals(EquipmentUpgradeImprovementCostDetail? x, EquipmentUpgradeImprovementCostDetail? y)
	{
		if (ReferenceEquals(x, y))
			return true;

		if (x is null || y is null)
			return false;

		if (x.DevmatCost != y.DevmatCost) return false;
		if (x.SliderDevmatCost != y.SliderDevmatCost) return false;
		if (x.ImproveMatCost != y.ImproveMatCost) return false;
		if (x.SliderImproveMatCost != y.SliderImproveMatCost) return false;

		foreach (EquipmentUpgradeImprovementCostItemDetail costItem in x.ConsumableDetail)
		{
			EquipmentUpgradeImprovementCostItemDetail? matchingCostItem = y.ConsumableDetail.Find(c => c.Id == costItem.Id);

			if (matchingCostItem is null || costItem.Count != matchingCostItem.Count)
				return false;
		}

		foreach (EquipmentUpgradeImprovementCostItemDetail costItem in x.EquipmentDetail)
		{
			EquipmentUpgradeImprovementCostItemDetail? matchingCostItem = y.EquipmentDetail.Find(c => c.Id == costItem.Id);

			if (matchingCostItem is null || costItem.Count != matchingCostItem.Count)
				return false;
		}

		return true;
	}

	public int GetHashCode([DisallowNull] EquipmentUpgradeImprovementModel obj) => GetHashCode(obj.Costs);

	private int GetHashCode(EquipmentUpgradeImprovementCost cost)
	{
		HashCode hash = new();

		hash.Add(cost.Fuel);
		hash.Add(cost.Ammo);
		hash.Add(cost.Steel);
		hash.Add(cost.Bauxite);

		hash.Add(GetHashCode(cost.Cost0To5));
		hash.Add(GetHashCode(cost.Cost6To9));

		return hash.ToHashCode();
	}

	private int GetHashCode(EquipmentUpgradeImprovementCostDetail cost)
	{
		HashCode hash = new();

		hash.Add(cost.DevmatCost);
		hash.Add(cost.SliderDevmatCost);
		hash.Add(cost.ImproveMatCost);
		hash.Add(cost.SliderImproveMatCost);

		foreach (EquipmentUpgradeImprovementCostItemDetail costItem in cost.ConsumableDetail.OrderBy(c => c.Id))
		{
			hash.Add(costItem.Id);
			hash.Add(costItem.Count);
		}

		foreach (EquipmentUpgradeImprovementCostItemDetail costItem in cost.EquipmentDetail.OrderBy(e => e.Id))
		{
			hash.Add(costItem.Id);
			hash.Add(costItem.Count);
		}

		return hash.ToHashCode();
	}
}
